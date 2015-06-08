using System;

namespace MessageGears.Model
{
    /// <summary>
    /// Used to submit the request to run a message preview api in the MessageGears service.
    /// </summary>
    public class MessagePreviewRequest : JobRequest
    {
        /// <summary>
        /// The Action Code that indicates the API being called to the MessageGears system.
        /// </summary>
        public const string Action = "MessagePreview";
        
        /// <summary>
        /// The XML structure that contains all of the information needed about an individual recipient.
        /// </summary>
        public string RecipientXml { get; set; }

        /// <summary>
        /// The optional XML data made available to the template code.
        /// It can used used to store data that is not specific to a recipient such as weekly specials, sales reps, etc.
        /// When the template is being rendered, this data is available to the code to apply advanced personalization.
        /// </summary>
        public string ContextDataXml { get; set; }
    }
}

