using RequestResponseLogging.API.Models;
using RequestResponseLogging.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestResponseLogging.API.Services.Interfaces
{
  public  interface IEmployeeService
    {
        void Insert(EmployeeViewModel model);
        List<Employee> Get();
    }
}
