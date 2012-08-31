using System;

namespace MessageGears.Model
{
	/// <summary>
	/// Class that stores information about a file attachment to an email message.
	/// </summary>
	public class Attachment
	{
		/// <summary>
		/// The URL where the file can be accessed by the MessaegGears service.
		/// The URL may be HTTP or HTTPS and can also specify basic authentication credentials.  It may also be a URL to a message stored in Amazon S3.
		/// </summary>
		public String Url { get; set; }
		
		/// <summary>
		/// The name of the attachment as it will be displayed in the email message content.
		/// </summary>
		public String DisplayName { get; set; }
		
		/// <summary>
		/// The content type of the attachment.
		/// </summary>
		public String ContentType { get; set; }
	}
}

