using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.DataServices
{
    interface IPTLData
    {
        public IEnumerable<PTL> GetPTLList();
        public PTL GetPTLEntryDetails(int id);       
    }
    public class PTLData : IPTLData
    {
        private readonly DataContext _context;

        public PTLData(DataContext context)
        {
            _context = context;            
        }

        public IEnumerable<PTL> GetPTLList()
        {
            IEnumerable<PTL> ptl = from p in _context.PTL
                    where p.ClockStart != null && p.ClockStop == null && p.MPI != 67066
                    orderby p.ClockStart
                    select p;
            return ptl;
        }

        public PTL GetPTLEntryDetails(int id)
        {
            PTL RefDet = _context.PTL.FirstOrDefault(r => r.ID == id);

            return RefDet;
        }
    }
}
