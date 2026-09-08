using System.Collections.Generic;
using System.Threading.Tasks;
using BmiApp.Data;
using BmiApp.Data.Model;
using BmiApp.Repository.Core;

namespace BmiApp.Repository.Persistence
{
    public interface IBmiRepository : IRepositoryBase<BmiRecord>
    {
        public Task<IEnumerable<BmiRecord>> GetByEmailAsync(string email);

        public void AddRecord(BmiRecord bmiRecord);
    }
}
