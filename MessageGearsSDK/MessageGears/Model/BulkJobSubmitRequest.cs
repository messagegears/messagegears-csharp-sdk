using System;

namespace MessageGears.Model
{
	/// <summary>
	/// Used to submit the request to run a bulk job (one or millions of emails) in the MessageGears service.
	/// </summary>
	public class BulkJobSubmitRequest : JobRequest
	{
		/// <summary>
		/// The Action Code that indicates the API being called to the MessageGears system.
		/// </summary>
		public const string Action = "BulkJobSubmit";
		
		/// <summary>
		/// The URL to a file containing the XML recipient data.
		/// This file may be compressed using the ZIP algorythm (if so, the file name must have a .zip extension).
		/// The URL may be HTTP or HTTPS and can also specify basic authentication credentials.  It may also be a URL to a message stored in Amazon S3.
		/// See the MessageGears online API docs for more information.
		/// </summary>
		public string RecipientListXmlUrl { get; set; }

		/// <summary>
		/// Gets or sets the html template URL.
		/// </summary>
		/// <value>
		/// The html template URL.
		/// </value>
		public string HtmlTemplateUrl { get; set; }

		/// <summary>
		/// Gets or sets the text template URL.
		/// </summary>
		/// <value>
		/// The text template URL.
		/// </value>
		public string TextTemplateUrl { get; set; }

		/// <summary>
		/// The optional XML data made available to the template code.
		/// It can used used to store data that is not specific to a recipient such as weekly specials, sales reps, etc.
		/// When the template is being rendered, this data is available to the code to apply advanced personalization.
		/// </summary>
		public string ContextDataXml { get; set; }
		
	}
}

