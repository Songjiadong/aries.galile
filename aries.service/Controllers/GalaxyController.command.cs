using aries.common.grpc;
using AriesGalaxyGrpc = aries.galaxy.grpc;
using aries.service.galaxy.Views.request;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;

namespace aries.service.Controllers
{
    public partial class GalaxyController
    {
        [HttpPut("org/submit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult OrgSubmit(OrgInfoReq req)
        {
            ActionResult result;
            AriesGalaxyGrpc.OrgInfoReq orgInfoReq = req.Convert();
            result = Submit<GalaxyController>(req.Id!, async () =>
            {
                return await client.InvokeMethodGrpcAsync<AriesGalaxyGrpc.OrgInfoReq, Empty>(this.daprappcommandId, "Graph$Command$Node$Organization$Submit", orgInfoReq);
            });
            return result;
        }
        [HttpDelete("org/{businessId}/delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult OrgDelete(string businessId)
        {
            ActionResult result;
            result = Delete<GalaxyController>(businessId, async () =>
            {
                return await client.InvokeMethodGrpcAsync<AriesIdReq, Empty>(this.daprappcommandId, "Graph$Command$Node$Organization$Delete", new AriesIdReq { Id= businessId });
            });
            return result;
        }
        [HttpPut("person/submit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PersonSubmit(PersonInfoReq req)
        {
            ActionResult result;
            AriesGalaxyGrpc.PersonInfoReq personInfoReq = req.Convert();
            result = Submit<GalaxyController>(req.Id!, async () =>
            {
                return await client.InvokeMethodGrpcAsync<AriesGalaxyGrpc.PersonInfoReq, Empty>(this.daprappcommandId, "Graph$Command$Node$Person$Submit", personInfoReq);
            });
            return result;
        }
        [HttpDelete("person/{businessId}/delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PersonDelete(string businessId)
        {
            ActionResult result;
            result = Delete<GalaxyController>(businessId, async () =>
            {
                return await client.InvokeMethodGrpcAsync<AriesIdReq, Empty>(this.daprappcommandId, "Graph$Command$Node$Person$Delete", new AriesIdReq() { Id = businessId });
            });
            return result;
        }
        [HttpPut("relation/add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult RelationAdd(OrgInfoReq req)
        {
            ActionResult result;
            AriesGalaxyGrpc.OrgInfoReq nodeInfoReq = req.Convert();
            result = Submit<GalaxyController>("", async () =>
            {
              return  await client.InvokeMethodGrpcAsync<AriesGalaxyGrpc.OrgInfoReq, Empty>(this.daprappcommandId, "Graph$Command$Relation$Add", nodeInfoReq);
            });
            return result;
        }
        [HttpDelete("relation/{businessId}/remove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult RelationRemove(string businessId) 
        {
            ActionResult result;
            result = Delete<GalaxyController>(businessId, async () =>
            {
                return await client.InvokeMethodGrpcAsync<AriesIdReq, Empty>(this.daprappcommandId, "Graph$Command$Relation$Remove", new AriesIdReq() { Id= businessId });
            });
            return result;
        }
    }
}
