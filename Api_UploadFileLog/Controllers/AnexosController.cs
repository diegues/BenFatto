using Api_UploadFileLog.Entidades;
using Api_UploadFileLog.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api_UploadFileLog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnexosController : Controller
    {
        private readonly IConfiguration _configuration;
        public AnexosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private int? IntTryParseNullable(string val)
        {
            int outValue;
            return int.TryParse(val, out outValue) ? (int?)outValue : null;
        }

        private DateTime ConvertDateTime(string date)
        {
            int day = Convert.ToInt32(date.Substring(0, 2));
            int month = DateTime.ParseExact(date.Substring(3, 3), "MMM", CultureInfo.InvariantCulture).Month;
            int year = Convert.ToInt32(date.Substring(7, 4));

            int hour = Convert.ToInt32(Convert.ToInt32(date.Substring(12, 2)));
            int minute = Convert.ToInt32(Convert.ToInt32(date.Substring(15, 2)));
            int second = Convert.ToInt32(Convert.ToInt32(date.Substring(18, 2)));

            DateTime dateConverted = new DateTime(year, month, day, hour, minute, second);

            return dateConverted;
        }

        private string findData(int start, string caracter, ref string linha)
        {
            string value = "";

            if (linha.Trim().Length > 0)
            {
                int index = linha.Length;
                int cont = 0;

                if (start > 0)
                    linha = linha.Substring(start);

                if (linha.IndexOf(caracter) > 0)
                {
                    index = linha.IndexOf(caracter);
                    cont = 1;
                }


                value = linha.Substring(0, index);
                linha = linha.Substring(value.Length + cont).Trim();
            }
            return value.Trim();
        }

        /// <summary>
        /// Upload File log
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadFile")]
        public string PostFile(IFormFile file)
        {
            var sb = new StringBuilder();
            var lstlog = new List<Log>();
            var result = string.Empty;

            try
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    while (reader.Peek() >= 0)
                    {
                        sb.Clear();
                        sb.AppendLine(reader.ReadLine());

                        string linha = sb.ToString();
                        string ip = findData(0, " ", ref linha);
                        string local = findData(0, " ", ref linha);
                        string usuario = findData(0, "[", ref linha);
                        string data = findData(0, "]", ref linha);
                        string requisicao = findData(1, "\"", ref linha);
                        string status = findData(0, " ", ref linha);
                        string time = findData(0, "\"", ref linha);
                        string origem = findData(0, "\"", ref linha);
                        string software = findData(1, "\"", ref linha);

                        //Fazer Entidade
                        Log log = new Log(0, ip, local, usuario, ConvertDateTime(data), requisicao, IntTryParseNullable(status), IntTryParseNullable(time), origem, software);
                        lstlog.Add(log);
                    }
                }

                if (new LogRepository(_configuration).AddList(lstlog) > 0)
                {
                    result = JsonSerializer.Serialize(lstlog);
                }
                else
                {
                    result = "Erro ao inserir dados.";
                }

            }
            catch (Exception exc)
            {
                return exc.ToString();
            }

            //Retornar Json
            return result;
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
        public string Insert(string ip, string local, string usuario, string data, string requisicao, string status, string time, string origem, string software )
        {
            var result = string.Empty;
            try
            {
                //Entidade
                Log log = new Log(0, ip, local, usuario, ConvertDateTime(data), requisicao, IntTryParseNullable(status), IntTryParseNullable(time), origem, software);

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

        [HttpPost]
        [Route("CreateLog")]
        public IActionResult PostCreateLog([FromBody] LogModel logModel)
        {
            return Json(logModel);
        }
    }
}
