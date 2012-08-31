using System;
namespace MessageGears.EventQueue
{
	/// <summary>
	/// Class used to store all of the properties and credentials needed to interface with your Amazon Web Services Account.
	/// </summary>
	public class MessageGearsAwsProperties
	{		
		/// <summary>
		/// The Amazon Web Services Canonical Id for the MessageGears AWS account.
		/// This value is needed to assign S3 permissions to the MessageGears account.
		/// </summary>
		public String MessageGearsAWSCanonicalId { get; set; }
		
		/// <summary>
		/// The Amazon Web Services Account Id for the MessageGears AWS account.
		/// This value is needed to assign SQS permissions to the MessageGears account so it may send messages to a queue.
		/// </summary>
		public String MessageGearsAWSAccountId { get; set; }
		
		/// <summary>
		/// Your Amazon Web Services Account Key.  This value is never sent to MessageGears and is only used to access your S3 and SQS resources in Amazon.
		/// </summary>
		public String MyAWSAccountKey { get; set; }
		
		/// <summary>
		/// Your Amazon Web Services Secret Key.  This value is never sent to MessageGears and is only used to access your S3 and SQS resources in Amazon.
		/// </summary>
		public String MyAWSSecretKey { get; set; }
		
		/// <summary>
		/// This is a very important setting.  When a batch of messages is retrieved by a poller, you will have this many seconds to process the entire batch
		/// before the messages will show up again on the queue.  It is recommended that you leave this number very large to allow for slowdowns in your system
		/// without introducing the risk of duplicate messages being received.
		/// </summary>
		public int SQSVisibilityTimeoutSecs { get; set; }

		/// <summary>
		/// The URL of the queue to be used to retrieve events origintaing from the MessageGears system.
		/// You will need to setup the real-time event feed feature in your MessageGears portal by entering this queue url and selecting the events that you wish to subscribe to.
		/// You will also need to grant permission to the MessageGears AWS account id to be able to send messages to this queue.
		/// </summary>
		public String MyAWSEventQueueUrl { get; set; }
		
		/// <summary>
		/// The number of poller threads your want to check for events on your queue.  One thread should be fine for small volume accounts.
		/// You may need to experiment with the number of threads for higher volume accounts.
		/// The more threads you run, the higher your access costs will be from Amazon.
		/// It costs approx $1.00 per million polls of an Amazon queue.
		/// </summary>
		public int NumberOfEventPollerThreads { get; set; }
		
		/// <summary>
		/// The number of seconds a polling thread should pause if no messages are found on the queue.  This will help reduce changes for the Amazon service.
		/// The poller will process messages as quickly as it can as long as messages are found on the queue.
		/// </summary>
		public int EmptyQueuePollingDelaySecs { get; set; }
		
		/// <summary>
		/// The maximum number of messages to be returned from Amazon SQS in a single call.  This is an Amazon SQS setting and the max value is 10.
		/// Batching SQS messages can improve performance and reduce costs of the SQS service.
		/// </summary>
		public int SQSMaxBatchSize { get; set; }
		
		/// <summary>
		/// Sets the maximum number of retry attempts for failed retryable requests (ex: 5xx error responses from services).
		/// </summary>
		public int SQSMaxErrorRetry  { get; set; }

		/// <summary>
		/// Sets the default timout for copying a file to S3.
		/// </summary>
		public int S3PutTimeoutSecs { get; set; }
		
		/// <summary>
		/// Sets the default number of retries for an S3 operation.
		/// </summary>
		public int S3MaxErrorRetry { get; set; }
		
		/// <summary>
		/// Sets the default wait before retrying an S3 operation, in milliseconds.
		/// </summary>
		public int S3RetryDelayInterval { get; set; }


		/// <summary>
		/// Set the default values for most of the properties.
		/// </summary>				
		public MessageGearsAwsProperties() {
			MessageGearsAWSCanonicalId = "2dd8e53f1a8e4dfe3a6893d1229635b4915661d95f5283df75215779ce462819";	
			MessageGearsAWSAccountId="406967126799";
			SQSVisibilityTimeoutSecs=600;
			S3PutTimeoutSecs=900;
			NumberOfEventPollerThreads=1;
			EmptyQueuePollingDelaySecs=30;
			SQSMaxBatchSize=1;
			SQSVisibilityTimeoutSecs=600;
			S3MaxErrorRetry=5;
			S3RetryDelayInterval=1000;
		}
		
		/// <summary>
		/// Dumps out all of the properties.
		/// </summary>
		/// <returns>
		/// A string containing a list of all the properties and their associated values.
		/// </returns>
		public override String ToString()
		{
			String dump = " MessageGearsAWSCanonicalId=" + MessageGearsAWSCanonicalId;
			dump = dump + " MessageGearsAWSAccountId=" + MessageGearsAWSAccountId;
			dump = dump + " MyAWSAccountKey=" + MyAWSAccountKey;
			dump = dump + " MyAWSSecretKey=" + "<hidden>";
			dump = dump + " SQSVisibilityTimeoutSecs=" + SQSVisibilityTimeoutSecs;
			dump = dump + " S3PutTimeoutSecs=" + S3PutTimeoutSecs;
			dump = dump + " MyAWSEventQueueUrl=" + MyAWSEventQueueUrl;
			dump = dump + " NumberOfEventPollerThreads=" + NumberOfEventPollerThreads;
			dump = dump + " EmptyQueuePollingDelaySecs=" + EmptyQueuePollingDelaySecs;
			dump = dump + " SQSMaxBatchSize=" + SQSMaxBatchSize;
			dump = dump + " SQSMaxErrorRetry=" + SQSMaxErrorRetry;
			dump = dump + " S3PutTimeoutSecs=" + S3PutTimeoutSecs;
			dump = dump + " S3MaxErrorRety=" + S3MaxErrorRetry;
			dump = dump + " S3RetryDelayInterval=" + S3RetryDelayInterval;
			return dump;
		}
	}
}
