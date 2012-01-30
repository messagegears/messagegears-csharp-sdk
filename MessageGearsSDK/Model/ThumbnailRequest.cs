using System;

namespace MessageGears.Model
{
	/// <summary>
	/// Used to generate a Thumbnail Image
	/// </summary>
	public class ThumbnailRequest
	{
		
		/// <summary>
		/// The Action Code that indicates the API being called to the MessageGears system.
		/// </summary>
		public const string Action = "Thumbnail";
		
		/// <summary>
		/// The ID to be used to name the image file <ImageId>.jpg
		/// </summary>
		public string ImageId { get; set; }
		
		/// <summary>
		/// The size of the image to be generated
		/// </summary>
		public ThumbnailSize ThumbnailSize { get; set; }

		/// <summary>
		/// The text/html to be used to render the image
		/// </summary>
		public string Content { get; set; }
	}
}

