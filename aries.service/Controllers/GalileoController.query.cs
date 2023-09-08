using aries.common.grpc;
using Microsoft.AspNetCore.Mvc;
using AriesGalileoGrpc = aries.galileo.grpc;
using AriesCollectorGrpc = aries.collector.grpc;
using AriesPorterGrpc = aries.porter.grpc;
using aries.service.galileo.Views.request;
using AriesPorter= aries.service.poarter.Views.request;
using aries.common;
using aries.common.net;
using Google.Protobuf.WellKnownTypes;
using aries.common.db;

namespace aries.service.Controllers
{
    public partial class GalileoController
    {
        [HttpPost("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Search(SearchReq searchReq)
        {
            ActionResult result;
            AriesGalileoGrpc.SearchReq req = searchReq.Convert();
            if (searchReq.Sort == galileo.SortTypeEnum.Score) { }
            else 
            {
                List<AriesGalileoGrpc.EsSortField> sortFields = new List<AriesGalileoGrpc.EsSortField>()
                {new AriesGalileoGrpc.EsSortField(){Sort= "PublishAt",SortType=(int)SortTypeEnum.Desc } };
                req.SortFields.AddRange(sortFields);
            }
            req.Boost = 1;
            //req.PhraseSlop = 1;
            List<AriesGalileoGrpc.EsQueryItemField> keywordFields = new()
            { 
                //资讯标题
                new AriesGalileoGrpc.EsQueryItemField { Boost=10,Item="Title"},
                //机构名称
                new AriesGalileoGrpc.EsQueryItemField{ Boost=10,Item="Name"},

            };
            req.KeywordFields.AddRange(keywordFields);
            List<AriesGalileoGrpc.EsQueryItemField> phraseFields = new()
            {
                //资讯摘要
                new AriesGalileoGrpc.EsQueryItemField{ Boost=2,Item ="Abstract"},
                //资讯正文
                new AriesGalileoGrpc.EsQueryItemField{ Boost=1,Item="Content"},
                //机构介绍
                new AriesGalileoGrpc.EsQueryItemField{ Boost=1,Item="Introduction"}
            };
            req.PhraseFields.AddRange(phraseFields);
            List<string> highlightFields = new()
            {
                "Title","Name","Abstract","Introduction"
            };
            req.HighlightFields.AddRange(highlightFields);
            result = SearchWithTotal<GalileoController, AriesJsonObjResp>( async () => {
                return await client.InvokeMethodGrpcAsync<AriesGalileoGrpc.SearchReq, AriesJsonObjResp>(daprappqueryId, "Galileo$Query$Search", req);
                });
            return result;
        }
        [HttpPost("searchByIndex")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult SearchByIndex(SearchByIndexReq searchReq)
        {
            ActionResult result;
            AriesGalileoGrpc.SearchByIndexReq req = searchReq.Convert();
            if (searchReq.Sort == galileo.SortTypeEnum.Score) { }
            else
            {
                List<AriesGalileoGrpc.EsSortField> sortFields = new List<AriesGalileoGrpc.EsSortField>() 
                {new AriesGalileoGrpc.EsSortField(){Sort= "PublishAt",SortType=(int)SortTypeEnum.Desc } };
                req.SortFields.AddRange(sortFields);
            }
            req.Boost = 1;
            //req.PhraseSlop = 1;
            List<AriesGalileoGrpc.EsQueryItemField> keywordFields = searchReq.Index switch
            {
                "organization" => new List<AriesGalileoGrpc.EsQueryItemField>()
                { 
                    //机构名称
                    new AriesGalileoGrpc.EsQueryItemField{ Boost=10,Item="Name"},
                },
                _ => new List<AriesGalileoGrpc.EsQueryItemField>()
                {
                     //资讯标题
                    new AriesGalileoGrpc.EsQueryItemField { Boost=10,Item="Title"},
                }
            };
            req.KeywordFields.AddRange(keywordFields);
            List<AriesGalileoGrpc.EsQueryItemField> phraseFields = searchReq.Index switch
            {
                "organization" => new List<AriesGalileoGrpc.EsQueryItemField>()
               {
                   //机构介绍
                   new AriesGalileoGrpc.EsQueryItemField { Boost = 1, Item = "Introduction" }
               },
                _ => new List<AriesGalileoGrpc.EsQueryItemField>()
               {
                     //资讯摘要
                    new AriesGalileoGrpc.EsQueryItemField{ Boost=2,Item ="Abstract"},
                    //资讯正文
                    new AriesGalileoGrpc.EsQueryItemField{ Boost=1,Item="Content"},
               }

            };
            req.PhraseFields.AddRange(phraseFields);
            List<string> highlightFields = searchReq.Index switch
            {
                "organization" => new List<string>()
               {     
                   //机构名称
                    "Name",
                   //机构介绍
                   "Introduction",

               },
                _ => new List<string>()
               {
                    //资讯标题
                    "Title",
                     //资讯摘要
                    "Abstract",

               }
            };
            req.HighlightFields.AddRange(highlightFields);
            result = SearchWithTotal<GalileoController, AriesJsonObjResp>(async () =>
            {
                return await client.InvokeMethodGrpcAsync<AriesGalileoGrpc.SearchByIndexReq, AriesJsonObjResp>(daprappqueryId, "Galileo$Query$SearchByIndex", req);
            });
            return result;
        }
        [HttpPost("autocomplete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult AutoComplete(SuggesterReq suggesterReq) 
        {
            ActionResult result;
            AriesGalileoGrpc.SuggesterReq req = suggesterReq.Convert();
            req.Field = "suggester";
            req.Name = "galileo-suggester";
            req.Size = 10;
            req.FuzzyEditDistance = 2;
            result = Search<GalileoController, AriesJsonListResp>(async () => {
                return await client.InvokeMethodGrpcAsync<AriesGalileoGrpc.SuggesterReq, AriesJsonListResp>(daprappqueryId, "Galileo$Query$AutoComplete", req);
            });
            return result;
        }
        [HttpPost("autocompleteByIndex")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult AutoCompleteByIndex(SuggesterByIndexReq suggesterReq)
        {
            ActionResult result;
            AriesGalileoGrpc.SuggesterReq req = suggesterReq.Convert();
            req.Field = "suggester";
            req.Name = "galileo-suggester";
            req.Size = 10;
            req.FuzzyEditDistance = 2;
            result = Search<GalileoController, AriesJsonListResp>(async () => {
                return await client.InvokeMethodGrpcAsync<AriesGalileoGrpc.SuggesterReq, AriesJsonListResp>(daprappqueryId, "Galileo$Query$AutoComplete", req);
            });
            return result;
        }

        [HttpPut("browse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public  ActionResult Browse(string url)
        {
            ActionResult result;
            AriesCollectorGrpc.CollectInfo req = new AriesCollectorGrpc.CollectInfo()
            {
                Url = url,
                Business = (Int32)BusinessEnum.Search,
                Ip = IpService.GetRealClientIpAddress(this.ControllerContext.HttpContext),
                Title = "",
                UserName = ""
            };
            result=DoAction<GalileoController>(async () =>
            {
                await client.InvokeMethodGrpcAsync<AriesCollectorGrpc.CollectInfo, Empty>(daprappqueryId, "Collector$Manage$UserBehaviorCollect", req);
            });
            return result;
        }
        [HttpGet("{topNum}/getTopList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult GetTopList(int topNum)
        {
            ActionResult result;
            AriesGalileoGrpc.TopReq req = new AriesGalileoGrpc.TopReq()
            {
                Top = topNum
            };
            result = Search<GalileoController, AriesJsonListResp>(async () =>
             {
                 return await client.InvokeMethodGrpcAsync<AriesGalileoGrpc.TopReq, AriesJsonListResp>(daprappqueryId, "Galileo$Query$GetTopList", req);
             });
            return result;
        }
        [HttpPost("similarRecommendList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult GetSimilarRecommendList(AriesPorter.SearchReq searchReq) 
        {
            ActionResult result;
            AriesPorterGrpc.SearchReq req = searchReq.Convert();
            result = Search<GalileoController, AriesJsonListResp>(async () =>
            {
                return await client.InvokeMethodGrpcAsync<AriesPorterGrpc.SearchReq, AriesJsonListResp>(daprappqueryId, "Porter$Query$GetSimilarRecommendList", req);
            });
            return result;
        }
    }
}