using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Transfer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ITransferService _trasnferService;

        public TransferController(ITransferService trasnferService)
        {
            _trasnferService = trasnferService;
        }

        // GET api/transfer
        [HttpGet]
        public ActionResult<IEnumerable<TransferLog>> Get()
        {
            return Ok(_trasnferService.GetTransferLogs());
        }

    }
}
