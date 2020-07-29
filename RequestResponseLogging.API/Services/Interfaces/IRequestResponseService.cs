using RequestResponseLogging.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestResponseLogging.API.Services.Interfaces
{
   public interface IRequestResponseService
    {
        void Insert(RequestResponseLog entity);
        List<RequestResponseLog> Get();
    }
}
