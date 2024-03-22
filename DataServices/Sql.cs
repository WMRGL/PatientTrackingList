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
            string OldComment = GetOldComments(sPPI);

            if (sComments != null && OldComment != sComments)
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

            SqlWriteAuditUpdate(sComments, OldComment, sUsername, sPPI);
        }

        public string GetOldComments(string sPPI)
        {
            string sCommentOld = "";
            cmd.CommandText = "select comments from PTL where PPI = '" + sPPI + "'";
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if(reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    sCommentOld = reader.GetString(0);
                }
            }
            con.Close();
            return sCommentOld;
        }

        public void SqlWriteAuditUpdate(string sComments, string sOldComments, string sUsername, string sPPI)
        {
            cmd.CommandText = "insert into AuditTable (TableName, RecordPrimaryKey, FieldName, LoginName, " +
                "MachineName, OriginalValue, NewValue, DateEdited, WhoEdited) Values ('PTL', '" + sPPI + "', 'Comments', " 
                + "(select EMPLOYEE_NUMBER from STAFF where STAFF_CODE = '" + sUsername + "'), '" + System.Environment.MachineName + "', '" + sOldComments + "', '" + sComments + "', '"
                + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + sUsername + "')";
            
            string bleh = cmd.CommandText;

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

    }
}
