using System;

namespace KubeMSSQL.Abstractions
{
    /// <summary>
    /// A class that contain custom parameters of stream requests.
    /// </summary>
    [Serializable]
    public class StreamParameters
    {
        /// <summary>
        /// Represent the amount of delay between events sending.(milliseconds).
        /// </summary>
        public int DelayBetweenEvents { get; set; }
        /// <summary>
        /// Represent the name of the channel to return reader events to.
        /// </summary>
        public string ReaderReturnChannel { get; set; }
        /// <summary>
        /// Represent the threshold amount of data rows in one Event to receive from KubeMQ MSSQLConnector.
        /// If left empty setting to default of 1.
        /// </summary>
        public int SliceThreshold { get; set; }
        /// <summary>
        /// Initialize a new instance of KubeMSSQL.Abstractions.StreamParameters.
        /// </summary>
        public StreamParameters()
        {
            SliceThreshold = 1;
        }
    }
}