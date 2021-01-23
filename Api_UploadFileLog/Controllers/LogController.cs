using Api_UploadFileLog.Entidades;
using Api_UploadFileLog.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace Api_UploadFileLog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : BaseController
    {
        private readonly IConfiguration _configuration;
        public LogController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Insert Log 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="local"></param>
        /// <param name="usuario"></param>
        /// <param name="data"></param>
        /// <param name="requisicao"></param>
        /// <param name="status"></param>
        /// <param name="time"></param>
        /// <param name="origem"></param>
        /// <param name="software"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Insert")]
        public string Insert(string ip, string local, string usuario, string data, string requisicao, string status, string time, string origem, string software)
        {
            var result = string.Empty;
            try
            {
                //Entidade
                Log log = new Log(0, ip, local, usuario, ConvertDateTime(data), ConvertTimeZone(data), requisicao, IntTryParseNullable(status), IntTryParseNullable(time), origem, software);

                if (new LogRepository(_configuration).Add(log) > 0)
                {
                    result = "Inserido com sucesso.";
                }
                else
                {
                    result = "Erro ao inserir dados.";
                }
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.ToString()));
            }

            return result;
        }

        [HttpPost]
        [Route("InsertJson")]
        public string InsertJson(Log log)
        {
            var result = string.Empty;
            try
            {
                //Entidade
                //Log log = new Log(0, ip, local, usuario, ConvertDateTime(data), requisicao, IntTryParseNullable(status), IntTryParseNullable(time), origem, software);

                //if (new LogRepository(_configuration).Add(log) > 0)
                //{
                //    result = "Inserido com sucesso.";
                //}
                //else
                //{
                //    result = "Erro ao inserir dados.";
                //}
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.ToString()));
            }

            return result;
        }
    }
}
