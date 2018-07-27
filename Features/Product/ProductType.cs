using EPiServer.Reference.Commerce.Site.Features.Product.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Shared;
using GraphQL.Types;

namespace EPiServer.Reference.Commerce.Site.Features.Product
{
    public class ProductType : ObjectGraphType<ProductTileViewModel>
    {
        public ProductType()
        {
            Name = "Product";

            Field(x => x.Brand);
            Field(x => x.Code);
            Field(x => x.IsAvailable);
            Field(x => x.Url);
            Field(x => x.PlacedPrice, type: typeof(MoneyType));
            Field(x => x.DiscountedPrice, type: typeof(MoneyType));
        }
    }
}