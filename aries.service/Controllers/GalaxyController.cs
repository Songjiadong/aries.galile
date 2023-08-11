using aries.common.logger;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
namespace aries.service.Controllers
{
    [Route("api/galaxy/member")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public partial class GalaxyController : ControllerBase
    {
        private readonly DaprClient client;
        private readonly IConfiguration configuration;
        private readonly string daprappqueryId;
        private readonly string daprappcommandId;
        public GalaxyController(DaprClient client, IConfiguration configuration)
        {
            this.client = client;
            this.configuration = configuration;
            this.daprappqueryId=CheckandGetAppId("dapr_galaxy_query_app_id");
            this.daprappcommandId= CheckandGetAppId("dapr_galaxy_command_app_id");

        }
        private string CheckandGetAppId(string key)
        {
            string value = this.configuration.GetValue<string>(key);
            if (string.IsNullOrEmpty(value))
            {
                LoggerService.Logger<GalaxyController>(new Exception($"{key} cannot be null or empty!"), LogLevel.Error);
                throw new NullReferenceException();
            }
            else 
            {
                return value;
            }
        }

    }
}
