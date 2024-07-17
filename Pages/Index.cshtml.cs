using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PatientTrackingList.Data;
using PatientTrackingList.Models;
using PatientTrackingList.DataServices;

namespace PatientTrackingList.Pages
{
    public class IndexModel : PageModel
    {   
        private readonly IConfiguration _config;
        private readonly IPTLData _ptlData;
        private readonly IStaffData _staffData;
        private readonly INotificationData _notificationData;
        private readonly ISqlServices _sql;
        private readonly DataContext _context;        
        public IndexModel(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            pageNumbers = new List<int>();
            _ptlData = new PTLData(_context);
            _staffData = new StaffData(_context);
            _notificationData = new NotificationData(_context);
            _sql = new SqlServices(_config);
        }
        public IEnumerable<PTL> PTL { get; set; }
        public List<int> pageNumbers;
        public List<StaffMembers> consultantList { get; set; }
        public List<StaffMembers> GCList { get; set; }
        public List<PTL> pageOfPTL { get; set; }
        public DateTime CurrentYear;
        public DateTime PreviousYear;
        public DateTime EighteenWeekDate;
        public DateTime FiftyTwoWeekDate;
        public DateTime LastUpdatedDate;
        public bool isSortDesc;
        
        public int currentPageNo;
        public int nextPage;
        public int previousPage;
        
        public int listTotal;
        public int currentYearTotal;
        public int prevYearTotal;
        public int olderTotal;
        public int breachingTotal;
        public int apptDueTotal;
        public int unapptTotal;

        public string pathwaySelected;
        public bool isCheckedSelected;
        public string prioritySelected;
        public string consultantSelected;
        public string GCSelected;
        public string commentSearch;
        public string notificationMessage;
        public bool isLive;
        public string Message;

