using RequestResponseLogging.API.Models;
using RequestResponseLogging.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestResponseLogging.API.Services
{
    public class RequestResponseService: IRequestResponseService
    {
        private readonly RequestResponseContext _context;
        public RequestResponseService(RequestResponseContext context)
        {
            _context =context;
        }
        public void Insert(RequestResponseLog entity)
        {
            try
            {
                _context.RequestResponse.Add(entity);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<RequestResponseLog> Get()
        {
            return _context.RequestResponse.ToList();
        }
    }
}
