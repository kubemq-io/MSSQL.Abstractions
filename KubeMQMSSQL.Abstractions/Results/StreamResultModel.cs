using System;
using System.Data;

namespace KubeMQMSSQL.Abstractions.Results
{
    /// <summary>
    /// A class that contain all the Stream result data.
    /// </summary>
    /// </summary>
    [Serializable]
    public class StreamResultModel
    {
        /// <summary>
        /// Represent the identifier that was set for the request.
        /// </summary>
        public string Key { get; private set; }
        /// <summary>
        /// Represent System.Data.DataTable class.
        /// </summary>
        public DataTable dataTable { get; private set; }
        /// <summary>
        /// initialize a new instance of KubeMQMSSQL.Abstractions.Results.StreamResultModel.
        /// </summary>
        /// <param name="_key"></param>
        /// <param name="_dataTable"></param>
        public StreamResultModel(string _key, DataTable _dataTable)
        {
            Key = _key;
            dataTable = _dataTable;
        }
    }
}
