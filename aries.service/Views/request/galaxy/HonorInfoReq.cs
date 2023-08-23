using aries.galaxy.grpc;
using aries.webapi;
using AriesGrpc = aries.galaxy.grpc;
namespace aries.service.galaxy.Views.request
{
    public class HonorInfoReq : TReq<AriesGrpc.HonorInfoReq>
    {
        public string? Id { get; set; }
        public string? Name { get; set; }

        public override AriesGrpc.HonorInfoReq Convert()
        {
            return new AriesGrpc.HonorInfoReq()
            {
                Honor = new AriesGrpc.HonorInfo()
                {
                    Id = Id,
                    Name = Name,
                }
            };
        }
    }
}
