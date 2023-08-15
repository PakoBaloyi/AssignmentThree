using AssignmentThreeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AssignmentThreeApi.Data
{
    public class PeopleDbContext : DbContext
    {
        public PeopleDbContext(DbContextOptions<PeopleDbContext> options) : base(options)
        {
        }
        public DbSet<PersonalDetails> PersonalDetailsTable { set; get; }
    }
}
