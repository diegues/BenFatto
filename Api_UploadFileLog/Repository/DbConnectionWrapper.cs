using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Api_UploadFileLog.Repository
{
    public class DbConnectionWrapper : IDbConnectionWrapper
    {
        private readonly IDbConnection _con;

        public DbConnectionWrapper(IDbConnection con)
        {
            _con = con;
        }

        public ConnectionState State => _con.State;

        public void Close() => _con.Close();

        public IDbCommand CreateCommand() => _con.CreateCommand();

        public void Open() => _con.Open();

        public int Execute(string sql, object param = null) => _con.Execute(sql, param);

        public IEnumerable<T> Query<T>(string sql, object param = null) => _con.Query<T>(sql, param);
    }
}
