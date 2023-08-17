using aries.common.grpc;
using aries.common.logger;
using Dapr;
using Microsoft.AspNetCore.Mvc;
using AriesGalileoGrpc = aries.galileo.grpc;
using AriesCollectorGrpc = aries.collector.grpc;
using aries.service.galileo.Views.request;
using aries.common;
using aries.common.net;
using Google.Protobuf.WellKnownTypes;
using System.Text.Json;

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
            result =  Search<GalileoController, AriesJsonListResp>( async () => {
                return await client.InvokeMethodGrpcAsync<AriesGalileoGrpc.SearchReq, AriesJsonListResp>(daprappqueryId, "Galileo$Query$Search", req);
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
            result = Search<GalileoController, AriesJsonListResp>(async () =>
            {
                return await client.InvokeMethodGrpcAsync<AriesGalileoGrpc.SearchByIndexReq, AriesJsonListResp>(daprappqueryId, "Galileo$Query$SearchByIndex", req);
            });
            return result;
        }
        [HttpPut("browse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task Browse(string url)
        {
            AriesCollectorGrpc.CollectInfo req = new AriesCollectorGrpc.CollectInfo()
            {
                Url = url,
                Business = (Int32)BusinessEnum.Search,
                Ip = IpService.GetRealClientIpAddress(this.ControllerContext.HttpContext),
                Title = "",
                UserName = ""
            };
            try
            {
                await client.InvokeMethodGrpcAsync<AriesCollectorGrpc.CollectInfo, Empty>(daprappqueryId, "Collector$Manage$UserBehaviorCollect", req);
            }
            catch (DaprApiException ex)
            {
                LoggerService.Logger<GalileoController>(ex, LogLevel.Error);
            }
            catch (DaprException ex)
            {

                LoggerService.Logger<GalileoController>(ex, LogLevel.Error);
            }
            catch (Exception ex)
            {
                LoggerService.Logger<GalileoController>(ex, LogLevel.Error);
            }
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
       
    }
}