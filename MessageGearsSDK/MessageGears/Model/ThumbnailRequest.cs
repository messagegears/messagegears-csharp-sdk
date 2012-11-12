using System;
using MessageGears.Model;

namespace MessageGears.Model
{
	/// <summary>
	/// Thumbnail request.
	/// </summary>
	public class ThumbnailRequest
	{
		/// <summary>
		/// The Action Code that indicates the API being called to the MessageGears system.
		/// </summary>
		public const string Action = "Thumbnail";

		/// <summary>
		/// Gets or sets the content.
		/// </summary>
		/// <value>
		/// The content.
		/// </value>
		public string Content { get; set; }

		/// <summary>
		/// Gets or sets the image identifier.
		/// </summary>
		/// <value>
		/// The image identifier.
		/// </value>
		public string ImageId { get; set; }

		/// <summary>
		/// Gets or sets the size of the image.
		/// </summary>
		/// <value>
		/// The size of the image.
		/// </value>
		public ThumbnailSize ThumbnailSize { get; set; }

	}
}

