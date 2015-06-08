using System;

namespace MessageGears.Model
{
    public class AccountActivityRequest
    {
        /// <summary>
        /// The Action Code that indicates the API being called to the MessageGears system.
        /// </summary>
        public const string Action = "AccountActivity";
        
        /// <summary>
        /// The activity date.
        /// </summary>
        public DateTime Date { get; set; }
        
        /// <summary>
        /// The activity type (e.g. bounces, clicks, opens, etc.) to retrieve.
        /// </summary>
        public ActivityType ActivityType { get; set; }

    }
}

