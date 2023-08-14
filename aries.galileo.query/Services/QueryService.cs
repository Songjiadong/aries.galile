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
           
            //2初始化elasticsearch
            EsConfigOptions esOps = ApolloPull<EsConfigOptions>("elasticsearch")!;
            esClient = ClientInit(() =>
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
        private TService? ClientInit<TService>(Func<TService> initFunc)
        {
            TService? result = default;
            try
            {
                result = initFunc.Invoke();
            }
            catch (Exception ex)
            {
                LoggerService.Logger<QueryService>(ex, LogLevel.Error);
            }
            return result;
        }
        private TConfigOptions? ApolloPull<TConfigOptions>(string sectionName) where TConfigOptions : ConfigOptions
        {
            TConfigOptions? ops = System.Activator.CreateInstance<TConfigOptions>();
            configuration.GetSection(sectionName).Bind(ops);
            if (ops!.Setted == false)
            {
                throw new Exception($"Apollo配置中心拉取{sectionName}数据失败,请联系管理员....");
            }
            return ops;
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