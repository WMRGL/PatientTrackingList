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
        private readonly MetaData meta;
        private readonly SqlServices sql;

        public ReferralDetailsModel(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            meta = new MetaData(_context);
            sql = new SqlServices(_config);
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

            RefDet = meta.GetPTLEntryDetails(ppi);
            var Referral = meta.GetReferralDetails(RefDet.RefID.GetValueOrDefault());
            ActivityList = meta.GetActivityList(Referral.CLINICNO);
            DiaryList = meta.GetDiaryList(RefDet.RefID.GetValueOrDefault());
            LetterList = meta.GetLetterList(RefDet.RefID.GetValueOrDefault());

            EighteenWeekDate = RefDet.ClockStart.GetValueOrDefault().AddDays(18 * 7);
            FiftyTwoWeekDate = RefDet.ClockStart.GetValueOrDefault().AddDays(365);
        }

        public void OnPost(string ppi, string comments, bool? isChecked=false)
        {
            try
            {
                RefDet = meta.GetPTLEntryDetails(ppi);
                var Referral = meta.GetReferralDetails(RefDet.RefID.GetValueOrDefault());
                ActivityList = meta.GetActivityList(Referral.CLINICNO);
                DiaryList = meta.GetDiaryList(RefDet.RefID.GetValueOrDefault());
                LetterList = meta.GetLetterList(RefDet.RefID.GetValueOrDefault());

                EighteenWeekDate = RefDet.ClockStart.GetValueOrDefault().AddDays(18 * 7);
                FiftyTwoWeekDate = RefDet.ClockStart.GetValueOrDefault().AddDays(365);

                int iChecked = 0; //because SQL needs it to be a binary value
                if(isChecked.GetValueOrDefault())
                {
                    iChecked = 1;
                }               

                string username = meta.GetStaffMemberDetails(User.Identity.Name).STAFF_CODE;

                if (comments != null)
                {
                    comments = comments.Replace("'", "''");
                }
                
                sql.SqlUpdateComments(comments, iChecked, username, ppi);
                                                
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