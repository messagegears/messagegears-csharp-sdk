using System;

namespace MessageGears.Model
{
	/// <summary>
	/// Used to create a subaccount
	/// </summary>
	public class UpdateAccountRequest : AccountRequest
	{
		
		/// <summary>
		/// The Action Code that indicates the API being called to the MessageGears system.
		/// </summary>
		public const string Action = "UpdateAccount";
		
		/// <summary>
		/// The numeric Id of the subaccount to be updated.
		/// </summary>
		public string Id { get; set; }

	}
}

