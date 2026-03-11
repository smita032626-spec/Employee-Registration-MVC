using EmployeeManagementCRUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmployeeManagementCRUD.Controllers
{
    public class EmployeeApiController : ApiController
    {
        BALEmployee bal = new BALEmployee();

        // GET api/EmployeeApi
        [HttpGet]
        public List<EmployeeProperties> Get()
        {
            return bal.FetchEmployeeList();
        }

        // GET api/EmployeeApi/5
        [HttpGet]
        public EmployeeProperties Get(int id)
        {
            return bal.FetchEmployeeById(id);
        }

        // POST api/EmployeeApi
        [HttpPost]
        public string Post(EmployeeProperties emp)
        {
            bal.InsertEmployee(emp);
            return "Employee Created";
        }

        // PUT api/EmployeeApi/5
        [HttpPut]
        public string Put(int id, EmployeeProperties emp)
        {
            emp.Id = id;
            bal.UpdateEmployee(emp);
            return "Employee Updated";
        }

        // DELETE api/EmployeeApi/5
        [HttpDelete]
        public string Delete(int id)
        {
            bal.DeleteEmployee(id);
            return "Employee Deleted";
        }
    }
}