using Microsoft.AspNetCore.Mvc.RazorPages;
using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.Pages
{
    public class ReferralDetailsModel : PageModel
    {
        private readonly DataContext _context;

        public ReferralDetailsModel(DataContext context)
        {
            _context = context;
        }
        public PTL RefDet { get; set; }
        public IEnumerable<Activity> ActivityList { get; set; }        
        public IEnumerable<Diary> DiaryList { get; set; }
        public void OnGet(string sPPI)
        {
            RefDet = _context.PTL.FirstOrDefault(r => r.PPI == sPPI);

            ActivityList = from r in _context.Activity
                        where r.MPI == RefDet.MPI 
                   select r;

            DiaryList = from d in _context.Diary
                        where d.WMFACSID == RefDet.WMFACSID
                        select d;
        }
    }
}
