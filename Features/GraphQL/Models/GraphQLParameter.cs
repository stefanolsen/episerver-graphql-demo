using Newtonsoft.Json.Linq;

namespace EPiServer.Reference.Commerce.Site.Features.GraphQL.Models
{
    public class GraphQLParameter
    {
        public string OperationName { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; }
    }
}