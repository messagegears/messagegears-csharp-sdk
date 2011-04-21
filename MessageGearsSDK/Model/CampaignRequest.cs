
using System;
using System.Collections.Generic;

namespace MessageGears.Model
{
	/// <summary>
	/// An abstract class used by several request classes that submit various job requests.
	/// </summary>
	public abstract class CampaignRequest : BaseJobRequest
	{
		/// <summary>
		/// The Id of the Campaign to be used to construct a message. 
		/// </summary>
		public String CampaignId { get; set; }
	}
}