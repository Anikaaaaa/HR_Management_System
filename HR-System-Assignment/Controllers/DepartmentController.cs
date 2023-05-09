using HR_System_Assignment.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HR_System_Assignment.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public IActionResult Index(string srch = "")
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("spGetDeptTable", sqlConnection);
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("word", srch);
                sqlDataAdapter.SelectCommand.ExecuteNonQuery();
                sqlDataAdapter.Fill(dataTable);
            }
            return View(dataTable);
        }

        public IActionResult Search(string srch = "")
        {
            DataTable dataTable = new DataTable();
            string search = "";
            if (srch != null)
                search = srch;
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("spGetDeptTable", sqlConnection);
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("word", search);
                sqlDataAdapter.SelectCommand.ExecuteNonQuery();
                sqlDataAdapter.Fill(dataTable);
            }
            return PartialView("~/Views/Shared/_DepartmentTablePartialView.cshtml", dataTable);
        }

        public IActionResult AddOrUpdate(int? id)
        {
            DepartmentViewModel departmentViewModel = new DepartmentViewModel();
            if (id > 0)
            {
                DataTable dataTable = new DataTable();
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("spGetDeptById", sqlConnection);
                    sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("id", id);
                    sqlDataAdapter.SelectCommand.ExecuteNonQuery();
                    sqlDataAdapter.Fill(dataTable);
                }
                if (dataTable.Rows.Count > 0)
                {
                    departmentViewModel.Id = Convert.ToInt32(dataTable.Rows[0]["dept_id"].ToString());
                    departmentViewModel.Name = dataTable.Rows[0]["dept_name"].ToString();
                    departmentViewModel.Description = dataTable.Rows[0]["dept_description"].ToString();
                }
            }
            return View(departmentViewModel);
        }

        [HttpPost]
        public IActionResult AddOrUpdate(DepartmentViewModel departmentViewModel)
        {
            if(ModelState.IsValid)
            {
                bool isValidName = IsValidName(departmentViewModel.Name);
                
                if (isValidName || departmentViewModel.Id != 0)
                {
                    if (departmentViewModel.Description == null)
                        departmentViewModel.Description = "";
                    using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                    {
                        sqlConnection.Open();
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("DeptAddOrUpdate", sqlConnection);
                        sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        sqlDataAdapter.SelectCommand.Parameters.AddWithValue("dept_id", departmentViewModel.Id);
                        sqlDataAdapter.SelectCommand.Parameters.AddWithValue("dept_name", departmentViewModel.Name);
                        sqlDataAdapter.SelectCommand.Parameters.AddWithValue("description", departmentViewModel.Description);
                        sqlDataAdapter.SelectCommand.ExecuteNonQuery();
                    }
                    return RedirectToAction("Index", "Department");
                }
                else
                {
                    ViewData["ValidateMessage"] = "already exists";
                }   
            }
            return View(departmentViewModel);
        }

        public int? getEmpCount(int id)
        {
            int empNo = 0 ;
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("spGetEmpCount", sqlConnection);
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("id", id);
                sqlDataAdapter.SelectCommand.ExecuteNonQuery();
                sqlDataAdapter.Fill(dataTable);
            }
            if (dataTable.Rows.Count > 0)
            {
                empNo = Convert.ToInt32(dataTable.Rows[0]["total"].ToString());
            }
            return empNo;
        }

        public bool IsValidName(string name)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("spIsValidName", sqlConnection);
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("name", name);
                sqlDataAdapter.SelectCommand.ExecuteNonQuery();
                sqlDataAdapter.Fill(dataTable);
            }
            if(dataTable.Rows.Count == 0)
                return true;
            return false;
        }

        public IActionResult Delete(int id, string srch)
        {

            DataTable dataTable = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("spDeleteDept", sqlConnection);
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("id", id);
                sqlDataAdapter.SelectCommand.ExecuteNonQuery();
            }

            return RedirectToAction("Search", "Department", new
            {
                srch
            });
        }

    }
}
