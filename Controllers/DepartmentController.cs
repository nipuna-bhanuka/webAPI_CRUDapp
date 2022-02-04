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

namespace vslearnAPINew.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                      select depId, depName 
                      from department
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using(SqlConnection mycon = new SqlConnection(sqlDataSource)) 
            {
                mycon.Open();
                using (SqlCommand myCommond = new SqlCommand(query,mycon))
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
        public JsonResult Post(Department dep)
        {
            string query = @"
                      insert into department
                      values (@DepName)
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand myCommond = new SqlCommand(query, mycon))
                {
                    myCommond.Parameters.AddWithValue("@DepName", dep.depName);
                    myReader = myCommond.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Added Sucessfully");
        }


        [HttpPut]
        public JsonResult Put(Department dep)
        {
            string query = @"
                      update department
                      set depName = @DepName
                      where depId = @DepID
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand myCommond = new SqlCommand(query, mycon))
                {
                    myCommond.Parameters.AddWithValue("@DepId", dep.depID);
                    myCommond.Parameters.AddWithValue("@DepName", dep.depName);
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
                      delete from department
                      where depId = @DepID
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand myCommond = new SqlCommand(query, mycon))
                {
                    myCommond.Parameters.AddWithValue("@DepId", id);
                    myReader = myCommond.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Delete Sucessfully");
        }
    }
}
