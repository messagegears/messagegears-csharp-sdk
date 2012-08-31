using System;
namespace MessageGears
{
	/// <summary>
	/// Class used to store all of the properties and credentials needed to interface with MessageGears and Amazon Web Services.
	/// </summary>
	public class MessageGearsProperties
	{
		/// <summary>
		/// The base URL for the MessageGears service.
		/// </summary>
		public String MessageGearsEndPoint { get; set; }
		
		/// <summary>
		/// Your MessageGears Account Id.
		/// </summary>
		public String MyMessageGearsAccountId { get; set; }
		
		/// <summary>
		/// Your MessageGears API Key.
		/// </summary>
		public String MyMessageGearsApiKey { get; set; }
		
		/// <summary>
		/// The directory in which activity file downloaded from MessageGears will be stored.
		/// </summary>
		public String DownloadDirectory  { get; set; }
		
		/// <summary>
		/// Public constructor setting default values for the MessageGearsProperties class.
		/// </summary>
		public MessageGearsProperties () {
			MessageGearsEndPoint="https://api.messagegears.net/3.1/WebService";
			DownloadDirectory=".";
		}

		/// <summary>
		/// Dumps out all of the properties.
		/// </summary>
		/// <returns>
		/// A string containing a list of all the properties and their associated values.
		/// </returns>
		public override String ToString()
		{
			String dump = "MessageGearsEndPoint=" + MessageGearsEndPoint;
			dump = dump + " MyMessageGearsAccountId=" + MyMessageGearsAccountId;
			dump = dump + " MyMessageGearsApiKey=" + "<hidden>";
			dump = dump + " DownloadDirectory=" + DownloadDirectory;
			return dump;
		}
	}
}

