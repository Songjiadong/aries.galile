using aries.common;
using aries.common.db.neo4j;

namespace aries.galaxy.query
{
    [DBTable(StorageName = "Organization", Description = "机构单位")]
    public class OrganizationInfo : GGraphEntityInfo
    {
        public OrganizationInfo() { }
        [DBField(StorageName = "orgtype", Description = "机构单位类别")]
        public OrganizationTypeEnum? OrgType { get; set; }
        
    }
}
