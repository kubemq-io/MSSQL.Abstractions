using System;
using System.Collections.Generic;
using System.Text;

namespace KubeMQMSSQL.Abstractions.Results
{
    [Serializable]
    public enum ResultsEnum
    {
        Done,
        Error=999,
    }

    [Serializable]
    public enum ProceduresType
    {
        Adapter,
        NonQuerry,
        Scalar,
        NotSupported
    }
}
