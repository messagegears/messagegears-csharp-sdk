using System;
namespace MessageGears
{
	/// <summary>
	/// Class used to store all of the properties and credentials needed to interface with MessageGears and Amazon Web Services.
	/// </summary>
	public class MessageGearsProperties
	{
		/// <summary>
		/// The base URL for the MessageGears service.
		/// </summary>
		public String MessageGearsEndPoint { get; set; }
		
		/// <summary>
		/// Your MessageGears Account Id.
		/// </summary>
		public String MyMessageGearsAccountId { get; set; }
		
		/// <summary>
		/// Your MessageGears API Key.
		/// </summary>
		public String MyMessageGearsApiKey { get; set; }
		
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
		/// This is a very important setting.  When a batch of messages is retrieved by a poller, you will have this many seconds to process the entire batch
		/// before the messages will show up again on the queue.  It is recommended that you leave this number very large to allow for slowdowns in your system
		/// without introducing the risk of duplicate messages being received.
		/// </summary>
		public int SQSVisibilityTimeoutSecs { get; set; }
		
		/// <summary>
		/// The directory in which activity file downloaded from MessageGears will be stored.
		/// </summary>
		public String DownloadDirectory  { get; set; }
		
		/// <summary>
		/// Sets the maximum number of retry attempts for failed retryable requests (ex: 5xx error responses from services).
		/// </summary>
		public int SQSMaxErrorRetry  { get; set; }

		/// <summary>
		/// Public constructor setting default values for the MessageGearsProperties class.
		/// </summary>
		public MessageGearsProperties () {
			MessageGearsEndPoint="https://api.messagegears.net/3.1/WebService";
			NumberOfEventPollerThreads=1;
			EmptyQueuePollingDelaySecs=30;
			SQSMaxBatchSize=1;
			SQSVisibilityTimeoutSecs=600;
			DownloadDirectory=".";
		}

		/// <summary>
		/// Dumps out all of the properties.
		/// </summary>
		/// <returns>
		/// A string containing a list of all the properties and their associated values.
		/// </returns>
		public override String ToString()
		{
			String dump = "MessageGearsEndPoint=" + MessageGearsEndPoint;
			dump = dump + " MyMessageGearsAccountId=" + MyMessageGearsAccountId;
			dump = dump + " MyMessageGearsApiKey=" + "<hidden>";
			dump = dump + " MyAWSEventQueueUrl=" + MyAWSEventQueueUrl;
			dump = dump + " NumberOfEventPollerThreads=" + NumberOfEventPollerThreads;
			dump = dump + " EmptyQueuePollingDelaySecs=" + EmptyQueuePollingDelaySecs;
			dump = dump + " SQSMaxBatchSize=" + SQSMaxBatchSize;
			dump = dump + " SQSVisibilityTimeoutSecs=" + SQSVisibilityTimeoutSecs;
			dump = dump + " DownloadDirectory=" + DownloadDirectory;
			dump = dump + " SQSMaxErrorRetry=" + SQSMaxErrorRetry;
			return dump;
		}
	}
}

