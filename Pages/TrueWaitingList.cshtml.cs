using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PatientTrackingList.Data;
using PatientTrackingList.DataServices;
using PatientTrackingList.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;


namespace PatientTrackingList.Pages
{
    public class TrueWaitingListModel : PageModel
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        private readonly ISqlServices _sql;
        private readonly IStaffData _staffData;
        private readonly ITrueWaitingListData _trueWaitingListData;
        private readonly IConfiguration Configuration;

        public TrueWaitingListModel(DataContext context, IConfiguration config, IConfiguration configuration)
        {
            _context = context;
            _config = config;
            _sql = new SqlServices(_config);
            _staffData = new StaffData(_context);
            _trueWaitingListData = new TrueWaitingListData(_context);
            Configuration = configuration;
        }

        public IEnumerable<TrueWaitingList> TrueWaitingList {get; set;}
        public int Total;
        public PaginatedList<TrueWaitingList> TrueWaitingLists { get; set; }
        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            var trueWaitingListQuery = _trueWaitingListData.GetTrueWaitingList();
            var pageSize = 10;
            TrueWaitingLists = await PaginatedList<TrueWaitingList>.CreateAsync(
                trueWaitingListQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
        }


    }
}
