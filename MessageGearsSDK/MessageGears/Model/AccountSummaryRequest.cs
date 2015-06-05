using System;

namespace MessageGears.Model
{
    public class AccountSummaryRequest
    {
    
        public bool _isMonthly = false;
    
		/// <summary>
		/// The Action Code that indicates the API being called to the MessageGears system.
		/// </summary>
		public const string Action = "AccountSummary";
		
        /// <summary>
        /// The activity date.
        /// </summary>
		public DateTime Date { get; set; }

        /// <summary>
        /// Indicates whether the activity should be summarized by month or by day.
        /// </summary>
        public Boolean IsMonthly {
            get { return this._isMonthly;}
            set { this._isMonthly = value; }
        }

	}
}

