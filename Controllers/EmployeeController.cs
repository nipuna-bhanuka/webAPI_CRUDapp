using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using vslearnAPINew.Models;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace vslearnAPINew.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _env;
        public EmployeeController(IConfiguration configuration, IHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                      select empId, empName , departmentName , dateOfJoined , photoOfFileName 
                      from employee
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand myCommond = new SqlCommand(query, mycon))
                {
                    myReader = myCommond.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Employee emp)
        {
            string query = @"
                      insert into employee (empName,departmentName,dateOfJoined,photoOfFileName )
                      values (@EmpName,@DepartmentName,@DateOfJoined,@PhotoOfFileName )
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand myCommond = new SqlCommand(query, mycon))
                {
                    myCommond.Parameters.AddWithValue("@EmpName", emp.empName);
                    myCommond.Parameters.AddWithValue("@DepartmentName", emp.departmentName);
                    myCommond.Parameters.AddWithValue("@DateOfJoined", emp.dateOfJoined);
                    myCommond.Parameters.AddWithValue("@PhotoOfFileName", emp.photoOfFileName);
                    myReader = myCommond.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Employee Added Sucessfully");
        }


        [HttpPut]
        public JsonResult Put(Employee emp)
        {
            string query = @"
                      update employee
                      set empName = @EmpName,
                          departmentName=@DepartmentName, 
                          dateOfJoined=@DateOfJoined, 
                          photoOfFileName=@PhotoOfFileName 
                      where empId = @EmpID
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand myCommond = new SqlCommand(query, mycon))
                {
                    myCommond.Parameters.AddWithValue("@EmpID", emp.empID);
                    myCommond.Parameters.AddWithValue("@EmpName", emp.empName);
                    myCommond.Parameters.AddWithValue("@DepartmentName", emp.departmentName);
                    myCommond.Parameters.AddWithValue("@DateOfJoined", emp.dateOfJoined);
                    myCommond.Parameters.AddWithValue("@PhotoOfFileName", emp.photoOfFileName);
                    myReader = myCommond.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Update Sucessfully");
        }


        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                      delete from employee
                      where empId = @EmpID
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand myCommond = new SqlCommand(query, mycon))
                {
                    myCommond.Parameters.AddWithValue("@EmpID", id);
                    myReader = myCommond.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Delete Sucessfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + fileName;

                using(var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(fileName);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }
    }
}
