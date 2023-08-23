using Dapr.AppCallback.Autogen.Grpc.v1;
using Dapr.Client.Autogen.Grpc.v1;
using Grpc.Core;


namespace aries.galaxy.command
{
    public partial class CommandService : AppCallback.AppCallbackBase
    {
        private readonly ILogger<CommandService> logger;
        public CommandService(ILogger<CommandService> logger)
        {
            this.logger = logger;
        }

        public override Task<InvokeResponse> OnInvoke(InvokeRequest request, ServerCallContext context)
        {
            var response = new InvokeResponse();
            switch (request.Method)
            {
                case "Graph$Command$Node$Organization$Submit":
                    response.Data = UnitSubmit(request, context);
                    break;
                case "Graph$Command$Node$Organization$Delete":
                    response.Data=UnitDelete(request, context);
                    break;
                case "Graph$Command$Node$Person$Submit":
                    break;
                case "Graph$Command$Node$Person$Delete":
                    break;
                case "Graph$Command$Relation$Add":
                    response.Data = RelationAdd(request, context);
                    break;
                case "Graph$Command$Relation$Remove":
                    response.Data= RelationRemove(request, context);
                    break;
                case "Graph$Command$Import":
                    response.Data= Import(request, context);
                    break;
                default:
                    throw new NotSupportedException($"this {request.Method} is not supported.");
            }
            return Task.FromResult(response);
        }
    }
}