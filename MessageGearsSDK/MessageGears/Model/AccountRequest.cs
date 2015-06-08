using System;

namespace MessageGears.Model
{
    public abstract class AccountRequest
    {
        /// <summary>
        /// The name to be assigned to the subaccount
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The string to be appended to each trackable url.
        /// </summary>
        public string UrlAppend { get; set; }

        /// <summary>
        /// Indicates if links should automatically be made trackable
        /// </summary>
        public bool AutoTrack { get; set; }

        /// <summary>
        /// The base URL to be used for trackable urls, open beacons, unsub links, etc.
        /// </summary>
        public string CustomTrackingDomain { get; set; }

    }
}

