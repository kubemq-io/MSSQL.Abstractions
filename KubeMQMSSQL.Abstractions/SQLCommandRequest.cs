using KubeMQMSSQL.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;

namespace KubeMSSQL.Abstractions
{
    /// <summary>
    /// A struct that help define the request type and value to the KubeMQMSSQL Connector.
    /// </summary>
    [Serializable]
    public class SQLCommandRequest
    {
        /// <summary>
        /// Instance of KubeMSSQL.Abstractions.StreamParameters.
        /// </summary>
        public StreamParameters StreamParameters { get; set; }
        /// <summary>
        /// Represent the type of System.Data.CommandType.
        /// </summary>
        public CommandType CommandType { get; set; }
        /// <summary>
        /// Represent the Procedure or the command text.
        /// </summary>
        public string CommandText { get; set; }
        private List<KubeMQSqlParameter> _kubeMQSqlParameters;
        /// <summary>
        /// Initialize a new instance of KubeMSSQL.Abstractions.SQLCommandRequest. 
        /// </summary>
        public SQLCommandRequest()
        {
            StreamParameters = new StreamParameters();
        }
        /// <summary>
        /// List of KubeMSSQL.Abstractions.KubeSqlParameter.
        /// </summary>
        public List<KubeMQSqlParameter> kubeMQSqlParameters
        {
            get
            {
                if (_kubeMQSqlParameters == null)
                {
                    _kubeMQSqlParameters = new List<KubeMQSqlParameter>();
                }
                return _kubeMQSqlParameters;
            }
            set
            {
                _kubeMQSqlParameters = value;
            }
        }
    }
}
