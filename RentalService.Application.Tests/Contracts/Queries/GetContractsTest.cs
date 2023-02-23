using AutoMapper;
using NUnit.Framework;
using RentalService.Application.Contracts.Models;
using RentalService.Application.Contracts.Queries;
using RentalService.Application.Infrastructure.Mapping;

namespace RentalService.Application.Tests.Contracts.Queries
{
    public class GetContractsTest : TestBase
    {
        private GetContracts.Handler handler;

        [SetUp]
        public void Setup()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = mapperConfig.CreateMapper();
            handler = new GetContracts.Handler(Context, mapper);
        }

        [Test]
        public async Task GetContractsTest_Success()
        {
            //Arrange
            var contractsBefore = Context.Contracts.Count();

            //Act
            var result = await handler.Handle(new GetContracts(), CancellationToken.None);

            //Assert
            Assert.That(result.Count(), Is.EqualTo(contractsBefore));
            Assert.That(result, Is.TypeOf(typeof(List<ContractModel>)));
        }
    }
}
