using System.Threading.Tasks;
using BmiApp.Repository.Persistence;

namespace BmiApp.Repository.Core
{
    public interface IUnitOfWork
    {
        IBmiRepository BmiRecords { get; }
        Task Save();
    }
}
