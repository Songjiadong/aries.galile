using aries.galaxy.grpc;
using System.Data;
using aries.common.db.neo4j;
using AriesNeo4j = aries.common.db.neo4j;
using aries.common.db;
using aries.common;

namespace aries.galaxy.query
{
    public class QueryHandler : IQueryHandler
    {
        private readonly AriesNeo4j.DBService client;
        public QueryHandler(AriesNeo4j.DBService neo4jClient)
        {
            this.client = neo4jClient;
        }
        public async Task<AriesDataTable> AutoCompleteAsync(SearchInfo searchInfo)
        {
            AriesDataTable result = new AriesDataTable();
            List<ConditionComponent> listCond = new List<ConditionComponent>();
            Condition keywordCond = new Condition(DBSource.Attribute.GetCypherColumnNameByPropertyName<GGraphEntityInfo, string?>(o => o.Name), searchInfo.Keyword!, DbType.String, DBOperatorEnum.Like);
            ConditionLeaf keyLeaf = new NoConditionLeaf(keywordCond);
            listCond.Add(keyLeaf);
            ColumnInfo columnInfo = new ColumnInfo();
            columnInfo.Items.Add(new Item() { Prefix = false, Name = "labels(item) as label" });
            columnInfo.Items.Add(new Item { Name = DBSource.Attribute.GetCypherColumnNameByPropertyName<GGraphEntityInfo, string?>(o => o.Id), Prefix = true });
            columnInfo.Items.Add(new Item { Name = DBSource.Attribute.GetCypherColumnNameByPropertyName<GGraphEntityInfo, string?>(o => o.Name), Prefix = true });
            result = await client.GetInfosAsync<NullGraphEntityInfo>(new NullTopFunc(), columnInfo, propertiesCond: new NullGraphEntityInfo(), whereCond: new NoConditionComposite(listCond));
            return result;
        }
        public async Task<AriesObject<GraphInfo>> GraphAsync(GraphDegreeReq request)
        {
            AriesObject<GraphInfo> result = new AriesObject<GraphInfo> { };
            List<ConditionComponent> conditions = new List<ConditionComponent>();
            Condition sourceCond = new Condition("source",
                DBSource.Attribute.GetCypherColumnNameByPropertyName<GGraphEntityInfo, string?>(o => o.Id),
                request.Id!,
                DbType.String,
                DBOperatorEnum.Equal);
            ConditionLeaf sourceLeaf = new NoConditionLeaf(sourceCond);
            conditions.Add(sourceLeaf);
            Condition targetCond = new Condition("target",
                DBSource.Attribute.GetCypherColumnNameByPropertyName<GGraphEntityInfo, string?>(o => o.Id),
                request.Id!,
                DbType.String,
                DBOperatorEnum.Equal);
            ConditionLeaf targetLeaf = new OrConditionLeaf(targetCond);
            conditions.Add(targetLeaf);
            NoConditionComposite cond = new (conditions);
            try
            {
                result = await client.GraphAsync(whereCond: cond, request.Degree);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<AriesObject<GraphInfo>> ShortestPathAsync<From, To>(RelationSearchInfo<From, To> request) where From : GGraphEntityInfo where To : GGraphEntityInfo
        {
            AriesObject<GraphInfo> result = new AriesObject<GraphInfo> { };
            List<ConditionComponent> conditions = new List<ConditionComponent>();
            Condition sourceCond = new Condition("start", 
                DBSource.Attribute.GetCypherColumnNameByPropertyName<GGraphEntityInfo, string?>(o => o.Name), 
                request.FromKeyword!,
                DbType.String,
                DBOperatorEnum.Like);
            ConditionLeaf sourceLeaf = new NoConditionLeaf(sourceCond);
            conditions.Add(sourceLeaf);
            Condition targetCond = new Condition("end", 
                DBSource.Attribute.GetCypherColumnNameByPropertyName<GGraphEntityInfo, string?>(o => o.Name),
                request.ToKeyword!,
                DbType.String, 
                DBOperatorEnum.Like);
            ConditionLeaf targetLeaf = new AndConditionLeaf(targetCond);
            conditions.Add(targetLeaf);
            NoConditionComposite cond = new NoConditionComposite(conditions);
            try
            {
                result =await client.ShortestPathAsync<From, To>(request.StartNode!, request.EndNode!, cond);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }
        public static List<string> GetAllLabelList()
        {
            List<string> result = typeof(OrganizationTypeEnum).GetDescriptionList();
            result.Add("人员");
            return result;
        }
        public static string LabelConvert(string label)
        {
            string result = string.Empty;
            switch (label)
            {
                case "GovernmentAgency":
                    result = "国家机关";
                    break;
                case "PublicInstitution":
                    result = "事业单位";
                    break;
                case "SocialOrganization":
                    result = "社会团体";
                    break;
                case "Federation":
                    result = "行业联合会";
                    break;
                case "Association":
                    result = "行业协会";
                    break;
                case "Society":
                    result = "行业学会";
                    break;
                case "University":
                    result = "高校";
                    break;
                case "Enterprise":
                    result = "企业";
                    break;
                case "Person":
                    result = "人员";
                    break;
                default:
                    throw new NotSupportedException("不支持");
            }
            return result;
        }
    }
}
