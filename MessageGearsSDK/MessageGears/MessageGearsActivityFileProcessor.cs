using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using log4net;

using MessageGears.Model.Generated;

namespace MessageGears
{
    /// <summary>
    /// This class is used to process the daily account activity files from MessageGears.
    /// This method is often used in place of the real-time event feed if receiving your activity data in real-time is not a requirement for your business.
    /// </summary>
    public class MessageGearsActivityFileProcessor
    {
        static readonly ILog log = LogManager.GetLogger(typeof(MessageGearsActivityFileProcessor));
        private const String ACCOUNT_ACTIVITY_ELEMENT = "AccountActivityResponse";
        private const String CLICK_ACTIVITY_ELEMENT = "ClickActivity";
        private const String OPEN_ACTIVITY_ELEMENT = "OpenActivity";
        private const String BOUNCE_ACTIVITY_ELEMENT = "BouncedMessageActivity";
        private const String DELIVERED_ACTIVITY_ELEMENT = "DeliveredMessageActivity";
        private const String SPAM_ACTIVITY_ELEMENT = "SpamComplaintActivity";
        private const String JOB_ERROR_ACTIVITY_ELEMENT = "JobErrorActivity";
        private const String RENDER_ERROR_ACTIVITY_ELEMENT = "RenderErrorActivity";
        private const String UNSUB_ACTIVITY_ELEMENT = "UnsubActivity";
        private int clickCount=0;
        private int openCount=0;
        private int bounceCount=0;
        private int spamComplaintCount=0;
        private int deliveryCount=0;
        private int renderErrorCount=0;
        private int jobErrorCount=0;
        private int unsubCount=0;
        private MessageGearsListener listener;
        
        /// <summary>
        /// Use to construct an instance of the ActivityFileProcessor class.
        /// </summary>
        /// <param name="listener">
        /// Your implementation of the MessageGearsListener interface that know what to do with each event type that can come back from the MessageGears system.<see cref="MessageGearsListener"/>
        /// </param>
        public MessageGearsActivityFileProcessor(MessageGearsListener listener)
        {
            this.listener = listener;
        }
        
        /// <summary>
        /// Starts the parsing of the activity XML file retrieved from MessageGears.  Once invoked, the methods in the listener instance will be fired for each item in the activity file.
        /// </summary>
        /// <param name="filename">
        /// The fully qualified name of the file to be processed.
        /// </param>
        public void process(String filename)
        {
            log.Debug("Starting Account Activity File Processor.");
            clickCount=0;
            openCount=0;
            bounceCount=0;
            spamComplaintCount=0;
            deliveryCount=0;
            renderErrorCount=0;
            jobErrorCount=0;
            unsubCount=0;
            StreamReader stream = new StreamReader (filename);
            XmlTextReader reader = null;    
            reader = new XmlTextReader (stream);
            String xml = null;
            String startElementType = null;
            String endElementType = null;
            String nameSpace = null;
            DateTime start = DateTime.Now;
            while (reader.Read()) 
            {
                switch (reader.NodeType) 
                {
                case XmlNodeType.Element:
                    
                    startElementType = reader.Name;
                    
                    if(startElementType.Equals(ACCOUNT_ACTIVITY_ELEMENT))
                    {
                        reader.MoveToNextAttribute();
                        nameSpace = reader.Value;
                        log.Debug("Using namespace: " + nameSpace);
                    }

                    if(isActivityElement(startElementType))
                    {
                        // Get the namespace from the root element so it can be used to deserialize subelements.  It is required by the deserializer.
                        xml = "<" + startElementType + " xmlns=\"" + nameSpace + "\">";
                    }
                    else
                    {
                        xml = xml + "<" + startElementType + ">";
                    }
                    break;
                case XmlNodeType.Text:
                    xml = xml + System.Security.SecurityElement.Escape(reader.Value);
                    break;
                case XmlNodeType. EndElement:
                    endElementType = reader.Name;
                    xml = xml + "</" + endElementType + ">";
                    if(isActivityElement(endElementType))
                       {
                        log.Debug("An event was found: " + xml);
                        processElement(endElementType, xml);
                    }
                    break;
                }
            }
            reader.Close();
            stream.Close();
            DateTime finish = DateTime.Now;
            TimeSpan ts = finish - start;
            string elapsedTime = string.Format ("{0:00}:{1:00}:{2:00}", (int)ts.TotalHours, ts.Minutes, ts.Seconds);
            log.Info("Account Activity File Processor finished in " + elapsedTime + " (total/o/c/b/d/je/re/sc/u): " +
                     (openCount + clickCount + bounceCount + deliveryCount + jobErrorCount + renderErrorCount + spamComplaintCount + unsubCount) + "/" +
                     openCount + "/" +
                     clickCount + "/" +
                     bounceCount + "/" +
                     deliveryCount + "/" +
                     jobErrorCount + "/" +
                     renderErrorCount + "/" +
                     spamComplaintCount + "/" +
                     unsubCount);
        }
        
