using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.Contracts.Models;
using RentalService.Persistence;

namespace RentalService.Application.Contracts.Queries
{
    public class GetContracts : IRequest<IEnumerable<ContractModel>>
    {
        public class Handler : IRequestHandler<GetContracts, IEnumerable<ContractModel>>
        {
            private readonly RentalServiceContext context;
            private readonly IMapper mapper;
            public Handler(RentalServiceContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<IEnumerable<ContractModel>> Handle(GetContracts request, CancellationToken cancellationToken)
            {
                return await context.Contracts
                    .AsNoTracking()
                    .ProjectTo<ContractModel>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }
        }
    }
}
