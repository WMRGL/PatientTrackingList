using Microsoft.AspNetCore.Mvc.RazorPages;
using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.Pages
{
    public class IndexModel : PageModel
    {

        private readonly DataContext _context;
        public IEnumerable<PTL> PTL { get; set; }
        public List<PTL> pageOfPTL { get; set; }
        public List<int> pageNumbers;

        public IndexModel(DataContext context)
        {
            _context = context;
            pageNumbers = new List<int>();
        }

        public DateTime CurrentYear;
        public DateTime PreviousYear;
        public DateTime EighteenWeekDate;
        public DateTime FiftyTwoWeekDate;
        public DateTime LastUpdatedDate;
        public bool isSortDesc; //not currently used as I can't make this work
        public int currentPageNo;
        public int nextPage;
        public int previousPage;
        

        public void OnGet(int? pNo, string? sortOrder = "", bool? isDesc=false, string? sNameSearch = null, string? sCGUSearch = null, bool? isUrgent=false, bool? isChecked=false)
        {
            int pageSize = 20;

            PTL = from p in _context.PTL
                  where p.ClockStart != null && p.ClockStop == null
                  orderby p.ClockStart
                  select p;

            isSortDesc = isDesc.GetValueOrDefault();

            CurrentYear = DateTime.Parse(DateTime.Now.Year + "-01-01");
            PreviousYear = DateTime.Parse((DateTime.Now.Year - 1) + "-01-01");
            EighteenWeekDate = DateTime.Now.AddDays(-18 * 7);
            FiftyTwoWeekDate = DateTime.Now.AddDays(-365);
            LastUpdatedDate = PTL.First().LastUpdatedDate;

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

            if (sNameSearch != null)
            {
                pageOfPTL = pageOfPTL.Where(p => p.PatientName.ToUpper().Contains(sNameSearch.ToUpper())).ToList();
            }

            if (sCGUSearch != null)
            {
                pageOfPTL = pageOfPTL.Where(p => p.CGUNo.Contains(sCGUSearch)).ToList();
            }

            if(isUrgent.GetValueOrDefault())
            {
                pageOfPTL = pageOfPTL.Where(p => p.Class == "Urgent").ToList();
            }

            if (isChecked.GetValueOrDefault())
            {
                pageOfPTL = pageOfPTL.Where(p => !p.isChecked).ToList();
            }

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
            //I can't do this calculation in the HTML so I have to do it here
            nextPage = currentPageNo + 1;
            previousPage = currentPageNo - 1;
        }


        public void OnPost(int? pNo, string? sortOrder = "", bool? isDesc=false, string? sNameSearch=null, string? sCGUSearch=null, bool? isUrgent=false, bool? isChecked=false)
        {
            int pageSize = 20;

            PTL = from p in _context.PTL
                  where p.ClockStart != null && p.ClockStop == null
                  orderby p.ClockStart
                  select p;

            pageOfPTL = PTL.Skip((pNo.GetValueOrDefault() + 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            
            Response.Redirect("Index?pNo=" + pNo + "&sortOrder=" + sortOrder + "&isDesc=" + isDesc + "&sNameSearch=" + sNameSearch + "&sCGUSearch=" + sCGUSearch + "&isUrgent=" + isUrgent + "&isChecked=" + isChecked);
        }
    }
}