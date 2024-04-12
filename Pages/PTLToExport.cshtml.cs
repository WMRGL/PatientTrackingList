using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PatientTrackingList.Models;
using PatientTrackingList.Data;
using PatientTrackingList.DataServices;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Net;
using System.Net.Mime;
using System;
using System.IO;

namespace PatientTrackingList.Pages
{
    public class PTLToExportModel : PageModel
    {
        private readonly DataContext _context;
        private readonly MetaData _meta;
        private readonly Exporter _exp;
        public PTLToExportModel(DataContext context)
        {
            _context = context;           
            _meta = new MetaData(_context);
            _exp = new Exporter(_context);
        }
        public List<PTL> ptlForExport {  get; set; }
        public string consFil;
        public string gcFil;
        public string pathFil;

        public void OnGet(string consultantFilter, string gcFilter, string pathwayFilter)
        {
            ptlForExport = _meta.GetPTLList().ToList();

            if (pathwayFilter != null && pathwayFilter != "")
            {
                ptlForExport = ptlForExport.Where(p => p.ReferralReason == pathwayFilter).ToList();
            }   

            if (consultantFilter != null && consultantFilter != "")
            {
                    ptlForExport = ptlForExport.Where(p => p.ReferralConsultant == consultantFilter).ToList();
                }

            if (gcFilter != null && gcFilter != "")
            {
                    ptlForExport = ptlForExport.Where(p => p.ReferralGC == gcFilter).ToList();               
            }

            consFil = consultantFilter;
            gcFil = gcFilter;
            pathFil = pathwayFilter;
        }

        public void OnPost(string consultantFilter, string gcFilter, string pathwayFilter)
        {
            ptlForExport = _meta.GetPTLList().ToList();

            if (pathwayFilter != null && pathwayFilter != "")
            {
                ptlForExport = ptlForExport.Where(p => p.ReferralReason == pathwayFilter).ToList();
            }

            if (consultantFilter != null && consultantFilter != "")
            {
                ptlForExport = ptlForExport.Where(p => p.ReferralConsultant == consultantFilter).ToList();
            }

            if (gcFilter != null && gcFilter != "")
            {
                ptlForExport = ptlForExport.Where(p => p.ReferralGC == gcFilter).ToList();
            }

           // _exp.ExportPTL(ptlForExport, User.Identity.Name);
            
            //DownloadFile2(_exp.dlFilePath);
            
            //Response.Redirect($"Index?pathwayFilter={pathwayFilter}&consultantFilter={consultantFilter}&gcFilter={gcFilter}");
        }

        
    }
}
