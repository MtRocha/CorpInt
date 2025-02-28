using Intranet_NEW.Models.WEB;
using Microsoft.EntityFrameworkCore;

namespace Intranet_NEW.Controllers.DAL
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Colaborador> Colaborador { get; set; }
    }

}
