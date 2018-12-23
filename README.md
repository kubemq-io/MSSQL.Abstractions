# KubeMQ MSSQL.Abstractions.
.NET SDK for communicating with the KubeMQMSSQLConnector.

# What is KubeMQ:
Please refer to : https://kubemq.io/
Please note that the KubeMQ MSSQL.Abstractions is an extension of https://github.com/kubemq-io/CSharp_SDK.

# What is KubeMQ/MSSQL.Abstractions

KubeMQ/MSSQL.Abstractions manage a single point of entry to your Data Base using KubeMQMSSQLConnector.

# General SDK description
The SDK is an assembly of structs that are required to send Query to the KubeMQMSSQLConnector.

# Install via Nuget:
```
  Install-Package KubeMQMSSQL.Abstractions -Version 1.0.4
```

# Supports:
- .NET Framework 4.6.1
- .NET Framework 4.7.1
- .NET Standard 2.0

### The 'SQLCommandRequest' object
Struct used to define the SQLCommand to the KubeMQMSSQLConnector ,

**Parameters**:
- CommandType - Mandatory. System.Data.CommandType.
- CommandText - Mandatory. string that represent the SQL Command or Procedure.
- sqlParameter - Optional. A struct that contain a set of KubeMQSqlParameter [See KubeMQSqlParameter](#the-kubemqsqlParameter-object)

Initialize SQLCommandRequest from code:
```C#
SQLCommandRequest sqlCommand = new SQLCommandRequest();
sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
sqlCommand.CommandText = "Procedure.Name";
```

# The KubeMQSqlParameter object:
Struct that is use to pass the SqlParameter to the KubeMQMSSQLConnector.
**Parameters**:
- SqlDbType - Mandatory. System.Data.SqlDbType, Specifies SQL Server-specific data type. 
- ParameterName - Mandatory. Represent the name of the SQLParameter.
- Size - Optional. Represent the size assign to the SQLParameter. if left omitted will be filled by a default value.
- Value - Optional. Represent the value assign to the SQLParameter.
- ParameterDirection - Mandatory. Represent the direction of the System.Data.ParameterDirection.
- StreamParameters - Mandatory in case of Reader Requests. A class that contain custom parameters of stream requests.

Initialize KubeMQSqlParameter Input from code:
```C#
KubeMQSqlParameter SqlParameterinner = new KubeMQSqlParameter("@parameter0", 501454, SqlDbType.Int);
SqlParameterinner.ParameterDirection = ParameterDirection.Input;
sqlCommand.kubeMQSqlParameters.Add(SqlParameterinner);
```


Initialize KubeMQSqlParameter OutPut from code:
```C#
KubeMQSqlParameter SqlParameterout = new KubeMQSqlParameter("@parameter1", SqlDbType.Int);
SqlParameterout.ParameterDirection = ParameterDirection.Output;
sqlCommand.kubeMQSqlParameters.Add(SqlParameterout);
```


# The 'ResultModel' object:
Struct that return the Data Base answer, can also indicate if any errors occurred when trying to run the DB Procedure.

**Parameters**:
- dataSet - Set internally - Return the System.Data.DataSet from the Data Base.(if there is any)
- exception - Set internally - Return any Exception that occurred during the DB connection process.
- Result - Set internally - Return result status error that the KubeMQMSSQLWorker had during the requested run.(mainly used to perform a check as if(resultModel.Result == (int)ResultsEnum.Error))
- ScalarResult - Set internally - Return the result of Scalar request.
- ReturnValue - Return result of OutPut parameter when using adapter .

CreateRequest from code
```C#
        private Request CreateRequest(int SqlCommandType, SQLCommandRequest sqlCommand)
        {

            Request request = new Request
            {
                Body = KubeMQ.SDK.csharp.Tools.Converter.ToByteArray(sqlCommand),
                Metadata = SqlCommandType.ToString()
            };
            return request;
        }
```


Reading ResultModel from code
```C#
Response response = initiatorChannel.SendRequest(CreateRequest("adapter", sqlCommand));
ResultModel resultModel = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(response.Body) as ResultModel;
if (resultModel.Result == (int)ResultsEnum.Error)
{
    \\\throw some errors
}
DataTable dataTable = resultModel.dataSet.Tables[0];
```


creating and sending NonQuery from code:
```C#
SQLCommandRequest sqlCommand = new SQLCommandRequest();
sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
sqlCommand.CommandText = "Procedure.Name";

Response response = initiatorChannel.SendRequest(CreateRequest((int)ProceduresType.NonQuery, sqlCommand));

ResultModel resultModel = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(response.Body) as ResultModel;
if (resultModel.Result == (int)ResultsEnum.Error)
{
    \\\throw some errors
}
int Result = resultModel.Result;
```

creating and sending Adapter from code:
```C#
SQLCommandRequest sqlCommand = new SQLCommandRequest();
sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
sqlCommand.CommandText = "Procedure.Name";

KubeMQSqlParameter SqlParameterinner = new KubeMQSqlParameter("@parameter0", 501454, SqlDbType.Int);
SqlParameterinner.ParameterDirection = ParameterDirection.Input;
sqlCommand.kubeMQSqlParameters.Add(SqlParameterinner);

KubeMQSqlParameter SqlParameterout = new KubeMQSqlParameter("@parameter1",SqlDbType.Int);
SqlParameterout.ParameterDirection = ParameterDirection.Output;
sqlCommand.kubeMQSqlParameters.Add(SqlParameterout);

Response response = initiatorChannel.SendRequest(CreateRequest((int)ProceduresType.Adapter, sqlCommand));
ResultModel resultModel = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(response.Body) as ResultModel;
if (resultModel.Result == (int)ResultsEnum.Error)
{
    \\\throw some errors
}
DataTable dataTable = resultModel.dataSet.Tables[0];
```

creating and sending Scalar from code:
```C#
SQLCommandRequest sqlCommand = new SQLCommandRequest();
sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
sqlCommand.CommandText = "Procedure.Name";

KubeMQSqlParameter SqlParameterinner = new KubeMQSqlParameter("@parameter0", 501454, SqlDbType.Int);
SqlParameterinner.ParameterDirection = ParameterDirection.Input;
sqlCommand.kubeMQSqlParameters.Add(SqlParameterinner);

KubeMQSqlParameter SqlParameterout = new KubeMQSqlParameter("@parameter1", 15388, SqlDbType.Int);
SqlParameterout.ParameterDirection = ParameterDirection.Input;
sqlCommand.kubeMQSqlParameters.Add(SqlParameterout);

Response response = initiatorChannel.SendRequest(CreateRequest((int)ProceduresType.Scalar, sqlCommand));
ResultModel resultModel = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(response.Body) as ResultModel;
if (resultModel.Result == (int)ResultsEnum.Error)
{
    \\\throw some errors
}
object Result = resultModel.ScalarResult;
```


creating A event Listen request for Reader Results from code:
```C#
private Request CreateRequest(int SqlCommandType, SQLCommandRequest sqlCommand)
{

      Request request = new Request
      {
          Body = KubeMQ.SDK.csharp.Tools.Converter.ToByteArray(sqlCommand),
          Metadata = SqlCommandType.ToString()
      };
      return request;
}
```

creating and sending DataReader that return one row at a time from code:
```C#
SubscribeRequest subscribeRequest = GetSubscribeRequest();
Subscriber.SubscribeToEvents(subscribeRequest, HandleIncomingEventsExecuteReader);

SQLCommandRequest sqlCommand = new SQLCommandRequest();
sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
sqlCommand.CommandText = "Procedure.Name";
sqlCommand.StreamParameters.ReaderReturnChannel = ChannelToReturnStream;
//Optional parameter for delay between events that are sent from KubeMQ MSSQLConnector.
sqlCommand.StreamParameters.DelayBetweenEvents = 500;
Response response = initiatorChannel.SendRequest(CreateRequest((int)ProceduresType.ExecuteDataReader, sqlCommand));
ResultModel resultModel = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(response.Body) as ResultModel;
if (resultModel.Result == (int)ResultsEnum.Error)
{
    \\\throw some errors
}
```

Handle Incoming Traffic from KubeMQMSSQLConnector for ExecuteReader from code:
```C#
private void HandleIncomingEventsExecuteReader(EventReceive eventReceive)
{
   logger.LogDebug("Received event");
   DataTable dataTable = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(eventReceive.Body) as DataTable;
   DataRow dataRow = dataTable.Rows[0];
   logger.LogDebug($"Received row from KubeMQ");
}
```

creating and sending DataReader that return a more than one row at a time from code:
```C#
SubscribeRequest subscribeRequest = GetSubscribeRequest();
Subscriber.SubscribeToEvents(subscribeRequest, HandleIncomingEventsReaderWithSlice);

SQLCommandRequest sqlCommand = new SQLCommandRequest();
sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
sqlCommand.CommandText = "Procedure.Name";
sqlCommand.StreamParameters.ReaderReturnChannel = ChannelToReturnStream;
//Optional parameter for delay between events that are sent from KubeMQ MSSQLConnector.
sqlCommand.StreamParameters.DelayBetweenEvents = 500;
//How many DataRows will return in one Event.
sqlCommand.StreamParameters.SliceThreshold = 20;

Response response = initiatorChannel.SendRequest(CreateRequest((int)ProceduresType.ExecuteDataReaderWithSlice, sqlCommand));
ResultModel resultModel = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(response.Body) as ResultModel;
if (resultModel.Result == (int)ResultsEnum.Error)
{
    \\\throw some errors
}
```

Handle Incoming Traffic from KubeMQMSSQLConnector for ExecuteReaderWithSlice from code:
```C#
private void HandleIncomingEventsExecuteReader(EventReceive eventReceive)
{
    logger.LogDebug("Received event");
    DataTable dataTable = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(eventReceive.Body) as DataTable;
    if (dataTable.Rows.Count == 20)
    {
        logger.LogDebug($"Received rows from KubeMQ");
    }
    else if (dataTable.Rows.Count < 20)
    {
        logger.LogDebug($"Received the last remaining rows from KubeMQ");
    }
}
```

creating and sending DataReader that return one table(use when there is more than one DataTable in the result) at a time from code:
```C#
SubscribeRequest subscribeRequest = GetSubscribeRequest();
Subscriber.SubscribeToEvents(subscribeRequest, HandleIncomingEventsReaderWithMultiTable);

SQLCommandRequest sqlCommand = new SQLCommandRequest();
sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
sqlCommand.CommandText = "Procedure.Name";
sqlCommand.StreamParameters.ReaderReturnChannel = ChannelToReturnStream;

Response response = initiatorChannel.SendRequest(CreateRequest((int)ProceduresType.ExecuteDataReaderWithMultiTable, sqlCommand));
ResultModel resultModel = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(response.Body) as ResultModel;
if (resultModel.Result == (int)ResultsEnum.Error)
{
    \\\throw some errors
}
```

Handle Incoming Traffic from KubeMQMSSQLConnector for ExecuteReaderWithSlice from code:
```C#
private void HandleIncomingEventsExecuteReader(EventReceive eventReceive)
{
    logger.LogDebug("Received event");
    DataTable dataTable = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(eventReceive.Body) as DataTable;
    if (dataTable != null)
    {
        logger.LogDebug($"Received dataTable from KubeMQ");
    }
}
```
