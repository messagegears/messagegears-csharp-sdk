using System;
namespace MessageGears.Model
{
	/// <summary>
	/// API used to update a subaccount in the MessageGears system.
	/// </summary>
	public class UpdateSubaccountRequest : SubaccountRequest
	{
		/// <summary>
		/// The Action Code to be used for this API
		/// </summary>
		public const string Action = "UpdateSubaccount";
		
		/// <summary>
		/// The Subaccount Id
		/// </summary>
		public long Id { get; set; }
	}
}

