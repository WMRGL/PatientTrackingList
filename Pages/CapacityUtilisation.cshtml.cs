using Microsoft.AspNetCore.Mvc.RazorPages;
using PatientTrackingList.Data;
using PatientTrackingList.DataServices;
using PatientTrackingList.Models;

namespace PatientTrackingList.Pages
{
    public class CapacityUtilisationModel : PageModel
    {
        private readonly DataContext _context;
        private readonly ClinicSlotData _clinicSlotData;
        public IEnumerable<ClinicSlots> ClinicSlots { get; set; }
        public List<ClinicSlots> pageOfSlot { get; set; }
        public List<string> Clinicians { get; set; }
        public List<string> Clinics { get; set; }
        public List<string> Stati { get; set; }
        public List<int> pageNumbers;
        
        public int currentPageNo;
        public int nextPage;
        public int previousPage;
        public int listTotal;
        public string clincianSelected;
        public string clinicSelected;
        public string statusSelected;
        public DateTime toDateSelected;
        public DateTime fromDateSelected;
        

        public CapacityUtilisationModel(DataContext context)
        {
            _context = context;
            _clinicSlotData = new ClinicSlotData(_context);
            pageNumbers = new List<int>();            
            Clinicians = new List<string>();
            Clinics = new List<string>();
            Stati = new List<string>();           
        }

        public void OnGet(int? pNo, string? clinician, string? clinic, DateTime? fromDate, DateTime? toDate, string? status)
        {
            if (User.Identity.Name is null)
            {
                Response.Redirect("Login");
            }

            int pageSize = 20;            

            ClinicSlots = _clinicSlotData.GetClinicSlotsList();

            //for total
            //listTotal = ClinicSlots.Count();

            //drop-down menus
            Clinicians = ClinicSlots.Select(c => c.Clinician).Distinct().OrderBy(c => c).ToList();
            Clinics = ClinicSlots.Select(c => c.Facility).Distinct().OrderBy(c => c).ToList();
            Stati = ClinicSlots.Select(c => c.SlotStatus).Distinct().OrderBy(c => c).ToList();

            if (clinician != null)
            {
                ClinicSlots = ClinicSlots.Where(w => w.Clinician == clinician);
                clincianSelected = clinician;
            }

            if (clinic != null)
            {
                ClinicSlots = ClinicSlots.Where(w => w.Facility == clinic);
                clinicSelected = clinic;
            }

            if (fromDate == null)
            {
                fromDate = DateTime.Now;
            }
            
            if (toDate == null)
            {
                toDate = DateTime.Now.AddDays(500);
            }

            ClinicSlots = ClinicSlots.Where(w => w.SlotDate >= fromDate && w.SlotDate <= toDate);
            toDateSelected = toDate.GetValueOrDefault();
            fromDateSelected = fromDate.GetValueOrDefault();
            

            if (status != null)
            {
                ClinicSlots = ClinicSlots.Where(w => w.SlotStatus == status);
                statusSelected = status;
            }

            //paginator
            //List <ClinicSlots> pageOfSlot = new List<ClinicSlots>();
            pageOfSlot = ClinicSlots.ToList();
            
            int pp = pageOfSlot.Count() / pageSize;

            for (int i = 1; i <= pp; i++)
            {
                pageNumbers.Add(i);
            }

            pageOfSlot = pageOfSlot.Skip((pNo.GetValueOrDefault() - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();            
            
            if (pNo == null)
            {
                currentPageNo = 1;
            }
            else
            {
                currentPageNo = pNo.GetValueOrDefault();
            }

            nextPage = currentPageNo + 1;
            previousPage = currentPageNo - 1;
            listTotal = ClinicSlots.Count();
        }

        public void OnPost(int? pNo, string? clinician, string? clinic, DateTime? fromDate, DateTime? toDate, string? status)
        {
            int pageSize = 20;
            ClinicSlots = _clinicSlotData.GetClinicSlotsList();

            pageOfSlot = ClinicSlots.Skip((pNo.GetValueOrDefault() + 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

            Clinicians = ClinicSlots.Select(c => c.ClinicianID).Distinct().OrderBy(c => c).ToList();
            Clinics = ClinicSlots.Select(c => c.ClinicID).Distinct().OrderBy(c => c).ToList();

            Response.Redirect($"CapacityUtilisation?pNo={pNo}&clinician={clinician}&clinic={clinic}&fromDate={fromDate.Value.ToString("yyyy-MM-dd")}&toDate={toDate.Value.ToString("yyyy-MM-dd")}&status={status}");
        }
    }
}
