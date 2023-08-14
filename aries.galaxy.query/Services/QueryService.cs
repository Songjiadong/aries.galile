using Dapr.AppCallback.Autogen.Grpc.v1;
using Dapr.Client.Autogen.Grpc.v1;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;
using aries.common.db.neo4j;
using AriesNeo4j = aries.common.db.neo4j;
using aries.common.db.elasticsearch;
using AriesEs = aries.common.db.elasticsearch;
using aries.common.logger;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using aries.common.db;
using System.Net;

namespace aries.galaxy.query
{
    public partial class QueryService : AppCallback.AppCallbackBase
    {
        private readonly IQueryHandler handler;
        private readonly IConfiguration configuration;
        public QueryService(IConfiguration configuration)
        {
            AriesNeo4j.DBService? neo4jClient = null;
            this.configuration = configuration;
            //1初始化neo4j

            Neo4jConfigOptions  neo4jOps = ApolloPull<Neo4jConfigOptions>("neo4j")!;
            neo4jClient=ClientInit(() =>
            {
                 return new AriesNeo4j.DBService(new AriesNeo4j.DBOpService<QueryService>(neo4jOps));
            });
          
            if (neo4jClient is not null)
            {
                handler = new QueryHandler(neo4jClient);
            }
            else 
            {
                throw new NullReferenceException("QueryHandler初始化失败...");
            }
        }
        private TService? ClientInit<TService>(Func<TService>initFunc) 
        {
            TService? result=default;
            try
            {
                result= initFunc.Invoke();
                

            }
            catch (Exception ex)
            {
                LoggerService.Logger<QueryService>(ex, LogLevel.Error);
            }
            return result;
        }
        private TConfigOptions? ApolloPull<TConfigOptions>(string sectionName) where TConfigOptions : ConfigOptions
        {
            TConfigOptions? ops= System.Activator.CreateInstance<TConfigOptions>();
            configuration.GetSection(sectionName).Bind(ops);
            if (ops!.Setted == false)
            {
                throw new Exception($"Apollo配置中心拉取{sectionName}数据失败,请联系管理员....");
            }
            return ops;
        }
        public override Task<InvokeResponse> OnInvoke(InvokeRequest request, ServerCallContext context)
        {
            var response = new InvokeResponse();
            try
            {
                switch (request.Method)
                {
                    case "Galaxy$Query$Search":
                        response.Data = Search(request, context);
                        break;
                    case "Galaxy$Query$ShortestPath":
                        response.Data = ShortestPath(request, context);
                        break;
                    default:
                        throw new NotSupportedException($"this {request.Method} is not supported.");
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger<QueryService>(ex, LogLevel.Error);
            }
            return Task.FromResult(response);
        }

    }
}