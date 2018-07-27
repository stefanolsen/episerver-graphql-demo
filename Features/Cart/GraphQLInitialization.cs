using System.Collections.Generic;
using EPiServer.Commerce.Order;
using EPiServer.Reference.Commerce.Site.Features.Cart.Mutations;
using EPiServer.Reference.Commerce.Site.Features.Cart.Services;
using EPiServer.Reference.Commerce.Site.Features.Cart.ViewModelFactories;
using EPiServer.Reference.Commerce.Site.Features.Product;
using EPiServer.Reference.Commerce.Site.Features.Product.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Search.Services;
using EPiServer.Reference.Commerce.Site.Features.Search.ViewModels;
using EPiServer.Reference.Commerce.Site.Infrastructure.GraphQL;
using EPiServer.ServiceLocation;
using GraphQL.Builders;
using GraphQL.Relay.Types;
using GraphQL.Types;

namespace EPiServer.Reference.Commerce.Site.Features.Cart
{
    [ServiceConfiguration(typeof(IGraphQLConfiguration))]
    public class GraphQLInitialization : IGraphQLConfiguration
    {
        private readonly ICartService _cartService;
        private readonly CartViewModelFactory _cartViewModelFactory;
        private readonly ISearchService _searchService;

        public GraphQLInitialization(
            ICartService cartService,
            CartViewModelFactory cartViewModelFactory,
            ISearchService searchService)
        {
            _cartService = cartService;
            _cartViewModelFactory = cartViewModelFactory;
            _searchService = searchService;
        }

        public void ConfigureMutations(MutationGraphType mutationContext)
        {
            mutationContext.Mutation<AddToCartInput, AddToCartPayload>("addToCart");
            mutationContext.Mutation<ChangeCartItemInput, ChangeCartItemPayload>("changeCartItem");
        }

        public void ConfigureQueries(ObjectGraphType queryContext)
        {
            queryContext.Connection<ProductType>()
                .Name("products")
                .Argument<StringGraphType>("query", "The search query")
                .Resolve(ResolveQuickSearch);

            queryContext.Field<CartType>("cart",
                resolve: context =>
                {
                    ICart cart = _cartService.LoadCart(_cartService.DefaultCartName);
                    return _cartViewModelFactory.CreateLargeCartViewModel(cart);
                });
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