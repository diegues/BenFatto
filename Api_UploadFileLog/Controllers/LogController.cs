using Api_UploadFileLog.Entidades;
using Api_UploadFileLog.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Api_UploadFileLog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : BaseController
    {
        private readonly ILogRepository _logRepository;

        public LogController(ILogRepository logRepository)
        {
            _logRepository = logRepository;
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
                Log log = new Log(0, ipAddressValido(ip), local, usuario, ConvertDateTime(data), zone, requisicao, IntTryParseNullable(status), IntTryParseNullable(time), origem, software);

                if (_logRepository.Add(log) > 0)
                {
                    result = "Inserido com sucesso.";
                }
                else
                {
                    result = "Erro ao inserir dados.";
                }
            }
            catch (ArgumentException ex)
            {
                result = ex.Message;
            }
            catch (DataException ex)
            {
                result = ex.Message;
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
        public string PostCreateLog([FromBody] LogModel log)
        {
            var result = string.Empty;
            try
            {
                Log logModel = new Log(0, ipAddressValido(log.ip), log.local, log.usuario, ConvertDateTime(log.data), log.zone, log.requisicao, IntTryParseNullable(log.status), IntTryParseNullable(log.time), log.origem, log.software);
                if (_logRepository.Add(logModel) > 0)
                {
                    result = "Inserido com sucesso.";
                }
                else
                {
                    result = "Erro ao inserir dados.";
                }
            }
            catch (ArgumentException ex)
            {
                result = ex.Message;
            }
            catch (DataException ex)
            {
                result = ex.Message;
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.ToString()));
            }
            return result;
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

                Int64 outValue = 0;
                Int64.TryParse(id, out outValue);

                //Entidade
                Log log = new Log(Convert.ToInt64(id), ipAddressValido(ip), local, usuario, dataF, zone, requisicao, IntTryParseNullable(status), IntTryParseNullable(time), origem, software);
                List<LogModel> logRetorn = _logRepository.SelectWithParameters(log);

                if (logRetorn == null || logRetorn.Count == 0)
                {
                    throw new ArgumentException("Sem dados para exibir!");
                }

                result = JsonSerializer.Serialize(logRetorn, new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                    WriteIndented = true
                });
            }
            catch (ArgumentException ex)
            {
                result = ex.Message;
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
            }

            return result;
        }
    }
}
