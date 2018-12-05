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
        private List<KubeSqlParameters> _ReturnValue;
        public List<KubeSqlParameters> ReturnValue
        {
            get
            {
                if (_ReturnValue == null)
                {
                    _ReturnValue = new List<KubeSqlParameters>();
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
            List<KubeSqlParameters> ReturnValue = new List<KubeSqlParameters>();
        }
    }
}
