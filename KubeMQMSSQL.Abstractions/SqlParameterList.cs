using System;
using System.Data;

namespace KubeMSSQL.Abstractions
{   /// <summary>
    /// Represents a parameter to a send to the DataBase using the KubeMQ.
    /// </summary>
    [Serializable]
    public class KubeSqlParamters
    {
        /// <summary>
        /// Represent System.Data.SqlDbType
        /// </summary>
        public SqlDbType SqlDbType { get; set; }
        /// <summary>
        /// Represent the name of the SQLParameter.
        /// </summary>
        public string ParameterName { get; set; }
        /// <summary>
        /// Represent the size assign to the SQLParameter.
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// Represent the value assign to the SQLParameter.
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// Represent the direction of the System.Data.ParameterDirection.
        /// </summary>
        public ParameterDirection ParameterDirection;
        /// <summary>
        /// Initializes a new instance of the KubeMSSQL.Abstractions.KubeSqlParamters without value.
        /// </summary>
        /// <param name="_ParameterName">The name of the parameter.</param>
        /// <param name="_SqlDbType">Represent the System.Data.SqlDbType of this parameter. </param>
        /// <param name="_Size">Represent the size of the System.Data.SqlDbType,If left empty will set to a default value depend on the System.Data.SqlDbType. </param>
        public KubeSqlParamters(string _ParameterName,SqlDbType _SqlDbType, int? _Size = null)
        {
            if (_Size == null)
            {
                SetSqlDBTypeAndName(_ParameterName, _SqlDbType);
            }
        }
        /// <summary>
        /// Initializes a new instance of the KubeMSSQL.Abstractions.KubeSqlParamters with value.
        /// </summary>
        /// <param name="_ParameterName">The name of the parameter.</param>
        /// <param name="_Value">Represent the value of the parameter.</param>
        /// <param name="_SqlDbType">Represent the System.Data.SqlDbType of this parameter. </param>
        /// <param name="_Size">Represent the size of the System.Data.SqlDbType,If left empty will set to a default value depend on the System.Data.SqlDbType.</param>
        public KubeSqlParamters(string _ParameterName, object _Value, SqlDbType _SqlDbType, int? _Size = null)
        {
            Value = _Value;
            if(_Size==null)
            {
                SetSqlDBTypeAndName(_ParameterName, _SqlDbType);
            }
        }
        /// <summary>
        /// Set to default size.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="sqlDbType"></param>
        private void SetSqlDBTypeAndName(string parameterName, SqlDbType sqlDbType)
        {
            ParameterName = parameterName;
            SqlDbType = sqlDbType;
            switch (SqlDbType)
            {
                case SqlDbType.BigInt:
                    Size = 8;
                    break;
                case SqlDbType.Binary:
                    Size = 50;
                    break;
                case SqlDbType.Bit:
                    Size = 1;
                    break;
                case SqlDbType.Char:
                    Size = 1;
                    break;
                case SqlDbType.Date:
                case SqlDbType.DateTime:
                case SqlDbType.DateTime2:
                case SqlDbType.DateTimeOffset:
                    Size = 8;
                    break;
                case SqlDbType.Decimal:
                    Size = 9;
                    break;
                case SqlDbType.Float:
                    Size = 8;
                    break;
                case SqlDbType.Image:
                    Size = 2147483647;
                    break;
                case SqlDbType.Int:
                    Size = 4;
                    break;
                case SqlDbType.Money:
                    Size = 8;
                    break;
                case SqlDbType.NChar:
                    Size = 4000;
                    break;
                case SqlDbType.NText:
                    Size = 1073741823;
                    break;
                case SqlDbType.NVarChar:
                    Size = 8000;
                    break;
                case SqlDbType.Real:
                    Size = 4;
                    break;
                case SqlDbType.SmallDateTime:
                    Size = 4;
                    break;
                case SqlDbType.SmallInt:
                    Size = 2;
                    break;
                case SqlDbType.SmallMoney:
                    Size = 4;
                    break;
                case SqlDbType.Text:
                    Size = 2147483647;
                    break;
                case SqlDbType.Time:
                case SqlDbType.Timestamp:
                    Size = 8;
                    break;
                case SqlDbType.TinyInt:
                    Size = 1;
                    break;
                case SqlDbType.UniqueIdentifier:
                    Size = 16;
                    break;
                case SqlDbType.VarBinary:
                    Size = 2147483647;
                    break;
                case SqlDbType.VarChar:
                    Size = 2147483647;
                    break;
                case SqlDbType.Variant:
                    Size = 8016;
                    break;
                default:
                    break;
            }
        }
    }
}