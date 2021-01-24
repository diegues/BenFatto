using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Api_UploadFileLog.Repository
{
    public interface IDbConnectionWrapper
    {
        ConnectionState State { get; }

        void Close();

        IDbCommand CreateCommand();

        void Open();

        int Execute(string sql, object param = null);

        IEnumerable<T> Query<T>(string sql, object param = null);

    }
}
