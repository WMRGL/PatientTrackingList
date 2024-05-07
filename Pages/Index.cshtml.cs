using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PatientTrackingList.Data;
using PatientTrackingList.Models;
using PatientTrackingList.DataServices;

namespace PatientTrackingList.Pages
{
    public class IndexModel : PageModel
    {

        private readonly DataContext _context;
        private readonly IConfiguration _config;
        public IEnumerable<PTL> PTL { get; set; }
        public List<PTL> pageOfPTL { get; set; }
        public IEnumerable<StaffMembers> staffMembers { get; set; }
        public List<StaffMembers> consultantList { get; set; }
        public List<StaffMembers> GCList { get; set; }
        public List<int> pageNumbers;
        private readonly PTLData _ptlData;
        private readonly StaffData _staffData;

        public IndexModel(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            pageNumbers = new List<int>();
            _ptlData = new PTLData(_context);
            _staffData = new StaffData(_context);
        }

        

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
        public bool isLive;

        [Authorize]
        public void OnGet(int? pNo, string? sortOrder = "", bool? isDesc=false, string? sNameSearch = null, 
            string? sCGUSearch = null, string? priorityFilter = null, bool? isChecked=false, string? pathwayFilter=null, 
            string? consultantFilter=null, string? gcFilter=null, string? commentsearch = null)
        {
            if (User.Identity.Name is null)
            {
                Response.Redirect("Login");
            }
            else
            {
                if (_staffData.GetIsClinical(User.Identity.Name))
                {
                    var staffUserType = _staffData.GetStaffMemberDetails(User.Identity.Name);

                    if (staffUserType.CLINIC_SCHEDULER_GROUPS == "GC" && gcFilter == null)
                    {
                        gcFilter = staffUserType.NAME;
                    }
                    else if ((staffUserType.CLINIC_SCHEDULER_GROUPS == "Consultant" || staffUserType.CLINIC_SCHEDULER_GROUPS == "SpR") && consultantFilter == null)
                    {
                        consultantFilter = staffUserType.NAME;
                    }
                    isLive = bool.Parse(_config.GetValue("IsLive", ""));
                }
            }

            int pageSize = 20;            

            PTL = _ptlData.GetPTLList();                       

            staffMembers = _staffData.GetStaffMemberList();
            
            consultantList = _staffData.GetStaffTypeList(staffMembers, "Consultant");
            GCList = _staffData.GetStaffTypeList(staffMembers, "GC");

            isSortDesc = isDesc.GetValueOrDefault();            

            CurrentYear = DateTime.Parse(DateTime.Now.Year + "-01-01");
            PreviousYear = DateTime.Parse((DateTime.Now.Year - 1) + "-01-01");
            EighteenWeekDate = DateTime.Now.AddDays(-18 * 7);
            FiftyTwoWeekDate = DateTime.Now.AddDays(-365);
            LastUpdatedDate = PTL.OrderByDescending(p => p.LastUpdatedDate).First().LastUpdatedDate;

            //variables for totals on main page
            listTotal = PTL.Count();
            currentYearTotal = PTL.Where(i => i.ReferralDate > CurrentYear).Count();
            prevYearTotal = PTL.Where(i => i.ReferralDate > PreviousYear && i.ReferralDate < CurrentYear).Count();
            olderTotal = PTL.Where(i => i.ReferralDate < PreviousYear).Count();
            breachingTotal = PTL.Where(i => i.ClockStart < EighteenWeekDate).Count();
            apptDueTotal = PTL.Where(i => i.TCIDate != null).Count();
            unapptTotal = PTL.Where(i => i.TCIDate == null).Count();

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

            pageOfPTL = PTL.ToList();            

            //for filtering/searching
            if (sNameSearch != null)
            {
                pageOfPTL = pageOfPTL.Where(p => p.PatientName.ToUpper().Contains(sNameSearch.ToUpper())).ToList();
            }

            if (sCGUSearch != null)
            {
                pageOfPTL = pageOfPTL.Where(p => p.CGUNo.Contains(sCGUSearch)).ToList();
            }

            /*if(priorityFilter.GetValueOrDefault())
            {
                pageOfPTL = pageOfPTL.Where(p => p.Class == "Urgent").ToList();
                prioritySelected = priorityFilter.GetValueOrDefault();
            }*/

            if (priorityFilter != null && priorityFilter != "")
            {
                pageOfPTL = pageOfPTL.Where(p => p.Class == priorityFilter).ToList();
                prioritySelected = priorityFilter;
            }

            if (isChecked.GetValueOrDefault())
            {
                pageOfPTL = pageOfPTL.Where(p => !p.isChecked).ToList();
                isCheckedSelected = isChecked.GetValueOrDefault();
            }

            if (pathwayFilter != null && pathwayFilter != "")
            {
                pageOfPTL = pageOfPTL.Where(p => p.ReferralReason == pathwayFilter).ToList();
                pathwaySelected = pathwayFilter;
            }

            if (consultantFilter != null && consultantFilter != "")
            {
                pageOfPTL = pageOfPTL.Where(p => p.ReferralConsultant == consultantFilter).ToList();
                consultantSelected = consultantFilter;
            }

            if (gcFilter != null && gcFilter != "")
            {
                pageOfPTL = pageOfPTL.Where(p => p.ReferralGC == gcFilter).ToList();
                GCSelected = gcFilter;
            }

            if (commentsearch != null && commentsearch != "")
            {
                pageOfPTL = pageOfPTL.Where(p => p.Comments != null && p.Comments.Contains(commentsearch)).ToList();
                commentSearch = commentsearch;
            }

            //pagination
            int pp = pageOfPTL.Count() / pageSize;

            for (int i = 1; i <= pp; i++)
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

        public void OnPost(int? pNo, string? sortOrder = "", bool? isDesc=false, string? sNameSearch=null, 
            string? sCGUSearch=null, string? priorityFilter = null, bool? isChecked=false, string? pathwayFilter=null, 
            string? consultantFilter=null, string? gcFilter=null, string? commentsearch=null)
        {
            int pageSize = 20;

            PTL = _ptlData.GetPTLList();

            staffMembers = _staffData.GetStaffMemberList();

            consultantList = _staffData.GetStaffTypeList(staffMembers, "Consultant");
            GCList = _staffData.GetStaffTypeList(staffMembers, "GC");

            pageOfPTL = PTL.Skip((pNo.GetValueOrDefault() + 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            
            Response.Redirect($"Index?pNo={pNo}&sortOrder={sortOrder}&isDesc={isDesc}&sNameSearch={sNameSearch}&sCGUSearch={sCGUSearch}" +
                $"&priorityFilter={priorityFilter}&isChecked={isChecked}&pathwayFilter={pathwayFilter}&consultantFilter={consultantFilter}&gcFilter={gcFilter}" +
                $"&commentsearch={commentsearch}");
        }
    }
}