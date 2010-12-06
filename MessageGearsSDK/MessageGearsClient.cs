using MessageGears.Model;
using MessageGears.Model.Generated;

using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon.Auth;
using Amazon.Auth.AccessControlPolicy;

using log4net;

using System;
using System.Text;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;
using System.Collections.Generic;
using System.Web;

namespace MessageGears
{
	/// <summary>
	/// MessageGears C# SDK main entry point. 
	/// </summary>
	public class MessageGearsClient
	{
		static readonly ILog log = LogManager.GetLogger(typeof(MessageGearsClient));
		MessageGearsProperties properties = null;
		AmazonSQS sqs = null;
		AmazonS3 s3 = null;
		
		/// <summary>
		/// Used to create a new instance of the MessageGears client.
		/// </summary>
		/// <param name="props">
		/// Contains the credentials needed to access MessageGears, Amazon S3, and Amazon SQS.<see cref="MessageGearsProperties"/>
		/// </param>
		public MessageGearsClient(MessageGearsProperties props)
		{
			this.properties = props;
			this.sqs = new AmazonSQSClient(properties.MyAWSAccountKey, properties.MyAWSSecretKey);
			this.s3 = new AmazonS3Client(properties.MyAWSAccountKey, properties.MyAWSSecretKey);
			log.Info("MessageGears client initialized");
		}
		
		/// <summary>
		/// Submits a Transactional Job (to a single recipient) for processing.
		/// </summary>
		/// <param name="request">
		/// A <see cref="TransactionalJobSubmitRequest"/>
		/// </param>
		/// <returns>
		/// A <see cref="TransactionalJobSubmitResponse"/>
		/// </returns>
		public TransactionalJobSubmitResponse TransactionalJobSubmit (TransactionalJobSubmitRequest request)
		{
			// build POST data 
			StringBuilder data = new StringBuilder ();
			data.Append ("Action=" + HttpUtility.UrlEncode (TransactionalJobSubmitRequest.Action));
			appendCredentials(ref data);
			appendBaseJobRequest(ref data, request);
			data.Append ("&RecipientXml=" + HttpUtility.UrlEncode (request.RecipientXml));
			
			// invoke endpoint
			string response = invoke (data);
			
			// deserialize response into TransactionalJobSubmitResponse
			XmlSerializer serializer = new XmlSerializer (typeof(TransactionalJobSubmitResponse));
			using (XmlTextReader sr = new XmlTextReader (new StringReader (response))) {
				TransactionalJobSubmitResponse objectResponse = (TransactionalJobSubmitResponse)serializer.Deserialize (sr);
				if(objectResponse.Result.Equals(Result.REQUEST_SUCCESSFUL))
				{
					log.Info("Transactional job successfully processed: " + objectResponse.RequestId);
				}
				else
				{
					log.Error("Transactional job failed: " + objectResponse.RequestId);
				}
				return objectResponse;
			}
		}
		
		/// <summary>
		/// Allows you to submit your message content for rendering without actually sending a email message.
		/// This can be used to build a UI tool to test content as it is being created.  A list of errors will be
		/// returned if there are and problems rendering the message.
		/// </summary>
		/// <param name="request">
		/// A <see cref="MessagePreviewRequest"/>
		/// </param>
		/// <returns>
		/// A <see cref="MessagePreviewResponse"/>
		/// </returns>
		public MessagePreviewResponse MessagePreview (MessagePreviewRequest request)
		{
			// build POST data 
			StringBuilder data = new StringBuilder ();
			data.Append ("Action=" + HttpUtility.UrlEncode (MessagePreviewRequest.Action));
			appendCredentials(ref data);
			appendBaseJobRequest(ref data, request);
			data.Append ("&RecipientXml=" + HttpUtility.UrlEncode (request.RecipientXml));
			
			// invoke endpoint
			string response = invoke (data);
			
			// deserialize response into TransactionalJobSubmitResponse
			XmlSerializer serializer = new XmlSerializer (typeof(MessagePreviewResponse));
			using (XmlTextReader sr = new XmlTextReader (new StringReader (response))) {
				MessagePreviewResponse objectResponse = (MessagePreviewResponse)serializer.Deserialize (sr);
				if(objectResponse.Result.Equals(Result.REQUEST_SUCCESSFUL))
				{
					log.Info("Message Preview successfully processed: " + objectResponse.RequestId);
				}
				else
				{
					log.Error("Message Preview failed: " + objectResponse.RequestId);
				}
				return objectResponse;
			}
		}

