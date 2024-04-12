using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PatientTrackingList.Data;
using PatientTrackingList.Models;
using System.Data;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Collections.Generic;

namespace PatientTrackingList.DataServices
{
    public class Exporter : Controller
    {
        public string dlFilePath;
        private readonly DataContext _context;
        private readonly MetaData _meta;

        public Exporter(DataContext context)
        {
            _context = context;
            _meta = new MetaData(_context);
        }
        public void ExportPTL(List<PTL> ptlToExport, string username)
        {
            DataTable table = new DataTable();
            
            table.Columns.Add("CGU Number", typeof(string));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Ref Date", typeof(string));
            table.Columns.Add("Clock Start", typeof(string));
            table.Columns.Add("Reason", typeof(string));
            table.Columns.Add("Urgency", typeof(string));
            table.Columns.Add("TCI Date", typeof(string));
            table.Columns.Add("Clock Ticking (Days)", typeof(int));
            table.Columns.Add("Clock Ticking (Weeks)", typeof(int));
            table.Columns.Add("Clock Days At TCI", typeof(string));
            table.Columns.Add("Comments", typeof(string));
            table.Columns.Add("Checked", typeof(string));

            foreach (var ptl in ptlToExport) 
            {
                int ctdays = 0;
                int ctweeks = 0;
                int tcidays = 0;
                string tcidate = "N/A";

                if (ptl.ClockStart != null)
                {
                    TimeSpan ct = DateTime.Now - ptl.ClockStart.GetValueOrDefault();
                    ctdays = (int)ct.TotalDays;
                    ctweeks = ctdays / 7;
                }
                if (ptl.TCIDate != null)
                {
                    tcidate = ptl.TCIDate.Value.ToString("dd/MM/yyyy");
                    TimeSpan tci = ptl.TCIDate.GetValueOrDefault() - ptl.ClockStart.GetValueOrDefault();
                    tcidays = (int)tci.TotalDays;
                }
                table.Rows.Add(ptl.CGUNo, 
                    ptl.PatientName, 
                    ptl.ReferralDate.Value.ToString("dd/MM/yyyy"), 
                    ptl.ClockStart.Value.ToString("dd/MM/yyyy"),
                    ptl.ReferralReason, 
                    ptl.Class, 
                    tcidate,
                    ctdays, 
                    ctweeks, 
                    tcidays, 
                    ptl.Comments, 
                    ptl.isChecked);
            }

            //return table;
            ToCSV(table, username);
        }

        public void ToCSV(DataTable table, string username)
        {
            //string filePath = $"C:\\CGU_DB\\ptl-{username}.csv";
            string fileName = $"ptl-{username}.csv";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Downloads\\" + fileName);
            StreamWriter sw = new StreamWriter(filePath, false);
            //headers
            for (int i = 0; i < table.Columns.Count; i++)
            {
                sw.Write(table.Columns[i]);
                if (i < table.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < table.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }            
            sw.Close();

            dlFilePath = filePath;

            //DownloadFile(filePath);
        }

        [HttpGet("download")]
        //public async Task<IActionResult> DownloadFile(string filePath)
        public async Task<IActionResult> DownloadFile(string consultantFilter, string gcFilter, string pathwayFilter, string username)
        {
            string filePath = "";
            
            List<PTL> ptlToExport = new List<PTL>();
            ptlToExport = _meta.GetPTLList().ToList();

            if (pathwayFilter != null && pathwayFilter != "")
            {
                ptlToExport = ptlToExport.Where(p => p.ReferralReason == pathwayFilter).ToList();
            }

            if (consultantFilter != null && consultantFilter != "")
            {
                ptlToExport = ptlToExport.Where(p => p.ReferralConsultant == consultantFilter).ToList();
            }

            if (gcFilter != null && gcFilter != "")
            {
                ptlToExport = ptlToExport.Where(p => p.ReferralGC == gcFilter).ToList();
            }


            ExportPTL(ptlToExport, username);

            filePath = dlFilePath;


            if (System.IO.File.Exists(filePath))
            {
                return File(System.IO.File.ReadAllBytes(filePath), "text/csv", System.IO.Path.GetFileName(filePath));
            }
            return Redirect("Error");
        }


    }
}
