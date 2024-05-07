using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.DataServices
{
    public class PTLData
    {
        private readonly DataContext _context;

        public PTLData(DataContext context)
        {
            _context = context;            
        }

        public IEnumerable<PTL> GetPTLList()
        {
            var ptl = from p in _context.PTL
                    where p.ClockStart != null && p.ClockStop == null && p.MPI != 67066
                    orderby p.ClockStart
                    select p;

            return ptl;
        }
                
        public PTL GetPTLEntryDetails(string ppi)
        {
            var RefDet = _context.PTL.FirstOrDefault(r => r.PPI == ppi);
            

            return RefDet;
        }

    }
}
