using KubeMQMSSQL.Abstractions.Results;
using Microsoft.Extensions.Logging;
using System.Data;
using KubeMQ.SDK.csharp.CommandQuery;
using KubeMSSQL.Abstractions;
using KubeMQ.SDK.csharp.Subscription;
using KubeMQ.SDK.csharp.Events;
using KubeMQ.SDK.csharp.Tools;

namespace MSSQLWorkerTester
{
    class Manager
    {
        private const int TimeOut = 300000;
        private KubeMQ.SDK.csharp.CommandQuery.Channel initiatorChannel { get; set; }
        private string KubeMQMSSQLConnectorChannelName { get; set; }
        private string ClientID { get; set; }
        private string KubeMQAddress { get; set; }

        private Subscriber Subscriber;
        private ILogger logger { get; set; }

        private string ChannelToReturnStream;
        public Manager()
        {
            ILoggerFactory loggerFactory = new LoggerFactory();
            KubeMQMSSQLConnectorChannelName = "MyChannelName";
            ClientID = "MyClientID";
            KubeMQAddress = "localhost:50000";
            ChannelToReturnStream = "MyChannelToReturn";
            logger = loggerFactory.CreateLogger<Manager>();
            Subscriber = new Subscriber(KubeMQAddress);
            initiatorChannel = new KubeMQ.SDK.csharp.CommandQuery.Channel(CreateChannelParam());
            TestExecuteNonQuery();
            TestAdapter();
            TestScalar();
            TestExecuteReader();
            TestExecuteReaderWithSlice();
            TestExecuteReaderWithMultiTable();
        }



