using aries.common;
using aries.common.db.neo4j;
using aries.galaxy.grpc;

namespace aries.galaxy.query
{
    public interface IQueryHandler: aries.common.cqrs.IQueryHandler
    {
        /// <summary>
        /// 搜索操作
        /// </summary>
        /// <param name="request">request</param>
        /// <returns>查询数据集</returns>
        AriesObject<GraphInfo> Search(GraphDegreeSearchReq request);
        /// <summary>
        /// 搜索提示操作
        /// </summary>
        /// <param name="request">request</param>
        /// <returns>提示数据集</returns>
        List<OrganizationDocInfo> AutoComplete(GraphSearchReq request);
        /// <summary>
        /// 查询最近距离操作
        /// </summary>
        /// <param name="request">request</param>
        /// <returns>查询数据集</returns>
        AriesObject<GraphInfo> ShortestPath<From, To>(RelationSearchInfo<From, To> searchInfo) 
            where From : GGraphEntityInfo where To : GGraphEntityInfo;
        
    }
}
