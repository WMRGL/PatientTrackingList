using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using PatientTrackingList.Data;
using PatientTrackingList.Models;
using PatientTrackingList.DataServices;

namespace PatientTrackingList.Pages
{
    public class ReferralDetailsModel : PageModel
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        private readonly PTLData _ptlData;
        private readonly SqlServices _sql;
        private readonly StaffData _staffData;
        private readonly ActivityData _activityData;
        private readonly DiaryData _diaryData;
        private readonly LetterData _letterData;

        public ReferralDetailsModel(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _ptlData = new PTLData(_context);
            _staffData = new StaffData(_context);
            _activityData = new ActivityData(_context);
            _diaryData = new DiaryData(_context);
            _letterData = new LetterData(_context);
            _sql = new SqlServices(_config);
        }

        public PTL RefDet { get; set; }
        public IEnumerable<Activity> ActivityList { get; set; }        
        public IEnumerable<Diary> DiaryList { get; set; }
        public IEnumerable<Letters> LetterList { get; set; }

        public DateTime EighteenWeekDate;
        public DateTime FiftyTwoWeekDate;
        public string Message;
        public bool isSuccess;

        [Authorize]
        public void OnGet(string ppi)
        {
            if (User.Identity.Name is null)
            {
                Response.Redirect("Login");
            }

            RefDet = _ptlData.GetPTLEntryDetails(ppi);
            var Referral = _activityData.GetReferralDetails(RefDet.RefID.GetValueOrDefault());
            ActivityList = _activityData.GetActivityList(Referral.CLINICNO);
            DiaryList = _diaryData.GetDiaryList(RefDet.RefID.GetValueOrDefault());
            LetterList = _letterData.GetLetterList(RefDet.RefID.GetValueOrDefault());

            EighteenWeekDate = RefDet.ClockStart.GetValueOrDefault().AddDays(18 * 7);
            FiftyTwoWeekDate = RefDet.ClockStart.GetValueOrDefault().AddDays(365);
        }

        public void OnPost(string ppi, string comments, bool? isChecked=false)
        {
            try
            {
                RefDet = _ptlData.GetPTLEntryDetails(ppi);
                var Referral = _activityData.GetReferralDetails(RefDet.RefID.GetValueOrDefault());
                ActivityList = _activityData.GetActivityList(Referral.CLINICNO);
                DiaryList = _diaryData.GetDiaryList(RefDet.RefID.GetValueOrDefault());
                LetterList = _letterData.GetLetterList(RefDet.RefID.GetValueOrDefault());

                EighteenWeekDate = RefDet.ClockStart.GetValueOrDefault().AddDays(18 * 7);
                FiftyTwoWeekDate = RefDet.ClockStart.GetValueOrDefault().AddDays(365);

                int iChecked = 0; //because SQL needs it to be a binary value
                if(isChecked.GetValueOrDefault())
                {
                    iChecked = 1;
                }               

                string username = _staffData.GetStaffMemberDetails(User.Identity.Name).STAFF_CODE;

                if (comments != null)
                {
                    comments = comments.Replace("'", "''");
                }

                _sql.SqlUpdateComments(comments, iChecked, username, ppi);
                                                
                isSuccess = true;
                Message = "Saved.";
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }
    }
}