		/// <summary>
		/// Submits a bulk job for processing.  A bulk job can be used to send a email message to 
		/// one or more (millions) of recipients.
		/// </summary>
		/// <param name="request">
		/// A <see cref="BulkJobSubmitRequest"/>
		/// </param>
		/// <returns>
		/// A <see cref="BulkJobSubmitRequest"/>
		/// </returns>
		public BulkJobSubmitResponse BulkJobSubmit (BulkJobSubmitRequest request)
		{
		    // build POST data 
			StringBuilder data = new StringBuilder ();
			data.Append ("Action=" + HttpUtility.UrlEncode (BulkJobSubmitRequest.Action));
			data.Append ("&RecipientListXmlUrl=" + HttpUtility.UrlEncode (request.RecipientListXmlUrl));
			data.Append ("&ContextDataXml=" + HttpUtility.UrlEncode (request.ContextDataXml));
			data.Append ("&CorrelationId=" + HttpUtility.UrlEncode (request.CorrelationId));
			appendCredentials(ref data);
			appendBaseJobRequest(ref data, request);
			
			// invoke endpoint
			string response = invoke (data);
			
			// deserialize response into BulkJobSubmitResponse
			XmlSerializer serializer = new XmlSerializer (typeof(BulkJobSubmitResponse));
			using (XmlTextReader sr = new XmlTextReader (new StringReader (response))) {
				BulkJobSubmitResponse objectResponse = (BulkJobSubmitResponse)serializer.Deserialize (sr);
				if(objectResponse.Result.Equals(Result.REQUEST_SUCCESSFUL))
				{
					log.Info("Bulk job successfully processed: " + objectResponse.RequestId);
				}
				else
				{
					log.Error("Bulk job failed: " + objectResponse.RequestId);
				}
				return objectResponse;
			}
			
		}
		
		/// <summary>
		/// Used to return a summary of total account activity (total clicks, bounces, etc.) for a given day.
		/// </summary>
		/// <param name="activityDate">
		/// A <see cref="AccountSummaryResponse"/>
		/// </param>
		/// <returns>
		/// A <see cref="AccountSummaryResponse"/>
		/// </returns>
		public AccountSummaryResponse AccountSummary (DateTime activityDate)
		{
			// build POST data 
			StringBuilder data = new StringBuilder ();
			data.Append ("Action=" + HttpUtility.UrlEncode ("AccountSummary"));
			appendCredentials(ref data);
			data.Append("&ActivityDate=" + HttpUtility.UrlEncode (activityDate.Year + "-" + activityDate.Month + "-" + activityDate.Day));
			
			// invoke endpoint
			string response = invoke (data);
			
			// deserialize response into BulkJobSummaryResponse
			XmlSerializer serializer = new XmlSerializer (typeof(AccountSummaryResponse));
			using (XmlTextReader sr = new XmlTextReader (new StringReader (response))) {
				AccountSummaryResponse objectResponse = (AccountSummaryResponse)serializer.Deserialize (sr);
				if(objectResponse.Result.Equals(Result.REQUEST_SUCCESSFUL))
				{
					log.Info("Account Summary successfully processed: " + objectResponse.RequestId);
				}
				else
				{
					log.Error("Account Summary failed: " + objectResponse.RequestId);
				}
				return objectResponse;
			}
		}

