using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.DataServices
{
    interface IWaitingListData
    {
        public IEnumerable<WaitingList> GetWaitingList();
    }
    public class WaitingListData : IWaitingListData
    { 
        private readonly DataContext _context;

        public WaitingListData(DataContext context)
        {
            _context = context;            
        }
                    
        public IEnumerable<WaitingList> GetWaitingList()
        {
            IEnumerable<WaitingList> waitingList = from w in _context.WaitingList
                          orderby w.PriorityLevel, w.AddedDate
                          select w;            
            
            return waitingList;
        }

                
    }
}
