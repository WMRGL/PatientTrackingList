using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PatientTrackingList.Data;
using PatientTrackingList.Models;
using PatientTrackingList.DataServices;

namespace PatientTrackingList.Pages
{
    public class ReferralDetailsModel : PageModel
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        private readonly IPTLData _ptlData;        
        private readonly IStaffData _staffData;
        private readonly IActivityData _activityData;
        private readonly IDiaryData _diaryData;
        private readonly ILetterData _letterData;
        private readonly ISqlServices _sql;

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
        public List<Activity> ActivityList { get; set; }        
        public List<Diary> DiaryList { get; set; }
        public List<Letters> LetterList { get; set; }

        public DateTime EighteenWeekDate;
        public DateTime FiftyTwoWeekDate;
        public string Message;
        public bool isSuccess;

        [Authorize]
        public void OnGet(int id, string? message = "", bool? success = false)
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
                    staffCode = _staffData.GetStaffMemberDetails(User.Identity.Name).STAFF_CODE;
                    _sql.SqlWriteUsageAudit(staffCode, $"PTL ID={id.ToString()}", "Details");
                }

                Message = message;
                isSuccess = success.GetValueOrDefault();
                RefDet = _ptlData.GetPTLEntryDetails(id);
                int refID = RefDet.RefID;
                var Referral = _activityData.GetReferralDetails(refID);
                ActivityList = _activityData.GetActivityList(Referral.CLINICNO);
                DiaryList = _diaryData.GetDiaryList(refID);
                LetterList = _letterData.GetLetterList(refID);

                EighteenWeekDate = RefDet.ClockStart.GetValueOrDefault().AddDays(18 * 7);
                FiftyTwoWeekDate = RefDet.ClockStart.GetValueOrDefault().AddDays(365);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        public void OnPost(int id, string? comments="", bool? isChecked=false)
        {
            try
            {                
                RefDet = _ptlData.GetPTLEntryDetails(id);
                int refID = RefDet.RefID;
                var Referral = _activityData.GetReferralDetails(refID);
                ActivityList = _activityData.GetActivityList(Referral.CLINICNO);
                DiaryList = _diaryData.GetDiaryList(refID);
                LetterList = _letterData.GetLetterList(refID);

                EighteenWeekDate = RefDet.ClockStart.GetValueOrDefault().AddDays(18 * 7);
                FiftyTwoWeekDate = RefDet.ClockStart.GetValueOrDefault().AddDays(365);

                int iChecked = 0; //because SQL needs it to be a binary value
                
                if(isChecked.GetValueOrDefault()) { iChecked = 1; }

                string username = _staffData.GetStaffMemberDetails(User.Identity.Name).STAFF_CODE;

                _sql.SqlUpdateComments(comments, iChecked, username, id);
                                                
                isSuccess = true;
                string message = "Saved.";
                
                Response.Redirect($"ReferralDetails?id={id.ToString()}&message={message}&success={isSuccess}");
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }
    }
}