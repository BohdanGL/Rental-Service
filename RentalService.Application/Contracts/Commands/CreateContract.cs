using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Domain.Entities;
using RentalService.Persistence;
using ValidationException = RentalService.Application.Infrastructure.Exceptions.ValidationException;

namespace RentalService.Application.Contracts.Commands
{
    public class CreateContract : IRequest<int>
    {
        public int FacilityId { get; set; }
        public int EquipmentId { get; set; }
        public int EquipmentCount { get; set; }

        public class Validator : AbstractValidator<CreateContract>
        {
            public Validator()
            {
                RuleFor(x => x.FacilityId)
                    .NotEmpty();

                RuleFor(x => x.EquipmentId)
                    .NotEmpty();

                RuleFor(x => x.EquipmentCount)
                    .NotEmpty()
                    .GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<CreateContract, int>
        {
            private readonly RentalServiceContext context;
            public Handler(RentalServiceContext context)
            {
                this.context = context;
            }

            public async Task<int> Handle(CreateContract request, CancellationToken cancellationToken)
            {
                var facility = await context.Facilities.FirstOrDefaultAsync(x => x.Id == request.FacilityId, cancellationToken);
                if (facility == null)
                {
                    throw new ValidationException($"Facility with ID {request.FacilityId} doesn't exist.");
                }

                var equipment = await context.Equipments.FirstOrDefaultAsync(x => x.Id == request.EquipmentId, cancellationToken);
                if (equipment == null)
                {
                    throw new ValidationException($"Equipment with ID {request.EquipmentId} doesn't exist.");
                }

                var usedSquare = context.Contracts
                    .AsNoTracking()
                    .Where(x => x.Facility.Id == facility.Id)
                    .Select(x => x.Equipment.Square * x.EquipmentCount)
                    .Sum();

                if (equipment.Square * request.EquipmentCount > (facility.AvailableSquare - usedSquare))
                {
                    throw new ValidationException($"No enough available square to place equipment");
                }

                var contract = new Contract
                {
                    Facility = facility,
                    Equipment = equipment,
                    EquipmentCount = request.EquipmentCount,
                };
                await context.Contracts.AddAsync(contract, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return contract.Id;
            }
        }
    }
}