		/// <summary>
		/// Used to return a summary of the total activity for a bulk job (total clicks, bounces, etc.).
		/// </summary>
		/// <param name="bulkJobRequestId">
		/// The id of the bulk job.
		/// </param>
		/// <returns>
		/// A <see cref="BulkJobSummaryResponse"/>
		/// </returns>
		public BulkJobSummaryResponse BulkJobSummary(String bulkJobRequestId) 
		{
			// build POST data 
			StringBuilder data = new StringBuilder ();
			data.Append ("Action=" + HttpUtility.UrlEncode ("BulkJobSummary"));
			appendCredentials(ref data);
			data.Append("&BulkJobRequestId=" + HttpUtility.UrlEncode (bulkJobRequestId));
			
			// invoke endpoint
			string response = invoke (data);
			
			// deserialize response into BulkJobSummaryResponse
			XmlSerializer serializer = new XmlSerializer (typeof(BulkJobSummaryResponse));
			using (XmlTextReader sr = new XmlTextReader (new StringReader (response))) {
				BulkJobSummaryResponse objectResponse = (BulkJobSummaryResponse)serializer.Deserialize (sr);
				if(objectResponse.Result.Equals(Result.REQUEST_SUCCESSFUL))
				{
					log.Info("Bulk Job Summary successfully processed: " + objectResponse.RequestId);
				}
				else
				{
					log.Error("Bulk Job Summary failed: " + objectResponse.RequestId);
				}
				return objectResponse;
			}
		}
		
		/// <summary>
		/// Used to create a file that contains all of the account activity of a certain type for the specified day.
		/// </summary>
		/// <param name="activityDate">
		/// The date of the activity data to be collected.<see cref="DateTime"/>
		/// </param>
		/// <param name="activityType">
		/// The tyoe of activity to be collected.<see cref="ActivityType"/>
		/// </param>
		/// <returns>
		/// The fully qualified name of the downloaded file (YYYY-MM-DD_activitytype.xml)
		/// </returns>
		public String AccountActivity(DateTime activityDate, ActivityType activityType)
		{
			DateTime start = DateTime.Now;
			String fileName = properties.DownloadDirectory + activityDate.Year + "-" + activityDate.Month + "-" + activityDate.Day + "_" + activityType + ".xml";
			// build POST data 
			StringBuilder data = new StringBuilder ();
			data.Append ("Action=" + HttpUtility.UrlEncode ("AccountActivity"));
			appendCredentials(ref data);
			data.Append("&ActivityDate=" + HttpUtility.UrlEncode (activityDate.Year + "-" + activityDate.Month + "-" + activityDate.Day));
			data.Append ("&ActivityType=" + HttpUtility.UrlEncode (activityType.ToString()));
			
			// invoke endpoint
			invoke (data, fileName);
			TimeSpan ts = DateTime.Now - start;
			FileInfo fi = new FileInfo(fileName);
			long mbyte = 1024 * 1024;
			if(fi.Length >= mbyte)
			{
				log.Info(string.Format("Downloaded file {0} of size {1:n} MB in {2:00}:{3:00}:{4:00},{5:000}", fi.Name, (float)fi.Length/mbyte, (int)ts.TotalHours, ts.Minutes, ts.Seconds, ts.Milliseconds));
			}
			else if(fi.Length >= 1024)
			{
				log.Info(string.Format("Downloaded file {0} of size {1:n} KB in {2:00}:{3:00}:{4:00},{5:000}", fi.Name, (float)fi.Length/1024, (int)ts.TotalHours, ts.Minutes, ts.Seconds, ts.Milliseconds));
			}
			else
			{
				log.Info(string.Format("Downloaded file {0} of size {1:##0} Bytes in {2:00}:{3:00}:{4:00},{5:000}", fi.Name, fi.Length, (int)ts.TotalHours, ts.Minutes, ts.Seconds, ts.Milliseconds));
			}
			return fileName;
		}
		
