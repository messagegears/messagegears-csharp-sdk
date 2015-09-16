using System;

namespace MessageGears.Model
{
    /// <summary>
    /// Used to submit the request to set job status in the MessageGears service.
    /// </summary>
    public class JobStateRequest
    {

        /// <summary>
        /// The Action Code that indicates the API being called to the MessageGears system.
        /// </summary>
        public const string Action = "JobStatus";

        /// <summary>
        /// The request id of the job whose status is to be set.
        /// </summary>
        public string JobRequestId { get; set; }

        /// <summary>
        /// The numeric representation of the state the job is to be set to.
        /// </summary>
        public string JobStatus { get; set; }


    }
}


