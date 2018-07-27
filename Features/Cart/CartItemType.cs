using EPiServer.Reference.Commerce.Site.Features.Cart.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Shared;
using GraphQL.Types;

namespace EPiServer.Reference.Commerce.Site.Features.Cart
{
    public class CartItemType : ObjectGraphType<CartItemViewModel>
    {
        public CartItemType()
        {
            Name = "CartItem";

            Field(x => x.Brand);
            Field(x => x.Code);
            Field(x => x.DisplayName);
            Field(x => x.Url);
            Field(x => x.ImageUrl);

            Field(x => x.DiscountedPrice, nullable: true, type: typeof(MoneyType));
            Field(x => x.PlacedPrice, type: typeof(MoneyType));
            Field(x => x.Quantity);
        }
    }
}