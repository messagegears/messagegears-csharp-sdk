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
			appendJobRequest(ref data, request);
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
		/// Submits a Transactional Campaign (to a single recipient) for processing.
		/// </summary>
		/// <param name="request">
		/// A <see cref="TransactionalCampaignSubmitRequest"/>
		/// </param>
		/// <returns>
		/// A <see cref="TransactionalJobSubmitResponse"/>
		/// </returns>
		public TransactionalJobSubmitResponse TransactionalCampaignSubmit (TransactionalCampaignSubmitRequest request)
		{
			// build POST data 
			StringBuilder data = new StringBuilder ();
			data.Append ("Action=" + HttpUtility.UrlEncode (TransactionalCampaignSubmitRequest.Action));
			appendCredentials(ref data);
			appendCampaignRequest(ref data, request);
			data.Append ("&RecipientXml=" + HttpUtility.UrlEncode (request.RecipientXml));
			
			// invoke endpoint
			string response = invoke (data);
			
			// deserialize response into TransactionalJobSubmitResponse
			XmlSerializer serializer = new XmlSerializer (typeof(TransactionalJobSubmitResponse));
			using (XmlTextReader sr = new XmlTextReader (new StringReader (response))) {
				TransactionalJobSubmitResponse objectResponse = (TransactionalJobSubmitResponse)serializer.Deserialize (sr);
				if(objectResponse.Result.Equals(Result.REQUEST_SUCCESSFUL))
				{
					log.Info("Transactional campaign successfully processed: " + objectResponse.RequestId);
				}
				else
				{
					log.Error("Transactional campaign failed: " + objectResponse.RequestId);
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
			appendJobRequest(ref data, request);
			data.Append ("&RecipientXml=" + HttpUtility.UrlEncode (request.RecipientXml));
			data.Append ("&ContextDataXml=" + HttpUtility.UrlEncode (request.ContextDataXml));
			
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
			appendCredentials(ref data);
			appendJobRequest(ref data, request);
			
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
		/// Submits a bulk campaign for processing.  A bulk campaign can be used to send a email message to 
		/// one or more (millions) of recipients.
		/// </summary>
		/// <param name="request">
		/// A <see cref="BulkCampaignSubmitRequest"/>
		/// </param>
		/// <returns>
		/// A <see cref="BulkJobSubmitResponse"/>
		/// </returns>
		public BulkJobSubmitResponse BulkCampaignSubmit (BulkCampaignSubmitRequest request)
		{
		    // build POST data 
			StringBuilder data = new StringBuilder ();
			data.Append ("Action=" + HttpUtility.UrlEncode (BulkCampaignSubmitRequest.Action));
			data.Append ("&RecipientListXmlUrl=" + HttpUtility.UrlEncode (request.RecipientListXmlUrl));
			appendCredentials(ref data);
			appendCampaignRequest(ref data, request);
			
			Console.WriteLine(request);
			// invoke endpoint
			string response = invoke (data);
			Console.WriteLine(response);
			
			// deserialize response into BulkJobSubmitResponse
			XmlSerializer serializer = new XmlSerializer (typeof(BulkJobSubmitResponse));
			using (XmlTextReader sr = new XmlTextReader (new StringReader (response))) {
				BulkJobSubmitResponse objectResponse = (BulkJobSubmitResponse)serializer.Deserialize (sr);
				if(objectResponse.Result.Equals(Result.REQUEST_SUCCESSFUL))
				{
					log.Info("Bulk campaign successfully processed: " + objectResponse.RequestId);
				}
				else
				{
					log.Error("Bulk campaign failed: " + objectResponse.RequestId);
				}
				return objectResponse;
			}
			
		}
		
		/// <summary>
		/// Used to return a summary of total account activity (total clicks, bounces, etc.) for a given month.
		/// </summary>
		/// <param name="activityYear">
		/// A <see cref="String"/> Must be in the form of YYYY.
		/// </param>
		/// <param name="activityMonth">
		/// A <see cref="String"/> Must be in the form of MM (eg "01", "12", etc).
		/// </param>
		/// <returns>
		/// A <see cref="AccountSummaryResponse"/>
		/// </returns>
		public AccountSummaryResponse AccountSummary (int activityYear, int activityMonth)
		{
			// build POST data 
			StringBuilder data = new StringBuilder ();
			data.Append ("Action=" + HttpUtility.UrlEncode ("AccountSummary"));
			appendCredentials(ref data);
			data.Append("&ActivityDate=" + HttpUtility.UrlEncode (activityYear.ToString("0000") + "-" + activityMonth.ToString("00")));
			
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
		/// Utility function to print response data to the console.
		/// </summary>
		/// <param name="response">
		/// The TransactionalJobSubmitResponse to be printed.<see cref="TransactionalJobSubmitResponse"/>
		/// </param>
		public void PrintResponse(TransactionalJobSubmitResponse response) {
			PrintResponse(response.Result, response.RequestErrors);
		}
		
		/// <summary>
		/// Utility function to print response data to the console.
		/// </summary>
		/// <param name="response">
		/// The BulkJobSubmitResponse to be printed.<see cref="BulkJobSubmitResponse"/>
		/// </param>
		public void PrintResponse(BulkJobSubmitResponse response) {
			PrintResponse(response.Result, response.RequestErrors);
		}
		
		/// <summary>
		/// Utility function to print response data to the console.
		/// </summary>
		/// <param name="response">
		/// The AccountSummaryResponse to be printed.<see cref="AccountSummaryResponse"/>
		/// </param>
		public void PrintResponse(AccountSummaryResponse response) {
			PrintResponse(response.Result, response.RequestErrors);
			if (response.Result == Result.REQUEST_SUCCESSFUL) {
				Console.WriteLine("AccountSummary For: " + response.AccountSummary.ActivityDate);
				Console.WriteLine("Messages: " + response.AccountSummary.Messages);
				Console.WriteLine("Deliveries: " + response.AccountSummary.Deliveries);
				Console.WriteLine("Bounces: " + response.AccountSummary.Bounces);
				Console.WriteLine("Opens: " + response.AccountSummary.Opens);
				Console.WriteLine("Clicks: " + response.AccountSummary.Clicks);
				Console.WriteLine("JobErrors: " + response.AccountSummary.JobErrors);
				Console.WriteLine("RenderErrors: " + response.AccountSummary.RenderErrors);
				Console.WriteLine("Complaints: " + response.AccountSummary.SpamComplaints);
				Console.WriteLine("Unsubscribes: " + response.AccountSummary.Unsubscribes);
			}
		}
		
		/// <summary>
		/// Utility function to print response data to the console.
		/// </summary>
		/// <param name="response">
		/// The BulkJobSummaryResponse to be printed.<see cref="BulkJobSummaryResponse"/>
		/// </param>
		public void PrintResponse(BulkJobSummaryResponse response) {
			PrintResponse(response.Result, response.RequestErrors);
			if (response.Result == Result.REQUEST_SUCCESSFUL) {
				Console.WriteLine("BulkJobSummary For: " + response.BulkJobSummary.BulkJobRequestId);
				Console.WriteLine("CorrelationId: " + response.BulkJobSummary.CorrelationId);
				
				if (response.BulkJobSummary.BulkJobErrors == null) {
					Console.WriteLine("Messages: " + response.BulkJobSummary.Messages);
					Console.WriteLine("Deliveries: " + response.BulkJobSummary.Deliveries);
					Console.WriteLine("Bounces: " + response.BulkJobSummary.Bounces);
					Console.WriteLine("Opens: " + response.BulkJobSummary.Opens);
					Console.WriteLine("Clicks: " + response.BulkJobSummary.Clicks);
					Console.WriteLine("RenderErrors: " + response.BulkJobSummary.RenderErrors);
					Console.WriteLine("Complaints: " + response.BulkJobSummary.SpamComplaints);
					Console.WriteLine("Unsubscribes: " + response.BulkJobSummary.Unsubscribes);
				} else {
					foreach(BulkJobError error in response.BulkJobSummary.BulkJobErrors) {
						Console.WriteLine("Error: " + error.ErrorCode + " - " + error.ErrorMessage);
					}
				}
			}
		}
		
		/// <summary>
		/// Utility function to print response data to the console.
		/// </summary>
		/// <param name="response">
		/// The MessagePreviewResponse to be printed.<see cref="MessagePreviewResponse"/>
		/// </param>
		public void PrintResponse(MessagePreviewResponse response) {
			PrintResponse(response.Result, response.RequestErrors);
			if (response.Result == Result.REQUEST_SUCCESSFUL) {				
				if (response.RenderErrors == null) {
					Console.WriteLine("FromName: " + response.PreviewContent.FromName +
					                  " <" + response.PreviewContent.FromAddress + ">");
					Console.WriteLine("OnBehalfOf: " + response.PreviewContent.OnBehalfOfName +
					                  " <" + response.PreviewContent.OnBehalfOfAddress + ">");
					Console.WriteLine("ReplyTo: " + response.PreviewContent.ReplyToAddress);
					Console.WriteLine("Subject: " + response.PreviewContent.SubjectLine);
					Console.WriteLine("HtmlContent: " + response.PreviewContent.HtmlContent);
					Console.WriteLine("TextContent: " + response.PreviewContent.TextContent);
					Console.WriteLine("SpamAssassinReport:");
					Console.WriteLine("SpamAssassinScore: " + response.PreviewContent.SpamAssassinReport.Score);
					foreach(SpamAssassinRule rule in response.PreviewContent.SpamAssassinReport.SpamAssassinRules) {
						Console.WriteLine(rule.Points + " Points - " + rule.RuleName + " : " + rule.Description);
					}
				} else {
					foreach(RenderError error in response.RenderErrors) {
						Console.WriteLine("RenderError: " + error.ErrorCode + " - " + error.ErrorMessage);
					}
				}
			} else {
				foreach(RequestError error in response.RequestErrors) {
					Console.WriteLine("RequestError: " + error.ErrorCode + " - " + error.ErrorMessage);
				}
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
		
		private void PrintResponse(Result result, RequestError[] requestErrors) {
			if(result == Result.REQUEST_SUCCESSFUL)
			{
				Console.WriteLine(Result.REQUEST_SUCCESSFUL.ToString());
			}
			else
			{
				Console.WriteLine(Result.REQUEST_FAILED.ToString());
				Console.WriteLine("Number of Errors: " + requestErrors.Length);
				foreach(RequestError error in requestErrors)
				{
					Console.WriteLine(error.ErrorCode + ":" + error.ErrorMessage);
				}
			}
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
		
		private void appendBaseJobRequest(ref StringBuilder data, BaseJobRequest request) 
		{
			data.Append ("&NotificationEmailAddress=" + HttpUtility.UrlEncode (request.NotificationEmailAddress));
			data.Append ("&CorrelationId=" + HttpUtility.UrlEncode (request.CorrelationId));
		}
		
		private void appendCampaignRequest(ref StringBuilder data, CampaignRequest request) 
		{
			appendBaseJobRequest(ref data, request);
			data.Append ("&CampaignId=" + HttpUtility.UrlEncode (request.CampaignId));
		}
		                               
		private void appendJobRequest(ref StringBuilder data, JobRequest request)
		{
			appendBaseJobRequest(ref data, request);
			data.Append ("&TextTemplate=" + HttpUtility.UrlEncode (request.TextTemplate));
			data.Append ("&FromAddress=" + HttpUtility.UrlEncode (request.FromAddress));
			data.Append ("&FromName=" + HttpUtility.UrlEncode (request.FromName));
			data.Append ("&SubjectLine=" + HttpUtility.UrlEncode (request.SubjectLine));
			data.Append ("&HtmlTemplate=" + HttpUtility.UrlEncode (request.HtmlTemplate));
			data.Append ("&TemplateLanguage=" + HttpUtility.UrlEncode (request.TemplateLanguage.ToString()));
			data.Append ("&CharacterSet=" + HttpUtility.UrlEncode (request.CharacterSet));
			data.Append ("&ReplyToAddress=" + HttpUtility.UrlEncode (request.ReplyToAddress));
			data.Append ("&OnBehalfOfAddress=" + HttpUtility.UrlEncode (request.OnBehalfOfAddress));
			data.Append ("&OnBehalfOfName=" + HttpUtility.UrlEncode (request.OnBehalfOfName));
			data.Append ("&AutoTrack=" + HttpUtility.UrlEncode (request.AutoTrack.ToString()));
			data.Append ("&UrlAppend=" + HttpUtility.UrlEncode (request.UrlAppend));
			data.Append ("&CustomTrackingDomain=" + HttpUtility.UrlEncode (request.CustomTrackingDomain));
			data.Append ("&UnsubscribeHeader=" + HttpUtility.UrlEncode (request.UnsubscribeHeader.ToString()));
			
			String attachmentCount;
			for (int i=0; i < request.attachments.Count; i++)
			{
				attachmentCount = (i+1).ToString("D");
				data.Append ("&AttachmentUrl." + attachmentCount + "=" + HttpUtility.UrlEncode (request.attachments[i].Url));
				data.Append ("&AttachmentName." + attachmentCount + "=" + HttpUtility.UrlEncode (request.attachments[i].DisplayName));
				data.Append ("&AttachmentContentType." + attachmentCount + "=" + HttpUtility.UrlEncode (request.attachments[i].ContentType));
			}

			String headerCount;
			for (int i=0; i < request.headers.Count; i++)
			{
				headerCount = (i+1).ToString("D");
				data.Append ("&HeaderName." + headerCount + "=" + HttpUtility.UrlEncode (request.headers[i].Name));
				data.Append ("&HeaderValue." + headerCount + "=" + HttpUtility.UrlEncode (request.headers[i].Value));
			}
		}
	}
}

