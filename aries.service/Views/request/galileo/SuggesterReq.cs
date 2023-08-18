using AriesGrpc = aries.galileo.grpc;
using aries.common;
using aries.webapi;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace aries.service.galileo.Views.request
{
    public class SuggesterReq : TReq<AriesGrpc.SuggesterReq>
    { /// <summary>
      /// 关键字
      /// </summary>
        public string? Keyword { get; set; }
        public int? Size { get; set; } = 10;
        public override AriesGrpc.SuggesterReq Convert()
        {
            AriesGrpc.SuggesterReq result = new AriesGrpc.SuggesterReq()
            {
                Keyword = this.Keyword,
                Size=this.Size
              
            };
            return result;
        }
    }
}
