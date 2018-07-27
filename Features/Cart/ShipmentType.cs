using EPiServer.Reference.Commerce.Site.Features.Cart.ViewModels;
using GraphQL.Types;

namespace EPiServer.Reference.Commerce.Site.Features.Cart
{
    public class ShipmentType : ObjectGraphType<ShipmentViewModel>
    {
        public ShipmentType()
        {
            Name = "Shipment";
            Field(x => x.ShipmentId);
            Field(x => x.ShippingMethodId, type: typeof(IdGraphType));
            Field(x => x.CartItems, type: typeof(ListGraphType<CartItemType>));
        }
    }
}