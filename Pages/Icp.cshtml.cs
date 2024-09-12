using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PatientTrackingList.Data;
using PatientTrackingList.DataServices;
using PatientTrackingList.Models;
using System.ComponentModel;
using System.Configuration;

namespace PatientTrackingList.Pages
{
    public class IcpModel : PageModel
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        private readonly ISqlServices _sql;
        private readonly IStaffData _staffData;
        private readonly IIcpData _icpData;
        private readonly IConfiguration Configuration;
        private readonly INotificationData _notificationData;


        public IcpModel(DataContext context, IConfiguration config, IConfiguration configuration)
        {
            _context = context;
            _config = config;
            _sql = new SqlServices(_config);
            _staffData = new StaffData(_context);
            _icpData = new IcpData(_context);
            _notificationData = new NotificationData(_context);
            Configuration = configuration;
        }

        public  Icp IcpDetail { get; set; }
        public IcpCancer IcpCancerDetail { get; set; }
        public IcpGeneral IcpGeneralDetail { get; set; }
        public string notificationMessage;
        public bool isLive;


        public void OnGet()
        {
            string staffCode = "";
            if (User.Identity.Name is null)
            {
                Response.Redirect("Login");
            }
            else
            {
                notificationMessage = _notificationData.GetMessage();

                isLive = bool.Parse(_config.GetValue("IsLive", ""));
                staffCode = _staffData.GetStaffMemberDetails(User.Identity.Name).STAFF_CODE;
                _sql.SqlWriteUsageAudit(staffCode, "", "Index");
            }

            
            IcpDetail = _icpData.GetIcp(9202398);

            var IcpId = IcpDetail.ICPID;

            if (IcpId != null)
            {
                IcpCancerDetail = _icpData.GetIcpCancer(IcpId);

            }

            if (IcpCancerDetail == null)
            {
                IcpGeneralDetail = _icpData.GetIcpGeneral(IcpId);
            }


        }
    }
}
