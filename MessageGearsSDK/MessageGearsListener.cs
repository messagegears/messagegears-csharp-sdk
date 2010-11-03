using System;

using MessageGears.Model.Generated;
	
namespace MessageGears
{
	/// <summary>
	/// Interface used to implement listeners used to process events returned from the MessageGears system.
	/// This interface is used to process events from queues as well as events from the AccountActivity downloads.
	/// </summary>
	public interface MessageGearsListener
	{
		/// <summary>
		/// Is triggered when a Click event is retrieved from the event queue for activity file.
		/// </summary>
		/// <param name="activity">
		/// A <see cref="ClickActivity"/>
		/// </param>
		void OnClick(ClickActivity activity); 

		/// <summary>
		/// Is triggered when an Open event is retrieved from the event queue for activity file.
		/// </summary>
		/// <param name="activity">
		/// A <see cref="OpenActivity"/>
		/// </param>
		void OnOpen(OpenActivity activity); 
		
		/// <summary>
		/// Is triggered when a Bounce event is retrieved from the event queue for activity file.
		/// </summary>
		/// <param name="activity">
		/// A <see cref="BouncedMessageActivity"/>
		/// </param>
		void OnBounce(BouncedMessageActivity activity); 

		/// <summary>
		/// Is triggered when a Delivery event is retrieved from the event queue for activity file.
		/// </summary>
		/// <param name="activity">
		/// A <see cref="DeliveredMessageActivity"/>
		/// </param>
		void OnDelivery(DeliveredMessageActivity activity); 
		
		/// <summary>
		/// Is triggered when a Spam Complaint (aka FBL) event is retrieved from the event queue for activity file.
		/// </summary>
		/// <param name="activity">
		/// A <see cref="SpamComplaintActivity"/>
		/// </param>
		void OnSpamComplaint(SpamComplaintActivity activity); 
		
		/// <summary>
		/// Is triggered when a Job Error event is retrieved from the event queue for activity file.
		/// It is important to pay attention to these events as they indicated that a batch or transactional job failed.
		/// This event will tell you which job failed and why.
		/// Job errors can result from missing or malformed recipient XML files or attachment files.
		/// </summary>
		/// <param name="activity">
		/// A <see cref="JobErrorActivity"/>
		/// </param>
		void OnJobError(JobErrorActivity activity); 
		
		/// <summary>
		/// Is triggered when a Render Error event is retrieved from the event queue for activity file.
		/// It is important to process these events as well as these messages indicate the a message was unable to be sent to an individual recipient becuase of a problem with the message content.
		/// These errors are almost always problems with the Freemarker or Velocity commands in your template and often result from references to non-existend recipient data.
		/// </summary>
		/// <param name="activity">
		/// A <see cref="RenderErrorActivity"/>
		/// </param>
		void OnRenderError(RenderErrorActivity activity); 
		
		/// <summary>
		/// This is a placeholder event type for a future feature which will allow MessageGears to notify you if someone clicks an unsubscribe link in a message or in their email reader.
		/// </summary>
		/// <param name="activity">
		/// A <see cref="UnsubActivity"/>
		/// </param>
		void OnUnsub(UnsubActivity activity); 
	}
}

