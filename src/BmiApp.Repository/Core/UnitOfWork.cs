using System.Threading.Tasks;
using BmiApp.Repository.Persistence;

namespace BmiApp.Repository.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IBmiRepository BmiRecords { get; }

        public UnitOfWork(ApplicationDbContext context, IBmiRepository bmiRepository)
        {
            this._context = context;
            this.BmiRecords = bmiRepository;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
