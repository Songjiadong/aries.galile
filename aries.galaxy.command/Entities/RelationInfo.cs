using aries.common;
using aries.common.db.rdms;
using System.Data;
using System.Numerics;

namespace aries.galaxy.command
{
    [DBTable(DBStorageName = "galaxy", StorageName = "relation_info", Description = "关系信息采集")]
    public class RelationInfo
    {
        public RelationInfo() { }
        [DBField(StorageName = "ID", SqlDbType = DbType.String, IsPrimaryKey = true, Description = "主键")]
        public string? Id { get; set; }
        [DBField(StorageName = "SOURCE_ID", SqlDbType = DbType.String, IsNotNull = true, Description = "源Id")]
        public string? SourceId { get; set; }
        [DBField(StorageName = "SOURCE_NAME", SqlDbType = DbType.String, IsNotNull = true, Description = "源")]
        public string? SourceName { get; set; }
        [DBField(StorageName = "DEST_ID", SqlDbType = DbType.String, IsNotNull = true, Description = "目的Id")]
        public string? DestinationId { get; set; }
        [DBField(StorageName = "DEST_NAME", SqlDbType = DbType.String, IsNotNull = true, Description = "目的")]
        public string? DestinationName { get; set;}
        [DBField(StorageName = "REL_NAME", SqlDbType = DbType.String, IsNotNull = true, Description = "关系名称")]
        public string? RelationName { get; set;}
    }
    [DBTable(DBStorageName = "galaxy", StorageName = "relation_history_info", Description = "关系信息历史采集")]
    public class RelationHistoryInfo 
    {
        public RelationHistoryInfo() { }
        [DBField(StorageName = "ID", SqlDbType = DbType.String,   Description = "主键")]
        public string? Id { get; set; }
        [DBField(StorageName = "SOURCE_ID", SqlDbType = DbType.String, IsNotNull = true, Description = "源Id")]
        public string? SourceId { get; set; }
        [DBField(StorageName = "SOURCE_NAME", SqlDbType = DbType.String, IsNotNull = true, Description = "源")]
        public string? SourceName { get; set; }
        [DBField(StorageName = "DEST_ID", SqlDbType = DbType.String, IsNotNull = true, Description = "目的Id")]
        public string? DestinationId { get; set; }
        [DBField(StorageName = "DEST_NAME", SqlDbType = DbType.String, IsNotNull = true, Description = "目的")]
        public string? DestinationName { get; set; }
        [DBField(StorageName = "REL_NAME", SqlDbType = DbType.String, IsNotNull = true, Description = "关系名称")]
        public string? RelationName { get; set; }
        [DBField(StorageName = "BEGIN_TIME", SqlDbType = DbType.Date, IsNotNull = true, Description = "开始时间")]
        public DateTime? BeginTime { get; set; }
        [DBField(StorageName = "END_TIME", SqlDbType = DbType.Date, Description = "结束时间")]
        public DateTime? EndTime { get; set;}
        [DBField(StorageName = "VALID", SqlDbType = DbType.Int32, IsNotNull = true, Description = "有效性")]
        public ValidEnum Valid { get; set; }
        [DBField(StorageName = "SORT_NO", SqlDbType = DbType.Int64,IsPrimaryKey =true, IsIdentification = true, Description = "排序")]
        public BigInteger? SortNo { get; set; }
    }
}
