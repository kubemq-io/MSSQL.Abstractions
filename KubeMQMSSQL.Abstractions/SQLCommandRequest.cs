using System;
using System.Collections.Generic;
using System.Data;

namespace KubeMSSQL.Abstractions
{
    [Serializable]
    /// <summary>
    /// KubeSqlParam inner SQL parameter class
    /// </summary>
    public class SQLCommandRequest
    {
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
