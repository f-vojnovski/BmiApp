using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BmiApp.Service.Dto.Bmi;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace BmiApp.Tests
{
    /// <summary>
    /// The record owner is taken from the validated token, so a caller reaches their own records
    /// and nothing else. These tests are the reason the endpoints take no email from the client.
    /// </summary>
    public class BmiRecordAuthorizationTests
    {
        private const string UserA = "a@example.com";
        private const string UserB = "b@example.com";

        [Fact]
        public async Task UserA_ReadingRecords_SeesOnlyTheirOwn()
        {
            using var host = new TestHost();
            host.Seed((UserA, 21.1), (UserA, 22.2), (UserB, 33.3));

            var result = await host.ControllerFor(UserA).GetBmiRecords();

            var records = Records(result);
            Assert.Equal(2, records.Count);
            Assert.DoesNotContain(records, r => Math.Abs(r.Bmi - 33.3) < 0.0001);
        }

        [Fact]
        public async Task UserA_CannotReadUserBsRecords_WhenUserAHasNone()
        {
            using var host = new TestHost();
            host.Seed((UserB, 33.3));

            var result = await host.ControllerFor(UserA).GetBmiRecords();

            Assert.Empty(Records(result));
        }

        [Fact]
        public async Task WritingARecord_StoresItAgainstTheCallerFromTheToken()
        {
            using var host = new TestHost();

            var response = await host.ControllerFor(UserA).CreateBmiRecord(Written(24.7));

            Assert.IsType<OkResult>(response);
            var stored = Assert.Single(host.StoredRecords());
            Assert.Equal(UserA, stored.Email);
        }

        [Fact]
        public async Task UserA_WritingARecord_LeavesUserBsHistoryUntouched()
        {
            using var host = new TestHost();
            host.Seed((UserB, 33.3));

            await host.ControllerFor(UserA).CreateBmiRecord(Written(24.7));

            var usersBRecords = host.StoredRecords().Where(r => r.Email == UserB).ToList();

            Assert.Equal(33.3, Assert.Single(usersBRecords).Bmi, 3);
        }

        [Fact]
        public async Task ReadingRecords_IsUnauthorized_WhenTheTokenCarriesNoIdentity()
        {
            using var host = new TestHost();
            host.Seed((UserB, 33.3));

            var result = await host.ControllerFor(null).GetBmiRecords();

            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        [Fact]
        public async Task WritingARecord_IsUnauthorized_WhenTheTokenCarriesNoIdentity()
        {
            using var host = new TestHost();

            var response = await host.ControllerFor(null).CreateBmiRecord(Written(24.7));

            Assert.IsType<UnauthorizedResult>(response);
            Assert.Empty(host.StoredRecords());
        }

        private static BmiWriteRecordDto Written(double bmi) =>
            new BmiWriteRecordDto { Weight = 80, Height = 180, Bmi = bmi };

        private static List<BmiReadRecordDto> Records(ActionResult<IEnumerable<BmiReadRecordDto>> result) =>
            Assert.IsAssignableFrom<IEnumerable<BmiReadRecordDto>>(
                Assert.IsType<OkObjectResult>(result.Result).Value).ToList();
    }
}
