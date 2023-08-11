using aries.collector.grpc;
using aries.common;
using aries.common.cqrs;
using Dapr.Client.Autogen.Grpc.v1;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System.Collections.Concurrent;
using System.Text.Json;
using AriesCollectorGrpc = aries.collector.grpc;

namespace aries.collector.manage
{
    public partial class CollectService
    {

        private static ConcurrentQueue<CollectInfo>? repository =null;
        /// <summary>
        /// 采集用户行为操作
        /// </summary>
        /// <param name="request">request</param>
        /// <param name="context">context</param>
        /// <returns></returns>
        private Any UserBehaviorCollect(InvokeRequest request, ServerCallContext context)
        {
            if (request.Data.TryUnpack<AriesCollectorGrpc.CollectInfo>(out AriesCollectorGrpc.CollectInfo req))
            {
                repository!.Enqueue(new CollectInfo()
                {
                    Id = Guid.NewGuid().ToString(),
                    Ip = req.Ip,
                    Business = ((BusinessEnum)req.Business).GetDescription(),
                    CreatedAt = DateTime.Now,
                    Title = req.Title,
                    Url = req.Url,
                    UserName = req.UserName,
                });
               
            }
            return Any.Pack(new Empty());
        }
      
        private  void run()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (repository!.IsEmpty == true)
                    {
                        Task.Delay(1000);
                    }
                    else
                    {
                        CollectInfo item = new CollectInfo();
                        if (repository.TryDequeue(out item!) == true)
                        {
                            //入库操作
                            Task task1=  this.handler.Submit(item);
                            Task task2=this.handler.TopCollect(new CollectCountInfo()
                            {
                                Title = item.Title,
                                Url = item.Url,
                                UpdatedAt=item.CreatedAt,
                            });
                            Task.WhenAll(task1, task2);
                        }
                    }
                }
            });

        }
    }
}
