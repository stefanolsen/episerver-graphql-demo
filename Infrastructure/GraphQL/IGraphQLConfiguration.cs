using GraphQL.Relay.Types;
using GraphQL.Types;

namespace EPiServer.Reference.Commerce.Site.Infrastructure.GraphQL
{
    public interface IGraphQLConfiguration
    {
        void ConfigureMutations(MutationGraphType mutationContext);
        void ConfigureQueries(ObjectGraphType queryContext);
    }
}
