using Api_UploadFileLog.Entidades;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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

        public List<LogModel> SelectWithParameters(Log log)
        {
            List<LogModel> result = new List<LogModel>();
            try
            {
                if (con.State == ConnectionState.Closed) con.Open();

                StringBuilder sb = new StringBuilder();
                sb.Append(" SELECT * FROM log t WHERE 1 = 1");

                if (log.id > 0)
                    sb.Append(" AND id = @id ");

                if (!string.IsNullOrEmpty(log.ip))
                {
                    log.ip = string.Format("%{0}%", log.ip);
                    sb.Append(" AND ip like @ip ");
                }

                if (!string.IsNullOrEmpty(log.local))
                {
                    log.local = string.Format("%{0}%", log.local);
                    sb.Append(" AND local like @local ");
                }

                if (!string.IsNullOrEmpty(log.usuario))
                {
                    log.usuario = string.Format("%{0}%", log.usuario);
                    sb.Append(" AND usuario like @usuario  ");
                }

                if (log.data != new DateTime())
                    sb.Append(" AND data = @data ");

                if (!string.IsNullOrEmpty(log.zone))
                {
                    log.zone = string.Format("%{0}%", log.zone);
                    sb.Append(" AND zone like @zone  ");
                }

                if (!string.IsNullOrEmpty(log.requisicao))
                {
                    log.requisicao = string.Format("%{0}%", log.requisicao);
                    sb.Append(" AND requisicao like @requisicao  ");
                }

                if (!string.IsNullOrEmpty(log.status.ToString()))
                    sb.Append(" AND status = @status ");

                if (!string.IsNullOrEmpty(log.time.ToString()))
                    sb.Append(" AND time = @time ");

                if (!string.IsNullOrEmpty(log.origem))
                {
                    log.origem = string.Format("%{0}%", log.origem);
                    sb.Append(" AND origem like @origem  ");
                }

                if (!string.IsNullOrEmpty(log.software))
                {
                    log.software = string.Format("%{0}%", log.software);
                    sb.Append(" AND software like @software ");
                }

                var sqlString = sb.ToString();
                result = con.Query<LogModel>(sqlString, log).ToList();
            }
            catch (Exception e)
            {

            }
            finally
            {
                con.Close();
            }

            return result;
        }
    }
}
