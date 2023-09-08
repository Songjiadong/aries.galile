using aries.common;
using AriesGrpc = aries.porter.grpc;
using aries.webapi;

namespace aries.service.poarter.Views.request
{
    public class SearchReq : TReq<AriesGrpc.SearchReq>
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string? Keyword { get; set; }
        /// <summary>
        /// 分页
        /// </summary>
        public int Top { get; set; }
        public override AriesGrpc.SearchReq Convert()
        {
            AriesGrpc.SearchReq result = new AriesGrpc.SearchReq()
            {
                Keyword = this.Keyword,
                Top=this.Top
            };
            return result;
        }
    }
}
