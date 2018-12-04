using KubeMSSQL.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KubeMQMSSQL.Abstractions.Results
{
    [Serializable]
    public class ResultModel
    {
        public DataSet dataSet { get; set; }
        public Exception exception { get; set; }
        public int Error { get; set; }
        public int Result { get; set; }
        public object ScalarResult;
        private List<KubeSqlParamters> _ReturnValue;
        public List<KubeSqlParamters> ReturnValue
        {
            get
            {
                if (_ReturnValue == null)
                {
                    _ReturnValue = new List<KubeSqlParamters>();
                }
                return _ReturnValue;
            }
            set
            {
                _ReturnValue = value;
            }
        }
        public ResultModel()
        {
            List<KubeSqlParamters> ReturnValue = new List<KubeSqlParamters>();
        }
    }
}
