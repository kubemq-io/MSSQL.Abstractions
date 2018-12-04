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
        /// Represent the KubeMSSQL.Abstractions.kubeSqlParamters.
        /// </summary>
        public KubeSqlParamters kubeSqlParamters { get; set; }
        /// <summary>
        /// Represent the type of System.Data.CommandType.
        /// </summary>
        public CommandType CommandType { get; set; }
        /// <summary>
        /// Represent the Procedure or the command text.
        /// </summary>
        public string CommandText { get; set; }
        private List<KubeSqlParamters> _sqlParameter;
        /// <summary>
        /// List of KubeMSSQL.Abstractions.KubeSqlParamters.
        /// </summary>
        public List<KubeSqlParamters> sqlParameter
        {
            get
            {
                if (_sqlParameter == null)
                {
                    _sqlParameter = new List<KubeSqlParamters>();
                }
                return _sqlParameter;
            }
            set
            {
                _sqlParameter = value;
            }
        }
    }
}
