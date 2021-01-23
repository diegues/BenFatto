using Api_UploadFileLog.Entidades;
using Api_UploadFileLog.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

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
        /// Insert Log por parametros
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
        public string Insert(string ip, string local, string usuario, string data, string zone, string requisicao, string status, string time, string origem, string software)
        {
            var result = string.Empty;
            try
            {
                //Entidade
                Log log = new Log(0, ip, local, usuario, ConvertDateTime(data), zone, requisicao, IntTryParseNullable(status), IntTryParseNullable(time), origem, software);

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

        /// <summary>
        /// Insert Log por Model
        /// </summary>
        [HttpPost]
        [Route("CreateLog")]
        public IActionResult PostCreateLog([FromBody] LogModel logModel)
        {
            var result = string.Empty;
            Log log = new Log(0, logModel.ip, logModel.local, logModel.usuario, ConvertDateTime(logModel.data), logModel.zone, logModel.requisicao, IntTryParseNullable(logModel.status), IntTryParseNullable(logModel.time), logModel.origem, logModel.software);
            if (new LogRepository(_configuration).Add(log) > 0)
            {
                result = "Inserido com sucesso.";
            }
            else
            {
                result = "Erro ao inserir dados.";
            }
            return Json(logModel);
        }

        [HttpPost]
        [Route("SelectFields")]
        public string SelectFields(string id = null, string ip = null, string local = null, string usuario = null, string data = null, string zone = null, string requisicao = null, string status = null, string time = null, string origem = null, string software = null)
        {
            var result = string.Empty;
            try
            {
                DateTime dataF = new DateTime();
                if (data != null)
                    dataF = ConvertDateTime(data);

                //Entidade
                Log log = new Log(Convert.ToInt64(id), ip, local, usuario, dataF, zone, requisicao, IntTryParseNullable(status), IntTryParseNullable(time), origem, software);
                List<LogModel> logRetorn = new LogRepository(_configuration).SelectWithParameters(log);

                result = JsonSerializer.Serialize(logRetorn, new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                    WriteIndented = true
                });
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.ToString()));
            }

            return result;
        }
    }
}
