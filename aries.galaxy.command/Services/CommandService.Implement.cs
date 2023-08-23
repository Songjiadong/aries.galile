using Dapr.Client.Autogen.Grpc.v1;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace aries.galaxy.command
{
    public partial class CommandService
    {
        /// <summary>
        /// 提交节点操作
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private Any UnitSubmit(InvokeRequest request, ServerCallContext context)
        {
            throw new Exception();

            //return Any.Pack(output);
        }
        /// <summary>
        /// 删除节点操作
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private Any UnitDelete(InvokeRequest request, ServerCallContext context) 
        {
            throw new Exception();
        }
        /// <summary>
        /// 添加关系操作
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private Any RelationAdd(InvokeRequest request, ServerCallContext context) 
        {
            throw new Exception();
        }
        /// <summary>
        /// 移除关系操作
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private Any RelationRemove(InvokeRequest request, ServerCallContext context) 
        {
            throw new Exception();
        }
        /// <summary>
        /// 数据导入
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private Any Import(InvokeRequest request, ServerCallContext context) 
        {
            throw new Exception();
        }
    }
}
