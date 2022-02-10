using System.Collections.Generic;
using System.Linq;
using Data;

namespace Repository.BmiRepository
{
    public class BmiRepository : IBmiRepository
    {
        private readonly ApplicationDbContext _context;

        public BmiRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public IEnumerable<BmiRecord> GetBmiRecordsByEmail(string email)
        {
            //var bmiRecords = context.BmiRecords.Where(r => r.Email == email);

            return null;
        }
    }
}
