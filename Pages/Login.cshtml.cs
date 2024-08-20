using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using PatientTrackingList.Models;

namespace PatientTrackingList.Pages
{
    public class LoginModel : PageModel
    {
        UserDataAccessLayer objUser = new UserDataAccessLayer();
        public UserDetails? UserDetails { get; set; }

        public async void OnGet(string username, string sPassword)
        {
            //if (ModelState.IsValid)
            if (username != null)
            {
                UserDetails user = new UserDetails();
                user.EMPLOYEE_NUMBER = username;
                user.PASSWORD = sPassword;

                string LoginStatus = objUser.ValidateLogin(user);

                if (LoginStatus == "Success")
                {

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.EMPLOYEE_NUMBER)
                    };
                    ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                    await HttpContext.SignInAsync(principal);
                    Response.Redirect("Index");
                }
                else
                {

                    TempData["error"] = "Login failed. Please try again.";
                    //return View();
                }
            }
        }
    }
}
