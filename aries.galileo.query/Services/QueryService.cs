using Dapr.AppCallback.Autogen.Grpc.v1;
using Dapr.Client.Autogen.Grpc.v1;
using Grpc.Core;
using System.Data;
using System.Text.Json;
using aries.common.db.elasticsearch;
using AriesEs = aries.common.db.elasticsearch;
using AriesPhoenix = aries.common.db.phoenix;
using aries.common.logger;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using aries.common.db;
using aries.common.db.neo4j;
using aries.common;

namespace aries.galileo.query
{
    public partial class QueryService : AppCallback.AppCallbackBase
    {
        private readonly IQueryHandler handler;
        public QueryService(IConfiguration configuration)
        {
            AriesEs.DBService? esClient = null;
            AriesPhoenix.DBService? phoenixClient=null;
            ConfigService<QueryService> configService = new ConfigService<QueryService>(configuration);
            //2初始化elasticsearch
            EsConfigOptions esOps = configService.ApolloPull<EsConfigOptions>("elasticsearch")!;
            esClient = configService.ClientInit(() =>
            {
                return new AriesEs.DBService(new AriesEs.DBOpService<QueryService>(esOps));
            });
            handler = new QueryHandler(esClient);
            ////3初始化phoenix
            //PhoenixConfigOptions phoenixOps = ApolloPull<PhoenixConfigOptions>("phoenix")!;
            //phoenixClient = ClientInit(() =>
            //{
            //    return new AriesPhoenix.DBService(new AriesPhoenix.DBOpService<QueryService>(phoenixOps));
            //});
            //if (esClient is not null&&phoenixClient is not null)
            //{
            //    handler = new QueryHandler(esClient,phoenixClient);
            //}
            //else
            //{
            //    throw new NullReferenceException($"{nameof(QueryHandler)}初始化失败...");
            //}
        }
        public override async Task<InvokeResponse> OnInvoke(InvokeRequest request, ServerCallContext context)
        {
            var response = new InvokeResponse();
            try
            {
                response.Data = request.Method switch
                {
                    "Galileo$Query$Search" => await SearchAsync(request, context),
                    "Galileo$Query$SearchByIndex" => await SearchByIndexAsync(request, context),
                    "Galileo$Query$GetTopList" => await GetTopListAsync(request, context),
                    _ => throw new NotSupportedException($"this {request.Method} is not supported."),
                };
            }
            catch (Exception ex)
            {
                LoggerService.Logger<QueryService>(ex, LogLevel.Error);
            }
            return response;
        }

    }
}