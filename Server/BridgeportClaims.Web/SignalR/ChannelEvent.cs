using System;
using ServiceStack;

namespace BridgeportClaims.Web.SignalR
{
    /// <summary>
    /// A generic object to represent a broadcasted event in our SignalR hubs
    /// </summary>
    public class ChannelEvent
    {
        /// <summary>
        /// The name of the event
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The name of the channel the event is associated with
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// The date/time that the event was created
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// The data associated with the event
        /// </summary>
        public object Data
        {
            get => _data;
            set
            {
                _data = value;
                Json = _data.ToJson();
            }
        }
        private object _data;

        /// <summary>
        /// A JSON representation of the event data. This is set automatically
        /// when the Data property is assigned.
        /// </summary>
        public string Json { get; private set; }

        public ChannelEvent() => Timestamp = DateTimeOffset.UtcNow;
    }
}