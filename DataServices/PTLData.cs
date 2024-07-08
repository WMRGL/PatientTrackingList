using Microsoft.AspNetCore.Identity;
using PatientTrackingList.Data;
using PatientTrackingList.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Razor.TagHelpers;

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
        private readonly IConfiguration _config;

        public PTLData(DataContext context)
        {
            _context = context;            
        }

        public IEnumerable<PTL> GetPTLList()
        {
            IEnumerable<PTL> ptl = _context.PTL.Where(p => p.ClockStart != null && p.MPI != 67066).AsNoTracking();
            
            return ptl;
        }

        public PTL GetPTLEntryDetails(int id)
        {
            PTL RefDet = _context.PTL.FirstOrDefault(r => r.ID == id);

            return RefDet;
        }
    }
}
