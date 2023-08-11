using aries.common.grpc;
using aries.common.logger;
using aries.galaxy.grpc;
using Dapr;
using Microsoft.AspNetCore.Mvc;


namespace aries.service.Controllers
{
    public partial class GalaxyController
    {
        [HttpPost("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Search(GraphDegreeSearchReq searchReq)
        {
            ActionResult result;
            try
            {
                var temp = await client.InvokeMethodGrpcAsync<GraphDegreeSearchReq, AriesJsonObjResp>(daprappqueryId, "Galaxy$Query$Search", searchReq);
                result = Ok(temp);
            }
            catch (DaprApiException ex)
            {
                result = BadRequest(ex);
                LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
            }
            catch (DaprException ex)
            {
                result = BadRequest(ex);
                LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
            }
            catch (Exception ex)
            {
                result = BadRequest(ex);
                LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
            }
            return result;
        }
        [HttpPost("autocomplete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AutoComplete(GraphSearchReq searchReq)
        {
            ActionResult result;
            try
            {
                var temp = await client.InvokeMethodGrpcAsync<GraphSearchReq, AriesJsonObjResp>(daprappqueryId, "Galaxy$Query$AutoComplete", searchReq);
                result = Ok(temp);
            }
            catch (DaprApiException ex)
            {
                result = BadRequest(ex);
                LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
            }
            catch (DaprException ex)
            {
                result = BadRequest(ex);
                LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
            }
            catch (Exception ex)
            {
                result = BadRequest(ex);
                LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
            }
            return result;
        }
        [HttpPost("shortestpath")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ShortestPath(ShortestPathReq searchReq)
        {
            ActionResult result;
            try
            {
                var temp = await client.InvokeMethodGrpcAsync<ShortestPathReq, AriesJsonObjResp>(daprappqueryId, "Galaxy$Query$ShortestPath", searchReq);
                result = Ok(temp);
            }
            catch (DaprApiException ex)
            {
                result = BadRequest(ex);
                LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
            }
            catch (DaprException ex)
            {
                result = BadRequest(ex);
                LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);

            }
            catch (Exception ex)
            {
                result = BadRequest(ex);
                LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
            }
            return result;
        }
        
    }
}
