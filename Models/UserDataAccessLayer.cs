using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using PatientTrackingList.DataServices;

namespace PatientTrackingList.Models
{
    public class UserDataAccessLayer : Controller
    {
        public static IConfiguration Configuration { get; set; }
        private readonly SqlServices _sql = new SqlServices(Configuration);

        private static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                //.AddJsonFile("appsettings.json")
                .AddJsonFile("secrets.json");

            Configuration = builder.Build();
            string connectionString = Configuration["ConnectionStrings:ConString"];
           
            return connectionString;
        }

        string connectionString = GetConnectionString();


        public string ValidateLogin(UserDetails user)
        {            
            return _sql.ValidateLogin(user);
        }
    }
}
