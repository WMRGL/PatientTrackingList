
using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.DataServices
{
    public class ActivityData
    {
        private readonly DataContext _context;

        public ActivityData(DataContext context)
        {
            _context = context;            
        }

        

        public Activity GetReferralDetails(int iRefID)
        {
            var Referral = _context.Activity.FirstOrDefault(r => r.RefID == iRefID);

            return Referral;
        }

        public List<Activity> GetActivityList(string clinicNo)
        {
            var ActivityList = from r in _context.Activity
                               where r.REFERRAL_CLINICNO == clinicNo
                               select r;

            return ActivityList.ToList();
        }

        

    }
}
