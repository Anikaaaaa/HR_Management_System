using HR_System_Assignment.Models;
using HR_System_Assignment.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Numerics;

namespace HR_System_Assignment.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IConfiguration _configuration;

        public EmployeeController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public IActionResult Index(string srch = "", string dept = "All")
        {
            List<string> allDepts = new List<string>();
            allDepts = GetAllDepts();
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("spGetAllEmpWithSearchAndSort", sqlConnection);
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("word", srch);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("dept", dept);
                sqlDataAdapter.SelectCommand.ExecuteNonQuery();
                sqlDataAdapter.Fill(dataTable);
            }
            ViewData["MyData"] = allDepts;
            var employees = getAllEmployees(dataTable);
            return View(employees);
        }

        public IActionResult Index1()
        { 
            List<string> allDepts = new List<string>();
            allDepts = GetAllDepts();
            DataTable dataTable = new DataTable();   
            string query = "SELECT * FROM EMPLOYEES";
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                sqlDataAdapter.SelectCommand.ExecuteNonQuery();
                sqlDataAdapter.Fill(dataTable);
            }
            ViewData["MyData"] = allDepts;
            var employees = getAllEmployees(dataTable);
            return View(employees);
        }

        public List<EmployeeViewModel> getAllEmployees(DataTable dataTable)
        {
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();
            for(int i=0; i<dataTable.Rows.Count; i++)
            {
                EmployeeViewModel employee = new EmployeeViewModel();
                employee.Id = Convert.ToInt32(dataTable.Rows[i]["emp_id"].ToString());
                employee.Name = dataTable.Rows[i]["emp_name"].ToString();
                var photo = dataTable.Rows[i]["emp_photo"].ToString();
                var photoName = getName(photo);
                employee.PhotoLink = photoName;
                employee.JoiningDate = Convert.ToDateTime(dataTable.Rows[i]["emp_joining_date"]).ToString("MM/dd/yyyy").Split(" ")[0];
                employee.ManagerName = dataTable.Rows[i]["manager_name"].ToString();
                employee.Deptartment = getDeptName(employee.Id);
                employees.Add(employee);
            }
            return employees;
        }

            public IActionResult SearchAndSort(string srch = "", string dept = "All")
        {
            List<string> allDepts = new List<string>();
            allDepts = GetAllDepts();
            DataTable dataTable = new DataTable();
            string search = "";
            if(srch != null)
                search= srch;
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("spGetAllEmpWithSearchAndSort", sqlConnection);
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("word", search);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("dept", dept);
                sqlDataAdapter.SelectCommand.ExecuteNonQuery();
                sqlDataAdapter.Fill(dataTable);
            }
            ViewData["MyData"] = allDepts;
            var employees = getAllEmployees(dataTable);
            return PartialView("~/Views/Shared/_EmployeeTablePartialView.cshtml", employees);
        }
        public List<string> GetAllDepts()
        {
            DataTable dataTable = new DataTable();
            List<string> departments = new List<string>();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("spGetAllDepts", sqlConnection);
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.ExecuteNonQuery();
                sqlDataAdapter.Fill(dataTable);
            }
            if (dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    string dept_names = dataTable.Rows[i]["dept_name"].ToString();
                    departments.Add(dept_names);
                }
            }
            return departments;
        }

        public IActionResult AddOrEditEmployee(int? id)
        {
            EmployeeViewModel employee = new EmployeeViewModel();
            List<string> departments = GetAllDepts();
            employee.Departments = departments;
            if (id != 0 && id != null)
            {
                DataTable dataTable2 = new DataTable();
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("spGetEmpById", sqlConnection);
                    sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("id", id);
                    sqlDataAdapter.SelectCommand.ExecuteNonQuery();
                    sqlDataAdapter.Fill(dataTable2);
                }
                if (dataTable2.Rows.Count > 0)
                {
                    employee.Id = Convert.ToInt32(dataTable2.Rows[0]["emp_id"].ToString());
                    employee.Name = dataTable2.Rows[0]["emp_name"].ToString();
                    var photo = dataTable2.Rows[0]["emp_photo"].ToString();
                    var photoName = getName(photo);
                    employee.PhotoLink = photoName;
                    employee.JoiningDate = Convert.ToDateTime (dataTable2.Rows[0]["emp_joining_date"]).ToString("MM/dd/yyyy").Split(" ")[0];
                    employee.ManagerName = dataTable2.Rows[0]["manager_name"].ToString();
                    string status = dataTable2.Rows[0]["emp_status"].ToString();
                    int dept = Convert.ToInt32(dataTable2.Rows[0]["dept_id"].ToString());

                    if(status == "Activate")
                        employee.Status = true; 
                    else  
                        employee.Status = false;

                    employee.Deptartment = getDeptName(id);
                }
            }
            return View(employee);
        }

        public string getName(string photoLink)
        {
            var fileName = Path.GetFileName(photoLink);
            return fileName;
        }

        public string getDeptName(int? id)
        {
            DataTable dataTable3 = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("spGetNameByDeptId", sqlConnection);
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("dept_id", id);
                sqlDataAdapter.SelectCommand.ExecuteNonQuery();
                sqlDataAdapter.Fill(dataTable3);
            }
            if (dataTable3.Rows.Count > 0)
            {
                string deptName = dataTable3.Rows[0]["dept_name"].ToString();
                return deptName;
            }
            return null;
        }
        public List<string> GetDepts()
        {
            List<string> departments = new List<string>();
            DataTable dataTable1 = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("spGetAllDepts", sqlConnection);
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.ExecuteNonQuery();
                sqlDataAdapter.Fill(dataTable1);
            }
            if (dataTable1.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable1.Rows.Count; i++)
                {
                    string dept_names = dataTable1.Rows[i]["dept_name"].ToString();
                    departments.Add(dept_names);
                }
            }
            return departments;
        }

        public int getDeptId(string dept)
        {
            int deptId = 0;
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("spGetIdByDeptName", sqlConnection);
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("dept_name", dept);
                sqlDataAdapter.SelectCommand.ExecuteNonQuery();
                sqlDataAdapter.Fill(dataTable);
            }
            if (dataTable.Rows.Count > 0)
            {
                deptId = Convert.ToInt32(dataTable.Rows[0]["dept_id"].ToString());
                return deptId;
            }
            return deptId;
        }
        public string UploadPhoto(IFormFile Photo)
        {
            if (Photo != null && Photo.Length > 0)
            {
                var fileName = Path.GetFileName(Photo.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Images", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Photo.CopyTo(fileStream);
                }
                return filePath;
            }
            return "";
        }

       
        [HttpPost]
        public IActionResult AddOrEditEmployee(EmployeeViewModel employee)
        {
            string status;
            string managername = "";
            int? deptId;
            
            employee.Departments = GetAllDepts();
            if (ModelState.IsValid)
            {
                if (employee.Status == true)
                {
                    status = "Activate";
                }
                else
                {
                    status = "Diactivate";
                }
                if(employee.Photo != null)
                {
                    employee.PhotoLink = UploadPhoto(employee.Photo);
                }
                else if(employee.PhotoLink!= null)
                {
                    employee.PhotoLink = employee.PhotoLink;
                }
                else
                {
                    employee.PhotoLink = "";
                }
                if(employee.ManagerName!= null) { 
                    managername= employee.ManagerName;
                }
                deptId = getDeptId(employee.Deptartment);
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("EmpAddOrUpdate", sqlConnection);
                    sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("emp_id", employee.Id);
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("emp_name", employee.Name);
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("emp_photo", employee.PhotoLink);
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("emp_joining_date", employee.JoiningDate);
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("manager_name", managername);
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("emp_status", status);
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("dept_id", deptId);
                    sqlDataAdapter.SelectCommand.ExecuteNonQuery();
                }
                return RedirectToAction("Index", "Employee");
            }
            return View(employee);
        }

        public IActionResult Delete(int id, string srch, string dept)
        {

            DataTable dataTable = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("spDeleteEmp", sqlConnection);
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("id", id);
                sqlDataAdapter.SelectCommand.ExecuteNonQuery();
            }

            return RedirectToAction("SearchAndSort", "Employee", new
            {
                 srch,
                 dept
            }); 
        }
    }
}
