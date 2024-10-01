using Microsoft.EntityFrameworkCore;

namespace Lab17
{
    public class Labcontext : DbContext
    {
        public DbSet<Example> Examples { get; set; }
        public Labcontext(DbContextOptions<Labcontext> options) : base(options) { }
    }
}
