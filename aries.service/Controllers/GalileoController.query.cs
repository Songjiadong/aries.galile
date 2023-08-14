using aries.common.grpc;
using aries.common.logger;
using Dapr;
using Microsoft.AspNetCore.Mvc;
using AriesGalileGrpc = aries.galileo.grpc;
using AriesCollectorGrpc = aries.collector.grpc;
using aries.service.galileo.Views.request;
using aries.common;
using aries.common.net;
using Google.Protobuf.WellKnownTypes;
using aries.common.db;
using System.Collections.Generic;

namespace aries.service.Controllers
{
    public partial class GalileoController
    {
        [HttpPost("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Search(SearchReq searchReq)
        {
            ActionResult result;
            AriesGalileGrpc.SearchReq req = searchReq.Convert();
            req.Boost = 1;
            req.PhraseSlop = 1;
            List<AriesGalileGrpc.EsQueryItemField> keywordFields = new List<AriesGalileGrpc.EsQueryItemField>()
            { 
                //资讯标题
                new AriesGalileGrpc.EsQueryItemField { Boost=1,Item="Title"},
                //机构名称
                new AriesGalileGrpc.EsQueryItemField{ Boost=1,Item="Name"},

            };
            req.KeywordFields.AddRange(keywordFields);
            List<AriesGalileGrpc.EsQueryItemField> phraseFields = new List<AriesGalileGrpc.EsQueryItemField>()
            {
                //资讯摘要
                new AriesGalileGrpc.EsQueryItemField{ Boost=1,Item ="Abstract"},
                //资讯正文
                new AriesGalileGrpc.EsQueryItemField{ Boost=1,Item="Content"},
                //机构介绍
                new AriesGalileGrpc.EsQueryItemField{ Boost=1,Item="Introduction"}
            };
            req.PhraseFields.AddRange(phraseFields);
            try
            {
                var temp = await client.InvokeMethodGrpcAsync<AriesGalileGrpc.SearchReq, AriesJsonObjResp>(daprappqueryId, "Galile$Query$Search", req);
                result = Ok(temp);
            }
            catch (DaprApiException ex)
            {
                result = BadRequest(ex);
                LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
            }
            catch (DaprException ex)
            {
                result = BadRequest(ex);
                LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
            }
            catch (Exception ex)
            {
                result = BadRequest(ex);
                LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
            }
            return result;
        }
        [HttpPost("searchByIndex")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SearchByIndex(SearchByIndexReq searchReq)
        {
            ActionResult result;
            AriesGalileGrpc.SearchByIndexReq req = searchReq.Convert();
            req.Boost = 1;
            req.PhraseSlop = 1;
            List<AriesGalileGrpc.EsQueryItemField> keywordFields = searchReq.Index switch
            {
                "organization" => new List<AriesGalileGrpc.EsQueryItemField>()
                { 
                    //机构名称
                    new AriesGalileGrpc.EsQueryItemField{ Boost=1,Item="Name"},
                },
                _ => new List<AriesGalileGrpc.EsQueryItemField>()
                {
                     //资讯标题
                    new AriesGalileGrpc.EsQueryItemField { Boost=1,Item="Title"},
                }
            };
            req.KeywordFields.AddRange(keywordFields);
            List<AriesGalileGrpc.EsQueryItemField> phraseFields = searchReq.Index switch
            {
                "organization" => new List<AriesGalileGrpc.EsQueryItemField>()
               {
                   //机构介绍
                   new AriesGalileGrpc.EsQueryItemField { Boost = 1, Item = "Introduction" }
               },
                _ => new List<AriesGalileGrpc.EsQueryItemField>()
               {
                     //资讯摘要
                    new AriesGalileGrpc.EsQueryItemField{ Boost=1,Item ="Abstract"},
                    //资讯正文
                    new AriesGalileGrpc.EsQueryItemField{ Boost=1,Item="Content"},
               }

            };
            req.PhraseFields.AddRange(phraseFields);
            try
            {
                var temp = await client.InvokeMethodGrpcAsync<AriesGalileGrpc.SearchByIndexReq, AriesJsonObjResp>(daprappqueryId, "Galile$Query$SearchByIndex", req);
                result = Ok(temp);
            }
            catch (DaprApiException ex)
            {
                result = BadRequest(ex);
                LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
            }
            catch (DaprException ex)
            {
                result = BadRequest(ex);
                LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
            }
            catch (Exception ex)
            {
                result = BadRequest(ex);
                LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
            }
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
                LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
            }
            catch (DaprException ex)
            {

                LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
            }
            catch (Exception ex)
            {

                LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
            }
        }
        [HttpGet("{topNum}/getTopList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetTopList(int topNum)
        {
            ActionResult result;
            AriesGalileGrpc.TopReq req = new AriesGalileGrpc.TopReq()
            {
                Top = topNum
            };
            try
            {
                var temp = await client.InvokeMethodGrpcAsync<AriesGalileGrpc.TopReq, AriesJsonObjResp>(daprappqueryId, "Galile$Query$GetTopList", req);
                result = Ok(temp);
            }
            catch (DaprApiException ex)
            {
                result = BadRequest(ex);
                LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
            }
            catch (DaprException ex)
            {
                result = BadRequest(ex);
                LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
            }
            catch (Exception ex)
            {
                result = BadRequest(ex);
                LoggerService.Logger<GalaxyController>(ex, LogLevel.Error);
            }
            return result;
        }
    }
}