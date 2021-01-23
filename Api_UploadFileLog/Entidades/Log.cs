using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_UploadFileLog.Entidades
{
    public class Log
    {
        public Log(Int64 id, string _ip, string _local, string _usuario, DateTime _data, string _zone, string _requisicao, int? _status, int? _time, string _origem, string _software)
        {
            id = _id;
            ip = _ip;
            local = _local;
            usuario = _usuario;
            data = _data;
            zone = _zone;
            requisicao = _requisicao;
            status = _status;
            time = _time;
            origem = _origem;
            software = _software;


        }
        private Int64 _id;

        public Int64 id { get => _id; set => _id = value; }

        private string _ip;
        public string ip { get => _ip ; private  set => _ip = value; }

        private string _local;
        public string local { get => _local; private set => _local = value; }

        private string _usuario;
        public string usuario { get => _usuario; private set => _usuario = value; }

        private DateTime _data;
        public DateTime data { get => _data; private set => _data = value; }

        private string _requisicao;
        public string requisicao { get => _requisicao; private set => _requisicao = value; }

        private string _zone;
        public string zone { get => _zone; private set => _zone = value; }

        private int? _status;
        public int? status { get => _status; private set => _status = value; }

        private int? _time;
        public int? time { get => _time; private set => _time = value; }

        private string _origem;
        public string origem { get => _origem; private set => _origem = value; }

        private string _software;
        public string software { get => _software; private set => _software = value; }
    }
}
