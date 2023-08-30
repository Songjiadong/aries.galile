using aries.businessapi;
using aries.common.cqrs;
using aries.common.db.rdms;

namespace aries.galaxy.command.organization
{
    public class OrganizationHandler : AriesBusinessAPIBase,
        ICommandHandler<AddCommandInfo>,
        ICommandHandler<DeleteCommandInfo>,
        ICommandHandler<UpdateCommandInfo> 
        
    {
        private readonly IDBService client;
        public OrganizationHandler(IDBService client)
        {
            this.client = client;
        }
        /// <summary>
        /// 提交操作
        /// </summary>
        /// <param name="command"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Handle(AddCommandInfo command)
        {
            Do<OrganizationHandler>(()=> {
                client.Insert(new OrganizationInfo()
                {
                    Id = Guid.NewGuid().ToString(),
                    Address = command.Address,
                    Name = command.Name,
                    BusinessScope = command.BusinessScope,
                    CreatedAt = DateTime.Now,
                    CertificationStatus = (int?)command.CertificationStatus,
                    OrgType = (int?)command.OrgType,
                    EnglishName = command.EnglishName,
                    AbbreviationName = command.AbbreviationName,
                    Logo = command.Logo,
                    Person = command.Person,
                    Url = command.Url,
                    USCC = command.USCC,
                    Source = (int?)command.Source,
                    UpdatedAt = DateTime.Now,
                }, out int count);
            });
          
        }
        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="command"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Handle(DeleteCommandInfo command)
        {
            Do<OrganizationHandler>(() => {
                client.Delete(new OrganizationInfo { Id = command.Id.ToString() });
            });
        }
        /// <summary>
        /// 修改操作
        /// </summary>
        /// <param name="command"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Handle(UpdateCommandInfo command)
        {
            Do<OrganizationHandler>(() => {
                client.Update<OrganizationInfo>(new OrganizationInfo()
                {
                    Id = command.Id.ToString(),

                }, out int count);
            });
        }
    }
}
