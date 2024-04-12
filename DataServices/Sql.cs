using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.DataServices
{
    public class SqlServices
    {
        private readonly IConfiguration _config;
        private readonly SqlConnection _con;
        private readonly SqlCommand _cmd;
        public SqlServices(IConfiguration config)
        {
            _config = config;
            _con = new SqlConnection(_config.GetConnectionString("ConString"));
            _cmd = new SqlCommand("", _con);
        }


        public void SqlUpdateComments(string comments, int isChecked, string username, string ppi)
        {
            string oldComment = GetOldComments(ppi);

            if (comments != null && oldComment != comments)
            {
                _cmd.CommandText = "update PTL set comments='" + comments + "', isChecked=" + isChecked +
                    ", UpdatedBy='" + username + "', UpdatedDate='" + DateTime.Now.ToString("yyyy-MM-dd") +
                    "' where PPI='" + ppi + "'";
            }
            else
            {
                _cmd.CommandText = "update PTL set comments=null, isChecked=" + isChecked +
                    ", UpdatedBy='" + username + "', UpdatedDate='" + DateTime.Now.ToString("yyyy-MM-dd") +
                    "' where PPI='" + ppi + "'";
            }
            _con.Open();                
            _cmd.ExecuteNonQuery();
            _con.Close();

            SqlWriteAuditUpdate(comments, oldComment, username, ppi);
        }

        public string GetOldComments(string ppi)
        {
            string commentOld = "";
            _cmd.CommandText = "select comments from PTL where PPI = '" + ppi + "'";
            
            _con.Open();
            SqlDataReader reader = _cmd.ExecuteReader();
            if(reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    commentOld = reader.GetString(0);
                }
            }
            _con.Close();
            
            return commentOld;
        }

        public void SqlWriteAuditUpdate(string comments, string oldComments, string username, string ppi)
        {
            _cmd.CommandText = "insert into AuditTable (TableName, RecordPrimaryKey, FieldName, LoginName, " +
                "MachineName, OriginalValue, NewValue, DateEdited, WhoEdited) Values ('PTL', '" + ppi + "', 'Comments', " 
                + "(select EMPLOYEE_NUMBER from STAFF where STAFF_CODE = '" + username + "'), '" + System.Environment.MachineName + "', '" + oldComments + "', '" + comments + "', '"
                + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + username + "')";
            
            _con.Open();
            _cmd.ExecuteNonQuery();
            _con.Close();
        }

        public string ValidateLogin(UserDetails user)
        {   
            SqlCommand cmd = new SqlCommand("sp_ValidateUserLogin", _con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@LoginID", user.EMPLOYEE_NUMBER);
            cmd.Parameters.AddWithValue("@LoginPassword", user.PASSWORD);

            _con.Open();
            string result = cmd.ExecuteScalar().ToString();
            _con.Close();

            return result;
        }

    }
}
