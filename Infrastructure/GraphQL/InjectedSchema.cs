using System;
using System.Collections.Generic;
using GraphQL.Relay.Types;
using GraphQL.Types;

namespace EPiServer.Reference.Commerce.Site.Infrastructure.GraphQL
{
    public class InjectedSchema : Schema
    {
        public InjectedSchema(
            Func<Type, IGraphType> resolveType,
            IEnumerable<IGraphQLConfiguration> configurations)
            : base(resolveType)
        {
            var siteMutation = new MutationGraphType();
            var siteQuery = new ObjectGraphType();

            foreach (var configuration in configurations)
            {
                configuration.ConfigureQueries(siteQuery);
                configuration.ConfigureMutations(siteMutation);
            }

            Mutation = siteMutation;
            Query = siteQuery;
        }
    }
}