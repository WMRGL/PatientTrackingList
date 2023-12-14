using Microsoft.EntityFrameworkCore;
using PatientTrackingList.Models;

namespace PatientTrackingList.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<PTL> PTL {  get; set; }
        public DbSet<Activity> Activity { get; set; }
        public DbSet<Diary> Diary { get; set; }

        public bool isDesc { get; set; }
    }
}
