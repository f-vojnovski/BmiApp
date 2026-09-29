using System.Collections.Generic;
using System.Threading.Tasks;
using BmiApp.Data.Model;
using Microsoft.EntityFrameworkCore;
using BmiApp.Repository.Core;

namespace BmiApp.Repository.Persistence
{
    public class BmiRepository : RepositoryBase<BmiRecord>, IBmiRepository
    {
        public BmiRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<BmiRecord>> GetByEmailAsync(string email)
        {
            var bmiRecords = await FindByCondition(r => r.Email == email).ToListAsync();

            return bmiRecords;
        }

        public void AddRecord(BmiRecord bmiRecord)
        {
            Create(bmiRecord);
        }
    }
}
