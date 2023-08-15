using aries.webapi;
using AriesGrpc = aries.galaxy.grpc;
using System;
namespace aries.service.galaxy.Views.request
{
    public class ShortestPathReq : TReq<AriesGrpc.ShortestPathReq>
    {
        public string? Start { get; set; }
        public string? End { get; set; }
        public override AriesGrpc.ShortestPathReq Convert()
        {
            return new AriesGrpc.ShortestPathReq { Start = Start, End = End };
        }

    }
}
