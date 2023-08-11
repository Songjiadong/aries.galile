using aries.galile.grpc;
using System.Data;
using aries.common.db.elasticsearch;
using AriesEs = aries.common.db.elasticsearch;
using AriesPhoenix = aries.common.db.phoenix;
using aries.common.db;
using aries.common;
using System.Text.Json.Nodes;
using aries.common.db.phoenix;

namespace aries.galile.query
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
        public async Task<AriesList<SearchItemInfo>> SearchByIndexAsync(SearchByIndexReq request) 
        {
            AriesList<SearchItemInfo> result = new AriesList<SearchItemInfo> { };
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
            EsSearchRequest req = new EsSearchRequest()
            {
                IndexList = new List<string> { request.Index },
                Page = new RSPage() { RowNum = request.Page.RowNum, Size = request.Page.Size },
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
                EsSearchResponse<JsonObject> resp = await this.esClient.SearchOp.SearchAsync<JsonObject>(req);
                //转化
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }
        public async Task<AriesList<SearchItemInfo>> SearchAsync(SearchReq request)
        {
            AriesList<SearchItemInfo> result = new AriesList<SearchItemInfo> { };
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
            EsSearchRequest req = new EsSearchRequest()
            {
                Page = new RSPage() { RowNum = request.Page.RowNum, Size = request.Page.Size },
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
              EsSearchResponse<JsonObject> resp=  await this.esClient.SearchOp.SearchAsync<JsonObject>(req);
              //转化
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }
        public async Task<AriesList<string>> AutoCompleteAsync(AutoCompleteReq request) 
        {
            AriesList<string> result = new AriesList<string> { };
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
                     Suggester=new EsSuggesterInfo()
                     {
                         Items = new List<EsSuggesterItem>() { new EsSuggesterItem() { Prefix = "title" } }
                     }

                };
                EsSearchResponse<JsonObject> resp = await this.esClient.SearchOp.SearchAsync<JsonObject>(req);
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
