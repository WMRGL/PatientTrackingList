using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientTrackingList.Data;
using PatientTrackingList.Models;

namespace PatientTrackingList.DataServices
{
    public class MetaData
    {
        private readonly DataContext _context;

        public MetaData(DataContext context)
        {
            _context = context;            
        }

        public IEnumerable<PTL> GetPTLList()
        {
            var ptl = from p in _context.PTL
                    where p.ClockStart != null && p.ClockStop == null
                    orderby p.ClockStart
                    select p;

            return ptl;
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
        
        public PTL GetPTLEntryDetails(string sPPI)
        {
            var RefDet = _context.PTL.FirstOrDefault(r => r.PPI == sPPI);
            

            return RefDet;
        }

        public Activity GetReferralDetails(int iRefID)
        {
            var Referral = _context.Activity.FirstOrDefault(r => r.RefID == iRefID);

            return Referral;
        }

        public List<Activity> GetActivityList(string sClinicNo)
        {
            var ActivityList = from r in _context.Activity
                               where r.REFERRAL_CLINICNO == sClinicNo
                               select r;

            return ActivityList.ToList();
        }

        public List<Diary> GetDiaryList(int iRefID)
        {
            var DiaryList = from d in _context.Diary
                        where d.RefID == iRefID
                        select d;

            return DiaryList.ToList();
        }

        public List<Letters> GetLetterList(int iRefID)
        {
            var LetterList = from l in _context.Letters
                         where l.RefID == iRefID
                         select l;

            return LetterList.ToList();
        }
        
        public StaffMembers GetStaffMemberDetails(string username)
        {
            var staffMember = _context.StaffMembers.FirstOrDefault(s => s.EMPLOYEE_NUMBER == username);

            return staffMember;
        }
    }
}
