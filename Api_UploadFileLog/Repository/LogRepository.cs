using Api_UploadFileLog.Entidades;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Api_UploadFileLog.Repository
{
    public class LogRepository
    {
        private readonly IConfiguration _configuration;
        private string queryInsert = @"insert into log (ip,local,usuario,data,zone,requisicao,status,time,origem,software)
                              values(@ip,@local,@usuario,@data,@zone,@requisicao,@status,@time,@origem,@software)";
        NpgsqlConnection con;

        public LogRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            con = new NpgsqlConnection(new BaseRepository(_configuration).GetConnection());
        }

        public int Add(Log log)
        {
            try
            {
                if(con.State == ConnectionState.Closed) con.Open();
                return con.Execute(queryInsert, log);
            }
            catch (Exception ex)
            {
                return 0;
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
                if (con.State == ConnectionState.Closed) con.Open();
                var affectedRows = con.Execute(queryInsert, listLog);
                result = affectedRows;
            }
            catch (Exception e)
            {
                result = 0;
        }
            finally
            {
                con.Close();
            }

            return result;
        }
    }
}
