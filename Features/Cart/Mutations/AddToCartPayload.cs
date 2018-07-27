using System.Linq;
using System.Web;
using EPiServer.Commerce.Order;
using EPiServer.Reference.Commerce.Site.Features.Cart.Services;
using EPiServer.Reference.Commerce.Site.Features.Cart.ViewModelFactories;
using EPiServer.Reference.Commerce.Site.Features.Cart.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Recommendations.Services;
using EPiServer.ServiceLocation;
using GraphQL;
using GraphQL.Relay.Types;
using GraphQL.Types;

namespace EPiServer.Reference.Commerce.Site.Features.Cart.Mutations
{
    public class AddToCartPayload : MutationPayloadGraphType
    {
        private readonly ICartService _cartService;
        private readonly CartViewModelFactory _cartViewModelFactory;
        private readonly IOrderRepository _orderRepository;
        private readonly IRecommendationService _recommendationService;
        private readonly ServiceAccessor<HttpContextBase> _httpContextBase;

        public AddToCartPayload(
            ICartService cartService,
            CartViewModelFactory cartViewModelFactory, 
            IOrderRepository orderRepository,
            IRecommendationService recommendationService,
            ServiceAccessor<HttpContextBase> httpContextBase)
        {
            _cartService = cartService;
            _cartViewModelFactory = cartViewModelFactory;
            _orderRepository = orderRepository;
            _recommendationService = recommendationService;
            _httpContextBase = httpContextBase;

            Name = "AddToCartPayload";

            Field<CartType>("cart");
        }

        public override object MutateAndGetPayload(MutationInputs inputs, ResolveFieldContext<object> context)
        {
            var cart = _cartService.LoadOrCreateCart(_cartService.DefaultCartName);

            AddToCartResult result = _cartService.AddToCart(cart, inputs.Get<string>("code"), inputs.Get<decimal>("quantity", 1m));
            if (!result.EntriesAddedToCart)
            {
                context.Errors.AddRange(result.ValidationMessages.Select(m => new ExecutionError(m)));
                return null;
            }

            _orderRepository.Save(cart);
            _recommendationService.TrackCartAsync(_httpContextBase.Invoke()).Wait();

            LargeCartViewModel largeCartViewModel = _cartViewModelFactory.CreateLargeCartViewModel(cart);

            return new {Cart = largeCartViewModel}; 
        }
    }
}