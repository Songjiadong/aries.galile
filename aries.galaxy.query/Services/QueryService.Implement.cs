using aries.common;
using aries.common.db.neo4j;
using aries.common.grpc;
using aries.galaxy.grpc;
using Dapr.Client.Autogen.Grpc.v1;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System.Text.Json;

namespace aries.galaxy.query
{
    public partial class QueryService
    {
        /// <summary>
        /// 图谱搜索
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private Any Search(InvokeRequest request, ServerCallContext context)
        {
            AriesJsonObjResp output = new AriesJsonObjResp();
            if (request.Data.TryUnpack<GraphDegreeSearchReq>(out GraphDegreeSearchReq searchReq))
            {
                var temp = handler!.Search(searchReq);
                if (temp.Result is not null)
                {
                    temp.Result.Labels = QueryHandler.GetAllLabelList();
                    foreach (var item in temp.Result.Nodes!.AsEnumerable())
                    {
                        for (var i = 0; i < item.Labels!.Count; i++)
                        {
                            item.Labels[i] = QueryHandler.LabelConvert(item.Labels[i]);
                        }
                    }
                    output.JsonObj = JsonSerializer.Serialize(temp, CommonSource.JsonDefaultOptions);
                }
            }
            return Any.Pack(output);
        }
        /// <summary>
        /// 图谱找关系
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private Any ShortestPath(InvokeRequest request, ServerCallContext context)
        {
            AriesJsonObjResp output = new AriesJsonObjResp();
            if (request.Data.TryUnpack<ShortestPathReq>(out ShortestPathReq graphReq))
            {
                var temp = handler.ShortestPath(new RelationSearchInfo<NullGraphEntityInfo, NullGraphEntityInfo>()
                {
                    StartNode = new NullGraphEntityInfo(),
                    EndNode = new NullGraphEntityInfo(),
                    FromKeyword = graphReq.Start,
                    ToKeyword = graphReq.End,
                });
                if (temp.Result is not null)
                {
                    temp.Result.Labels = QueryHandler.GetAllLabelList();
                    foreach (var item in temp.Result.Nodes!.AsEnumerable())
                    {
                        for (var i = 0; i < item.Labels!.Count; i++)
                        {
                            item.Labels[i] = QueryHandler.LabelConvert(item.Labels[i]);
                        }
                    }
                    output.JsonObj = JsonSerializer.Serialize(temp, CommonSource.JsonDefaultOptions);
                }
            }
            else
            {
                throw new Exception();
            }
            return Any.Pack(output);
        }
       
    }
}
