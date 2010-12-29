using System;
namespace MessageGears.Model
{
	/// <summary>
	/// An abstract class used to represent a subaccount
	/// </summary>
	public abstract class SubaccountRequest
	{
		/// <summary>
		/// The user-supplied name of the subaccount
		/// </summary>
		public String Name { get; set; }
		
		/// <summary>
		/// The "URL Append" value to be used by default for all job submission requests for this subaccount
		/// </summary>
		public String UrlAppend { get; set; }
		
		/// <summary>
		/// The "Auto Track" value to be used by default for all job submission requests for this subaccount
		/// </summary>
		public Boolean AutoTrack { get; set; }
		
		/// <summary>
		/// The "Custom Tracking Domain" value to be used by default for all job submission requests for this subaccount
		/// </summary>
		public String CustomTrackingDomain { get; set; }
	}
}

