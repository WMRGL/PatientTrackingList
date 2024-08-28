using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.DataServices
{
    interface ITrueWaitingListData
    {
        public IEnumerable<TrueWaitingList> GetTrueWaitingList();
    }

    public class TrueWaitingListData : ITrueWaitingListData
    {
        private readonly DataContext _context;

        public TrueWaitingListData(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<TrueWaitingList> GetTrueWaitingList()
        {
            IEnumerable<TrueWaitingList> waitingList = from w in _context.TrueWaitingLists
                                                       where w.Complete == "Active" && w.RefDate != null && w.ClockStartDate != null && w.Status_Admin != "Complete"
                                                       && w.PATHWAY != null
                                                   select w;

            return waitingList;
        }


    }
}



