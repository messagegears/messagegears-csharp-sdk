using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;

using log4net;

using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

using MessageGears;
using MessageGears.Model;
using MessageGears.Model.Generated;

namespace MessageGears.EventQueue
{
	/// <summary>
	/// This class is used to poll an Amazon SQS queue for activity items generated from the MessageGears service.
	/// </summary>
	public class MessageGearsAwsQueuePoller
	{
		static readonly ILog log = LogManager.GetLogger(typeof(MessageGearsAwsQueuePoller));
		private AmazonSQS sqs;
		private ReceiveMessageRequest receiveMessageRequest;
		private DeleteMessageRequest deleteMessageRequest;
		private MessageGearsListener listener;
		private MessageGearsAwsProperties props;
		private bool isRunning = false;
		int emptyQueueDelayMillis;
		
		/// <summary>
		/// Instantiates the Poller.
		/// </summary>
		/// <param name="props">
		/// A <see cref="MessageGearsProperties"/>
		/// </param>
		/// <param name="listener">
		/// A <see cref="MessageGearsListener"/>
		/// </param>
		/// <param name="myAwsAccountKey">
		/// You AWS Account Key
		/// </param>
		/// <param name="myAwsSecretKey">
		/// Your AWS Secret Key
		/// </param>
		public MessageGearsAwsQueuePoller(MessageGearsAwsProperties props, MessageGearsListener listener)
		{
			this.props = props;
			this.emptyQueueDelayMillis = props.EmptyQueuePollingDelaySecs * 1000;
			this.listener = listener;
			AmazonSQSConfig config = new AmazonSQSConfig().WithMaxErrorRetry(props.SQSMaxErrorRetry);
			this.sqs = AWSClientFactory.CreateAmazonSQSClient (props.MyAWSAccountKey, props.MyAWSSecretKey, config);
			this.receiveMessageRequest = new ReceiveMessageRequest ()
				.WithQueueUrl (props.MyAWSEventQueueUrl)
				.WithMaxNumberOfMessages (props.SQSMaxBatchSize)
				.WithAttributeName("ApproximateReceiveCount")
				.WithVisibilityTimeout(props.SQSVisibilityTimeoutSecs);
			this.deleteMessageRequest = new DeleteMessageRequest().WithQueueUrl(props.MyAWSEventQueueUrl);
		}
		
		/// <summary>
		/// Starts the polling thread(s) when invoked.
		/// </summary>
		public void Start ()
		{
			isRunning = true;
			log.Info("Starting queue poller(s) with the following properties: " + props.ToString());

			for(int i=1; i <= props.NumberOfEventPollerThreads; i++)
			{
				log.Info("Starting queue poller thread # " + i);
				Thread poller = new Thread (this.Run);
				poller.Start ();
			}
			
			log.Info("Poller(s) started");
		}
		
		/// <summary>
		/// Stops the polling thread(s) when invoked.
		/// </summary>
		public void Stop() {
			isRunning = false;
		}
		
		private void Run ()
		{
	
			int numReceived;

			while(isRunning)
			{
				numReceived = ProcessMessage();
				if(numReceived == 0)
				{
					log.Info(string.Format("Poller received no items.  Thread will sleep for {0} seconds.", props.EmptyQueuePollingDelaySecs));
					Thread.Sleep(emptyQueueDelayMillis);
				}
				else
				{
					log.Info(string.Format("Poller received {0} SQS messages", numReceived));
				}
			}
		}
		
		private int ProcessMessage ()
		{
			int numReceived = 0;
			ActivityItems items = null;
			
			try {
				
				ReceiveMessageResponse receiveMessageResponse = sqs.ReceiveMessage (receiveMessageRequest);

				if (receiveMessageResponse.IsSetReceiveMessageResult ()) {
					
					ReceiveMessageResult receiveMessageResult = receiveMessageResponse.ReceiveMessageResult;
					numReceived = receiveMessageResult.Message.Count;
					
					foreach (Message message in receiveMessageResult.Message) {
											
						items = getActivityItems(message);
						dispatchItems(items);
						
						deleteMessageRequest.WithReceiptHandle(message.ReceiptHandle);
						sqs.DeleteMessage(deleteMessageRequest);
					}
				}
				
			} catch (AmazonSQSException ex) {
				amazonExceptionHandler(ex);
			} catch (Exception ex) {
				log.Warn("An unknown error has occurred.  Will retry.", ex);
			}
			
			return numReceived;
		}
		
