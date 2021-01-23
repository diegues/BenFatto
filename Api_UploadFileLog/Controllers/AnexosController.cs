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
    public class AnexosController : BaseController
    {
        private readonly IConfiguration _configuration;
        public AnexosController(IConfiguration configuration)
        {
            _configuration = configuration;
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
                        Log log = new Log(0, ip, local, usuario, ConvertDateTime(data), ConvertTimeZone(data), requisicao, IntTryParseNullable(status), IntTryParseNullable(time), origem, software);
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

            return result;
        }
    }
}
