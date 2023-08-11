using aries.common.cqrs;
using aries.common.db.rdms;
using aries.common.logger;

namespace aries.graph.command.organization
{
    public class OrganizationHandler :
        ICommandHandler<AddCommandInfo>,
        ICommandHandler<DeleteCommandInfo>,
        ICommandHandler<UpdateCommandInfo>
    {
        private readonly IDBService mysqlClient;
        public OrganizationHandler(IDBService mysqlClient)
        {
            this.mysqlClient = mysqlClient;
        }
        /// <summary>
        /// 提交操作
        /// </summary>
        /// <param name="command"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Handle(AddCommandInfo command)
        {
            int count = 0;
            try
            {
                mysqlClient.Insert(new OrganizationInfo()
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
                }, out count);
            }
            catch (Exception ex) 
            {
                LoggerService.Logger<OrganizationHandler>(ex, LogLevel.Warning);
            }
        }
        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="command"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Handle(DeleteCommandInfo command)
        {
            try 
            {
                mysqlClient.Delete(new OrganizationInfo { Id = command.Id.ToString() });
            }
            catch(Exception ex) 
            {
                LoggerService.Logger<OrganizationHandler>(ex, LogLevel.Warning);
            }
        }
        /// <summary>
        /// 修改操作
        /// </summary>
        /// <param name="command"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Handle(UpdateCommandInfo command)
        {
            int count = 0;
            try 
            {
                mysqlClient.Update<OrganizationInfo>(new OrganizationInfo() 
                {
                    Id = command.Id.ToString(),
                    
                }, out count);
            } 
            catch (Exception ex) 
            {
                LoggerService.Logger<OrganizationHandler>(ex, LogLevel.Warning);
            }
        }
    }
}
