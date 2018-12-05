using KubeMQMSSQL.Abstractions;
using KubeMQMSSQL.Abstractions.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using KubeMQ.SDK.csharp.CommandQuery;
using KubeMSSQL.Abstractions;

namespace MSSQLWorkerTester
{
    class Manager
    {
        private const int TimeOut  = 300000;
        private Channel initiatorChannel { get; set; }
        private string ChannelName { get; set; }
        private string ClientID { get; set; }
        private string KubeMQAddress { get; set; }
        private ILogger logger { get; set; }
        public Manager()
        {
            ILoggerFactory loggerFactory = new LoggerFactory();
            ChannelName = "MyChannelName";
            ClientID = "MyClientID";
            KubeMQAddress = "localhost:50000";
            logger = loggerFactory.CreateLogger<Manager>();
            initiatorChannel = new KubeMQ.SDK.csharp.CommandQuery.Channel(CreateChannelParam());
            TestExcuteNonQuery();
            TestAdapter();
            TestScalar();
        }


        /// <summary>
        /// Create a basic Channel KubeMQ.SDK.csharp.CommandQuery.ChannelParameters.
        /// </summary>
        /// <returns></returns>
        private ChannelParameters CreateChannelParam()
        {
            ChannelParameters channelParameters = new ChannelParameters(RequestType.Query, ChannelName, ClientID, TimeOut, "", 0, KubeMQAddress, logger);
            return channelParameters;
        }

        /// <summary>
        /// Executes a Transact-SQL statement and return a System.Data.DataSet.
        /// </summary>
        private bool TestAdapter()
        {
            SQLCommandRequest sqlCommand = new SQLCommandRequest();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "Procedure.Name";

            KubeMQSqlParameter SqlParameterinner = new KubeMQSqlParameter("@parameter0", 501454, SqlDbType.Int);
            SqlParameterinner.ParameterDirection = ParameterDirection.Input;
            sqlCommand.kubeMQSqlParameters.Add(SqlParameterinner);

            KubeMQSqlParameter SqlParameterout = new KubeMQSqlParameter("@parameter1",SqlDbType.Int);
            SqlParameterout.ParameterDirection = ParameterDirection.Output;
            sqlCommand.kubeMQSqlParameters.Add(SqlParameterout);

            Response response = initiatorChannel.SendRequest(CreateRequest(ProceduresType.Adapter.ToString(), sqlCommand));
            ResultModel resultModel = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(response.Body) as ResultModel;
            if (resultModel.Error == (int)ResultsEnum.Error)
            {
                return false;
            }
            DataTable dataTable = resultModel.dataSet.Tables[0];
            return true;
        }

        /// <summary>
        /// Executes a Transact-SQL statement and return count of rows that were changed.
        /// </summary>
        /// <returns></returns>
        public bool TestExcuteNonQuery()
        {
            SQLCommandRequest sqlCommand = new SQLCommandRequest();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "Procedure.Name";

            Response response = initiatorChannel.SendRequest(CreateRequest(ProceduresType.NonQuery.ToString(), sqlCommand));

            ResultModel resultModel = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(response.Body) as ResultModel;
            if (resultModel.Error == (int)ResultsEnum.Error)
            {
                return false;
            }
            int Result = resultModel.Result;
            return true;
        }
        /// <summary>
        /// Executes a Transact-SQL statement and return a single value result.
        /// </summary>
        /// <returns></returns>
        public bool TestScalar()
        {
            SQLCommandRequest sqlCommand = new SQLCommandRequest();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "Procedure.Name";

            KubeMQSqlParameter SqlParameterinner = new KubeMQSqlParameter("@parameter0", 501454, SqlDbType.Int);
            SqlParameterinner.ParameterDirection = ParameterDirection.Input;
            sqlCommand.kubeMQSqlParameters.Add(SqlParameterinner);

            KubeMQSqlParameter SqlParameterout = new KubeMQSqlParameter("@parameter1", 15388, SqlDbType.Int);
            SqlParameterout.ParameterDirection = ParameterDirection.Input;
            sqlCommand.kubeMQSqlParameters.Add(SqlParameterout);

            Response response = initiatorChannel.SendRequest(CreateRequest(ProceduresType.Scalar.ToString(), sqlCommand));
            ResultModel resultModel = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(response.Body) as ResultModel;
            if (resultModel.Error == (int)ResultsEnum.Error)
            {
                return false;
            }
            object Result = resultModel.ScalarResult;
            return true;
        }
        /// <summary>
        /// Create Dynamic Request
        /// </summary>
        /// <param name="SqlCommandType">Metadata</param>
        /// <param name="sqlCommand">DB Procedure parameter</param>
        /// <returns></returns>
        private Request CreateRequest(string SqlCommandType, SQLCommandRequest sqlCommand)
        {
            Request request = new Request
            {
                Body = KubeMQ.SDK.csharp.Tools.Converter.ToByteArray(sqlCommand),
                Metadata = SqlCommandType
            };
            return request;
        }
    }
}
