using KubeMQMSSQL.Abstractions;
using KubeMQMSSQL.Abstractions.Results;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using KubeMQ.SDK.csharp.Tools;

namespace MSSQLWorkerTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Manager manager = new Manager();
        }
    }
}
