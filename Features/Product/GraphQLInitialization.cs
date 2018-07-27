using System.Collections.Generic;
using EPiServer.Reference.Commerce.Site.Features.Product.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Search.Services;
using EPiServer.Reference.Commerce.Site.Features.Search.ViewModels;
using EPiServer.Reference.Commerce.Site.Infrastructure.GraphQL;
using EPiServer.ServiceLocation;
using GraphQL.Builders;
using GraphQL.Relay.Types;
using GraphQL.Types;

namespace EPiServer.Reference.Commerce.Site.Features.Product
{
    [ServiceConfiguration(typeof(IGraphQLConfiguration))]
    public class GraphQLInitialization : IGraphQLConfiguration
    {
        private readonly ISearchService _searchService;

        public GraphQLInitialization(
            ISearchService searchService)
        {
            _searchService = searchService;
        }

        public void ConfigureMutations(MutationGraphType mutationContext)
        {
        }

        public void ConfigureQueries(ObjectGraphType queryContext)
        {
            queryContext.Connection<ProductType>()
                .Name("products")
                .Argument<StringGraphType>("query", "The search query")
                .Resolve(ResolveQuickSearch);
        }

        private object ResolveQuickSearch(ResolveConnectionContext<object> context)
        {
            var viewModel = new FilterOptionViewModel
            {
                Page = ConnectionUtils.OffsetOrDefault(context.After, 0) / context.PageSize ?? 10,
                PageSize = context.PageSize ?? 10,
                Q = context.GetArgument<string>("query")
            };
            IEnumerable<ProductTileViewModel> products =
                _searchService.QuickSearch(viewModel);

            return ConnectionUtils.ToConnection(
                products,
                context,
                viewModel.Page * viewModel.PageSize,
                viewModel.TotalCount);
        }
    }
}