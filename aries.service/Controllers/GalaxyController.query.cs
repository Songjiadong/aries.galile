using aries.common.grpc;
using aries.common.logger;
using Dapr;
using Microsoft.AspNetCore.Mvc;
using AriesGalaxyGrpc = aries.galaxy.grpc;
using aries.service.galaxy.Views.request;

namespace aries.service.Controllers
{
    public partial class GalaxyController
    {
        [HttpPost("graph")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Graph(GraphDegreeReq graphReq)
        {
            ActionResult result;
            AriesGalaxyGrpc.GraphDegreeReq req = graphReq.Convert();
            try
            {
                var temp = await client.InvokeMethodGrpcAsync<AriesGalaxyGrpc.GraphDegreeReq, AriesJsonObjResp>(daprappqueryId, "Galaxy$Query$Graph", req);
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
        //[HttpPost("autocomplete")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<ActionResult> AutoComplete(GraphSearchReq searchReq)
        //{
        //    ActionResult result;
        //    try
        //    {
        //        var temp = await client.InvokeMethodGrpcAsync<GraphSearchReq, AriesJsonObjResp>(daprappqueryId, "Galaxy$Query$AutoComplete", searchReq);
        //        result = Ok(temp);
        //    }
        //    catch (DaprApiException ex)
        //    {
        //        result = BadRequest(ex);
        //        LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
        //    }
        //    catch (DaprException ex)
        //    {
        //        result = BadRequest(ex);
        //        LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
        //    }
        //    catch (Exception ex)
        //    {
        //        result = BadRequest(ex);
        //        LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
        //    }
        //    return result;
        //}
        [HttpPost("shortestpath")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ShortestPath(ShortestPathReq searchReq)
        {
            ActionResult result;
            AriesGalaxyGrpc.ShortestPathReq req = searchReq.Convert();
            try
            {
                var temp = await client.InvokeMethodGrpcAsync<AriesGalaxyGrpc.ShortestPathReq, AriesJsonObjResp>(daprappqueryId, "Galaxy$Query$ShortestPath", req);
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
