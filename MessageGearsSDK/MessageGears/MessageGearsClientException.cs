using System;

namespace MessageGears
{
    /// <summary>
    /// Message gears client exception.
    /// </summary>
    public class MessageGearsClientException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageGearsSDK.MessageGearsClientException"/> class.
        /// </summary>
        public MessageGearsClientException ()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageGearsSDK.MessageGearsClientException"/> class.
        /// </summary>
        /// <param name='message'>
        /// Message.
        /// </param>
        public MessageGearsClientException (string message): base(message)
        {
        }
    }
}

