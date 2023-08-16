﻿using aries.common;
using aries.galileo.grpc;
using System.Text.Json.Nodes;

namespace aries.galileo.query
{
    public interface IQueryHandler : aries.common.cqrs.IQueryHandler
    {
        /// <summary>
        /// 搜索操作
        /// </summary>
        /// <param name="request">request</param>
        /// <returns>查询数据集</returns>
        Task<AriesObject<JsonArray>> SearchAsync(SearchReq request);
        /// <summary>
        /// 根据Index进行搜索操作
        /// </summary>
        /// <param name="request">request</param>
        /// <returns>查询数据集</returns>
        Task<AriesObject<JsonArray>> SearchByIndexAsync(SearchByIndexReq requst);
        /// <summary>
        /// 提示操作
        /// </summary>
        /// <param name="request">request</param>
        /// <returns>提示数据集</returns>
        Task<AriesList<string>> AutoCompleteAsync(AutoCompleteReq request);
        /// <summary>
        /// 获取热榜列表
        /// </summary>
        /// <param name="topNum">topReq</param>
        /// <returns>热榜列表</returns>
        Task<AriesList<TopItemInfo>> GetTopListAsync(TopReq topReq);



    }
}
