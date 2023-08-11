using AriesGrpc = aries.galile.grpc;
using aries.common;
using aries.webapi;

namespace aries.service.galile.Views.request
{
    public class SearchByIndexReq: TReq<AriesGrpc.SearchByIndexReq>
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string? Keyword { get; set; }
        /// <summary>
        /// 索引
        /// </summary>
        public string? Index { get; set; }
        /// <summary>
        /// 分页
        /// </summary>
        public RSPage? Page { get; set; }
        public override AriesGrpc.SearchByIndexReq Convert()
        {
            AriesGrpc.SearchByIndexReq result = new AriesGrpc.SearchByIndexReq()
            {
                Keyword = Keyword,
                Index=Index,
                Page = Page is not null ? new common.grpc.AriesPage()
                {
                    RowNum = Page.RowNum,
                    Size = Page.Size,
                } : null
            };
            return result;
        }
    }
}
