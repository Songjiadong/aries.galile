using aries.common;
using aries.common.db.neo4j;
using aries.galaxy.grpc;
using System.Data;

namespace aries.galaxy.query
{
    public interface IQueryHandler: aries.common.cqrs.IQueryHandler
    {
        /// <summary>
        /// 搜索操作
        /// </summary>
        /// <param name="request">request</param>
        /// <returns>查询数据集</returns>
        Task<AriesObject<GraphInfo>> GraphAsync(GraphDegreeReq request);
        /// <summary>
        /// 查询最近距离操作
        /// </summary>
        /// <param name="request">request</param>
        /// <returns>查询数据集</returns>
        Task<AriesObject<GraphInfo>> ShortestPathAsync<From, To>(RelationSearchInfo<From, To> searchInfo) 
            where From : GGraphEntityInfo where To : GGraphEntityInfo;
        /// <summary>
        /// 下拉菜单提示操作
        /// </summary>
        /// <param name="searchInfo">关键字</param>
        /// <returns>提示结果集</returns>
        Task<AriesDataTable> AutoCompleteAsync(SearchInfo searchInfo);
    }
}
