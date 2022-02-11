using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Model;
using Microsoft.EntityFrameworkCore;
using Repository.Core;
using Repository.Repositories;

namespace Repository.Persistence
{
    public class BmiRepository : RepositoryBase<BmiRecord>, IBmiRepository
    {
        public BmiRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<BmiRecord>> GetByEmailAsync(string email)
        {
            var bmiRecords = await Context.BmiRecords.Where(r => r.Email == email).ToListAsync();

            return bmiRecords;
        }

        public void AddRecord(BmiRecord bmiRecord)
        {
            Create(bmiRecord);
        }
    }
}
