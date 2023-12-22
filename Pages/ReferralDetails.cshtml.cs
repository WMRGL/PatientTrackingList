using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.Pages
{
    public class ReferralDetailsModel : PageModel
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;

        public ReferralDetailsModel(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public PTL RefDet { get; set; }
        public IEnumerable<Activity> ActivityList { get; set; }        
        public IEnumerable<Diary> DiaryList { get; set; }
        public IEnumerable<Letters> LetterList { get; set; }


        public DateTime EighteenWeekDate;
        public DateTime FiftyTwoWeekDate;
        public string Message;
        public bool isSuccess;
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

        public void OnPost(string sPPI, string sComments)
        {
            try
            {
                RefDet = _context.PTL.FirstOrDefault(r => r.PPI == sPPI);

                var Referral = _context.Activity.FirstOrDefault(r => r.RefID == RefDet.RefID);


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

                SqlConnection con = new SqlConnection(_config.GetConnectionString("ConString"));
                SqlCommand cmd = new SqlCommand();
                               

                if (sComments != null)
                {
                    sComments = sComments.Replace("'", "''");
                    cmd = new SqlCommand("update PTL set comments='" + sComments + "' where PPI='" + RefDet.PPI + "'", con);
                }
                else
                {
                    cmd = new SqlCommand("update PTL set comments = null where PPI='" + RefDet.PPI + "'", con);
                }
                
                con.Open();                
                cmd.ExecuteNonQuery();
                con.Close();

                //Response.Redirect("ReferralDetails?sPPi=" + RefDet.PPI);
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
