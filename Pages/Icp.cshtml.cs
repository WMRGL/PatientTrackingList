using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PatientTrackingList.Data;
using PatientTrackingList.DataServices;
using PatientTrackingList.Models;
using System;
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

        public Icp IcpDetail { get; set; }
        public IcpCancer IcpCancerDetail { get; set; }
        public IcpGeneral IcpGeneralDetail { get; set; }
        public string notificationMessage;
        public bool isLive;
        public string patientName;
        public string address;
        public string referralType;
        public string? referringClicnician;
        public string? nhsNo;
        public string? consultant;
        public string? gc;
        public string? adminContact;
        public string? pathway;
        public string? referralClass;
        public string? indication;
        public string? indicationNotes;
        public DateTime? breachDate;
        public DateTime? referralDate;
        public DateTime? dob;


        public void OnGet(int? sReferralId, string? sName, string? sAddress, string? sRefType, DateTime? sRefDate, string? sRefClicnician, DateTime? sDob,
            string? sNhsNo, string? sConsultant, string? sGc, string? sAdminContact, string? sPathway, string? sRefClass, string? sIndication, DateTime? sBreachDate,
            string? sIndicationNotes
            )
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

            if (sName != null || sAddress != null || sRefType != null || sRefDate != null || sRefClicnician != null || sDob != null || sNhsNo != null
                || sConsultant != null || sGc != null || sAdminContact != null || sPathway != null || sRefClass !=null || sIndication != null || sBreachDate != null
                || sIndicationNotes != null
                )
            {
                patientName = sName;
                address = sAddress;
                referralDate = sRefDate;
                referralType = sRefType;
                referringClicnician = sRefClicnician;
                dob = sDob;
                nhsNo = sNhsNo;
                consultant = sConsultant;
                gc = sGc;
                adminContact = sAdminContact;
                pathway = sPathway;
                referralClass = sRefClass;
                indication = sIndication;
                breachDate = sBreachDate;
                indicationNotes = sIndicationNotes;
            }

            if (sReferralId != null)
            {

                IcpDetail = _icpData.GetIcp(sReferralId);

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
}
