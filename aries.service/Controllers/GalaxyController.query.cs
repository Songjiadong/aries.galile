using aries.common.grpc;
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
        public ActionResult Graph(GraphDegreeReq graphReq)
        {
            ActionResult result;
            AriesGalaxyGrpc.GraphDegreeReq req = graphReq.Convert();
            result= Get<GalaxyController, AriesJsonObjResp>(async () =>
            {
              return  await client.InvokeMethodGrpcAsync<AriesGalaxyGrpc.GraphDegreeReq, AriesJsonObjResp>(daprappqueryId, "Galaxy$Query$Graph", req);
            });
            return result;
        }
       
        [HttpPost("shortestpath")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult ShortestPath(ShortestPathReq shortestPathReq)
        {
            ActionResult result;
            AriesGalaxyGrpc.ShortestPathReq req = shortestPathReq.Convert();
            result = Get<GalaxyController, AriesJsonObjResp>(async () =>
            {
              return  await client.InvokeMethodGrpcAsync<AriesGalaxyGrpc.ShortestPathReq, AriesJsonObjResp>(daprappqueryId, "Galaxy$Query$ShortestPath", req);
            });  
            return result;
        }
        
    }
}
