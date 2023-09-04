using aries.galileo.grpc;
using System.Data;
using aries.common.db.elasticsearch;
using AriesEs = aries.common.db.elasticsearch;
using AriesPhoenix = aries.common.db.phoenix;
using aries.common.db;
using aries.common;
using System.Text.Json.Nodes;
using aries.common.db.phoenix;
using System.Text.Json;
using aries.common.db.neo4j;

namespace aries.galileo.query
{
    public class QueryHandler : IQueryHandler
    {

        private readonly AriesEs.DBService esClient;
        private readonly AriesPhoenix.DBService phoenixClient;
        public QueryHandler(AriesEs.DBService esClient)
        {
            this.esClient = esClient;
            // this.phoenixClient = phoenixClient;
        }
        public async Task<AriesObject<JsonObject>> SearchByIndexAsync(SearchByIndexReq request)
        {
            AriesObject<JsonObject> result = new AriesObject<JsonObject>();
            List<string> keywordFieldList = new List<string>();
            foreach (var item in request.KeywordFields)
            {
                string tmp = item.Item;
                if (item.Boost != 1)
                {
                    tmp += $"^{item.Boost}";
                }
                keywordFieldList.Add(tmp);
            }
            List<string> phraseFieldList = new List<string>();
            foreach (var item in request.PhraseFields)
            {
                string tmp = item.Item;
                if (item.Boost != 1)
                {
                    tmp += $"^{item.Boost}";
                }
                phraseFieldList.Add(tmp);
            }
            Dictionary<string, EsHightLightFieldInfo> highLights = new Dictionary<string, EsHightLightFieldInfo>();
            foreach (var item in request.HighlightFields)
            {
                highLights.Add(item, new EsHightLightFieldInfo()
                {
                    Analyzer = "ik_sync_smart",
                    PostTags = "</font>",
                    PreTags = "<font color='red'>"
                });
            }
            EsSearchRequest req = new EsSearchRequest()
            {
                IndexList = new List<string> { request.Index },
                Page = new RSPage() { RowNum = request.Page.RowNum, Size = request.Page.Size },
                HighLight = new EsHighLightInfo()
                {
                    Fields = highLights
                },
                Keyword = new EsKeywordQueryInfo()
                {
                    Keyword = request.Keyword,
                    KeywordFieldList = keywordFieldList,
                    PhraseFieldList = phraseFieldList,
                    Boost = request.Boost,
                    PhraseSlop = request.PhraseSlop,
                }
            };
            try
            {
                result.Result = await this.esClient.SearchOp.SearchAsync<dynamic>(req);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }
        public async Task<AriesObject<JsonObject>> SearchAsync(SearchReq request)
        {
            AriesObject<JsonObject> result = new();
            List<string> keywordFieldList = new();
            foreach (var item in request.KeywordFields)
            {
                string tmp = item.Item;
                if (item.Boost != 1)
                {
                    tmp += $"^{item.Boost}";
                }
                keywordFieldList.Add(tmp);
            }
            List<string> phraseFieldList = new List<string>();
            foreach (var item in request.PhraseFields)
            {
                string tmp = item.Item;
                if (item.Boost != 1)
                {
                    tmp += $"^{item.Boost}";
                }
                phraseFieldList.Add(tmp);
            }
            Dictionary<string, EsHightLightFieldInfo> highLights = new Dictionary<string, EsHightLightFieldInfo>();
            foreach (var item in request.HighlightFields)
            {
                highLights.Add(item, new EsHightLightFieldInfo()
                {
                    Analyzer = "ik_sync_smart",
                    PostTags = "</font>",
                    PreTags = "<font color='red'>"
                });
            }
            EsSearchRequest req = new EsSearchRequest()
            {
                Page = new RSPage() { RowNum = request.Page.RowNum, Size = request.Page.Size },
                HighLight = new EsHighLightInfo()
                {
                    Fields = highLights
                },
                Keyword = new EsKeywordQueryInfo()
                {
                    Keyword = request.Keyword,
                    Analyzer = "ik_sync_smart",
                    KeywordFieldList = keywordFieldList,
                    PhraseFieldList = phraseFieldList,
                    Boost = request.Boost,
                    PhraseSlop = request.PhraseSlop,
                },
                   
            };
            if (request.SortFields is not null && request.SortFields.Count > 0) 
            {
                SortList sortList = new SortList();
                foreach (var item in request.SortFields)
                {
                    sortList.Sorts.Add(new SortInfo()
                    {
                        SortName = item.Sort,
                        SortType = ((SortTypeEnum)item.SortType)
                    });
                }
                req.Sort = sortList;
            }
            try
            {

                result.Result = await this.esClient.SearchOp.SearchAsync<dynamic>(req);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }
        public async Task<AriesObject<JsonArray>> AutoCompleteAsync(SuggesterReq request)
        {
            AriesObject<JsonArray> result = new();
            EsSuggesterRequest req = new()
            {
                IndexList = (request.Index is null || request.Index.Count == 0) ? new List<string>() : request.Index.ToList(),
                Suggester = new EsSuggesterInfo()
                {
                    Items = new List<EsSuggesterItem>()
                    {
                        new EsSuggesterItem()
                        {
                            Name = request.Name,
                            Text=request.Keyword,
                            Completion=new EsCompletionSuggester()
                            {
                                Field = request.Field,
                                Analyzer = "ik_sync_max_word",
                                Prefix = request.Prefix,
                                SkipDuplicates = true,
                                Size = request.Size,
                                Fuzzy=request.FuzzyEditDistance is  null?null:new EsSuggestFuzziness()
                                {
                                    Fuzziness=EsFuzziness.EditDistance(request.FuzzyEditDistance!.Value),
                                }
                            }
                        }
                    }
                }
            };
            try
            {
                result.Result = await this.esClient.SearchOp.SuggesterAsync<dynamic>(req);

            }
            catch (Exception ex)
            {

                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<AriesList<TopItemInfo>> GetTopListAsync(TopReq topReq)
        {
            AriesList<TopItemInfo> result = new AriesList<TopItemInfo>();
            List<TopItemInfo> db = new List<TopItemInfo>() 
            {
                  new TopItemInfo() {Title="机械工业信息中心",Url="http://www.miic.com.cn",Count=1000},
                 new TopItemInfo() {Title="中国机械工业联合会",Url="http://www.cmif.org.cn",Count=999},
                 new TopItemInfo() {Title="CMIF",Url="http://www.cmif.org.cn",Count=890},
                 new TopItemInfo() {Title="徐念沙会长一行调研大连光洋科技集团",Url="http://www.mei.net.cn/jcgj/202308/474497406495397781.html?l=ldhd",Count=600},
                 new TopItemInfo() {Title="叶定达总经济师会见新疆工信厅副厅长柳奇一行",Url="http://www.mei.net.cn/jxgy/202308/475627214939920277.html?l=ldhd",Count=600},
                 new TopItemInfo(){ Title="通用技术集团党组召开理论学习中心组2023年第十二次集体学习暨主题教育专题学习",Url="https://www.gt.cn/xwzx/jtxw/202308/t20230828_40913.html",Count=100},
                 new TopItemInfo(){ Title="2023两岸机械工业交流会在新乡成功召开",Url="http://www.mei.net.cn/jxgy/202308/475899333900801941.html?l=gzdt",Count=400},
                 new TopItemInfo(){Title="中央企业团工委、中央企业青联关于动员中央企业各级团青组织和广大团员青年积极投身防汛救灾工作的倡议书 ",Url="http://www.sasac.gov.cn/n2588030/n2588919/c28540065/content.html",Count=399 },
                 new TopItemInfo() {Title="机经网",Url="http://www.mei.net.cn",Count=300},
                 new TopItemInfo() {Title="叶定达总经济师会见新疆生产建设兵团工信局局长郇恒赛一行",Url="http://www.mei.net.cn/jxgy/202308/475627508943853461.html",Count=100}
            };
            result.Result = db.Take(topReq.Top).ToList();
            
            //string sql = $"select url,title,top_count from galile_top_info limit {topReq.Top}";
            //var dt = await ((IDBService)this.phoenixClient).QueryAsync(sql);
            //if (dt.Message != string.Empty)
            //{
            //    result.Message = dt.Message;
            //}
            //foreach (var dr in dt.Result.AsEnumerable())
            //{
            //    result.Result!.Add(new TopItemInfo()
            //    {
            //        Title = dr["title"].ToString(),
            //        Url = dr["url"].ToString(),
            //        Count = Convert.ToInt64(dr["top_count"])
            //    });
            //}
            return await Task.FromResult(result);
        }
    }
}
