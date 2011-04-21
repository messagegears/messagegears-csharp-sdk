using System;
using System.Collections.Generic;

namespace MessageGears.Model
{
	/// <summary>
	/// An abstract class used by several request classes that submit various job requests.
	/// </summary>
	public abstract class JobRequest : BaseJobRequest
	{
		private TemplateLanguage _templateLanguage = TemplateLanguage.FREEMARKER;
		
		/// <summary>
		/// The value place in the "from address" email header.
		/// This value can be dynamically generated using the seleced template language.
		/// </summary>
		public String FromAddress { get; set; }
		
		/// <summary>
		/// The optional value place in the "from address" email header and is often called the pretty from address.
		/// It can may your emails much more visually appealing by allowing your to place a name along with an email address to make your messages more readable.
		/// This value can be dynamically generated using the seleced template language.
		/// </summary>
		public String FromName { get; set; }
		
		/// <summary>
		/// The value to be placed in the subject line header.
		/// This value can be dynamically generated using the seleced template language.
		/// </summary>
		public String SubjectLine { get; set; }

		/// <summary>
		/// The content to be place in the html part of the message.  This value is optional, but if not provided you must supply a Text Template.
		/// If both the HTML and Text template are provided, a multi-part message will be sent to the recipients of the message containing both forms of content.
		/// This value can be dynamically generated using the seleced template language.
		/// </summary>
		public String HtmlTemplate { get; set; }
		
		/// <summary>
		/// The content to be place in the html part of the message.  This value is optional, but if not provided you must supply a Text Template.
		/// If both the HTML and Text template are provided, a multi-part message will be sent to the recipients of the message containing both forms of content.
		/// This value can be dynamically generated using the seleced template language.
		/// </summary>
		public String TextTemplate { get; set; }

		/// <summary>
		/// Defines the template language to be used to render any dynamic portions of the message headers or content.
		/// The default value is FREEMARKER.
		/// </summary>
		public TemplateLanguage TemplateLanguage {
			get { return this._templateLanguage; }
			set { _templateLanguage = value; }
		}
		
		/// <summary>
		/// The character set to be used for the message headers and content.  If no value is specified, UTF8 is used.
		/// </summary>
		public String CharacterSet { get; set; }
			
		/// <summary>
		/// Used to set the reply-to address header.  This address is often set to "do_not_reply@mycompany.com" or "sales@mycompany.com".
		/// The MessageGears service does not act on messages sent to it as a result of a reply-to and customers should use their own email domain for this value.
		/// </summary>
		public String ReplyToAddress { get; set; }
		
		/// <summary>
		/// A sometimes used field which has special meaning to some email readers.  It can be useful if your are sending an email job on behalf of another company and need to show 
		/// both companies name and address in the from headers.  See the MessageGears API documention for examples and screenshots of how this header can be used.
		/// </summary>
		public String OnBehalfOfAddress { get; set; }

		/// <summary>
		/// Used in conjunction with the OnBehalfOfAddress field and privides the "pretty from address".
		/// </summary>
		public String OnBehalfOfName { get; set; }

		/// <summary>
		/// A list of up to 5 attachments to send with the job's email messages.
		/// </summary>
		public List<Attachment> attachments = new List<Attachment> ();
		
		/// <summary>
		/// If set to “true” (case insensitive) all links in the HTML content will be made trackable.
		/// Otherwise, they will not.  If true, any link/href inside an anchor “<a>” tag, or image map “<map>” tag will be marked as trackable.
		/// If the tag specifies a “name” attribute, the name will be set as the link name in your activity data.
		/// </summary>
		public Boolean AutoTrack { get; set; }
		
		/// <summary>
		/// Used to specify a string to be appended to each trackable link in your HTML content.
		/// This parameter will only be accepted if the AutoTrack option above is set to “true”.
		/// It can be helpful when used in conjunction with a web analytics system such as Google Analytics to add your campaign Id and other data to each of your links.
		/// </summary>
		public String UrlAppend { get; set; }
		
		/// <summary>
		/// Used to provide a custom domain name to be used for trackable links and the open tracking URL.
		/// You must set a CNAME in your DNS that points to www.messagegears.net.  Please test this carefully before using.
		/// </summary>
		public String CustomTrackingDomain { get; set; }
		
		/// <summary>
		/// If set to "true", the "List-Unsubscribe" header will be unserted into the messages sent for a job.  This header has special meaning to some ISPs (like GMail).
		/// When these headers are present, some Email Readers will present an "Unsubscribe" button to the email recipient.  This can provide a better option for the recipient and 
		/// they may press "unsubscribe" instead of "spam" when they negatively to your message.
		/// </summary>
		public Boolean UnsubscribeHeader { get; set; }
	
		/// <summary>
		/// A list of up to 5 headers that will be set for each message in the job.
		/// </summary>
		public List<Header> headers = new List<Header> ();
		
	}
}

