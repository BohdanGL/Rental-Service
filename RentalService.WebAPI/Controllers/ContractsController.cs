using MediatR;
using Microsoft.AspNetCore.Mvc;
using RentalService.Application.Contracts.Commands;
using RentalService.Application.Contracts.Models;
using RentalService.Application.Contracts.Queries;
using RentalService.WebAPI.Infrastructure.ApiKey;

namespace RentalService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractsController : BaseController
    {
        public ContractsController(IMediator mediator)
            : base(mediator)
        {
        }

        /// <summary>
        /// Gets all contracts.
        /// </summary>
        /// <response code="200">Success.</response>
        /// <returns>List of contracts.</returns>
        /// <example>
        /// GET: api/contracts.
        /// </example>
        [HttpGet]
        [ApiKey]
        [ProducesResponseType(typeof(IEnumerable<ContractModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetContracts(), HttpContext.RequestAborted));
        }

        /// <summary>
        /// Create contract.
        /// </summary>
        /// <response code="200">Success.</response>
        /// <returns>Craeted contract id.</returns>
        /// <example>
        /// POST: api/contracts.
        /// </example>
        [HttpPost()]
        [ApiKey]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<IActionResult> CreateContract(CreateContract command)
        {
            return Ok(await Mediator.Send(command, HttpContext.RequestAborted));
        }
    }
}
