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
        public List<StaffMembers> consultantList { get; set; }
        public List<StaffMembers> GCList { get; set; }
        public int Total;
        public PaginatedList<TrueWaitingList> TrueWaitingLists { get; set; }
        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentSort { get; set; }
        public string notificationMessage;
        public bool isLive;
        public string searchCGU_No;
        public string searchName;
        public string searchReferralDate;
        public string searchRangeDate;
        public string searchYearPicker;
        public string searchConsultant;
        public string searchGC;
        public bool isSortDesc;
        public int currentPageNo;
        public int nextPage;
        public int previousPage;

        public async Task OnGetAsync(string sortOrder, int? pageIndex, string? sCGU_No, string? sName, string? sReferralDate, string? sRangeDate, string? syearPicker,
            string? sConsultants, string? sGC, bool? isDesc = false
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

            var trueWaitingListQuery = _trueWaitingListData.GetTrueWaitingList();
            var trueWaitingListQueryAll = _trueWaitingListData.GetTrueWaitingList(true);
            isSortDesc = isDesc.GetValueOrDefault();

            consultantList = _staffData.GetStaffTypeList("Consultant");
            GCList = _staffData.GetStaffTypeList("GC");

            if (sCGU_No != null)
            {
                trueWaitingListQuery = trueWaitingListQuery.Where(p => p.CGU_No.Contains(sCGU_No));
                _sql.SqlWriteUsageAudit(staffCode, $"CGU_No={sCGU_No}", "TrueWaitingList");

                if (!trueWaitingListQuery.Any())
                {
                    trueWaitingListQuery = trueWaitingListQueryAll.Where(p => p.CGU_No.Contains(sCGU_No));
                    _sql.SqlWriteUsageAudit(staffCode, $"CGU_No={sCGU_No}", "TrueWaitingList");
                }
                searchCGU_No = sCGU_No;
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
                searchName = sName;
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
                searchReferralDate = sReferralDate;
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
                searchRangeDate = sRangeDate;
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
                searchYearPicker = syearPicker;
            }

            if (sConsultants != null)
            {


                trueWaitingListQuery = trueWaitingListQuery.Where(p => p.LeadClinician == sConsultants);
                _sql.SqlWriteUsageAudit(staffCode, $"Consultant={sConsultants}", "TrueWaitingList");

                if (!trueWaitingListQuery.Any())
                {
                    trueWaitingListQuery = trueWaitingListQueryAll.Where((p => p.LeadClinician == sConsultants));
                    _sql.SqlWriteUsageAudit(staffCode, $"Consultant={sConsultants}", "TrueWaitingList");
                }
                searchConsultant = sConsultants;
            }

            if (sGC != null)
            {

                trueWaitingListQuery = trueWaitingListQuery.Where(p => p.GC == sGC);
                _sql.SqlWriteUsageAudit(staffCode, $"GC={sGC}", "TrueWaitingList");

                if (!trueWaitingListQuery.Any())
                {
                    trueWaitingListQuery = trueWaitingListQueryAll.Where((p => p.GC == sGC));
                    _sql.SqlWriteUsageAudit(staffCode, $"GC={sGC}", "TrueWaitingList");
                }
                searchGC = sGC;
            }


            switch (sortOrder)
            {
                case "ref_date":
                    if (isSortDesc)
                    {
                        trueWaitingListQuery = trueWaitingListQuery.OrderByDescending(p => p.RefDate);
                    }
                    else
                    {
                        trueWaitingListQuery = trueWaitingListQuery.OrderBy(p => p.RefDate);
                    }
                break;
                
                case "clock_start_date":
                    if (isSortDesc)
                    {
                        trueWaitingListQuery = trueWaitingListQuery.OrderByDescending(p => p.ClockStartDate);
                    }
                    else
                    {
                        trueWaitingListQuery = trueWaitingListQuery.OrderBy(p => p.ClockStartDate);
                    }
                break;
                case "clock_stop_date":
                    if (isSortDesc)
                    {
                        trueWaitingListQuery = trueWaitingListQuery.OrderByDescending(p => p.ClockStopDate);
                    }
                    else
                    {
                        trueWaitingListQuery = trueWaitingListQuery.OrderBy(p => p.ClockStopDate);
                    }
                break;
                case "status_admin":
                    if (isSortDesc)
                    {
                        trueWaitingListQuery = trueWaitingListQuery.OrderByDescending(p => p.Status_Admin);
                    }
                    else
                    {
                        trueWaitingListQuery = trueWaitingListQuery.OrderBy(p => p.Status_Admin);
                    }
                break;
                case "pathway":
                    if (isSortDesc)
                    {
                        trueWaitingListQuery = trueWaitingListQuery.OrderByDescending(p => p.PATHWAY);
                    }
                    else
                    {
                        trueWaitingListQuery = trueWaitingListQuery.OrderBy(p => p.PATHWAY);
                    }
                break;
            }

            if (pageIndex == 0 || pageIndex == null)
            {
                currentPageNo = 1;
            }
            else
            {
                currentPageNo = pageIndex.GetValueOrDefault();
            }

            nextPage = currentPageNo + 1;
            previousPage = currentPageNo - 1; 
            var pageSize = 20;
            Total = trueWaitingListQuery.Count();
            TrueWaitingLists = await PaginatedList<TrueWaitingList>.CreateAsync(
                trueWaitingListQuery.AsNoTracking(), pageIndex ?? 1, pageSize);

            
        }


    }
}
