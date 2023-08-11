using aries.common.cqrs;
using aries.common.db.rdms;

namespace aries.graph.command.relation
{
    public class RelationHandler :
        ICommandHandler<AddCommandInfo>,
        ICommandHandler<RemoveCommandInfo>
    {
        private readonly IDBService mysqlClient;
        public RelationHandler(IDBService mysqlClient) 
        { 
            this.mysqlClient = mysqlClient;
        }
        /// <summary>
        /// 添加关系操作
        /// </summary>
        /// <param name="command"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Handle(AddCommandInfo command)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 移除关系操作
        /// </summary>
        /// <param name="command"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Handle(RemoveCommandInfo command)
        {
            throw new NotImplementedException();
        }
    }
}
