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
        static readonly string ACTIVITY_DATE_FORMAT = "yyyy-MM-dd";
        static readonly string ACTIVITY_DATE_MONTHLY_FORMAT = "yyyy-MM";
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

        public AccountActivityResponse AccountActivity (AccountActivityRequest request)
        {
            // build POST data 
            StringBuilder data = new StringBuilder ();
            data.Append ("Action=" + HttpUtility.UrlEncode (AccountActivityRequest.Action));
            appendCredentials (ref data);
            data.Append ("&ActivityDate=" + request.Date.ToString(ACTIVITY_DATE_FORMAT));
            data.Append ("&ActivityType=" + HttpUtility.UrlEncode (request.ActivityType.ToString()));

            // invoke endpoint
            string response = invoke (data);
            
            XmlSerializer serializer = new XmlSerializer (typeof(AccountActivityResponse));
            using (XmlTextReader sr = new XmlTextReader (new StringReader (response))) {
                AccountActivityResponse objectResponse = (AccountActivityResponse)serializer.Deserialize (sr);
                if (objectResponse.Result.Equals (Result.REQUEST_SUCCESSFUL)) {
                    log.Info ("AccountActivity successfully processed: " + objectResponse.RequestId);
                } else {
                    log.Error ("AccountActivity failed: " + objectResponse.RequestId);
                }
                return objectResponse;
            }
        }

        public AccountSummaryResponse AccountSummary (AccountSummaryRequest request)
        {
            // build POST data 
            StringBuilder data = new StringBuilder ();
            data.Append ("Action=" + HttpUtility.UrlEncode (AccountSummaryRequest.Action));
            appendCredentials (ref data);
            if (request.IsMonthly) {
                data.Append ("&ActivityDate=" + request.Date.ToString(ACTIVITY_DATE_MONTHLY_FORMAT));
            } else {
                data.Append ("&ActivityDate=" + request.Date.ToString(ACTIVITY_DATE_FORMAT));
            }

            // invoke endpoint
            string response = invoke (data);
            
            XmlSerializer serializer = new XmlSerializer (typeof(AccountSummaryResponse));
            using (XmlTextReader sr = new XmlTextReader (new StringReader (response))) {
                AccountSummaryResponse objectResponse = (AccountSummaryResponse)serializer.Deserialize (sr);
                if (objectResponse.Result.Equals (Result.REQUEST_SUCCESSFUL)) {
                    log.Info ("AccountSummary successfully processed: " + objectResponse.RequestId);
                } else {
                    log.Error ("AccountSummary failed: " + objectResponse.RequestId);
                }
                return objectResponse;
            }
        }

        /// <summary>
        /// Thumbnail the specified request.
        /// </summary>
        /// <param name='request'>
        /// Request.
        /// </param>
        public ThumbnailResponse Thumbnail (ThumbnailRequest request)
        {
            // build POST data 
            StringBuilder data = new StringBuilder ();
            data.Append ("Action=" + HttpUtility.UrlEncode (ThumbnailRequest.Action));
            appendCredentials (ref data);
            data.Append ("&Content=" + HttpUtility.UrlEncode (request.Content));
            data.Append ("&ImageId=" + HttpUtility.UrlEncode (request.ImageId));
            data.Append ("&ImageSize=" + HttpUtility.UrlEncode (request.ThumbnailSize.ToString()));

            // invoke endpoint
            string response = invoke (data);
            
            // deserialize response into CreateAccountResponse
            XmlSerializer serializer = new XmlSerializer (typeof(ThumbnailResponse));
            using (XmlTextReader sr = new XmlTextReader (new StringReader (response))) {
                ThumbnailResponse objectResponse = (ThumbnailResponse)serializer.Deserialize (sr);
                if (objectResponse.Result.Equals (Result.REQUEST_SUCCESSFUL)) {
                    log.Info ("Thumbnail successfully processed: " + objectResponse.RequestId);
                } else {
                    log.Error ("Thumbnail failed: " + objectResponse.RequestId);
                }
                return objectResponse;
            }
        }

        /// <summary>
        /// Used to create a new subaccount
        /// </summary>
        /// <param name="request">
        /// A <see cref="CreateAccountRequest"/>
        /// </param>
        /// <returns>
        /// A <see cref="CreateAccountResponse"/>
        /// </returns>
        public CreateAccountResponse CreateAccount (CreateAccountRequest request)
        {
            // build POST data 
            StringBuilder data = new StringBuilder ();
            data.Append ("Action=" + HttpUtility.UrlEncode (CreateAccountRequest.Action));
            appendCredentials(ref data);
            appendAccountRequest(ref data, request);
            
            // invoke endpoint
            string response = invoke (data);
            
            // deserialize response into CreateAccountResponse
            XmlSerializer serializer = new XmlSerializer (typeof(CreateAccountResponse));
            using (XmlTextReader sr = new XmlTextReader (new StringReader (response))) {
                CreateAccountResponse objectResponse = (CreateAccountResponse)serializer.Deserialize (sr);
                if(objectResponse.Result.Equals(Result.REQUEST_SUCCESSFUL))
                {
                    log.Info("Create account successfully processed: " + objectResponse.RequestId);
                }
                else
                {
                    log.Error("Create account failed: " + objectResponse.RequestId);
                }
                return objectResponse;
            }
        }
        
        /// <summary>
        /// Used to update a subaccount
        /// </summary>
        /// <param name="request">
        /// A <see cref="UpdateAccountRequest"/>
        /// </param>
        /// <returns>
        /// A <see cref="UpdateAccountResponse"/>
        /// </returns>
        public UpdateAccountResponse UpdateAccount (UpdateAccountRequest request)
        {
            // build POST data 
            StringBuilder data = new StringBuilder ();
            data.Append ("Action=" + HttpUtility.UrlEncode (UpdateAccountRequest.Action));
            appendCredentials(ref data);
            appendAccountRequest(ref data, request);
            data.Append ("&Id=" + HttpUtility.UrlEncode (request.Id));

            // invoke endpoint
            string response = invoke (data);
            
            // deserialize response into UpdateAccountResponse
            XmlSerializer serializer = new XmlSerializer (typeof(UpdateAccountResponse));
            using (XmlTextReader sr = new XmlTextReader (new StringReader (response))) {
                UpdateAccountResponse objectResponse = (UpdateAccountResponse)serializer.Deserialize (sr);
                if(objectResponse.Result.Equals(Result.REQUEST_SUCCESSFUL))
                {
                    log.Info("Update account successfully processed: " + objectResponse.RequestId);
                }
                else
                {
                    log.Error("Update account failed: " + objectResponse.RequestId);
                }
                return objectResponse;
            }
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
            data.Append ("&ContextDataXml=" + HttpUtility.UrlEncode (request.ContextDataXml));
            
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
            data.Append ("&ContextDataXml=" + HttpUtility.UrlEncode (request.ContextDataXml));
            
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
            
            if (!String.IsNullOrEmpty (request.SuppressionXmlUrl)) {
                data.Append ("&SuppressionXmlUrl=" + HttpUtility.UrlEncode (request.SuppressionXmlUrl));
            }
            if (!String.IsNullOrEmpty (request.SuppressionCsvUrl)) {
                data.Append ("&SuppressionCsvUrl=" + HttpUtility.UrlEncode (request.SuppressionCsvUrl));
            }
            
            // add optional templates via URL if set
            if (!String.IsNullOrEmpty (request.HtmlTemplateUrl)) {
                data.Append ("&HtmlTemplateUrl=" + HttpUtility.UrlEncode (request.HtmlTemplateUrl));
            }
            if (!String.IsNullOrEmpty (request.TextTemplateUrl)) {
                data.Append ("&TextTemplateUrl=" + HttpUtility.UrlEncode (request.TextTemplateUrl));
            }

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
            data.Append ("&ContextDataXml=" + HttpUtility.UrlEncode (request.ContextDataXml));
            if (!String.IsNullOrEmpty (request.SuppressionXmlUrl)) {
                data.Append ("&SuppressionXmlUrl=" + HttpUtility.UrlEncode (request.SuppressionXmlUrl));
            }
            if (!String.IsNullOrEmpty (request.SuppressionCsvUrl)) {
                data.Append ("&SuppressionCsvUrl=" + HttpUtility.UrlEncode (request.SuppressionCsvUrl));
            }
            appendCredentials(ref data);
            appendCampaignRequest(ref data, request);
            
            // invoke endpoint
            string response = invoke (data);
            
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
        /// Used to set a job to a given state (pause/resume/cancel)
        /// </summary>
        /// <param name="jobStateRequest">
        /// The representation of the request, including the job request id to set the state on, and the state to set
        /// </param>
        /// <returns>
        /// A <see cref="JobStateResponse"/>
        /// </returns>
        public JobStateResponse SetJobState(JobStateRequest jobStateRequest)
        {
            // build POST data 
            StringBuilder data = new StringBuilder();
            data.Append("Action=" + HttpUtility.UrlEncode(JobStateRequest.Action));
            appendCredentials(ref data);
            data.Append("&jobRequestId=" + HttpUtility.UrlEncode(jobStateRequest.JobRequestId));
            data.Append("&status=" + HttpUtility.UrlEncode(jobStateRequest.JobStatus));
            // invoke endpoint
            string response = invoke(data);

            // deserialize response into JobStateResponse
            XmlSerializer serializer = new XmlSerializer(typeof(JobStateResponse));
            using (XmlTextReader sr = new XmlTextReader(new StringReader(response)))
            {
                JobStateResponse objectResponse = (JobStateResponse)serializer.Deserialize(sr);
                if (objectResponse.Result.Equals(Result.REQUEST_SUCCESSFUL))
                {
                    log.Info("Set Job State successfully processed: " + objectResponse.RequestId);
                }
                else
                {
                    log.Error("Set Job State failed: " + objectResponse.RequestId);
                }
                return objectResponse;
            }
        }

        /// <summary>
        /// Used to retrieve the current state of a given job (pause/resume/cancel)
        /// </summary>
        /// <param name="jobStateRetrievalRequest">
        /// The representation of the request, including the job request id to get the state
        /// </param>
        /// <returns>
        /// A <see cref="JobStateResponse"/>
        /// </returns>
        public JobStateResponse RetrieveJobState(JobStateRetrievalRequest jobStateRequest)
        {
            // build POST data 
            StringBuilder data = new StringBuilder();
            data.Append("Action=" + HttpUtility.UrlEncode(JobStateRetrievalRequest.Action));
            appendCredentials(ref data);
            data.Append("&jobRequestId=" + HttpUtility.UrlEncode(jobStateRequest.JobRequestId));
            // invoke endpoint
            string response = invoke(data);

            // deserialize response into JobStateResponse
            XmlSerializer serializer = new XmlSerializer(typeof(JobStateResponse));
            using (XmlTextReader sr = new XmlTextReader(new StringReader(response)))
            {
                JobStateResponse objectResponse = (JobStateResponse)serializer.Deserialize(sr);
                if (objectResponse.Result.Equals(Result.REQUEST_SUCCESSFUL))
                {
                    log.Info("Retrieve Job State successfully processed: " + objectResponse.RequestId);
                }
                else
                {
                    log.Error("Retrieve Job State failed: " + objectResponse.RequestId);
                }
                return objectResponse;
            }
        }

        /// <summary>
        /// Used to return content of a transactional job or campaign
        /// </summary>
        /// <param name="originalRequestId">
        /// The id of the job or campaign to be retrieved.
        /// </param>
        /// <returns>
        /// A <see cref="TransactionalContent"/>
        /// </returns>
        public TransactionalContent TransactionalContentRetrieval(TransactionalContentRetrievalRequest request) 
        {
            // build POST data 
            StringBuilder data = new StringBuilder ();
            data.Append ("Action=" + HttpUtility.UrlEncode ("TransactionalContentRetrieval"));
            appendCredentials(ref data);
            data.Append("&OriginalRequestId=" + HttpUtility.UrlEncode (request.OriginalRequestId));
            
            // invoke endpoint
            string response = invoke (data);
            
            // deserialize response into TransactionalContent
            XmlSerializer serializer = new XmlSerializer (typeof(TransactionalContent));
            using (XmlTextReader sr = new XmlTextReader (new StringReader (response))) {
                TransactionalContent objectResponse = (TransactionalContent)serializer.Deserialize (sr);
                if(objectResponse.Result.Equals(Result.REQUEST_SUCCESSFUL))
                {
                    log.Info("Transactional Content Retrieval successfully processed: " + objectResponse.RequestId);
                }
                else
                {
                    log.Error("Transactional Content Retrieval  failed: " + objectResponse.RequestId);
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
        public BulkJobSummaryResponse BulkJobSummary(BulkJobSummaryRequest bulkJobSummary) 
        {
            // build POST data 
            StringBuilder data = new StringBuilder ();
            data.Append ("Action=" + HttpUtility.UrlEncode ("BulkJobSummary"));
            appendCredentials(ref data);
            data.Append("&BulkJobCorrelationId=" + HttpUtility.UrlEncode (bulkJobSummary.BulkJobCorrelationId));
            data.Append("&BulkJobRequestId=" + HttpUtility.UrlEncode (bulkJobSummary.BulkJobRequestId));
            
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
        /// Used to return a summary of the total activity for a bulk job (total clicks, bounces, etc.)
        /// queried by correlation id.
        /// </summary>
        /// <param name="bulkJobCorrelationId">
        /// The correlation id of the bulk job.
        /// </param>
        /// <returns>
        /// A <see cref="BulkJobSummaryResponse"/>
        /// </returns>
        public BulkJobSummaryResponse BulkJobSummaryByCorrelationId(String bulkJobCorrelationId) 
        {
            // build POST data 
            StringBuilder data = new StringBuilder ();
            data.Append ("Action=" + HttpUtility.UrlEncode ("BulkJobSummary"));
            appendCredentials(ref data);
            data.Append("&BulkJobCorrelationId=" + HttpUtility.UrlEncode (bulkJobCorrelationId));
            
            // invoke endpoint
            string response = invoke (data);
            
            // deserialize response into BulkJobSummaryResponse
            XmlSerializer serializer = new XmlSerializer (typeof(BulkJobSummaryResponse));
            using (XmlTextReader sr = new XmlTextReader (new StringReader (response))) {
                BulkJobSummaryResponse objectResponse = (BulkJobSummaryResponse)serializer.Deserialize (sr);
                if(objectResponse.Result.Equals(Result.REQUEST_SUCCESSFUL))
                {
                    log.Info("Bulk Job Summary by correlation id successfully processed: " + objectResponse.RequestId);
                }
                else
                {
                    log.Error("Bulk Job Summary by correlation id failed: " + objectResponse.RequestId);
                }
                return objectResponse;
            }
        }
        /// <summary>
        /// Utility function to print response data to the console.
        /// </summary>
        /// <param name="response">
        /// The ThumbnailResponse to be printed.<see cref="ThumbnailResponse"/>
        /// </param>
        public void PrintResponse(ThumbnailResponse response) {
            PrintResponse(response.Result, response.RequestErrors);
            if(response.Result.Equals(Result.REQUEST_SUCCESSFUL)) {
                Console.WriteLine("Account Id: " + response.imageUrl);
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
        /// The CreateAccountResponse to be printed.<see cref="CreateAccountResponse"/>
        /// </param>
        public void PrintResponse(CreateAccountResponse response) {
            PrintResponse(response.Result, response.RequestErrors);
            if(response.Result.Equals(Result.REQUEST_SUCCESSFUL)) {
                Console.WriteLine("Account Id: " + response.Account.Id);
                Console.WriteLine("Account API Key: " + response.Account.ApiKey);
            }
        }

        /// <summary>
        /// Utility function to print response data to the console.
        /// </summary>
        /// <param name="response">
        /// The UpdateAccountResponse to be printed.<see cref="UpdateAccountResponse"/>
        /// </param>
        public void PrintResponse(UpdateAccountResponse response) {
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
        /// The TransactionalContent to be printed.<see cref="TransactionalContent"/>
        /// </param>
        public void PrintResponse(TransactionalContent response) {
            PrintResponse(response.Result, response.RequestErrors);
            if (response.Result == Result.REQUEST_SUCCESSFUL) {                
                if (response.RenderErrors == null) {
                    Console.WriteLine("FromName: " + response.FromName +
                                      " <" + response.FromAddress + ">");
                    Console.WriteLine("Subject: " + response.SubjectLine);
                    Console.WriteLine("HtmlContent: " + response.HtmlContent);
                    Console.WriteLine("TextContent: " + response.TextContent);
                    Console.WriteLine("Attachments:");
                    foreach(MessageGears.Model.Generated.Attachment attachment in response.Attachments) {
                        Console.WriteLine("            Name: " + attachment.Name);
                        Console.WriteLine("    Content-Type: " + attachment.ContentType);
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
            DateTime now = DateTime.Now;
            // validate that activityDate is not in the future
            if(activityDate > now) 
            {
                log.Info("Activity Date cannot be in the future. ActivityDate: " + activityDate + " Date Now: " + now);
                throw new MessageGearsClientException("Parmameter: ActivityDate. Value: " + activityDate + " Error Message: This field cannot be a date in the future.");
            }

            DateTime start = now;
            String fileName = properties.DownloadDirectory + activityDate.Year + "-" + activityDate.Month + "-" + activityDate.Day + "_" + activityType + "_" + properties.MyMessageGearsAccountId + ".xml";
            // build POST data 
            StringBuilder data = new StringBuilder ();
            data.Append ("Action=" + HttpUtility.UrlEncode ("AccountActivity"));
            appendCredentials(ref data);
            data.Append("&ActivityDate=" + HttpUtility.UrlEncode (activityDate.Year + "-" + activityDate.Month + "-" + activityDate.Day));
            data.Append ("&ActivityType=" + HttpUtility.UrlEncode (activityType.ToString()));
            
            // invoke endpoint - either writing the contents to a temporary file or throwing exception if error are encountered
            invokeAccountActivity(data, fileName);
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
        
        private void invokeAccountActivity (StringBuilder data, String fileName)
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
                string contentDisposition = response.GetResponseHeader("Content-Disposition");
                log.Debug("AccountActivityReponse header Content-Disposition: " + contentDisposition);

                // check for Content-Disposition header - if not set assume file was not returned
                if(contentDisposition.Length == 0) 
                {
                    log.Info("Content-Disposition header not set. Assuming AccountActivity file not returned.");
                      StreamReader reader = new StreamReader (response.GetResponseStream ());
                    // read the entire response as a string since it should be an XML response in the absence of the Content-Disposition header
                    string responseText = reader.ReadToEnd ();

                    // deserialize response into AccountActivityResponse
                    XmlSerializer serializer = new XmlSerializer (typeof(AccountActivityResponse));
                    using (XmlTextReader sr = new XmlTextReader (new StringReader (responseText))) {
                        AccountActivityResponse objectResponse = (AccountActivityResponse)serializer.Deserialize (sr);

                        // append errors and throw exception with errors as the message
                        StringBuilder builder = new StringBuilder();
                        foreach(RequestError error in objectResponse.RequestErrors)
                        {
                            builder.Append(error.ErrorMessage);
                        }
                        throw new MessageGearsClientException(builder.ToString());
                    }
                        
                }

                // otherwise, Get the response stream  
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
        
        private void appendAccountRequest(ref StringBuilder data, AccountRequest request) 
        {
            data.Append ("&Name=" + HttpUtility.UrlEncode (request.Name));
            data.Append ("&AutoTrack=" + HttpUtility.UrlEncode (request.AutoTrack.ToString()));
            data.Append ("&CustomTrackingDomain=" + HttpUtility.UrlEncode (request.CustomTrackingDomain));
            data.Append ("&UrlAppend=" + HttpUtility.UrlEncode (request.UrlAppend));
        }

        private void appendCampaignRequest(ref StringBuilder data, CampaignRequest request) 
        {
            appendBaseJobRequest(ref data, request);
            data.Append ("&CampaignId=" + HttpUtility.UrlEncode (request.CampaignId));
        }
                                       
        private void appendJobRequest(ref StringBuilder data, JobRequest request)
        {
            appendBaseJobRequest (ref data, request);

            // optionallly set text and html template if supplied. otherwise, assume they are supplied as urls instead of inline
            if (!String.IsNullOrEmpty (request.TextTemplate)) {
                data.Append ("&TextTemplate=" + HttpUtility.UrlEncode (request.TextTemplate));
            }
            if (!String.IsNullOrEmpty (request.HtmlTemplate)) {
                data.Append ("&HtmlTemplate=" + HttpUtility.UrlEncode (request.HtmlTemplate));
            }

            // if CharacterSet not specified then default to UTF-8
            if (!String.IsNullOrEmpty (request.CharacterSet)) {
                data.Append ("&CharacterSet=" + HttpUtility.UrlEncode (request.CharacterSet));
            } else {
                data.Append ("&CharacterSet=UTF-8");
            }

            data.Append ("&FromAddress=" + HttpUtility.UrlEncode (request.FromAddress));
            data.Append ("&FromName=" + HttpUtility.UrlEncode (request.FromName));
            data.Append ("&SubjectLine=" + HttpUtility.UrlEncode (request.SubjectLine));
            data.Append ("&TemplateLanguage=" + HttpUtility.UrlEncode (request.TemplateLanguage.ToString()));
            data.Append ("&ReplyToAddress=" + HttpUtility.UrlEncode (request.ReplyToAddress));
            data.Append ("&OnBehalfOfAddress=" + HttpUtility.UrlEncode (request.OnBehalfOfAddress));
            data.Append ("&OnBehalfOfName=" + HttpUtility.UrlEncode (request.OnBehalfOfName));
            data.Append ("&AutoTrack=" + HttpUtility.UrlEncode (request.AutoTrack.ToString()));
            data.Append ("&UrlAppend=" + HttpUtility.UrlEncode (request.UrlAppend));
            data.Append ("&CustomTrackingDomain=" + HttpUtility.UrlEncode (request.CustomTrackingDomain));
            data.Append ("&UnsubscribeHeader=" + HttpUtility.UrlEncode (request.UnsubscribeHeader.ToString()));
            data.Append ("&TemplateLibrary=" + HttpUtility.UrlEncode (request.TemplateLibrary));
            data.Append ("&JobCategory=" + HttpUtility.UrlEncode (request.JobCategory));
            
            String attachmentCount; 
            for (int i=0; i < request.attachments.Count; i++)
            {
                attachmentCount = (i+1).ToString("D");
                data.Append ("&AttachmentContent." + attachmentCount + "=" + HttpUtility.UrlEncode(request.attachments[i].Content));
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

