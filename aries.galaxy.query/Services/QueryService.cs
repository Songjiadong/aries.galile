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
using aries.common;

namespace aries.galaxy.query
{
    public partial class QueryService : AppCallback.AppCallbackBase
    {
        private readonly IQueryHandler handler;
        public QueryService(IConfiguration configuration)
        {
            AriesNeo4j.DBService? neo4jClient = null;
            ConfigService<QueryService> configService = new ConfigService<QueryService>(configuration);
            //1初始化neo4j
            Neo4jConfigOptions  neo4jOps = configService.ApolloPull<Neo4jConfigOptions>("neo4j")!;
            neo4jClient= configService.ClientInit(() =>
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
        public override Task<InvokeResponse> OnInvoke(InvokeRequest request, ServerCallContext context)
        {
            var response = new InvokeResponse();
            try
            {
                response.Data = request.Method switch
                {
                    "Galaxy$Query$Graph" => Graph(request, context),
                    "Galaxy$Query$ShortestPath" => ShortestPath(request, context),
                    _ => throw new NotSupportedException($"this {request.Method} is not supported."),
                };
            }
            catch (Exception ex)
            {
                LoggerService.Logger<QueryService>(ex, LogLevel.Error);
            }
            return Task.FromResult(response);
        }

    }
}