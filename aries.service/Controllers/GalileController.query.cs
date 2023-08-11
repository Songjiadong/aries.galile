using aries.common.grpc;
using aries.common.logger;
using Dapr;
using Microsoft.AspNetCore.Mvc;
using AriesGlileGrpc = aries.galile.grpc;
using AriesCollectorGrpc = aries.collector.grpc;
using aries.service.galile.Views.request;
using aries.common;
using aries.common.net;
using Google.Protobuf.WellKnownTypes;

namespace aries.service.Controllers
{
    public partial class GalileController
    {
        [HttpPost("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Search(SearchReq searchReq)
        {
            ActionResult result;
            AriesGlileGrpc.SearchReq req = searchReq.Convert();
            try
            {
                var temp = await client.InvokeMethodGrpcAsync<AriesGlileGrpc.SearchReq, AriesJsonObjResp>(daprappqueryId, "Galile$Query$Search", req);
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
        [HttpPost("searchByIndex")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SearchByIndex(SearchByIndexReq searchReq) 
        {
            ActionResult result;
            AriesGlileGrpc.SearchByIndexReq req = searchReq.Convert();
           
            try
            {
                var temp = await client.InvokeMethodGrpcAsync<AriesGlileGrpc.SearchByIndexReq, AriesJsonObjResp>(daprappqueryId, "Galile$Query$SearchByIndex", req);
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
        [HttpPut("browse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task Browse(string url)
        {
            AriesCollectorGrpc.CollectInfo req = new AriesCollectorGrpc.CollectInfo()
            {
                Url = url,
                Business = (Int32)BusinessEnum.Search,
                Ip = IpService.GetRealClientIpAddress(this.ControllerContext.HttpContext),
                Title="",
                UserName = ""
            };
            try
            {
                await client.InvokeMethodGrpcAsync<AriesCollectorGrpc.CollectInfo, Empty>(daprappqueryId, "Collector$Manage$UserBehaviorCollect", req);
            }
            catch (DaprApiException ex)
            {
                LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
            }
            catch (DaprException ex)
            {

                LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
            }
            catch (Exception ex)
            {

                LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
            }
        }
        [HttpGet("{topNum}/getTopList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetTopList(int topNum)
        {
            ActionResult result;
            AriesGlileGrpc.TopReq req = new AriesGlileGrpc.TopReq()
            {
                Top = topNum
            };
            try
            {
                var temp = await client.InvokeMethodGrpcAsync<AriesGlileGrpc.TopReq, AriesJsonObjResp>(daprappqueryId, "Galile$Query$GetTopList", req);
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