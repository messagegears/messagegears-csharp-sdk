using System;

namespace MessageGears.Model
{
    /// <summary>
    /// Used to submit the request to retrieve transactional job or campaign content in the MessageGears service.
    /// </summary>
    public class TransactionalContentRetrievalRequest
    {

        /// <summary>
        /// The Action Code that indicates the API being called to the MessageGears system.
        /// </summary>
        public const string Action = "TransactionalContentRetrieval";
        
        /// <summary>
        /// The request id of the job or campaign whose data is to be retrieved.
        /// </summary>
        public string OriginalRequestId { get; set; }

    }
}

