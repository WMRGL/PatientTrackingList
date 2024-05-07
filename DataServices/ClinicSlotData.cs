using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.DataServices
{
    public class ClinicSlotData
    {
        private readonly DataContext _context;

        public ClinicSlotData(DataContext context)
        {
            _context = context;            
        }
        
        public List<ClinicSlots> GetClinicSlotsList() 
        {
            var clinicSlots = from s in _context.ClinicSlots
                              orderby s.SlotDate
                              select s;

            return clinicSlots.ToList();
        }
                
    }
}