		/// <summary>
		/// Copies a file to Amazon S3 and grants READ-ONLY access to MessageGears.
		/// </summary>
		/// <param name="fileName">
		/// The fully qualified name of the file to be copied to S3.
		/// </param>
		/// <param name="bucketName">
		/// The name of the S3 bucket where the file will be copied.
		/// </param>
		/// <param name="key">
		/// The S3 key of the file to be created.
		/// </param>
		public void PutS3File(String fileName, String bucketName, String key)
		{
			// Check to see if the file already exists in S3
			ListObjectsRequest listRequest = new ListObjectsRequest().WithBucketName(bucketName).WithPrefix(key);
			ListObjectsResponse listResponse = s3.ListObjects(listRequest);
			if(listResponse.S3Objects.Count > 0)
			{
				String message = "File " + fileName + " already exists.";
				log.Warn("PutS3File failed: " + message);
				throw new ApplicationException(message);
			}
			
			// Copy a file to S3
			PutObjectRequest request = new PutObjectRequest().WithKey(key).WithFilePath(fileName).WithBucketName(bucketName);
			s3.PutObject(request);
			
			// Get the ACL for the file and retrieve the owner ID (not sure how to get it otherwise).
			GetACLRequest getAclRequest = new GetACLRequest().WithBucketName(bucketName).WithKey(key);
			GetACLResponse aclResponse = s3.GetACL(getAclRequest);
			Owner owner = aclResponse.AccessControlList.Owner;
			
			// Create a grantee as the MessageGears account
			S3Grantee grantee = new S3Grantee().WithCanonicalUser(properties.MessageGearsAWSCanonicalId, "MessageGears");

			// Create an new ACL for the file and give MessageGears Read-only access, and the owner full control.
			S3AccessControlList acl = new S3AccessControlList().WithOwner(owner);
			acl.AddGrant(grantee, S3Permission.READ);
			grantee = new S3Grantee().WithCanonicalUser(owner.Id, "MyAWSId");
			acl.AddGrant(grantee, S3Permission.FULL_CONTROL);
			SetACLRequest aclRequest = new SetACLRequest().WithACL(acl).WithBucketName(bucketName).WithKey(key);
			s3.SetACL(aclRequest);
			
			log.Info("PutS3File successful: " + fileName);
		}
		
		/// <summary>
		/// Deletes a file from Amazon S3.
		/// </summary>
		/// <param name="bucketName">
		/// The name of the bucket where the file resides.
		/// </param>
		/// <param name="key">
		/// The key of the file to be deleted.
		/// </param>
		public void DeleteS3File(String bucketName, String key)
		{
			// Copy a file to S3
			DeleteObjectRequest request = new DeleteObjectRequest().WithBucketName(bucketName).WithKey(key);
			s3.DeleteObject(request);
			log.Info("DeleteS3File successful: " + bucketName + "/" + key);

		}
		
		/// <summary>
		/// Creates a new queue in Amazon SQS and grants SendMessage only access to MessageGears.
		/// </summary>
		/// <param name="queueName">
		/// The name of the queue to be created.
		/// </param>
		/// <returns>
		/// The full url of the newly created queue.
		/// </returns>
		public String CreateQueue(String queueName)
		{
			CreateQueueRequest request = new CreateQueueRequest()
				.WithQueueName(queueName)
				.WithDefaultVisibilityTimeout(properties.SQSVisibilityTimeoutSecs);

			CreateQueueResponse response = sqs.CreateQueue(request);
			
			addQueuePermission(response.CreateQueueResult.QueueUrl);
			
			log.Info("Create queue successful: " + queueName);
			
			return response.CreateQueueResult.QueueUrl;
		}
		
		private void addQueuePermission(String queueUrl)
		{
			AddPermissionRequest permissionRequest = new AddPermissionRequest()
				.WithActionName("SendMessage")
				.WithAWSAccountId(properties.MessageGearsAWSAccountId)
				.WithLabel("MessageGears Send Permission")
				.WithQueueUrl(queueUrl);
		
			sqs.AddPermission(permissionRequest);
		}
		
		private void invoke (StringBuilder data, String fileName)
		{
			
			Uri address = new Uri (properties.MessageGearsEndPoint);
			
			// Create the web request  
			HttpWebRequest request = WebRequest.Create (address) as HttpWebRequest;
			
			// Set type to POST  
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			
			log.Debug("Post request: " + data.ToString());
			
			// Create a byte array of the data we want to send  
			byte[] byteData = UTF8Encoding.UTF8.GetBytes (data.ToString ());
			
			// Set the content length in the request headers  
			request.ContentLength = byteData.Length;
			
			// Write data  
			using (Stream postStream = request.GetRequestStream ()) {
				postStream.Write (byteData, 0, byteData.Length);
			}
			
			// Get response  
			using (HttpWebResponse response = request.GetResponse () as HttpWebResponse) {
				// Get the response stream  
				using (Stream file = File.OpenWrite(fileName))
				{
    				CopyStream(response.GetResponseStream (), file);
				}
			}
		}
		
