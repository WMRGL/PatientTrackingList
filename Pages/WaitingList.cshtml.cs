using Microsoft.AspNetCore.Mvc.RazorPages;
using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.Pages
{
    public class WaitingListModel : PageModel
    {
        private readonly DataContext _context;
        public IEnumerable<WaitingList> WaitingList { get; set; }
        public List<WaitingList> WL { get; set; }
        public List<string> Clinicians { get; set; }
        public List<string> Clinics { get; set; }
        public List<int> pageNumbers;
        
        public int currentPageNo;
        public int nextPage;
        public int previousPage;
        public int listTotal;

        public WaitingListModel(DataContext context)
        {
            _context = context;
            pageNumbers = new List<int>();
            WL = new List<WaitingList>();
            Clinicians = new List<string>();
            Clinics = new List<string>();
        }

        public void OnGet(int? pNo, string? sClinician, string? sClinic)
        {
            int pageSize = 20;

            WaitingList = from w in _context.WaitingList
                        orderby w.AddedDate
                        select w;

            //drop-down menus
            Clinicians = WaitingList.Select(c => c.ClinicianID).Distinct().OrderBy(c => c).ToList();
            Clinics = WaitingList.Select(c => c.ClinicID).Distinct().OrderBy(c => c).ToList();

            //for WL total
            listTotal = WaitingList.Count();

            if (sClinician != null)
            {
                WaitingList = WaitingList.Where(w => w.ClinicianID == sClinician);
            }

            if (sClinic != null)
            {
                WaitingList = WaitingList.Where(w => w.ClinicID == sClinic);
            }

            List < WaitingList > pageOfWL = new List<WaitingList>();
            pageOfWL = WaitingList.ToList();
            
            int pp = pageOfWL.Count() / pageSize;

            for (int i = 1; i <= pp; i++)
            {
                pageNumbers.Add(i);
            }

            pageOfWL = pageOfWL.Skip((pNo.GetValueOrDefault() - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();          
            
            foreach (var w in pageOfWL)
            {
                string sCGU;                
                
                if (_context.Patients.Where(p => p.INTID == w.IntID).Count() > 0)
                {
                    sCGU = _context.Patients.FirstOrDefault(p => p.INTID == w.IntID).CGU_No;
                }
                else
                {
                    sCGU = "Unknown";
                }
                    
                WL.Add(new WaitingList
                {
                    CGU_No = sCGU,
                    ClinicianID = w.ClinicianID,
                    ClinicID = w.ClinicID,
                    AddedDate = w.AddedDate,
                    Instructions = w.Instructions,
                    Comment = w.Comment
                });
            }
            
            if (pNo == null)
            {
                currentPageNo = 1;
            }
            else
            {
                currentPageNo = pNo.GetValueOrDefault();
            }

            nextPage = currentPageNo + 1;
            previousPage = currentPageNo - 1;
        }

        public void OnPost(int? pNo, string? sClinician, string? sClinic)
        {
            WaitingList = from w in _context.WaitingList
                          orderby w.AddedDate
                          select w;

            //have to give it something, even if I'm instantly redirecting, or it'll throw a fit
            Clinicians = WaitingList.Select(c => c.ClinicianID).Distinct().OrderBy(c => c).ToList();
            Clinics = WaitingList.Select(c => c.ClinicID).Distinct().OrderBy(c => c).ToList();
            WL = WaitingList.ToList();

            Response.Redirect("WaitingList?pNo=" + pNo + "&sClinician=" + sClinician + "&sClinic=" + sClinic);
        }
    }
}
