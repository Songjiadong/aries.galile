using aries.common.logger;
using aries.webapi;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace aries.service.Controllers
{
    [Route("api/galileo")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public partial class GalileoController : AriesWebAPIControllerBase
    {
        private readonly DaprClient client;

        private readonly string daprappqueryId;
        public GalileoController(DaprClient client, IConfiguration configuration)
        {
            this.client = client;
            this.configuration = configuration;
            this.daprappqueryId = CheckandGetAppId<GalileoController>("dapr_galileo_query_app_id");
        }
       
    }
}
