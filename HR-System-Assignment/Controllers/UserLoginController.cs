using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HR_System_Assignment.Data;
using HR_System_Assignment.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace HR_System_Assignment.Controllers
{
    public class UserLoginController : Controller
    {
       // private readonly HR_System_AssignmentContext _context;
        private readonly IConfiguration _configuration;

        public UserLoginController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // GET: UserLogin
        public IActionResult Index()
        {
            DataTable dataTable= new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
             sqlConnection.Open();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("viewLogins", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.Fill(dataTable);

            }
              return View(dataTable);
        }

        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if(claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Employee");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginViewModel userLoginViewModel)
        {
            if (userLoginViewModel.Email != null && userLoginViewModel.Password != null)
            {
                var authenticationResult = Authenticate(userLoginViewModel);
                if (authenticationResult)
                {
                    var claims = new List<Claim>
                {
            new Claim(ClaimTypes.NameIdentifier, userLoginViewModel.Email),
            new Claim("OtherProperties", "Example Role"),
             };
                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        IsPersistent = true,
                    };
                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                               new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Employee");
                }
            }
            return View();
        }

        public bool Authenticate(UserLoginViewModel userLoginView)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("spIsAuthUser", sqlConnection);
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("email", userLoginView.Email);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("password", userLoginView.Password);
                sqlDataAdapter.Fill(dataTable);
                if (dataTable.Rows.Count == 1)
                {
                    return true;
                }
                else
                {
                    ViewData["ValidateMessage"] = "empty";
                }
            }
            return false;
        }
    }
}
