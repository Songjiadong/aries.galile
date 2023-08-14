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
        private readonly IConfiguration configuration;
        public QueryService(IConfiguration configuration)
        {
            AriesEs.DBService? esClient = null;
            AriesPhoenix.DBService? phoenixClient=null;
            this.configuration = configuration;
            ConfigService<QueryService> configService = new common.ConfigService<QueryService>(configuration);
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
                switch (request.Method)
                {
                    case "Galileo$Query$Search":
                        response.Data =await SearchAsync(request, context);
                        break;
                    case "Galileo$Query$SearchByIndex":
                        response.Data = await SearchByIndexAsync(request, context);
                        break;
                    case "Galileo$Query$GetTopList":
                        response.Data =await GetTopListAsync(request, context);
                        break;
                    default:
                        throw new NotSupportedException($"this {request.Method} is not supported.");
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger<QueryService>(ex, LogLevel.Error);
            }
            return response;
        }

    }
}