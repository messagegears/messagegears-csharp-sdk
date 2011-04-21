using System;

namespace MessageGears.Model
{
	/// <summary>
	/// Used to submit the request to run a bulk campaign (one or millions of emails) in the MessageGears service.
	/// </summary>
	public class BulkCampaignSubmitRequest : CampaignRequest
	{
		/// <summary>
		/// The Action Code that indicates the API being called to the MessageGears system.
		/// </summary>
		public const string Action = "BulkCampaignSubmit";
		
		/// <summary>
		/// The URL to a file containing the XML recipient data.
		/// This file may be compressed using the ZIP algorythm (if so, the file name must have a .zip extension).
		/// The URL may be HTTP or HTTPS and can also specify basic authentication credentials.  It may also be a URL to a message stored in Amazon S3.
		/// See the MessageGears online API docs for more information.
		/// </summary>
		public string RecipientListXmlUrl { get; set; }
	}
}


