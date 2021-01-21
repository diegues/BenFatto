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
                        sb.AppendLine(reader.ReadLine());
                }
            }
            catch(Exception exc)
            {
                return  exc.ToString();
            }

            return sb.ToString();
        }
    }
}
