using aries.common;
using aries.common.grpc;
using aries.galileo.grpc;
using Dapr.Client.Autogen.Grpc.v1;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System.Text.Json;

namespace aries.galileo.query
{
    public partial class QueryService
    {
        /// <summary>
        /// 搜索操作
        /// </summary>
        /// <param name="request">调用请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        private async Task<Any> SearchAsync(InvokeRequest request, ServerCallContext context)
        {
            AriesJsonObjResp output = new AriesJsonObjResp();
            if (request.Data.TryUnpack(out SearchReq searchReq))
            {
                var temp = await handler!.SearchAsync(searchReq);
                output.JsonObj=temp.Result.ToJsonString(CommonSource.JsonDefaultOptions);
                if (!string.IsNullOrEmpty(temp.Message)) 
                {
                    output.Error = new AriesErr()
                    {
                        ErrCode="",
                        ErrMsg=temp.Message
                    };
                }
            }
            return Any.Pack(output);
        }
        /// <summary>
        /// 按照Index 进行搜索操作
        /// </summary>
        /// <param name="request">调用请求</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        private async Task<Any> SearchByIndexAsync(InvokeRequest request, ServerCallContext context)
        {
            AriesJsonObjResp output = new AriesJsonObjResp();
            if (request.Data.TryUnpack(out SearchByIndexReq searchReq))
            {
                var temp = await handler!.SearchByIndexAsync(searchReq);
                output.JsonObj = temp.Result.ToJsonString(CommonSource.JsonDefaultOptions);
                if (!string.IsNullOrEmpty(temp.Message))
                {
                    output.Error = new AriesErr()
                    {
                        ErrCode = "",
                        ErrMsg = temp.Message
                    };
                }
            }
            
            return Any.Pack(output);
        }
      
        private async Task<Any> AutoCompleteAsync(InvokeRequest request, ServerCallContext context)
        {
            AriesJsonListResp output = new AriesJsonListResp();
            if (request.Data.TryUnpack(out SuggesterReq searchReq))
            {
                var temp = await handler!.AutoCompleteAsync(searchReq);
                output.JsonList = temp.Result.ToJsonString(CommonSource.JsonDefaultOptions);
                if (!string.IsNullOrEmpty(temp.Message))
                {
                    output.Error = new AriesErr()
                    {
                        ErrCode = "",
                        ErrMsg = temp.Message
                    };
                }
            }
            return Any.Pack(output);
        }
        /// <summary>
        /// 获取热榜列表操作
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task<Any> GetTopListAsync(InvokeRequest request, ServerCallContext context)
        {
            AriesJsonListResp output = new AriesJsonListResp();
            if (request.Data.TryUnpack(out TopReq topReq))
            {
                var temp =await handler!.GetTopListAsync(topReq);
                output.JsonList = JsonSerializer.Serialize(temp.Result, CommonSource.JsonDefaultOptions);
                if (!string.IsNullOrEmpty(temp.Message))
                {
                    output.Error = new AriesErr()
                    {
                        ErrCode = "",
                        ErrMsg = temp.Message
                    };
                }
            }
            return Any.Pack(output);
        }
    }
}
