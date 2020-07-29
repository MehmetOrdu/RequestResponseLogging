using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RequestResponseLogging.API.Models
{
    public class RequestResponseLog
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
        public string QueryString { get; set; }
        public string RequestHeader { get; set; }
        public string RequestBody { get; set; }
        public DateTime RequestDate { get; set; }
        public string ResponseHeader { get; set; }
        public string ResponseContent { get; set; }
        public DateTime ResponseDate { get; set; }
        public string UserName { get; set; }
        public string DeviceId { get; set; }
        public string TraceId { get; set; }
        public string MachineId { get; set; }
        public string LogType { get; set; }
        public string MethodInfo { get; set; }
        public string ExtraMessage { get; set; }
        public string Scheme { get; set; }
        public int StatusCode { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
