using aries.common.db.phoenix;
using System.Data;

namespace aries.collector
{
    [DBTable(StorageName = "galile_top_info", Description = "垂直搜索热榜表")]
    public class CollectCountInfo
    {
        [DBField(IsPrimaryKey = true, SqlDbType = DbType.String, StorageName = "url", Description = "Url地址")]
        public string? Url { get; set; }
        [DBField(SqlDbType = DbType.String, StorageName = "title", Description = "标题")]
        public string? Title { get; set; }
        
        [DBField(SqlDbType = DbType.DateTime, StorageName = "updated",IsNotNull =true, Description = "最后修改时间")]
        public DateTime? UpdatedAt { get; set; }
        [DBField(SqlDbType = DbType.Int64, StorageName = "top_count", Description = "计数器")]
        public long Count { get; set; }
    }
}
