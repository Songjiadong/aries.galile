using aries.webapi;
using AriesGrpc = aries.galaxy.grpc;

namespace aries.service.galaxy.Views.request
{
    public class PersonInfoReq : TReq<AriesGrpc.PersonInfoReq>
    {
        public PersonInfoReq() { }
        public string? Id { get; set; }
        public string? Name { get; set; }
        public override AriesGrpc.PersonInfoReq Convert()
        {
            return new AriesGrpc.PersonInfoReq
            {
                 Person=new AriesGrpc.PersonInfo() 
                 {

                 }
            };
        }
    }
}
