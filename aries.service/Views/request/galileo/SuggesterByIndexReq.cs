using AriesGrpc = aries.galileo.grpc;
using aries.common;
using aries.webapi;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace aries.service.galileo.Views.request
{
    public class SuggesterByIndexReq : TReq<AriesGrpc.SuggesterReq>
    { /// <summary>
      /// 关键字
      /// </summary>
        public string? Keyword { get; set; }
        /// <summary>
        /// 最多返回N条记录
        /// </summary>
        public int? Size { get; set; } = 10;
        /// <summary>
        /// 索引
        /// </summary>
        public string? Index { get; set; }
        public override AriesGrpc.SuggesterReq Convert()
        {
            AriesGrpc.SuggesterReq result = new AriesGrpc.SuggesterReq()
            {
                Keyword = this.Keyword,
                Size = this.Size,
            };
            result.Index.Add(this.Index!);
            return result;
        }
    }
}