		private void amazonExceptionHandler(AmazonSQSException ex)
		{
			if(ex.ErrorCode.Equals("AWS.SimpleQueueService.NonExistentQueue"))
			{
				// Fatal Exception
				String message = "The specified queue does not exist.";
				log.Fatal(message);
				throw new ApplicationException(message, ex);
			}
			else if(ex.ErrorCode.Equals("AccessDenied"))
			{
				// Fatal Exception
				String message = "You do not have permission to read the queue.";
				log.Fatal(message);
				throw new ApplicationException(message, ex);
			}
			else
			{
				log.Warn("An unknown error has occurred.  Will retry.", ex);
			}
		}
		
		private ActivityItems getActivityItems(Message message)
		{
			XmlSerializer serializer = new XmlSerializer (typeof(ActivityItems));
			ActivityItems items = null;
			// deserialize the body of the SQS message as an ActivityItems instance
			using (XmlTextReader sr = new XmlTextReader (new StringReader (message.Body))) {
				log.Debug("Received SQS message: " + message.MessageId + ": " + message.Body);
				items = (ActivityItems)serializer.Deserialize (sr);
			}
			
			// Print out the message metadata
			foreach (Amazon.SQS.Model.Attribute attr in message.Attribute) {
				if(attr.Name.Equals("ApproximateReceiveCount")) {
					if(Int16.Parse(attr.Value) == 1) {
						log.Info(attr.Name + ": " + attr.Value);
					} else {
						log.Warn(attr.Name + ": " + attr.Value);
					}
				}
			}

			return items;
		}
		
		private void dispatchItems(ActivityItems items)
		{
			if(items.BouncedMessageActivity != null)
			{
				foreach(BouncedMessageActivity bounce in items.BouncedMessageActivity)
				{
					log.Info("Received a bounce message belonging to job: " + bounce.RequestId);
					listener.OnBounce(bounce);
				}
			}

			if(items.ClickActivity != null)
			{
				foreach(ClickActivity click in items.ClickActivity)
				{
					log.Info("Received a click message belonging to job: " + click.RequestId);
					listener.OnClick(click);
				}
			}
			
			if(items.DeliveredMessageActivity != null)
			{
				foreach(DeliveredMessageActivity delivery in items.DeliveredMessageActivity)
				{
					log.Info("Received a delivery message belonging to job: " + delivery.RequestId);
					listener.OnDelivery(delivery);
				}
			}

			if(items.JobErrorActivity != null)
			{
				foreach(JobErrorActivity jobError in items.JobErrorActivity)
				{
					log.Info("Received a job error message belonging for job: " + jobError.RequestId);
					listener.OnJobError(jobError);
				}
			}

			if(items.OpenActivity != null)
			{
				foreach(OpenActivity open in items.OpenActivity)
				{
					log.Info("Received an open message belonging for job: " + open.RequestId);
					listener.OnOpen(open);
				}
			}

			if(items.RenderErrorActivity != null)
			{
				foreach(RenderErrorActivity renderError in items.RenderErrorActivity)
				{
					log.Info("Received a render error message belonging to job: " + renderError.RequestId);
					listener.OnRenderError(renderError);
				}
			}

			if(items.SpamComplaintActivity != null)
			{
				foreach(SpamComplaintActivity spamComplaint in items.SpamComplaintActivity)
				{
					log.Info("Received a spam complaint message belonging to job: " + spamComplaint.RequestId);
					listener.OnSpamComplaint(spamComplaint);
				}
			}

			if(items.UnsubActivity != null)
			{
				foreach(UnsubActivity unsub in items.UnsubActivity)
				{
					log.Info("Received an unsub message belonging to job: " + unsub.RequestId);
					listener.OnUnsub(unsub);
				}		
			}
		}
	}
}