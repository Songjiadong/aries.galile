using aries.common.logger;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace aries.service.Controllers
{
    [Route("api/galile")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public partial class GalileoController : ControllerBase
    {
        private readonly DaprClient client;
        private readonly IConfiguration configuration;
        private readonly string daprappqueryId;
        public GalileoController(DaprClient client, IConfiguration configuration)
        {
            this.client = client;
            this.configuration = configuration;
            this.daprappqueryId = CheckandGetAppId("dapr_galile_query_app_id");
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
