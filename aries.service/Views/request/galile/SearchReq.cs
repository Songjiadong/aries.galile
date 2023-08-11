﻿using aries.common;
using AriesGrpc = aries.galile.grpc;
using aries.webapi;

namespace aries.service.galile.Views.request
{
    public class SearchReq : TReq<AriesGrpc.SearchReq>
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string? Keyword { get; set; }
        /// <summary>
        /// 分页
        /// </summary>
        public RSPage? Page { get; set; }
        public override AriesGrpc.SearchReq Convert()
        {
            AriesGrpc.SearchReq result = new AriesGrpc.SearchReq()
            {
                Keyword = Keyword,
                Page = Page is not null ? new common.grpc.AriesPage()
                {
                    RowNum = Page.RowNum,
                    Size = Page.Size,
                } : null
            };
            return result;
        }
    }
}
