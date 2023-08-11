using aries.common.db.neo4j;

namespace aries.galaxy.query
{
    public class RelationSearchInfo<From, To> where From : GGraphEntityInfo where To : GGraphEntityInfo
    {
        public string? FromKeyword { get; set; }
        public string? ToKeyword { get; set; }
        public From? StartNode { get; set; }
        public To? EndNode { get; set; }
    }
}
