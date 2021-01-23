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
            var con = new NpgsqlConnection(new BaseRepository(_configuration).GetConnection());
            try
            {
                con.Open();
                string queryInsert = @"insert into log (ip,local,usuario,data,requisicao,status,time,origem,software)
                              values(@ip,@local,@usuario,@data,@requisicao,@status,@time,@origem,@software)";
                return con.Execute(queryInsert, log);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public int AddList(List<Log> listLog)
        {
            int result = 1;
            try
            {
                using (var connection = new NpgsqlConnection(new BaseRepository(_configuration).GetConnection()))
                {
                    string queryInsert = @"insert into log (ip,local,usuario,data,requisicao,status,time,origem,software)
                              values(@ip,@local,@usuario,@data,@requisicao,@status,@time,@origem,@software)";
                    var affectedRows = connection.Execute(queryInsert, listLog);
                    result = affectedRows;
                }
            }
            catch (Exception e)
            {
                result = 0;
            }

            return result;
        }
    }
}
