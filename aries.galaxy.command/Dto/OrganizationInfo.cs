using aries.common;
using aries.common.db.rdms;
using System.Numerics;
using System.Data;

namespace aries.graph.command
{
    [DBTable(DBStorageName = "galaxy", StorageName = "organization_info", Description ="机构信息采集")]
    public class OrganizationInfo
    {
        public OrganizationInfo() { }
        [DBField(StorageName = "ID", SqlDbType =DbType.String, IsPrimaryKey = true, Description ="主键")]
        public string? Id { get; set; }
        [DBField(StorageName = "NAME", SqlDbType = DbType.String, IsNotNull = true, Description = "中文名称")]
        public string? Name { get; set; }
        [DBField(StorageName = "ENG_NAME", SqlDbType = DbType.String, Description = "英文名称")]
        public string? EnglishName { get; set; }
        [DBField(StorageName = "ABBR_NAME", SqlDbType = DbType.String, Description = "简称")]
        public string? AbbreviationName { get; set; }
        [DBField(StorageName = "ORG_TYPE", SqlDbType = DbType.Int32, Description = "机构类别")]
        public int? OrgType { get; set; }
        [DBField(StorageName = "USCC", SqlDbType = DbType.String, Description = "社会信用统一代码")]
        public string? USCC { get; set; }
        [DBField(StorageName = "LOGO", SqlDbType = DbType.String, Description = "Logo")]
        public string? Logo { get; set; }
        [DBField(StorageName = "URL", SqlDbType = DbType.String, Description = "官网Url")]
        public string? Url { get; set; }
        [DBField(StorageName = "PERSON", SqlDbType = DbType.String, Description = "法人")]
        public string? Person { get; set; }
        [DBField(StorageName = "ADDRESS", SqlDbType = DbType.String, Description = "地址")]
        public string? Address { get; set; }
        [DBField(StorageName = "BUSINESS_SCOPE", SqlDbType = DbType.String, Description = "经营范围")]
        public string? BusinessScope { get; set; }
        [DBField(StorageName = "CREATED", SqlDbType = DbType.Date, IsNotNull = true, Description = "创建时间")]
        public DateTime? CreatedAt { get; set; }
        [DBField(StorageName = "UPDATED", SqlDbType = DbType.Date, IsNotNull = true, Description = "修改时间")]
        public DateTime? UpdatedAt { get;set; }
        [DBField(StorageName = "PUBLISHED", SqlDbType = DbType.Date, Description = "发布时间")]
        public DateTime? PublishedAt { get; set; }
        [DBField(StorageName = "SOURCE", SqlDbType = DbType.Int32, IsNotNull = true, Description = "信息获取来源")]
        public int? Source { get; set; }
        [DBField(StorageName = "CERTIFICATION_STATUS", SqlDbType = DbType.Int32, IsNotNull = true, Description = "认证状态")]
        public int? CertificationStatus { get; set; }  
    }
    [DBTable(DBStorageName = "galaxy", StorageName = "organization_history_info", Description = "机构信息历史采集")]
    public class OrganizationHistoryInfo
    {
        public OrganizationHistoryInfo() { }
        [DBField(StorageName = "ID", SqlDbType = DbType.String, Description = "主键")]
        public string? Id { get; set; }
        [DBField(StorageName = "NAME", SqlDbType = DbType.String, IsNotNull = true, Description = "中文名称")]
        public string? Name { get; set; }
        [DBField(StorageName = "ENG_NAME", SqlDbType = DbType.String, Description = "英文名称")]
        public string? EnglishName { get; set; }
        [DBField(StorageName = "ABBR_NAME", SqlDbType = DbType.String, Description = "简称")]
        public string? AbbreviationName { get; set; }
        [DBField(StorageName = "ORG_TYPE", SqlDbType = DbType.Int32, Description = "机构类别")]
        public int? OrgType { get; set; }
        [DBField(StorageName = "USCC", SqlDbType = DbType.String, Description = "社会信用统一代码")]
        public string? USCC { get; set; }
        [DBField(StorageName = "LOGO", SqlDbType = DbType.String, Description = "Logo")]
        public string? Logo { get; set; }
        [DBField(StorageName = "URL", SqlDbType = DbType.String, Description = "官网Url")]
        public string? Url { get; set; }
        [DBField(StorageName = "PERSON", SqlDbType = DbType.String, Description = "法人")]
        public string? Person { get; set; }
        [DBField(StorageName = "ADDRESS", SqlDbType = DbType.String, Description = "地址")]
        public string? Address { get; set; }
        [DBField(StorageName = "BUSINESS_SCOPE", SqlDbType = DbType.String, Description = "经营范围")]
        public string? BusinessScope { get; set; }
        [DBField(StorageName = "SOURCE", SqlDbType = DbType.Int32,IsNotNull =true, Description = "信息获取来源")]
        public int? Source { get; set; }
        [DBField(StorageName = "CERTIFICATION_STATUS", SqlDbType = DbType.Int32, IsNotNull = true, Description = "认证状态")]
        public int? CertificationStatus { get; set; }
        [DBField(StorageName = "BEGIN_TIME", SqlDbType = DbType.Date, IsNotNull = true, Description = "开始时间")]
        public DateTime? BeginTime { get; set; }
        [DBField(StorageName = "END_TIME", SqlDbType = DbType.Date,  Description = "结束时间")]
        public DateTime? EndTime { get; set;}
        [DBField(StorageName = "VALID", SqlDbType = DbType.Int32,IsNotNull = true, Description = "结束时间")]
        public ValidEnum? Valid { get; set; }
        [DBField(StorageName = "SORT_NO",  SqlDbType = DbType.Int64, IsPrimaryKey = true, IsIdentification =true, Description = "排序")]
        public BigInteger? SortNo { get; set; }
    }
}
