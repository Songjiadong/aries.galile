using aries.webapi;
using AriesGrpc = aries.galaxy.grpc;

namespace aries.service.galaxy.Views.request
{
    public class GraphDegreeReq : TReq<AriesGrpc.GraphDegreeReq>
    {
        public GraphDegreeReq() { }
       public string? Id {get;set;}
        public int Degree { get; set; } = 1;

        public override AriesGrpc.GraphDegreeReq Convert()
        {
            return new AriesGrpc.GraphDegreeReq()
            {
                Id = Id,
                Degree = Degree
            };
        }
    }
}
