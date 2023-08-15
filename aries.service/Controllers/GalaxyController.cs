using aries.common.logger;
using aries.webapi;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
namespace aries.service.Controllers
{
    [Route("api/galaxy/member")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public partial class GalaxyController : AriesWebAPIControllerBase
    {
        private readonly DaprClient client;
        private readonly string daprappqueryId;
        private readonly string daprappcommandId;
        public GalaxyController(DaprClient client, IConfiguration configuration)
        {
            this.client = client;
            this.configuration = configuration;
            this.daprappqueryId = CheckandGetAppId<GalaxyController>("dapr_galaxy_query_app_id");
            this.daprappcommandId= CheckandGetAppId<GalaxyController>("dapr_galaxy_command_app_id");

        }

    }
}
