using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_UploadFileLog.Repository
{
    public class BaseRepository
    {
        private readonly  IConfiguration _configuration;
        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnection()
        {
            return _configuration.GetSection("ConnectionStrings").GetSection("DatabaseConnection").Value;
        }
    }
}
