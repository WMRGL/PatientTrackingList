
using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.DataServices
{
    interface IActivityData
    {
        public Activity GetReferralDetails(int iRefID);
        public List<Activity> GetActivityList(string clinicNo);
    }
    public class ActivityData : IActivityData
    {
        private readonly DataContext _context;

        public ActivityData(DataContext context)
        {
            _context = context;            
        }        

        public Activity GetReferralDetails(int iRefID)
        {
            Activity Referral = _context.Activity.FirstOrDefault(r => r.RefID == iRefID);            

            return Referral;
        }

        public List<Activity> GetActivityList(string clinicNo)
        {
            IQueryable<Activity> ActivityList = from r in _context.Activity
                               where r.REFERRAL_CLINICNO == clinicNo
                               select r;

            return ActivityList.ToList();
        }

        

    }
}
