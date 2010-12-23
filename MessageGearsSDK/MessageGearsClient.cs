using MessageGears.Model;
using MessageGears.Model.Generated;

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
		
		/// <summary>
		/// Constructor which sets the properties
		/// </summary>
		/// <param name="properties">
		/// A <see cref="MessageGearsProperties"/>
		/// </param>
		public MessageGearsClient(MessageGearsProperties properties)
		{
			this.properties = properties;
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
		/// Used to return a summary of the total activity for a bulk job (total clicks, bounces, etc.).
		/// </summary>
		/// <param name="request">
		/// The request object.
		/// </param>
		/// <returns>
		/// A <see cref="BulkJobSummaryResponse"/>
		/// </returns>
		public BulkJobSummaryResponse BulkJobSummary(BulkJobSummaryRequest request) 
		{
			// build POST data
			StringBuilder data = new StringBuilder ();
			data.Append ("Action=" + BulkJobSummaryRequest.Action);
			appendCredentials(ref data);
			data.Append("&BulkJobRequestId=" + HttpUtility.UrlEncode (request.BulkJobRequestId));
			data.Append("&BulkJobCorrelationId=" + HttpUtility.UrlEncode (request.BulkJobCorrelationId));
			
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
			data.Append ("&TextTemplate=" + HttpUtility.UrlEncode (request.TextTemplate));
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