		private static void CopyStream(Stream input, Stream output)
		{
    		byte[] buffer = new byte[1 * 1024];
    		int len;
			int byteCount=0;
    		while ( (len = input.Read(buffer, 0, buffer.Length)) != 0)
    		{
				byteCount =+ len;
				output.Write(buffer, 0, len);
    		}
			log.Debug("Number of bytes downloaded: " + Convert.ToString(byteCount));
		}

		private String invoke (StringBuilder data)
		{
			
			Uri address = new Uri (properties.MessageGearsEndPoint);
			
			// Create the web request  
			HttpWebRequest request = WebRequest.Create (address) as HttpWebRequest;
			
			// Set type to POST  
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			
			log.Debug("Post request: " + data.ToString());
			
			// Create a byte array of the data we want to send  
			byte[] byteData = UTF8Encoding.UTF8.GetBytes (data.ToString ());
			
			// Set the content length in the request headers  
			request.ContentLength = byteData.Length;
			
			// Write data  
			using (Stream postStream = request.GetRequestStream ()) {
				postStream.Write (byteData, 0, byteData.Length);
			}
			
			// Get response  
			using (HttpWebResponse response = request.GetResponse () as HttpWebResponse) {
				// Get the response stream  
				StreamReader reader = new StreamReader (response.GetResponseStream ());
				string responseText = reader.ReadToEnd ();
				log.Debug("Post response: " + responseText);
				return responseText;
			}
		}
		
		private void appendCredentials(ref StringBuilder data)
		{
		    data.Append ("&AccountId=" + HttpUtility.UrlEncode (properties.MyMessageGearsAccountId));
			data.Append ("&ApiKey=" + HttpUtility.UrlEncode (properties.MyMessageGearsApiKey));
		}
	
		private void appendBaseJobRequest(ref StringBuilder data, JobRequest request)
		{
			data.Append ("&FromAddress=" + HttpUtility.UrlEncode (request.FromAddress));
			data.Append ("&FromName=" + HttpUtility.UrlEncode (request.FromName));
			data.Append ("&SubjectLine=" + HttpUtility.UrlEncode (request.SubjectLine));
			data.Append ("&HtmlTemplate=" + HttpUtility.UrlEncode (request.HtmlTemplate));
			data.Append ("&TemplateLanguage=" + HttpUtility.UrlEncode (request.TemplateLanguage.ToString()));
			data.Append ("&IpSelector=" + HttpUtility.UrlEncode (request.IpSelector));
			data.Append ("&CharacterSet=" + HttpUtility.UrlEncode (request.CharacterSet));
			data.Append ("&NotificationEmailAddress=" + HttpUtility.UrlEncode (request.NotificationEmailAddress));
			data.Append ("&ReplyToAddress=" + HttpUtility.UrlEncode (request.ReplyToAddress));
			data.Append ("&OnBehalfOfAddress=" + HttpUtility.UrlEncode (request.OnBehalfOfAddress));
			data.Append ("&OnBehalfOfName=" + HttpUtility.UrlEncode (request.OnBehalfOfName));
			data.Append ("&AutoTrack=" + HttpUtility.UrlEncode (request.AutoTrack.ToString()));
			data.Append ("&UrlAppend=" + HttpUtility.UrlEncode (request.UrlAppend));
			data.Append ("&CustomTrackingDomain=" + HttpUtility.UrlEncode (request.CustomTrackingDomain));
			
			String attachmentCount;
			for (int i=0; i < request.attachments.Count; i++)
			{
				attachmentCount = (i+1).ToString("D");
				data.Append ("&AttachmentUrl." + attachmentCount + "=" + HttpUtility.UrlEncode (request.attachments[i].Url));
				data.Append ("&AttachmentName." + attachmentCount + "=" + HttpUtility.UrlEncode (request.attachments[i].DisplayName));
				data.Append ("&AttachmentContentType." + attachmentCount + "=" + HttpUtility.UrlEncode (request.attachments[i].ContentType));
			}
		}
	}
}

