using System;
using System.Threading.Tasks;
using BmiApp.Repository.Persistence;

namespace BmiApp.Repository.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IBmiRepository BmiRecords { get; }
        Task Save();
    }
}
