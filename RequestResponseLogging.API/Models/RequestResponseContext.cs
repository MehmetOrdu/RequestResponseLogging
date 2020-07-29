using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestResponseLogging.API.Models
{
    public class RequestResponseContext : DbContext
    {
        public RequestResponseContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<RequestResponseLog> RequestResponse { get; set; }

    }
}
