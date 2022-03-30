﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ch6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get(
            [FromServices] IPaymentService paymentService)
        {
            return paymentService.GetMessage();
        }
    }
}
