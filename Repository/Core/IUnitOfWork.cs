using System;
using System.Threading.Tasks;
using Repository.Persistence;

namespace Repository.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IBmiRepository BmiRecords { get; }
        Task Save();
    }
}
