using BmiApp.Api.Controllers;
using BmiApp.Service.Dto.Auth;
using BmiApp.Service.Dto.Bmi;
using Microsoft.AspNetCore.Authorization;
using Xunit;

namespace BmiApp.Tests
{
    /// <summary>
    /// Guards the shape of the request contract. An owner or a role arriving from the client is
    /// what made escalation possible, so these assert there is no such field to send.
    /// </summary>
    public class RequestContractTests
    {
        [Fact]
        public void ReadingRecordsTakesNoParameter()
        {
            var method = typeof(BmiRecordsController).GetMethod(nameof(BmiRecordsController.GetBmiRecords));

            Assert.NotNull(method);
            Assert.Empty(method.GetParameters());
        }

        [Fact]
        public void RecordEndpointsRequireAnAuthorizedCaller()
        {
            Assert.NotEmpty(typeof(BmiRecordsController)
                .GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true));
        }

        [Fact]
        public void WriteRecordDtoCarriesNoOwner()
        {
            Assert.Null(typeof(BmiWriteRecordDto).GetProperty("Email"));
        }

        [Fact]
        public void ReadRecordDtoDoesNotExposeTheOwner()
        {
            Assert.Null(typeof(BmiReadRecordDto).GetProperty("Email"));
        }

        [Fact]
        public void RegistrationCarriesNoRoles()
        {
            Assert.Null(typeof(UserDto).GetProperty("Roles"));
        }
    }
}
