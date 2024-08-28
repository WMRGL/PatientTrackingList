using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PatientTrackingList.Data;
using PatientTrackingList.DataServices;
using PatientTrackingList.Models;

namespace PatientTrackingList.Pages
{
    public class TrueWaitingListModel : PageModel
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        private readonly ISqlServices _sql;
        private readonly IStaffData _staffData;
        private readonly ITrueWaitingListData _trueWaitingListData;

        public TrueWaitingListModel(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _sql = new SqlServices(_config);
            _staffData = new StaffData(_context);
            _trueWaitingListData = new TrueWaitingListData(_context);
            
        }

        public IEnumerable<TrueWaitingList> TrueWaitingList {get; set;}
        public int Total;

        public void OnGet()
        {

            TrueWaitingList = _trueWaitingListData.GetTrueWaitingList();
            Total = TrueWaitingList.Count();
            //cancelled outcome table = clin_outcomes
        }
    }
}