        [Authorize]
        public void OnGet(int? pNo, string? sortOrder = "", bool? isDesc=false, string? sNameSearch = null, 
            string? sCGUSearch = null, string? priorityFilter = null, bool? isChecked=false, string? pathwayFilter=null, 
            string? consultantFilter=null, string? gcFilter=null, string? commentsearch = null)
        {
            try
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

                int pageSize = 20;

                PTL = _ptlData.GetPTLList();

                consultantList = _staffData.GetStaffTypeList("Consultant");
                GCList = _staffData.GetStaffTypeList("GC");

                isSortDesc = isDesc.GetValueOrDefault();

                CurrentYear = DateTime.Parse($"{DateTime.Now.Year}-01-01");
                PreviousYear = DateTime.Parse($"{(DateTime.Now.Year - 1)}-01-01");
                EighteenWeekDate = DateTime.Now.AddDays(-18 * 7);
                FiftyTwoWeekDate = DateTime.Now.AddDays(-365);

                //for sorting (ascending and descending on each column)

                switch (sortOrder)
                {
                    case "ref_date":
                        if (isSortDesc)
                        {
                            PTL = PTL.OrderByDescending(p => p.ReferralDate);
                        }
                        else
                        {
                            PTL = PTL.OrderBy(p => p.ReferralDate);
                        }
                        break;
                    case "cs_date":
                        if (isSortDesc)
                        {
                            PTL = PTL.OrderByDescending(p => p.ClockStart);
                        }
                        else
                        {
                            PTL = PTL.OrderBy(p => p.ClockStart);
                        }
                        break;
                    case "ref_reason":
                        if (isSortDesc)
                        {
                            PTL = PTL.OrderByDescending(p => p.ReferralReason);
                        }
                        else
                        {
                            PTL = PTL.OrderBy(p => p.ReferralReason);
                        }
                        break;
                    case "ref_class":
                        if (isSortDesc)
                        {
                            PTL = PTL.OrderByDescending(p => p.Class);
                        }
                        else
                        {
                            PTL = PTL.OrderBy(p => p.Class);
                        }
                        break;
                    case "clock_day":
                        if (isSortDesc)
                        {
                            PTL = PTL.OrderByDescending(p => DateTime.Now - p.ClockStart);
                        }
                        else
                        {
                            PTL = PTL.OrderBy(p => DateTime.Now - p.ClockStart);
                        }
                        break;
                    case "tci_date":
                        if (isSortDesc)
                        {
                            PTL = PTL.OrderByDescending(p => p.TCIDate);
                        }
                        else
                        {
                            PTL = PTL.OrderBy(p => p.TCIDate);
                        }
                        break;
                    case "clock_tci":
                        if (isSortDesc)
                        {
                            PTL = PTL.OrderByDescending(p => p.ClockDaysAtTCI);
                        }
                        else
                        {
                            PTL = PTL.OrderBy(p => p.ClockDaysAtTCI);
                        }
                        break;
                    default:
                        if (isSortDesc)
                        {
                            PTL = PTL.OrderByDescending(p => p.ClockStart);
                        }
                        else
                        {
                            PTL = PTL.OrderBy(p => p.ClockStart);
                        }
                        break;
                }

                pageOfPTL = PTL.ToList(); //converting to list here makes it much faster!

                //variables for totals on main page
                listTotal = pageOfPTL.Count();
                currentYearTotal = pageOfPTL.Where(i => i.ReferralDate > CurrentYear).Count();
                prevYearTotal = pageOfPTL.Where(i => i.ReferralDate > PreviousYear && i.ReferralDate < CurrentYear).Count();
                olderTotal = pageOfPTL.Where(i => i.ReferralDate < PreviousYear).Count();
                breachingTotal = pageOfPTL.Where(i => i.ClockStart < EighteenWeekDate).Count();
                apptDueTotal = pageOfPTL.Where(i => i.TCIDate != null).Count();
                unapptTotal = pageOfPTL.Where(i => i.TCIDate == null).Count();

                //for filtering/searching
                if (sNameSearch != null)
                {
                    pageOfPTL = pageOfPTL.Where(p => p.PatientName.Contains(sNameSearch, StringComparison.OrdinalIgnoreCase)).ToList();
                    _sql.SqlWriteUsageAudit(staffCode, $"Name={sNameSearch}", "Index");
                }

                if (sCGUSearch != null)
                {
                    pageOfPTL = pageOfPTL.Where(p => p.CGUNo.Contains(sCGUSearch)).ToList();
                    _sql.SqlWriteUsageAudit(staffCode, $"CGU_No={sCGUSearch}", "Index");
                }

                if (priorityFilter != null && priorityFilter != "")
                {
                    pageOfPTL = pageOfPTL.Where(p => p.Class == priorityFilter).ToList();
                    _sql.SqlWriteUsageAudit(staffCode, $"Priority={priorityFilter}", "Index");
                    prioritySelected = priorityFilter;
                }

                if (isChecked.GetValueOrDefault())
                {
                    pageOfPTL = pageOfPTL.Where(p => !p.isChecked).ToList();
                    _sql.SqlWriteUsageAudit(staffCode, $"Checked={isChecked}", "Index");
                    isCheckedSelected = isChecked.GetValueOrDefault();
                }

                if (pathwayFilter != null && pathwayFilter != "")
                {
                    pageOfPTL = pageOfPTL.Where(p => p.ReferralReason == pathwayFilter).ToList();
                    _sql.SqlWriteUsageAudit(staffCode, $"Pathway={pathwayFilter}", "Index");
                    pathwaySelected = pathwayFilter;
                }

                if (consultantFilter != null && consultantFilter != "")
                {
                    pageOfPTL = pageOfPTL.Where(p => p.ReferralConsultant == consultantFilter).ToList();
                    _sql.SqlWriteUsageAudit(staffCode, $"Consultant={consultantFilter}", "Index");
                    consultantSelected = consultantFilter;
                }

                if (gcFilter != null && gcFilter != "")
                {
                    pageOfPTL = pageOfPTL.Where(p => p.ReferralGC == gcFilter).ToList();
                    _sql.SqlWriteUsageAudit(staffCode, $"GC={gcFilter}", "Index");
                    GCSelected = gcFilter;
                }

                if (commentsearch != null && commentsearch != "")
                {
                    pageOfPTL = pageOfPTL.Where(p => p.Comments != null && p.Comments.Contains(commentsearch, StringComparison.OrdinalIgnoreCase)).ToList();
                    _sql.SqlWriteUsageAudit(staffCode, $"Comments={commentsearch}", "Index");
                    commentSearch = commentsearch;
                }

                //pagination
                int pp = pageOfPTL.Count() / pageSize;

                for (int i = 1; i <= pp + 1; i++)
                {
                    pageNumbers.Add(i);
                }

                pageOfPTL = pageOfPTL.Skip((pNo.GetValueOrDefault() - 1) * pageSize)
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
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        public void OnPost(int? pNo, string? sortOrder = "", bool? isDesc=false, string? sNameSearch=null, 
            string? sCGUSearch=null, string? priorityFilter = null, bool? isChecked=false, string? pathwayFilter=null, 
            string? consultantFilter=null, string? gcFilter=null, string? commentsearch=null)
        {
            try
            {
                int pageSize = 20;

                PTL = _ptlData.GetPTLList();

                consultantList = _staffData.GetStaffTypeList("Consultant");
                GCList = _staffData.GetStaffTypeList("GC");

                pageOfPTL = PTL.Skip((pNo.GetValueOrDefault() + 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                Response.Redirect($"Index?pNo={pNo}&sortOrder={sortOrder}&isDesc={isDesc}&sNameSearch={sNameSearch}&sCGUSearch={sCGUSearch}" +
                    $"&priorityFilter={priorityFilter}&isChecked={isChecked}&pathwayFilter={pathwayFilter}&consultantFilter={consultantFilter}&gcFilter={gcFilter}" +
                    $"&commentsearch={commentsearch}");
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }

        }
    }
}