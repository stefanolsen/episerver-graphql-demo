using EPiServer.Reference.Commerce.Site.Features.Cart.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Shared;
using GraphQL.Types;

namespace EPiServer.Reference.Commerce.Site.Features.Cart
{
    public class CartType : ObjectGraphType<LargeCartViewModel>
    {
        public CartType()
        {
            Name = "Cart";
            Field(x => x.Total, type: typeof(MoneyType)).Description("The total amount.");
            Field(x => x.TotalDiscount, type: typeof(MoneyType)).Description("The total amount of discount.");
            Field(x => x.Shipments, type: typeof(ListGraphType<ShipmentType>));
        }
    }
}