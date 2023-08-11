using aries.collector;
using aries.common.db;
using aries.common.logger;
using Dapr.AppCallback.Autogen.Grpc.v1;
using Dapr.Client.Autogen.Grpc.v1;
using Grpc.Core;
using aries.common.db.phoenix;
using System.Net;
using System.Collections.Concurrent;

namespace aries.collector.manage
{
    public partial class CollectService : AppCallback.AppCallbackBase
    {
        private readonly IConfiguration configuration;
        private readonly CollectHandler handler;
        private static bool isFirst;
        public CollectService(IConfiguration configuration)
        {
            this.configuration = configuration;
            DBService? client = null;
            //初始化phoenix
            PhoenixConfigOptions ops = ApolloPull<PhoenixConfigOptions>("phoenix")!;
            client = ClientInit(() =>
            {
                return new DBService(new DBOpService<CollectService>(ops));
            });
            if (client is not null)
            {
                handler = new CollectHandler(client);
            }
            else
            {
                throw new NullReferenceException($"{nameof(CollectHandler)}初始化失败...");
            }
            if (repository is null)
            {
                repository = new ConcurrentQueue<CollectInfo>();
                isFirst = true;
            }
            else 
            {
                isFirst = false;
            }
            
            if (isFirst == true)
            {
                run();
            }
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
                LoggerService.Logger<CollectService>(ex, LogLevel.Error);
            }
            return result;
        }
        private TConfigOptions? ApolloPull<TConfigOptions>(string sectionName) where TConfigOptions : ConfigOptions
        {
            TConfigOptions? ops = default;
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
            switch (request.Method)
            {
                case "Collector$Manage$UserBehaviorCollect":
                    response.Data = UserBehaviorCollect(request, context);
                    break;
               
                default:
                    throw new NotSupportedException($"this {request.Method} is not supported.");
            }
            return Task.FromResult(response);
        }

    }
}