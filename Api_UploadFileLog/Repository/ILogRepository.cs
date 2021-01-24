using Api_UploadFileLog.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_UploadFileLog.Repository
{
    public interface ILogRepository
    {
        int Add(Log log);

        int AddList(List<Log> listLog);

        List<LogModel> SelectWithParameters(Log log);
    }
}
