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
  Install-Package KubeMQMSSQL.Abstractions -Version 1.0.0
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
- Error - Set internally - Return any error that the KubeMQMSSQLWorker had during the requested run.(mainly used to perform a check as if(!=Error))
- ScalarResult - Set internally - Return the result of Scalar request.
- ReturnValue - internall use only

Reading ResultModel from code
```C#
Response response = initiatorChannel.SendRequest(CreateRequest("adapter", sqlCommand));
ResultModel resultModel = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(response.Body) as ResultModel;
if (resultModel.Error == (int)ResultsEnum.Error)
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

Response response = initiatorChannel.SendRequest(CreateRequest(ProceduresType.NonQuery.ToString(), sqlCommand));

ResultModel resultModel = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(response.Body) as ResultModel;
if (resultModel.Error == (int)ResultsEnum.Error)
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

Response response = initiatorChannel.SendRequest(CreateRequest(ProceduresType.Adapter.ToString(), sqlCommand));
ResultModel resultModel = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(response.Body) as ResultModel;
if (resultModel.Error == (int)ResultsEnum.Error)
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

Response response = initiatorChannel.SendRequest(CreateRequest(ProceduresType.Scalar.ToString(), sqlCommand));
ResultModel resultModel = KubeMQ.SDK.csharp.Tools.Converter.FromByteArray(response.Body) as ResultModel;
if (resultModel.Error == (int)ResultsEnum.Error)
{
    \\\throw some errors
}
object Result = resultModel.ScalarResult;
```


