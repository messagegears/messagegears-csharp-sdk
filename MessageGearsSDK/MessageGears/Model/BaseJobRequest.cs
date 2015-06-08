
using System;
using System.Collections.Generic;

namespace MessageGears.Model
{
    /// <summary>
    /// An abstract class used by several request classes that submit various job requests.
    /// </summary>
    public abstract class BaseJobRequest
    {    
        /// <summary>
        /// Used to store a user-defined string.  This should be a unique value for each job submitted.
        /// For bulk jobs, you can retrieve the bulk job summary data by supplying a correlation id to search by.
        /// This can be very useful if a networking error interrupts a bulk job submission and you are not sure if the job was accepted.
        /// </summary>
        public String CorrelationId { get; set; }
        
        /// <summary>
        /// This value is used to send an email message should a Job Error occur.  It can be very useful in testing but should be used cautiously for transactional messages in a
        /// high volume account.
        /// </summary>
        public String NotificationEmailAddress { get; set; }
        
    }
}

