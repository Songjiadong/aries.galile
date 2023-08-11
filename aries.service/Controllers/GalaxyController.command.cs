using aries.common.grpc;
using aries.common.logger;
using aries.galaxy.grpc;
using Dapr;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;

namespace aries.service.Controllers
{
    public partial class GalaxyController
    {
        [HttpPut("org/submit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UnitSubmit(UnitInfoReq nodeReq)
        {
            ActionResult result;
            try
            {
                await client.InvokeMethodGrpcAsync<UnitInfoReq, Empty>(this.daprappcommandId, "Graph$Command$Node$Organization$Submit", nodeReq);
                result = Ok();
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
        [HttpDelete("org/delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UnitDelete(AriesIdReq idReq)
        {
            ActionResult result;
            try
            {
                await client.InvokeMethodGrpcAsync<AriesIdReq, Empty>(this.daprappcommandId, "Graph$Command$Node$Organization$Delete", idReq);
                result = Ok();
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
        [HttpPut("person/submit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PersonSubmit(PersonInfoReq nodeReq)
        {
            ActionResult result;
            try
            {
                await client.InvokeMethodGrpcAsync<PersonInfoReq, Empty>(this.daprappcommandId, "Graph$Command$Node$Person$Submit", nodeReq);
                result = Ok();
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
        [HttpDelete("person/delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PersonDelete(AriesIdReq idReq)
        {
            ActionResult result;
            try
            {
                await client.InvokeMethodGrpcAsync<AriesIdReq, Empty>(this.daprappcommandId, "Graph$Command$Node$Person$Delete", idReq);
                result = Ok();
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
        [HttpPut("relation/add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RelationAdd(UnitInfoReq nodeReq)
        {
            ActionResult result;
            try
            {
                await client.InvokeMethodGrpcAsync<UnitInfoReq, Empty>(this.daprappcommandId, "Graph$Command$Relation$Add", nodeReq);
                result = Ok();
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
        [HttpDelete("relation/remove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RelationRemove(AriesIdReq idReq) 
        {
            ActionResult result;
            try
            {
                await client.InvokeMethodGrpcAsync<AriesIdReq, Empty>(this.daprappcommandId, "Graph$Command$Relation$Remove", idReq);
                result = Ok();
            }
            catch (DaprApiException ex)
            {
                result = BadRequest(ex);
                LoggerService.Logger<GalaxyController>(ex,LogLevel.Error);
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