        private bool isActivityElement(String elementName)
        {
            bool isActivityElement = false;
            switch(elementName)
            {
            case CLICK_ACTIVITY_ELEMENT:
                isActivityElement = true;
                break;
            case OPEN_ACTIVITY_ELEMENT:
                isActivityElement = true;
                break;
            case BOUNCE_ACTIVITY_ELEMENT:
                isActivityElement = true;
                break;
            case DELIVERED_ACTIVITY_ELEMENT:
                isActivityElement = true;
                break;
            case SPAM_ACTIVITY_ELEMENT:
                isActivityElement = true;
                break;
            case JOB_ERROR_ACTIVITY_ELEMENT:
                isActivityElement = true;
                break;
            case RENDER_ERROR_ACTIVITY_ELEMENT:
                isActivityElement = true;
                break;
            case UNSUB_ACTIVITY_ELEMENT:
                isActivityElement = true;
                break;
            default:
                break;
            }
            return isActivityElement;
        }

        private void processElement(String elementType, String xml)
        {
            XmlSerializer serializer;
            switch(elementType)
            {
            case CLICK_ACTIVITY_ELEMENT:
                serializer = new XmlSerializer (typeof(ClickActivity));
                ClickActivity clickActivity = null;
                using (XmlTextReader sr = new XmlTextReader (new StringReader (xml))) {
                    clickActivity = (ClickActivity)serializer.Deserialize (sr);
                    log.Debug("A click event was found for job: " + clickActivity.RequestId);
                }
                listener.OnClick(clickActivity);
                clickCount++;
                break;
            case OPEN_ACTIVITY_ELEMENT:
                serializer = new XmlSerializer (typeof(OpenActivity));
                OpenActivity openActivity = null;
                using (XmlTextReader sr = new XmlTextReader (new StringReader (xml))) {
                    openActivity = (OpenActivity)serializer.Deserialize (sr);
                    log.Debug("An open event was found for job: " + openActivity.RequestId);
                }
                listener.OnOpen(openActivity);
                openCount++;
                break;
            case BOUNCE_ACTIVITY_ELEMENT:
                serializer = new XmlSerializer (typeof(BouncedMessageActivity));
                BouncedMessageActivity bouncedMessageActivity = null;
                using (XmlTextReader sr = new XmlTextReader (new StringReader (xml))) {
                    bouncedMessageActivity = (BouncedMessageActivity)serializer.Deserialize (sr);
                    log.Debug("A bounce event was found for job: " + bouncedMessageActivity.RequestId);
                }
                listener.OnBounce(bouncedMessageActivity);
                bounceCount++;
                break;
            case DELIVERED_ACTIVITY_ELEMENT:
                serializer = new XmlSerializer (typeof(DeliveredMessageActivity));
                DeliveredMessageActivity deliveredMessageActivity = null;
                using (XmlTextReader sr = new XmlTextReader (new StringReader (xml))) {
                    deliveredMessageActivity = (DeliveredMessageActivity)serializer.Deserialize (sr);
                    log.Debug("A delivery event was found for job: " + deliveredMessageActivity.RequestId);
                }
                listener.OnDelivery(deliveredMessageActivity);
                deliveryCount++;
                break;
            case SPAM_ACTIVITY_ELEMENT:
                serializer = new XmlSerializer (typeof(SpamComplaintActivity));
                SpamComplaintActivity spamComplaintActivity = null;
                using (XmlTextReader sr = new XmlTextReader (new StringReader (xml))) {
                    spamComplaintActivity = (SpamComplaintActivity)serializer.Deserialize (sr);
                    log.Debug("A spam complaint event was found for job: " + spamComplaintActivity.RequestId);
                }
                listener.OnSpamComplaint(spamComplaintActivity);
                break;
            case JOB_ERROR_ACTIVITY_ELEMENT:
                serializer = new XmlSerializer (typeof(JobErrorActivity));
                JobErrorActivity jobErrorActivity = null;
                using (XmlTextReader sr = new XmlTextReader (new StringReader (xml))) {
                    jobErrorActivity = (JobErrorActivity)serializer.Deserialize (sr);
                    log.Debug("A job error event was found for job: " + jobErrorActivity.RequestId);
                }
                listener.OnJobError(jobErrorActivity);
                jobErrorCount++;
                break;
            case RENDER_ERROR_ACTIVITY_ELEMENT:
                serializer = new XmlSerializer (typeof(RenderErrorActivity));
                RenderErrorActivity renderErrorActivity = null;
                using (XmlTextReader sr = new XmlTextReader (new StringReader (xml))) {
                    renderErrorActivity = (RenderErrorActivity)serializer.Deserialize (sr);
                    log.Debug("A render error event was found for job: " + renderErrorActivity.RequestId);
                }
                listener.OnRenderError(renderErrorActivity);
                renderErrorCount++;
                break;
            case UNSUB_ACTIVITY_ELEMENT:
                serializer = new XmlSerializer (typeof(UnsubActivity));
                UnsubActivity unsubActivity = null;
                using (XmlTextReader sr = new XmlTextReader (new StringReader (xml))) {
                    unsubActivity = (UnsubActivity)serializer.Deserialize (sr);
                    log.Debug("An unsubscribe event was found for job: " + unsubActivity.RequestId);
                }
                listener.OnUnsub(unsubActivity);
                unsubCount++;
                break;
            default:
                // This should never occur as the case statement here should match the one above.
                log.Error("An unknown event was found: " + xml);
                break;
            }
        }
    }
}        