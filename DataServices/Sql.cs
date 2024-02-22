using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PatientTrackingList.Data;

namespace PatientTrackingList.DataServices
{
    public class SqlServices
    {
        private readonly IConfiguration _config;
        private SqlConnection con;
        private SqlCommand cmd;
        public SqlServices(IConfiguration config)
        {
            _config = config;
            con = new SqlConnection(_config.GetConnectionString("ConString"));
            cmd = new SqlCommand("", con);
        }


        public void SqlUpdateComments(string sComments, int isChecked, string sUsername, string sPPI)
        {
            if (sComments != null)
            {
                cmd.CommandText = "update PTL set comments='" + sComments + "', isChecked=" + isChecked +
                    ", UpdatedBy='" + sUsername + "', UpdatedDate='" + DateTime.Now.ToString("yyyy-MM-dd") +
                    "' where PPI='" + sPPI + "'";
            }
            else
            {
                cmd.CommandText = "update PTL set comments=null, isChecked=" + isChecked +
                    ", UpdatedBy='" + sUsername + "', UpdatedDate='" + DateTime.Now.ToString("yyyy-MM-dd") +
                    "' where PPI='" + sPPI + "'";
            }
            con.Open();                
            cmd.ExecuteNonQuery();
            con.Close();
        }


    }
}
