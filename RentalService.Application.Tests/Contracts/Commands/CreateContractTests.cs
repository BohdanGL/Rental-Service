using NUnit.Framework;
using RentalService.Application.Contracts.Commands;
using RentalService.Application.Infrastructure;
using RentalService.Application.Infrastructure.Exceptions;

namespace RentalService.Application.Tests.Contracts.Commands
{
    public class CreateContractTests : TestBase
    {
        private CreateContract.Handler handler;
        private CreateContract.Validator validator;
        private RequestValidationBehaviour<CreateContract, int> behaviour;

        [SetUp]
        public void Setup()
        {
            handler = new CreateContract.Handler(Context);
            validator = new CreateContract.Validator();
            behaviour = new RequestValidationBehaviour<CreateContract, int>(new[] { validator });
        }

        [Test]
        public async Task CreateContractTest_Handler_Success()
        {
            //Arrange
            var contractsBefore = Context.Contracts.Count();
            var facilityId = Context.Facilities.Last().Id;
            var equipmentId = Context.Equipments.Last().Id;

            //Act
            var result = await handler.Handle(new CreateContract()
            {
                FacilityId = facilityId,
                EquipmentId = equipmentId,
                EquipmentCount = 1,
            }, CancellationToken.None);

            //Assert
            var contractsAfter = Context.Contracts.Count();
            Assert.That(result, Is.Not.Zero);
            Assert.That(contractsAfter, Is.EqualTo(contractsBefore + 1));
        }

        [Test]
        public async Task CreateContractTest_Handler_Fail_NoAvailableSquare()
        {
            //Arrange
            var facilityId = Context.Facilities.Last().Id;
            var equipmentId = Context.Equipments.Last().Id;

            //Act
            var result = new AsyncTestDelegate(async () => await handler.Handle(
                new CreateContract
                {
                    FacilityId = facilityId,
                    EquipmentId = equipmentId,
                    EquipmentCount = 999,
                }, CancellationToken.None));

            //Assert
            Assert.ThrowsAsync<ValidationException>(result);
        }

        [Test]
        public async Task CreateContractTest_Validator_Fail_NoEquipmentCount()
        {
            //Arrange
            var facilityId = Context.Facilities.Last().Id;
            var equipmentId = Context.Equipments.Last().Id;
            var nextResponse = 1;

            //Act
            var result = new AsyncTestDelegate(async () => await behaviour.Handle(
                new CreateContract
                {
                    FacilityId = facilityId,
                    EquipmentId = equipmentId,
                }, () => Task.FromResult(nextResponse),
                CancellationToken.None));

            //Assert
            Assert.ThrowsAsync<ValidationException>(result);
        }
    }
}
