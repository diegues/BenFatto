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
        private string queryInsert = @"insert into log (ip,local,usuario,data,requisicao,status,time,origem,software)
                              values(@ip,@local,@usuario,@data,@requisicao,@status,@time,@origem,@software) RETURNING id";

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
            int result = 0;
            var con = new NpgsqlConnection(new BaseRepository(_configuration).GetConnection());
            con.Open();
            NpgsqlTransaction transaction = con.BeginTransaction();
            try
            {

                foreach (var log in listLog)
                {
                    log.id = (Int64)con.ExecuteScalar(queryInsert, log);
                    result++;
                }
            }
            catch (Exception e)
            {
                result = 0;
                transaction.Rollback();
            }
            finally
            {
                if (result > 0)
                {
                    transaction.Commit();
                }
                transaction.Dispose();
                con.Close();

            }

            return result;
        }
    }
}
