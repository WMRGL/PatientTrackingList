using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.DataServices
{
    public class StaffData
    {
        private readonly DataContext _context;

        public StaffData(DataContext context)
        {
            _context = context;            
        }
               

        public IEnumerable<StaffMembers> GetStaffMemberList() 
        { 
            var staffMembers = from s in _context.StaffMembers
                               where s.InPost == true
                               select s;

            return staffMembers;
        }

        public List<StaffMembers> GetStaffTypeList(IEnumerable<StaffMembers> staffMembers, string sStaffType) 
        { 
            var staffList = staffMembers.Where(s => s.CLINIC_SCHEDULER_GROUPS == sStaffType).OrderBy(s => s.NAME).ToList();

            return staffList;
        }
        
        
        public StaffMembers GetStaffMemberDetails(string username)
        {
            var staffMember = _context.StaffMembers.FirstOrDefault(s => s.EMPLOYEE_NUMBER == username);

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
