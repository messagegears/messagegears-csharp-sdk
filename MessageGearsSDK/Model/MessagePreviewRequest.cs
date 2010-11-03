using System;

namespace MessageGears.Model
{
	/// <summary>
	/// Used to submit the request to run a message preview api in the MessageGears service.
	/// </summary>
	public class MessagePreviewRequest : JobRequest
	{
		/// <summary>
		/// The Action Code that indicates the API being called to the MessageGears system.
		/// </summary>
		public const string Action = "MessagePreview";
		
		/// <summary>
		/// The XML structure that contains all of the information needed about an individual recipient.
		/// </summary>
		public string RecipientXml { get; set; }

	}
}

