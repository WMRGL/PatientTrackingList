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
        public IEnumerable<Letters> LetterList { get; set; }


        public DateTime EighteenWeekDate;
        public DateTime FiftyTwoWeekDate;
        public void OnGet(string sPPI)
        {
            RefDet = _context.PTL.FirstOrDefault(r => r.PPI == sPPI);

            var Referral = _context.Activity.FirstOrDefault(r => r.RefID == RefDet.RefID);

            //ActivityList = from r in _context.Activity
            //            where r.WMFACSID == RefDet.WMFACSID 
            //            select r;

            ActivityList = from r in _context.Activity
                           where r.REFERRAL_CLINICNO == Referral.CLINICNO
                           select r;

            DiaryList = from d in _context.Diary
                        where d.RefID == RefDet.RefID
                        select d;

            LetterList = from l in _context.Letters
                         where l.RefID == RefDet.RefID
                         select l;

            EighteenWeekDate = RefDet.ClockStart.GetValueOrDefault().AddDays(18 * 7);
            FiftyTwoWeekDate = RefDet.ClockStart.GetValueOrDefault().AddDays(365);
        }
    }
}
