using Microsoft.AspNetCore.Mvc.RazorPages;
using PatientTrackingList.Data;
using PatientTrackingList.DataServices;
using PatientTrackingList.Models;

namespace PatientTrackingList.Pages
{
    public class WaitingListModel : PageModel
    {
        private readonly DataContext _context;
        private readonly IWaitingListData _waitingListData;
        
        

        public WaitingListModel(DataContext context)
        {
            _context = context;
            _waitingListData = new WaitingListData(_context);
            pageNumbers = new List<int>();
            Clinicians = new List<string>();
            Clinics = new List<string>();
        }
        public IEnumerable<WaitingList> WaitingList { get; set; }
        public List<WaitingList> pageOfWL { get; set; }
        public List<string> Clinicians { get; set; }
        public List<string> Clinics { get; set; }
        public List<int> pageNumbers;

        public int currentPageNo;
        public int nextPage;
        public int previousPage;
        public int listTotal;
        public string clincianSelected;
        public string clinicSelected;

        public void OnGet(int? pNo, string? clinician, string? clinic)
        {
            if (User.Identity.Name is null)
            {
                Response.Redirect("Login");
            }

            int pageSize = 20;            

            WaitingList = _waitingListData.GetWaitingList();

            //for WL total
            //listTotal = WaitingList.Count();

            //drop-down menus
            Clinicians = WaitingList.Select(c => c.ClinicianID).Distinct().OrderBy(c => c).ToList();
            Clinics = WaitingList.Select(c => c.ClinicID).Distinct().OrderBy(c => c).ToList();

            if (clinician != null)
            {
                WaitingList = WaitingList.Where(w => w.ClinicianID == clinician);
                clincianSelected = clinician;
            }

            if (clinic != null)
            {
                WaitingList = WaitingList.Where(w => w.ClinicID == clinic);
                clinicSelected = clinic;
            }

            //paginator
            //List < WaitingList > pageOfWL = new List<WaitingList>();
            pageOfWL = WaitingList.ToList();
            
            int pp = pageOfWL.Count() / pageSize;

            for (int i = 1; i <= pp; i++)
            {
                pageNumbers.Add(i);
            }

            pageOfWL = pageOfWL.Skip((pNo.GetValueOrDefault() - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();          
                        
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

            listTotal = WaitingList.Count();
        }

        public void OnPost(int? pNo, string? clinician, string? clinic)
        {
            WaitingList = _waitingListData.GetWaitingList();

            //have to give it something, even if I'm instantly redirecting, or it'll throw a fit
            Clinicians = WaitingList.Select(c => c.ClinicianID).Distinct().OrderBy(c => c).ToList();
            Clinics = WaitingList.Select(c => c.ClinicID).Distinct().OrderBy(c => c).ToList();
            //WL = WaitingList.ToList();

            Response.Redirect("WaitingList?pNo=" + pNo + "&clinician=" + clinician + "&clinic=" + clinic);
        }
    }
}
