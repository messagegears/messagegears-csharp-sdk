using System;

namespace MessageGears.Model
{
	/// <summary>
	/// Used to create a subaccount
	/// </summary>
	public class CreateAccountRequest : AccountRequest
	{
		
		/// <summary>
		/// The Action Code that indicates the API being called to the MessageGears system.
		/// </summary>
		public const string Action = "CreateAccount";

	}
}

