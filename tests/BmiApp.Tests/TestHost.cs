using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using BmiApp.Api.Controllers;
using BmiApp.Data.Model;
using BmiApp.Repository;
using BmiApp.Repository.Core;
using BmiApp.Repository.Persistence;
using BmiApp.Service;
using BmiApp.Service.Dto.Bmi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BmiApp.Tests
{
    /// <summary>
    /// Builds the real controller, service, unit of work and repository over an in-memory
    /// database, so a test runs the same path a request runs without the HTTP layer in the way.
    /// Each instance gets its own database.
    /// </summary>
    internal sealed class TestHost : IDisposable
    {
        public ApplicationDbContext Context { get; }

        public TestHost()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("bmi-" + Guid.NewGuid())
                .Options;

            Context = new ApplicationDbContext(options);
        }

        /// <summary>
        /// A controller whose token identity is <paramref name="email"/>. Pass null to model a
        /// principal that carries no name claim.
        /// </summary>
        public BmiRecordsController ControllerFor(string email)
        {
            var unitOfWork = new UnitOfWork(Context, new BmiRepository(Context));

            var identity = email == null
                ? new ClaimsIdentity()
                : new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }, "TestAuth");

            return new BmiRecordsController(new BmiService(unitOfWork, Mapper()))
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
                }
            };
        }

        public void Seed(params (string Email, double Bmi)[] records)
        {
            foreach (var (email, bmi) in records)
            {
                Context.BmiRecords.Add(new BmiRecord
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    Weight = 80,
                    Height = 180,
                    Bmi = bmi
                });
            }

            Context.SaveChanges();
        }

        public IReadOnlyList<BmiRecord> StoredRecords() =>
            Context.BmiRecords.AsNoTracking().ToList();

        /// <summary>
        /// A stand-in for the AutoMapper profile. What these tests assert is which records a
        /// caller reaches and which email a new record is stored under, and the mapper takes no
        /// part in either decision.
        /// </summary>
        private static IMapper Mapper()
        {
            var mapper = new Mock<IMapper>();

            mapper.Setup(m => m.Map<BmiRecord>(It.IsAny<object>()))
                .Returns<object>(source =>
                {
                    var dto = (BmiWriteRecordDto)source;

                    return new BmiRecord
                    {
                        Id = Guid.NewGuid(),
                        Weight = dto.Weight,
                        Height = dto.Height,
                        Bmi = dto.Bmi
                    };
                });

            mapper.Setup(m => m.Map<IEnumerable<BmiReadRecordDto>>(It.IsAny<object>()))
                .Returns<object>(source => ((IEnumerable<BmiRecord>)source)
                    .Select(r => new BmiReadRecordDto { Weight = r.Weight, Height = r.Height, Bmi = r.Bmi })
                    .ToList());

            return mapper.Object;
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
