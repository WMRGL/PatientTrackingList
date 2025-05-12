using Microsoft.AspNetCore.Mvc.RazorPages;
using PatientTrackingList.Data;
using PatientTrackingList.DataServices;
using ClinicalXPDataConnections.Meta;
using ClinicalXPDataConnections.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.Pages
{
    public class WaitingListHistoryModel : PageModel
    {
        private readonly DataContext _context;
        private readonly ClinicalContext _clinicalContext;
        private readonly IWaitingListHistoryData _waitingListData;
        private readonly IConfiguration _config;
        private readonly ISqlServices _sql;
        private readonly IStaffUserData _staffData;
        

        public WaitingListHistoryModel(DataContext context, ClinicalContext clinicalContext, IConfiguration config)
        {
            _context = context;
            _clinicalContext = clinicalContext;
            _config = config;
            _waitingListData = new WaitingListHistoryData(_context);            
            _sql = new SqlServices(_config);
            _staffData = new StaffUserData(_clinicalContext);
        }
        public IEnumerable<WLHistory> WaitingList { get; set; }        
        public List<string> Clinicians { get; set; }
        public List<string> Clinics { get; set; }
                        
        public DateTime dFrom;
        public DateTime dTo;

        public void OnGet(DateTime? startDate, DateTime? endDate)
        {
            
            IPAddressFinder _ip = new IPAddressFinder(HttpContext);
            string staffCode = "";
            if (User.Identity.Name is null)
            {
                Response.Redirect("Login");
            }
            else
            {
                staffCode = _staffData.GetStaffMemberDetails(User.Identity.Name).STAFF_CODE;
                _sql.SqlWriteUsageAudit(staffCode, "", "Waiting List History", _ip.GetIPAddress());
            }

            if (startDate == null)
            {
                startDate = DateTime.Now.AddDays(-30);
            }
            if (endDate == null)
            {
                endDate = DateTime.Now;
            }

            dFrom = startDate.GetValueOrDefault();
            dTo = endDate.GetValueOrDefault();

            WaitingList = _waitingListData.GetWaitingListHistory(startDate.GetValueOrDefault(), endDate.GetValueOrDefault());            
        }

        public void OnPost(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null)
            {
                startDate = DateTime.Now.AddDays(-30);
            }
            if (endDate == null)
            {
                endDate = DateTime.Now;
            }

            dFrom = startDate.GetValueOrDefault();
            dTo = endDate.GetValueOrDefault();

            WaitingList = _waitingListData.GetWaitingListHistory(startDate.GetValueOrDefault(), endDate.GetValueOrDefault());            
        }
    }
}
