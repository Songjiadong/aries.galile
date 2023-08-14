using aries.galaxy.grpc;
using System.Data;
using aries.common.db.neo4j;
using AriesNeo4j = aries.common.db.neo4j;
using aries.common.db.elasticsearch;
using AriesEs = aries.common.db.elasticsearch;
using aries.common.db;
using aries.common;

namespace aries.galaxy.query
{
    public class QueryHandler : IQueryHandler
    {
        private readonly AriesNeo4j.DBService client;
        public QueryHandler( AriesNeo4j.DBService neo4jClient)
        {
            this.client = neo4jClient;
        }

        public AriesObject<GraphInfo> Search(GraphDegreeSearchReq request)
        {
            AriesObject<GraphInfo> result = new AriesObject<GraphInfo> { };
            List<ConditionComponent> conditions = new List<ConditionComponent>();
            Condition sourceCond = new Condition("source", "name", request.Keyword!, DbType.String, DBOperatorEnum.Like);
            ConditionLeaf sourceLeaf = new NoConditionLeaf(sourceCond);
            conditions.Add(sourceLeaf);
            Condition targetCond = new Condition("target", "name", request.Keyword!, DbType.String, DBOperatorEnum.Like);
            ConditionLeaf targetLeaf = new OrConditionLeaf(targetCond);
            conditions.Add(targetLeaf);
            NoConditionComposite cond = new NoConditionComposite(conditions);
            try
            {
                result = client.Graph(whereCond: cond, request.Degree);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public AriesObject<GraphInfo> ShortestPath<From, To>(RelationSearchInfo<From, To> request) where From : GGraphEntityInfo where To : GGraphEntityInfo
        {
            AriesObject<GraphInfo> result = new AriesObject<GraphInfo> { };
            List<ConditionComponent> conditions = new List<ConditionComponent>();
            Condition sourceCond = new Condition("start", DBSource.Attribute.GetCypherColumnNameByPropertyName<GGraphEntityInfo, string?>(o => o.Name), request.FromKeyword!, DbType.String, DBOperatorEnum.Like);
            ConditionLeaf sourceLeaf = new NoConditionLeaf(sourceCond);
            conditions.Add(sourceLeaf);
            Condition targetCond = new Condition("end", DBSource.Attribute.GetCypherColumnNameByPropertyName<GGraphEntityInfo, string?>(o => o.Name), request.ToKeyword!, DbType.String, DBOperatorEnum.Like);
            ConditionLeaf targetLeaf = new AndConditionLeaf(targetCond);
            conditions.Add(targetLeaf);
            NoConditionComposite cond = new NoConditionComposite(conditions);
            try
            {
                result = client.ShortestPath<From, To>(request.StartNode!, request.EndNode!, cond);
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
