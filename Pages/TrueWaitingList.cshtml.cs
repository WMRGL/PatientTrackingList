using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PatientTrackingList.Data;
using PatientTrackingList.DataServices;
using PatientTrackingList.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;


namespace PatientTrackingList.Pages
{
    public class TrueWaitingListModel : PageModel
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        private readonly ISqlServices _sql;
        private readonly IStaffData _staffData;
        private readonly INotificationData _notificationData;
        private readonly ITrueWaitingListData _trueWaitingListData;
        private readonly IConfiguration Configuration;

        public TrueWaitingListModel(DataContext context, IConfiguration config, IConfiguration configuration)
        {
            _context = context;
            _config = config;
            _sql = new SqlServices(_config);
            _staffData = new StaffData(_context);
            _notificationData = new NotificationData(_context);
            _trueWaitingListData = new TrueWaitingListData(_context);
            Configuration = configuration;
        }

        public IEnumerable<TrueWaitingList> TrueWaitingList {get; set;}
        public int Total;
        public PaginatedList<TrueWaitingList> TrueWaitingLists { get; set; }
        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentSort { get; set; }
        public string notificationMessage;
        public bool isLive;

        public async Task OnGetAsync(string sortOrder, int? pageIndex, string? sCGU_No, string? sName, string? sReferralDate, string? sRangeDate, string? syearPicker)
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

            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            var trueWaitingListQuery = _trueWaitingListData.GetTrueWaitingList();
            var trueWaitingListQueryAll = _trueWaitingListData.GetTrueWaitingList(true);

            if (sCGU_No != null)
            {
                trueWaitingListQuery = trueWaitingListQuery.Where(p => p.CGU_No.Contains(sCGU_No));
                _sql.SqlWriteUsageAudit(staffCode, $"CGU_No={sCGU_No}", "TrueWaitingList");

                if (!trueWaitingListQuery.Any())
                {
                    trueWaitingListQuery = trueWaitingListQueryAll.Where(p => p.CGU_No.Contains(sCGU_No));
                    _sql.SqlWriteUsageAudit(staffCode, $"CGU_No={sCGU_No}", "TrueWaitingList");
                }
            }

            if (sName != null)
            {
                trueWaitingListQuery = trueWaitingListQuery.Where(p => p.Firstname.Contains(sName) || p.Lastname.Contains(sName));
                _sql.SqlWriteUsageAudit(staffCode, $"Name={sName}", "TrueWaitingList");

                if (!trueWaitingListQuery.Any())
                {
                    trueWaitingListQuery = trueWaitingListQueryAll.Where(p => p.Firstname.Contains(sName) || p.Lastname.Contains(sName));
                    _sql.SqlWriteUsageAudit(staffCode, $"Name={sName}", "TrueWaitingList");
                }
            }
           
            if (sReferralDate != null)
            {
                DateTime referralDate = DateTime.Parse(sReferralDate);
                trueWaitingListQuery = trueWaitingListQuery.Where(p => p.RefDate.HasValue && p.RefDate.Value.Date == referralDate.Date);
                _sql.SqlWriteUsageAudit(staffCode, $"ReferralDate={sReferralDate}", "TrueWaitingList");

                if (!trueWaitingListQuery.Any())
                {
                    trueWaitingListQuery = trueWaitingListQuery.Where(p => p.RefDate.HasValue && p.RefDate.Value.Date == referralDate.Date);
                    _sql.SqlWriteUsageAudit(staffCode, $"ReferralDate={sReferralDate}", "TrueWaitingList");
                }
            }

            if (sRangeDate != null)
            {
                string[] parts = sRangeDate.Split(" to ");
                string startDateString = parts[0];
                string endDateString = parts[1];
                
                DateTime startDate = DateTime.ParseExact(startDateString, "yyyy-MM-dd", null);
                DateTime endDate = DateTime.ParseExact(endDateString, "yyyy-MM-dd", null);

                trueWaitingListQuery = trueWaitingListQuery.Where(p => p.RefDate.HasValue && p.RefDate.Value >= startDate && p.RefDate.Value <= endDate);
                _sql.SqlWriteUsageAudit(staffCode, $"ReferralDate={startDateString} {endDateString}", "TrueWaitingList");

                if (!trueWaitingListQuery.Any())
                {
                    trueWaitingListQuery = trueWaitingListQueryAll.Where(p => p.RefDate.HasValue && p.RefDate.Value >= startDate && p.RefDate.Value <= endDate);
                    _sql.SqlWriteUsageAudit(staffCode, $"ReferralDate={startDateString} {endDateString}", "TrueWaitingList");
                }

            }

            if (syearPicker != null)
            {

                int year = int.Parse(syearPicker);
                DateTime dateTime = new DateTime(year, 1, 1);

                trueWaitingListQuery = trueWaitingListQuery.Where(p => p.RefDate.HasValue && p.RefDate.Value.Year == dateTime.Year);
                _sql.SqlWriteUsageAudit(staffCode, $"ReferralDate={dateTime.Year}", "TrueWaitingList");

                if (!trueWaitingListQuery.Any())
                {
                    trueWaitingListQuery = trueWaitingListQueryAll.Where(p => p.RefDate.HasValue && p.RefDate.Value.Year == dateTime.Year);
                    _sql.SqlWriteUsageAudit(staffCode, $"ReferralDate={dateTime.Year}", "TrueWaitingList");
                }

            }


            var pageSize = 20;
            Total = trueWaitingListQuery.Count();
            TrueWaitingLists = await PaginatedList<TrueWaitingList>.CreateAsync(
                trueWaitingListQuery.AsNoTracking(), pageIndex ?? 1, pageSize);

            
        }


    }
}
