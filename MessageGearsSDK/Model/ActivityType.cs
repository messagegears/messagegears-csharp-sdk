using System;
namespace MessageGears.Model
{
	/// <summary>
	/// Used to designate the accetable activity type in the MessageGears platform.
	/// </summary>
	public enum ActivityType
	{
		/// <summary>
		/// Bounces.
		/// </summary>
		BOUNCES,
		
		/// <summary>
		/// Clicked Urls.
		/// </summary>
		CLICKS,
		
		/// <summary>
		/// Opened Messages.
		/// </summary>
		OPENS,
		
		/// <summary>
		/// Delivered Messages.
		/// </summary>
		DELIVERIES,
		
		/// <summary>
		/// Render Errors.
		/// </summary>
		RENDER_ERRORS,
		
		/// <summary>
		/// Job Errors.
		/// </summary>
		JOB_ERRORS,
		
		/// <summary>
		/// Spam Complaints (aks FBL or Feedback Loop).
		/// </summary>
		SPAM_COMPLAINTS,
		
		/// <summary>
		/// Unsubscribes.
		/// </summary>
		UNSUBSCRIBES
	}
}

