using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_UploadFileLog.Entidades
{
    public class LogModel
    {
        public string id { get; set; }
        public string ip { get; set; }
        public string local { get; set; }

        public string usuario { get; set; }

        public string data { get; set; }
        public string zone { get; set; }
        public string requisicao { get; set; }

        public string status { get; set;}

        public string time { get;  set;}

        public string origem { get;  set;}

        public string software { get; set; }
    }
}
