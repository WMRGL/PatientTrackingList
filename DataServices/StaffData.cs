using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.DataServices
{
    interface IStaffData
    {
        public List<StaffMembers> GetStaffMemberList();
        public List<StaffMembers> GetStaffTypeList(IEnumerable<StaffMembers> staffMembers, string sStaffType);
        public StaffMembers GetStaffMemberDetails(string username);
        public bool GetIsClinical(string username);
    }
    public class StaffData : IStaffData
    {
        private readonly DataContext _context;

        public StaffData(DataContext context)
        {
            _context = context;            
        }
               

        public List<StaffMembers> GetStaffMemberList() 
        { 
            IQueryable<StaffMembers> staffMembers = from s in _context.StaffMembers
                               where s.InPost == true
                               select s;

            return staffMembers.ToList();
        }

        public List<StaffMembers> GetStaffTypeList(IEnumerable<StaffMembers> staffMembers, string sStaffType) 
        {
            IEnumerable<StaffMembers> staffList = staffMembers.Where(s => s.CLINIC_SCHEDULER_GROUPS == sStaffType).OrderBy(s => s.NAME);

            return staffList.ToList();
        }
        
        
        public StaffMembers GetStaffMemberDetails(string username)
        {
            StaffMembers staffMember = _context.StaffMembers.FirstOrDefault(s => s.EMPLOYEE_NUMBER == username);

            return staffMember;
        }


        public bool GetIsClinical(string username)
        {
            string strStaffType = _context.StaffMembers.FirstOrDefault(s => s.EMPLOYEE_NUMBER == username).CLINIC_SCHEDULER_GROUPS;

            if (strStaffType == "GC" || strStaffType == "Consultant" || strStaffType == "SpR") return true;

            return false;
        }

    }
}
