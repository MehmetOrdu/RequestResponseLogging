using RequestResponseLogging.API.Models;
using RequestResponseLogging.API.Services.Interfaces;
using RequestResponseLogging.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestResponseLogging.API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly RequestResponseContext _context;
        public EmployeeService(RequestResponseContext context)
        {
            _context = context;
        }
        public void Insert(EmployeeViewModel model)
        {
            _context.Employee.Add(new Employee
            {
                Name = model.Name,
                LastName = model.LastName,
                Gender = model.Gender,
                CreateDate = DateTime.Now
            });
            _context.SaveChanges();
        }
        public List<Employee> Get()
        {
            return _context.Employee.ToList();
        }
    }
}
