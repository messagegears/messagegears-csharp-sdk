using System;

namespace MessageGears.Model
{
	/// <summary>
	/// Used to submit the request to run a bulk job (one or millions of emails) in the MessageGears service.
	/// </summary>
	public class BulkJobSummaryRequest
	{

		/// <summary>
		/// The Action Code that indicates the API being called to the MessageGears system.
		/// </summary>
		public const string Action = "BulkJobSummary";
		
		/// <summary>
		/// The request id of the bulk job whose data is to be retrieved.
		/// </summary>
		public string BulkJobRequestId { get; set; }
		
		/// <summary>
		/// The correlation id of the bulk job whose data is to be retrieved.
		/// This is useful in error handling logic when a bulk  job request is submitted, but a response is never received.
		/// You can use this method to retrieve a bulk job summary by the correlation id that was supplied in the bulk job submit request.
		/// Your retry logic should including waiting for a perioid of time (maybe 5 mins), then query MessageGears using this method to see if the job exists.
		/// If so, there is no need to resubmit the job.  Otherwise, resubmit the job for processing.</br>
		/// The correlation ids you specify when submitting a job must be unique.  If not, you will receive an error when attempting to retrieve them by correlation id.
		/// </summary>
		public string BulkJobCorrelationId { get; set; }

	}
}

