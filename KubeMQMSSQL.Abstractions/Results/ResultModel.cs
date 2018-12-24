using KubeMSSQL.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;

namespace KubeMQMSSQL.Abstractions.Results
{
    /// <summary>
    /// A class that contain all the execution result data.
    /// </summary>
    [Serializable]
    public class ResultModel
    {
        /// <summary>
        /// Represent System.Data.DataSet class.
        /// </summary>
        public DataSet dataSet { get; set; }
        /// <summary>
        ///  Represents errors that occur during application execution.
        ///  If no error is present will be null.
        /// </summary>
        public Exception exception { get; set; }
        /// <summary>
        /// ResultsEnum Result code.
        /// </summary>
        public int Result { get; set; }
        /// <summary>
        /// Represent ScalarResult when using Scalar Request.
        /// </summary>
        public object ScalarResult;
        private List<KubeMQSqlParameter> _ReturnValue;
        /// <summary>
        /// Use in-case of OutPut parameter when using adapter 
        /// </summary>
        public List<KubeMQSqlParameter> ReturnValue
        {
            get
            {
                if (_ReturnValue == null)
                {
                    _ReturnValue = new List<KubeMQSqlParameter>();
                }
                return _ReturnValue;
            }
            set
            {
                _ReturnValue = value;
            }
        }
        /// <summary>
        /// initialize a new instance of KubeMQMSSQL.Abstractions.Results.ResultModel.
        /// </summary>
        public ResultModel()
        {
            List<KubeMQSqlParameter> ReturnValue = new List<KubeMQSqlParameter>();
        }
    }
}
