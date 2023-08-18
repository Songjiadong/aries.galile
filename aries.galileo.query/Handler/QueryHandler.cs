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
            AriesObject<JsonArray> result= new();
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
            Dictionary<string, EsHightLightFieldInfo> highLights= new Dictionary<string, EsHightLightFieldInfo>();
            foreach (var item in request.HighlightFields) 
            {
                highLights.Add(item, new EsHightLightFieldInfo()
                {
                    Analyzer = "ik_sync_smart",
                    PostTags = "</font>",
                    PreTags= "<font color='red'>"
                }) ;
            }
            EsSearchRequest req = new EsSearchRequest()
            {
                Page = new RSPage() { RowNum = request.Page.RowNum, Size = request.Page.Size },
                HighLight=new EsHighLightInfo() 
                {
                     Fields=highLights
                },
                Keyword = new EsKeywordQueryInfo()
                {
                    Keyword = request.Keyword,
                    Analyzer= "ik_sync_smart",
                    KeywordFieldList = keywordFieldList,
                    PhraseFieldList = phraseFieldList,
                    Boost = request.Boost,
                    PhraseSlop = request.PhraseSlop,
                }   
            };
            try
            {

              result.Result =  await this.esClient.SearchOp.SearchAsync<dynamic>(req);
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
            
            try
            {
                EsSearchRequest req = new EsSearchRequest()
                {
                     Suggester=new EsSuggesterInfo()
                     {
                         Items = new List<EsSuggesterItem>() { 
                             new EsSuggesterItem() {
                                 Name="galileo-suggester",
                                 Prefix = "title" ,
                                 Term=new EsTermSuggester()
                                 {
                                      Text=request.Keyword,
                                      Field="",
                                      
                                      Analyzer="ik_sync_max_word",
                                 },
                                 Phrase=new EsPhraseSuggester()
                                 {
                                     Text=request.Keyword,
                                     Field="",
                                     Analyzer="ik_sync_max_word",
                                     MaxErrors=5,
                                 }
                             } 
                         }
                     }

                };
                result.Result = await this.esClient.SearchOp.SearchAsync<dynamic>(req);
               

            }
            catch (Exception ex)
            {

                result.Message = ex.Message;
            }
            return result;
        }
        public async Task<AriesObject<JsonArray>> AutoCompleteByIndexAsync(SearchByIndexReq request)
        {
            AriesObject<JsonArray> result = new();
            try
            {
                EsSearchRequest req = new EsSearchRequest()
                {
                    Keyword = new EsKeywordQueryInfo()
                    {
                        Keyword = request.Keyword,
                        KeywordFieldList = new List<string>() { "title", "source", "author" },
                        PhraseFieldList = new List<string>() { "abstract", "content" }
                    },
                    Suggester = new EsSuggesterInfo()
                    {
                        Items = new List<EsSuggesterItem>() { new EsSuggesterItem() { Prefix = "title" } }
                    }

                };
                JsonArray resp = await this.esClient.SearchOp.SearchAsync<JsonObject>(req);
                //转化
            }
            catch (Exception ex)
            {

                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<AriesList<TopItemInfo>> GetTopListAsync(TopReq topReq)
        {
            AriesList < TopItemInfo > result=new AriesList<TopItemInfo>();
            string sql = $"select url,title,top_count from galile_top_info limit {topReq.Top}";
            var dt=  await ((IDBService)this.phoenixClient).QueryAsync(sql);
            if (dt.Message != string.Empty) 
            {
                result.Message=dt.Message;
            }
            foreach (var dr in dt.Result.AsEnumerable()) 
            {
                result.Result!.Add(new TopItemInfo()
                {
                    Title = dr["title"].ToString(),
                    Url = dr["url"].ToString(),
                    Count =Convert.ToInt64(dr["top_count"])
                });
            }
            return result;
        }
    }
}
