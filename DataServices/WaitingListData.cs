using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.DataServices
{
    public class WaitingListData
    { 
        private readonly DataContext _context;

        public WaitingListData(DataContext context)
        {
            _context = context;            
        }
                    
        public List<WaitingList> GetWaitingList()
        {
            var waitingList = from w in _context.WaitingList
                          orderby w.AddedDate
                          select w;            
            
            return waitingList.ToList();
        }

                
    }
}
