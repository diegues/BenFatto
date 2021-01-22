using Api_UploadFileLog.Entidades;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_UploadFileLog.Repository
{
    public class LogRepository
    {
        private readonly IConfiguration _configuration;

        public LogRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int Add(Log log)
        {
            using (var con = new NpgsqlConnection(new BaseRepository(_configuration).GetConnection()))
            {
                try
                {
                    con.Open();
                    var query = @"insert into log (ip,local,usuario,data,requisicao,status,time,origem,software)
                              values(@ip,@local,@usuario,@data,@requisicao,@status,@time,@origem,@software)";
                    return con.Execute(query, log);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}
