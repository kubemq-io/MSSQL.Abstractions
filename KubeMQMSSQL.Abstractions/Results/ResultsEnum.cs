using System;
namespace KubeMQMSSQL.Abstractions.Results
{
    /// <summary>
    /// ResultEnum , represent if there was an error while preforming the request.
    /// </summary>
    [Serializable]
    public enum ResultsEnum
    {
        Done,
        Error = 999,
    }

    /// <summary>
    /// Represent the type of action to preform with the KubeMQMSSQL worker.
    /// </summary>
    [Serializable]
    public enum ProceduresType
    {
        Adapter,
        NonQuery,
        Scalar,
        ExecuteDataReader,
        ExecuteDataReaderWithSlice,
        ExecuteDataReaderWithMultiTable,
        NotSupported = 99999,
    }
}