        /// <summary>
        /// Executes a Transact-SQL statement and return a System.Data.DataSet.
        /// </summary>
        private bool TestAdapter()
        {
            SQLCommandRequest sqlCommand = new SQLCommandRequest();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "Procedure.Name";

            KubeMQSqlParameter SqlParameterin0 = new KubeMQSqlParameter("@ParameterIn", SqlDbType.Int);
            SqlParameterin0.ParameterDirection = ParameterDirection.Input;
            sqlCommand.kubeMQSqlParameters.Add(SqlParameterin0);

            KubeMQSqlParameter SqlParameterout1 = new KubeMQSqlParameter("@Parameterout1", SqlDbType.NVarChar);
            SqlParameterout1.ParameterDirection = ParameterDirection.Output;
            sqlCommand.kubeMQSqlParameters.Add(SqlParameterout1);

            KubeMQSqlParameter SqlParameterout2 = new KubeMQSqlParameter("@Parameterout2", SqlDbType.DateTime);
            SqlParameterout2.ParameterDirection = ParameterDirection.Output;
            sqlCommand.kubeMQSqlParameters.Add(SqlParameterout2);

            Response response = initiatorChannel.SendRequest(CreateRequest((int)ProceduresType.Adapter, sqlCommand));
            ResultModel resultModel = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(response.Body) as ResultModel;
            if (resultModel.Result == (int)ResultsEnum.Error)
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
        public bool TestExecuteNonQuery()
        {
            SQLCommandRequest sqlCommand = new SQLCommandRequest();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "Procedure.Name";

            Response response = initiatorChannel.SendRequest(CreateRequest((int)ProceduresType.NonQuery, sqlCommand));

            ResultModel resultModel = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(response.Body) as ResultModel;
            if (resultModel.Result == (int)ResultsEnum.Error)
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

            KubeMQSqlParameter SqlParameterout = new KubeMQSqlParameter("@parameter1", "VeryLongNVarChar", SqlDbType.NVarChar);
            SqlParameterout.Size = 40000;
            SqlParameterout.ParameterDirection = ParameterDirection.Input;
            sqlCommand.kubeMQSqlParameters.Add(SqlParameterout);

            Response response = initiatorChannel.SendRequest(CreateRequest((int)ProceduresType.Scalar, sqlCommand));
            ResultModel resultModel = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(response.Body) as ResultModel;
            if (resultModel.Result == (int)ResultsEnum.Error)
            {
                return false;
            }
            object Result = resultModel.ScalarResult;
            return true;
        }


        /// <summary>
        /// Executes DataReader and return one row at a time using KubeMQEvent.
        /// </summary>
        private bool TestExecuteReader()
        {
            SubscribeRequest subscribeRequest = GetSubscribeRequest();
            Subscriber.SubscribeToEvents(subscribeRequest, HandleIncomingEventsExecuteReader);
            logger.LogDebug("Started subscribe to events for TestExecuteReader");
            SQLCommandRequest sqlCommand = new SQLCommandRequest();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "Procedure.Name";
            sqlCommand.StreamParameters.ReaderReturnChannel = ChannelToReturnStream;
            //Optional parameter for delay between events that are sent from KubeMQ MSSQLConnector.
            sqlCommand.StreamParameters.DelayBetweenEvents = 500;
            Response response = initiatorChannel.SendRequest(CreateRequest(((int)ProceduresType.ExecuteDataReader), sqlCommand));
            ResultModel resultModel = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(response.Body) as ResultModel;
            if (resultModel.Result == (int)ResultsEnum.Error)
            {
                return false;
            }
            logger.LogInformation("Finished MSSQLConnector Finished sending events");
            return true;
        }

        /// <summary>
        /// Executes DataReader and return a set of data rows KubeMQEvent.
        /// Note: if the number of rows from response does not divide equally by the SliceTrashfold , the KubeMQ MSSQLConnector will send the remaining line in the last event.
        /// </summary>
        private bool TestExecuteReaderWithSlice()
        {
            SubscribeRequest subscribeRequest = GetSubscribeRequest();
            Subscriber.SubscribeToEvents(subscribeRequest, HandleIncomingEventsReaderWithSlice);
            logger.LogDebug("Started subscribe to events for TextExecuteReaderWithSlice");
            SQLCommandRequest sqlCommand = new SQLCommandRequest();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "Procedure.Name";
            sqlCommand.StreamParameters.ReaderReturnChannel = ChannelToReturnStream;
            //Optional parameter for delay between events that are sent from KubeMQ MSSQLConnector.
            sqlCommand.StreamParameters.DelayBetweenEvents = 500;
            sqlCommand.StreamParameters.SliceThreshold = 20;
            Response response = initiatorChannel.SendRequest(CreateRequest(((int)ProceduresType.ExecuteDataReaderWithSlice), sqlCommand));
            ResultModel resultModel = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(response.Body) as ResultModel;
            if (resultModel.Result == (int)ResultsEnum.Error)
            {
                return false;
            }
            logger.LogInformation("Finished MSSQLConnector Finished sending events");
            return true;
        }

        /// <summary>
        /// Executes DataReader and return one table at a time.
        /// Best use when expecting a procedure to return more than one DataTable.
        /// </summary>
        private bool TestExecuteReaderWithMultiTable()
        {
            SubscribeRequest subscribeRequest = GetSubscribeRequest();
            Subscriber.SubscribeToEvents(subscribeRequest, HandleIncomingEventsReaderWithMultiTable);
            logger.LogDebug("Started subscribe to events for TestExecuteReaderWithMultiTable");
            SQLCommandRequest sqlCommand = new SQLCommandRequest();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "Procedure.Name";
            sqlCommand.StreamParameters.ReaderReturnChannel = ChannelToReturnStream;
            Response response = initiatorChannel.SendRequest(CreateRequest(((int)ProceduresType.ExecuteDataReaderWithMultiTable), sqlCommand));
            ResultModel resultModel = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(response.Body) as ResultModel;
            if (resultModel.Result == (int)ResultsEnum.Error)
            {
                return false;
            }
            logger.LogInformation("Finished MSSQLConnector Finished sending events");
            return true;
        }



        #region Utils
        /// <summary>
        /// Create Dynamic Request
        /// </summary>
        /// <param name="SqlCommandType">Metadata</param>
        /// <param name="sqlCommand">DB Procedure parameter</param>
        /// <returns></returns>
        private Request CreateRequest(int SqlCommandType, SQLCommandRequest sqlCommand)
        {

            Request request = new Request
            {
                Body = KubeMQ.SDK.csharp.Tools.Converter.ToByteArray(sqlCommand),
                Metadata = SqlCommandType.ToString()
            };
            return request;
        }
        /// <summary>
        /// Create a basic SubscribeRequest.
        /// </summary>
        /// <returns></returns>
        private SubscribeRequest GetSubscribeRequest()
        {
            return new SubscribeRequest
            {
                Channel = ChannelToReturnStream,
                ClientID = "MyClientID",
                SubscribeType = SubscribeType.Events,
                EventsStoreType = EventsStoreType.Undefined,
                EventsStoreTypeValue = 0,
                Group = ""
            };
        }
        /// <summary>
        /// Create a basic Channel KubeMQ.SDK.csharp.CommandQuery.ChannelParameters.
        /// </summary>
        /// <returns></returns>
        private KubeMQ.SDK.csharp.CommandQuery.ChannelParameters CreateChannelParam()
        {
            KubeMQ.SDK.csharp.CommandQuery.ChannelParameters channelParameters = new KubeMQ.SDK.csharp.CommandQuery.ChannelParameters(RequestType.Query, KubeMQMSSQLConnectorChannelName, ClientID, TimeOut, "", 0, KubeMQAddress, logger);
            return channelParameters;
        }
        #endregion

        #region Handle Incoming Events
        /// <summary>
        /// Handle Incoming events Expecting a one DataRow at a time from KubeMQ MSSQLConnector.
        /// </summary>
        /// <param name="eventReceive"></param>
        private void HandleIncomingEventsExecuteReader(EventReceive eventReceive)
        {
            logger.LogDebug("Received event");
            StreamResultModel streamResult = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(eventReceive.Body) as StreamResultModel;
            DataRow dataRow = streamResult.dataTable.Rows[0];
            logger.LogDebug($"Received row from KubeMQ");
        }

        /// <summary>
        /// Handle Incoming events Expecting a set amount of dataRows from KubeMQ MSSQLConnector.
        /// </summary>
        /// <param name="eventReceive"></param>
        private void HandleIncomingEventsReaderWithSlice(EventReceive eventReceive)
        {
            logger.LogDebug("Received event");
            StreamResultModel streamResult = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(eventReceive.Body) as StreamResultModel;
            if (streamResult.dataTable.Rows.Count == 20)
            {
                logger.LogDebug($"Received rows from KubeMQ");
            }
            else if (streamResult.dataTable.Rows.Count < 20)
            {
                logger.LogDebug($"Received the last remaining rows from KubeMQ");
            }
        }

        /// <summary>
        /// Handle Incoming events Expecting a one DataTable at a time from KubeMQ MSSQLConnector.
        /// </summary>
        /// <param name="eventReceive"></param>
        private void HandleIncomingEventsReaderWithMultiTable(EventReceive eventReceive)
        {
            logger.LogDebug("Received event");
            StreamResultModel streamResult = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(eventReceive.Body) as StreamResultModel;
            if (streamResult != null)
            {
                logger.LogDebug($"Received dataTable from KubeMQ");
            }
        }

        /// <summary>
        /// Handle Incoming events and act according to key received.
        /// </summary>
        /// <param name="eventReceive"></param>
        private void HandleIncomingEventsReaderAllTypes(EventReceive eventReceive)
        {
            StreamResultModel streamResultModel = Converter.FromByteArray(eventReceive.Body) as StreamResultModel;

            switch (streamResultModel.Key)
            {
                case "MyKeySingleRowReader":
                    //Do some SingleRow logic
                    break;
                case "ReaderWithSlice":
                    //Do some Reader with slice logic
                    break;
                case "ReaderMultiTable":
                    //Do some MultiTable Logic
                    break;
            }
        }
        #endregion
    }
}