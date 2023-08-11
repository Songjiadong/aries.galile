using aries.common.db.phoenix;
using aries.common.logger;

namespace aries.collector.manage
{
    public class CollectHandler
    {
        private readonly DBService client;
        public CollectHandler(DBService client)
        {
            this.client = client;
        }
        /// <summary>
        /// 提交采集操作
        /// </summary>
        /// <param name="collectInfo">采集信息</param>
        /// <returns></returns>
        public async Task Submit(CollectInfo collectInfo) 
        {
            var result = await((IDBService)client).SubmitAsync(collectInfo, true);
            if (result.Result == false) 
            {
                LoggerService.Logger<CollectHandler>(new Exception(message: result.Message), LogLevel.Error);
            }
           
        }
        /// <summary>
        /// 更新Top榜单
        /// </summary>
        /// <param name="collectCountInfo">计数采集信息</param>
        /// <returns></returns>
        public async Task TopCollect(CollectCountInfo collectCountInfo) 
        {
            string sql = $"upsert into galile_top_info(url,title,updated,top_count) values('{collectCountInfo.Url}','{collectCountInfo.Title}','{collectCountInfo.UpdatedAt}',1) on duplicate key update top_count=top_count+1";
            var result= await ((IDBService)client).ExcuteAsync(sql);
            if (result.Result == false)
            {
                LoggerService.Logger<CollectHandler>(new Exception(message:result.Message), LogLevel.Error);
            }
        }
    }
}
