using System;

namespace MessageGears.Model
{
    /// <summary>
    /// Used to submit the request to retrieve the job status in the MessageGears service.
    /// </summary>
    public class JobStateRetrievalRequest
    {

        /// <summary>
        /// The Action Code that indicates the API being called to the MessageGears system.
        /// </summary>
        public const string Action = "JobStatusRetrieval";

        /// <summary>
        /// The request id of the job whose status is to be set.
        /// </summary>
        public string JobRequestId { get; set; }

    }
}
