using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api_UploadFileLog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnexosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AnexosController(IConfiguration configuration)
        {
            _configuration = configuration;
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

        [HttpPost]
        [Route("UploadFile")]
        public string PostFile(IFormFile file)
        {
            var sb = new StringBuilder();

            try
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    while (reader.Peek() >= 0)
                    {
                        sb.Clear();
                        sb.AppendLine(reader.ReadLine());

                        //Fazer Entidade

                        string linha = sb.ToString();
                        string ip = findData(0, " ", ref linha);
                        string local = findData(0, " ", ref linha);
                        string user = findData(0, "[", ref linha);
                        string data = findData(0, "]", ref linha);
                        string requisicao = findData(1, "\"", ref linha);
                        string status = findData(0, " ", ref linha);
                        string time = findData(0, "\"", ref linha);
                        string origem = findData(0, "\"", ref linha);
                        string software = findData(1, "\"", ref linha);

                        //Salvar no banco
                    }
                }
            }
            catch (Exception exc)
            {
                return exc.ToString();
            }

            //Retornar Json
            return sb.ToString();
        }
    }
}
