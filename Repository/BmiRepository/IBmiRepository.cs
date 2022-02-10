using System.Collections.Generic;
using Data;

namespace Repository.BmiRepository
{
    public interface IBmiRepository
    {
        public IEnumerable<BmiRecord> GetBmiRecordsByEmail(string email);
    }
}
