using aries.common.db.neo4j;
namespace aries.galaxy.query
{
    [DBTable(StorageName = "Person", Description = "人")]
    public class PersonInfo:GGraphEntityInfo
    {
        public PersonInfo() { }
    }
}
