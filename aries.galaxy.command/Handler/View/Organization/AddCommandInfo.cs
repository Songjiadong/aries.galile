using aries.common;
using aries.common.cqrs;

namespace aries.graph.command.organization
{
    public class AddCommandInfo:CommandInfo
    {
        public AddCommandInfo() { }
        public string? Name { get; set; }
       
        public string? EnglishName { get; set; }
        
        public string? AbbreviationName { get; set; }
       
        public OrganizationTypeEnum OrgType { get; set; }
        public string? USCC { get; set; }
        public string? Logo { get; set; }
        public string? Url { get; set; }
        public string? Person { get; set; }
        public string? Address { get; set; }
        public string? BusinessScope { get; set; }
        public InfoSourceEnum? Source { get; set; }
        public CertificationStatusEnum CertificationStatus { get; set; }
    }
}
