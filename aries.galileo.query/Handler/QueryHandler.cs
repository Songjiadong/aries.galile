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
using Nest;
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
        public async Task<AriesObject<JsonArray>> SearchByIndexAsync(SearchByIndexReq request)
        {
            AriesObject<JsonArray> result = new AriesObject<JsonArray>();
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
        public async Task<AriesObject<JsonArray>> SearchAsync(SearchReq request)
        {
            AriesObject<JsonArray> result = new();
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
        public async Task<AriesObject<JsonArray>> AutoCompleteAsync(SuggesterReq request)
        {
            AriesObject<JsonArray> result = new();
            EsSuggesterRequest req = new ()
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
            string sql = $"select url,title,top_count from galile_top_info limit {topReq.Top}";
            var dt = await ((IDBService)this.phoenixClient).QueryAsync(sql);
            if (dt.Message != string.Empty)
            {
                result.Message = dt.Message;
            }
            foreach (var dr in dt.Result.AsEnumerable())
            {
                result.Result!.Add(new TopItemInfo()
                {
                    Title = dr["title"].ToString(),
                    Url = dr["url"].ToString(),
                    Count = Convert.ToInt64(dr["top_count"])
                });
            }
            return result;
        }
    }
}
