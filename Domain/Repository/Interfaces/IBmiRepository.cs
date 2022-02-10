using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repository.Interfaces
{
    public interface IBmiRepository
    {
        Task<IEnumerable<BMI>> GetAllAsync();
    }
}
