
using System;

namespace MessageGears.Model
{
    /// <summary>
    /// Used to submit the request to run a transactional campaign (single recipient) in the MessageGears service.
    /// </summary>
    public class TransactionalCampaignSubmitRequest : CampaignRequest
    {
        /// <summary>
        /// The Action Code that indicates the API being called to the MessageGears system.
        /// </summary>
        public const string Action = "TransactionalCampaignSubmit";
        
        /// <summary>
        /// The XML structure that contains all of the information needed about an individual recipient.
        /// </summary>
        public string RecipientXml { get; set; }

        /// <summary>
        /// The XML structure that contains non-recipient information needed for the job.
        /// </summary>
        public string ContextDataXml { get; set; }

    }
}