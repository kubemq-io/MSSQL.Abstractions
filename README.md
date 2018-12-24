# KubeMQ MSSQL.Abstractions.
.NET SDK for communicating with the KubeMQ/MSSQL Connector.

# What is KubeMQ:
Please refer to : https://kubemq.io/
Please note that the KubeMQ MSSQL.Abstractions is an extension of https://github.com/kubemq-io/CSharp_SDK.

# What is KubeMQ/MSSQL.Abstractions
KubeMQ/MSSQL Connector can manage a single point of entry to your SQL Data Base.
KubeMQ/MSSQL Abstractions is asimple SDK to communicate with common objects with the KubeMQ/MSSQL Connector.

# General SDK description
The SDK is an assembly of structs that are required to send Query to the KubeMQ/MSSQL Connector.

# Install via Nuget:
```
  Install-Package KubeMQMSSQL.Abstractions -Version 1.0.4
```

# Supports:
- .NET Framework 4.6.1
- .NET Framework 4.7.1
- .NET Standard 2.0

### The 'SQLCommandRequest' object
Struct used to define the SQLCommand to the KubeMQM/SSQL Connector, inspired by the common used metodology.

**Parameters**:
- CommandType - Mandatory, System.Data.CommandType.
- CommandText - Mandatory, string that represent the SQL Command or Procedure.
- sqlParameter - Optional, a struct that contain a set of KubeMQSqlParameter [See KubeMQSqlParameter] (#the-kubemqsqlParameter-object)

Initialize SQLCommandRequest from code:
```C#
SQLCommandRequest sqlCommand = new SQLCommandRequest();
sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
sqlCommand.CommandText = "Procedure.Name";
```

# The KubeMQSqlParameter object:
Struct that is use to pass the SqlParameter to the KubeMQ/MSSQL Connector, inspired by the common used metodology.
**Parameters**:
- SqlDbType - Mandatory, System.Data.SqlDbType, specifies SQL Server-specific data type. 
- ParameterName - Mandatory, represent the name of the SQLParameter.
- Size - Optional, represent the size assign to the SQLParameter. If left omitted will be filled by a default value.
- Value - Optional, represent the value assign to the SQLParameter.
- ParameterDirection - Mandatory, represent the direction of the System.Data.ParameterDirection.
- StreamParameters - Mandatory, in case of Reader Requests, this class will contain custom parameters of stream requests.

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
Struct that return the Data Base answer result, can also indicate if any errors occurred when trying to run the DB Procedure.

**Parameters**:
- dataSet - Set internally - Return the System.Data.DataSet from the Data Base.(if there is any)
- exception - Set internally - Return any Exception thrown by the KubeMQM/SSQL Connector process.
- Result - Set internally - Return result status code the KubeMQMSSQLWorker of the request execution.(exmple: resultModel.Result == (int)ResultsEnum.Error))
- ScalarResult - Set internally - Return the result of Scalar request.
- ReturnValue - Return result of OutPut parameter when using adapter .

Create a request and from code:
```C#
  Response response = initiatorChannel.SendRequest(CreateRequest("adapter", sqlCommand));
  ResultModel resultModel = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(response.Body) as ResultModel;
  if (resultModel.Result == (int)ResultsEnum.Error)
  {
      \\\throw some errors
  }
  DataTable dataTable = resultModel.dataSet.Tables[0];
  
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

Creating and sending NonQuery from code:
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

Creating and sending Adapter from code:
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

Ccreating and sending Scalar from code:
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

Creating a DataReader ProceduresType.ExecuteDataReader that streams evet of a single row to ReaderReturnChannel:
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

Handle Incoming Traffic from KubeMQ/MSSQL Connector for ProceduresType.ExecuteDataReader from code:
```C#
  private void HandleIncomingEventsExecuteReader(EventReceive eventReceive)
  {
     logger.LogDebug("Received event");
     DataTable dataTable = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(eventReceive.Body) as DataTable;
     DataRow dataRow = dataTable.Rows[0];
     logger.LogDebug($"Received row from KubeMQ");
  }
```

Creating a DataReader ProceduresType.ExecuteDataReaderWithSlice that streams evet of multiple rows to ReaderReturnChannel:
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

Handle incoming traffic from KubeMQ/MSSQL Connector for ProceduresType.ExecuteDataReaderWithSlice from code:
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

Creating a DataReader ProceduresType.ExecuteDataReaderWithMultiTable that streams single table to ReaderReturnChannel:
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

Handle incoming traffic from KubeMQM/SSQL Connector for ProceduresType.ExecuteDataReaderWithMultiTable from code:
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
