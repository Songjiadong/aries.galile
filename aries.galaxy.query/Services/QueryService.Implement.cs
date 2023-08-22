using aries.common;
using aries.common.db.neo4j;
using aries.common.grpc;
using aries.galaxy.grpc;
using Dapr.Client.Autogen.Grpc.v1;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System.Data;
using System.Text.Json;

namespace aries.galaxy.query
{
    public partial class QueryService
    {
        private async Task<Any> AutoCompleteAsync(InvokeRequest request, ServerCallContext context) 
        {
            AriesJsonListResp output = new AriesJsonListResp();
            if (request.Data.TryUnpack<GraphSearchReq>(out GraphSearchReq searchReq))
            {
                var searchInfo = new SearchInfo()
                {
                    Keyword = searchReq.Keyword
                };
                AriesDataTable dt =await handler!.AutoCompleteAsync(searchInfo);
                var temp = from dr in dt.Result.AsEnumerable()
                           select new
                           {
                               //Category = GraphManage.LabelConvert(dr["label"].ToString()!),
                               Id = dr["item." + DBSource.Attribute.GetCypherColumnNameByPropertyName<GGraphEntityInfo, string?>(o => o.Id)].ToString(),
                               Name = dr["item." + DBSource.Attribute.GetCypherColumnNameByPropertyName<GGraphEntityInfo, string?>(o => o.Name)].ToString()
                           };
                output.JsonList = JsonSerializer.Serialize(temp);
            }
            else
            {
                throw new Exception();
            }
            return Any.Pack(output);
        }
        /// <summary>
        /// 图谱搜索
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<Any> GraphAsync(InvokeRequest request, ServerCallContext context)
        {
            AriesJsonObjResp output = new AriesJsonObjResp();
            if (request.Data.TryUnpack<GraphDegreeReq>(out GraphDegreeReq graphReq))
            {
                var temp = await handler!.GraphAsync(graphReq);
                if (temp.Result is not null)
                {
                    temp.Result.Labels = QueryHandler.GetAllLabelList();
                    foreach (var item in temp.Result.Nodes!.AsEnumerable())
                    {
                        string label = item.Properties!.ContainsKey("orgtype") ? item.Properties!["orgtype"].ToString()! : item.Labels![0].ToString();
                        item.Labels![0] = QueryHandler.LabelConvert(label);

                    }
                    output.JsonObj = JsonSerializer.Serialize(temp.Result, CommonSource.JsonDefaultOptions);
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
        private async Task<Any> ShortestPathAsync(InvokeRequest request, ServerCallContext context)
        {
            AriesJsonObjResp output = new AriesJsonObjResp();
            if (request.Data.TryUnpack<ShortestPathReq>(out ShortestPathReq graphReq))
            {
                var temp =await handler.ShortestPathAsync(new RelationSearchInfo<NullGraphEntityInfo, NullGraphEntityInfo>()
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
                        if (item.Properties!.ContainsKey("orgtype"))
                        {
                            var orgType = item.Properties["orgtype"];
                            item.Labels[0] = QueryHandler.LabelConvert(orgType.ToString());
                        }

                    }
                    output.JsonObj = JsonSerializer.Serialize(temp.Result, CommonSource.JsonDefaultOptions);
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
