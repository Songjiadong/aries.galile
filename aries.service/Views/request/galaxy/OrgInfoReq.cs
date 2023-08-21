using aries.webapi;
using AriesGrpc = aries.galaxy.grpc;

namespace aries.service.galaxy.Views.request
{
    public class OrgInfoReq : TReq<AriesGrpc.OrgInfoReq>
    {
        public OrgInfoReq() { }
        public string? Id { get; set; }
        public string? Name { get; set; }
        public override AriesGrpc.OrgInfoReq Convert()
        {
            return new AriesGrpc.OrgInfoReq()
            {
                Organization = new AriesGrpc.OrgInfo()
                {
                    Id = Id,
                    Name = Name,
                }
            };
        }
    }
}
