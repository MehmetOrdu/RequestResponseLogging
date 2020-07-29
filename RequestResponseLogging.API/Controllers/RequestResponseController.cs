using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RequestResponseLogging.API.Services;
using RequestResponseLogging.API.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RequestResponseLogging.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestResponseController : ControllerBase
    {
        private readonly IRequestResponseService _service;

        public RequestResponseController(IRequestResponseService service)
        {
            _service = service;
        }
        // GET: api/<RequestResponseController>
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_service.Get());
        }
    }
}
