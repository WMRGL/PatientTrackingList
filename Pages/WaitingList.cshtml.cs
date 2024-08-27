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
        private readonly IConfiguration _config;
        private readonly ISqlServices _sql;
        private readonly IStaffData _staffData;
        

        public WaitingListModel(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _waitingListData = new WaitingListData(_context);
            Clinicians = new List<string>();
            Clinics = new List<string>();
            _sql = new SqlServices(_config);
            _staffData = new StaffData(_context);
        }
        public IEnumerable<WaitingList> WaitingList { get; set; }
        public List<WaitingList> pageOfWL { get; set; }
        public List<string> Clinicians { get; set; }
        public List<string> Clinics { get; set; }

        public int listTotal;
        public string clincianSelected;
        public string clinicSelected;

        public void OnGet(string? clinician, string? clinic)
        {
            string staffCode = "";
            if (User.Identity.Name is null)
            {
                Response.Redirect("Login");
            }
            else
            {
                staffCode = _staffData.GetStaffMemberDetails(User.Identity.Name).STAFF_CODE;
                _sql.SqlWriteUsageAudit(staffCode, "", "Waiting List");
            }

                 

            WaitingList = _waitingListData.GetWaitingList();

            //for WL total
            //listTotal = WaitingList.Count();

            //drop-down menus
            Clinicians = WaitingList.Select(c => c.ClinicianID).Distinct().OrderBy(c => c).ToList();
            Clinics = WaitingList.Select(c => c.ClinicID).Distinct().OrderBy(c => c).ToList();

            if (clinician != null)
            {
                WaitingList = WaitingList.Where(w => w.ClinicianID == clinician);
                _sql.SqlWriteUsageAudit(staffCode, $"Clinician={clinician}", "Waiting List");
                clincianSelected = clinician;
            }

            if (clinic != null)
            {
                WaitingList = WaitingList.Where(w => w.ClinicID == clinic);
                _sql.SqlWriteUsageAudit(staffCode, $"Clinic={clinic}", "Waiting List");
                clinicSelected = clinic;
            }

            //paginator
            //List < WaitingList > pageOfWL = new List<WaitingList>();
            pageOfWL = WaitingList.ToList();
          

            listTotal = WaitingList.Count();
        }

        public void OnPost(string? clinician, string? clinic)
        {
            WaitingList = _waitingListData.GetWaitingList();

            //have to give it something, even if I'm instantly redirecting, or it'll throw a fit
            Clinicians = WaitingList.Select(c => c.ClinicianID).Distinct().OrderBy(c => c).ToList();
            Clinics = WaitingList.Select(c => c.ClinicID).Distinct().OrderBy(c => c).ToList();
            //WL = WaitingList.ToList();

            Response.Redirect($"clinician={clinician}&clinic={clinic}");
        }
    }
}
