using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Data.Model;
using Repository.Core;

namespace Repository.Persistence
{
    public interface IBmiRepository : IRepositoryBase<BmiRecord>
    {
        public Task<IEnumerable<BmiRecord>> GetByEmailAsync(string email);

        public void AddRecord(BmiRecord bmiRecord);
    }
}
