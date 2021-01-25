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
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Api_UploadFileLog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnexosController : BaseController
    {
        private readonly ILogRepository _logRepository;
        public AnexosController(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }


        /// <summary>
        /// Upload File log
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadFile")]
        public IActionResult PostFile(IFormFile file)
        {
            StringBuilder sb = new StringBuilder();
            List<Log> lstlog = new List<Log>();
            StringBuilder result = new StringBuilder();
            int linhaArquivo = 0;

            try
            {
                if (!file.FileName.Contains(".log"))
                    throw new Exception("Arquivo com extensão inválida.");

                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    while (reader.Peek() >= 0)
                    {
                        DateTime dataArquivo = new DateTime();
                        linhaArquivo++;
                        string lArquivo = string.Empty;
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

                        try
                        {
                            ip = ipAddressValido(ip);
                        }
                        catch (ArgumentException)
                        {
                            lArquivo += "Ip Inválido,";
                        }

                        try
                        {
                            dataArquivo = ConvertDateTime(data);
                        }
                        catch (ArgumentException)
                        {
                            lArquivo += "Data Inválida (dd/MMM/yyyy HH:mm:ss),";
                        }

                        if (!string.IsNullOrEmpty(lArquivo))
                        {
                            lArquivo = lArquivo.Remove(lArquivo.Length - 1);
                            result.AppendLine(string.Format("Erro Linha {0}: {1}", linhaArquivo, lArquivo));
                        }
                        else
                        {
                            Log log = new Log(0, ip, local, usuario, dataArquivo, ConvertTimeZone(data), requisicao, IntTryParseNullable(status), IntTryParseNullable(time), origem, software);
                            lstlog.Add(log);
                        }
                    }
                }

                if (_logRepository.AddList(lstlog) == linhaArquivo)
                {
                    return new ObjectResult(lstlog);
                }
                else 
                {
                    return new ObjectResult(result.ToString());
                }

            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message);
            }
        }
    }
}
