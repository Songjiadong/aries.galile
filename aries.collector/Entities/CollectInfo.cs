using aries.common.db.phoenix;
using System.Data;

namespace aries.collector
{
    [DBTable(StorageName ="collect_info",Description ="用户行为信息采集表")]
    public class CollectInfo
    {
        public CollectInfo() { }
        [DBField(IsPrimaryKey =true,SqlDbType =DbType.String,StorageName ="id",Description ="主键ID")]
        public string? Id { get; set; }
        [DBField(SqlDbType = DbType.String, StorageName = "ip_address",IsNotNull =true, Description = "IP地址")]
        public string? Ip { get; set; }
        [DBField(SqlDbType = DbType.String, StorageName = "user_name", IsNotNull = true, Description = "用户名",DefaultStorageName = "anonymous")]
        public string? UserName { get; set; }
        [DBField(SqlDbType = DbType.String, StorageName = "business", IsNotNull = true, Description = "所属业务")]
        public string? Business { get; set; }
        [DBField(SqlDbType = DbType.String, StorageName = "title",  Description = "点击标题")]
        public string? Title { get; set;}
        [DBField(SqlDbType = DbType.String, StorageName = "title", Description = "点击Url地址")]
        public string? Url { get; set; }
        [DBField(SqlDbType = DbType.DateTime, StorageName = "created", IsNotNull =true,Description = "用户行为时间")]
        public DateTime? CreatedAt { get; set; }
    }
}
