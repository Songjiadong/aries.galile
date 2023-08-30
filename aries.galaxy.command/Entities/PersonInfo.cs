using aries.common.db.rdms;
using System.Data;

namespace aries.galaxy.command
{
    public class PersonInfo
    {
        [DBField(StorageName = "ID", SqlDbType = DbType.String, IsPrimaryKey = true, Description = "主键")]
        public string? Id { get; set; }
        [DBField(StorageName = "NAME", SqlDbType = DbType.String, IsNotNull = true, Description = "中文名称")]
        public string? Name { get; set; }
    }
}
