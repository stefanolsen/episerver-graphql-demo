using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using EPiServer.Reference.Commerce.Site.Features.GraphQL.Models;
using EPiServer.Security;
using GraphQL;
using GraphQL.Types;
using Newtonsoft.Json;

namespace EPiServer.Reference.Commerce.Site.Features.GraphQL.Controllers
{
    [RoutePrefix("api/graphql")]
    public class GraphQLController : ApiController
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;
        private readonly PermissionService _permissionService;

        public GraphQLController(
            IDocumentExecuter documentExecuter,
            ISchema schema,
            PermissionService permissionService)
        {
            _documentExecuter = documentExecuter;
            _schema = schema;
            _permissionService = permissionService;
        }

        [Route("query")]
        public async Task<IHttpActionResult> Query(CancellationToken cancellationToken)
        {
            string requestBody = await Request.Content.ReadAsStringAsync();
            GraphQLParameter parameters = JsonConvert.DeserializeObject<GraphQLParameter>(requestBody);
            if (parameters == null)
            {
                return BadRequest();
            }

            var executionOptions = new ExecutionOptions
            {
                CancellationToken = cancellationToken,
                Schema = _schema,
                OperationName = parameters.OperationName,
                Query = parameters.Query,
                Inputs = parameters.Variables.ToInputs(),
                UserContext = User,
#if DEBUG
                ExposeExceptions = true
#else
                ExposeExceptions = _permissionService.IsPermitted(User, SystemPermissions.DetailedErrorMessage)
#endif
            };
            
            var result = await _documentExecuter.ExecuteAsync(executionOptions);
            if (result.Errors?.Count > 0)
            {
                return new NegotiatedContentResult<ExecutionResult>(HttpStatusCode.BadRequest, result, this);
            }

            return Ok(result);
        }
    }
}