using GraphQL.Relay.Types;
using GraphQL.Types;

namespace EPiServer.Reference.Commerce.Site.Features.Cart.Mutations
{
    public class AddToCartInput : MutationInputGraphType
    {
        public AddToCartInput()
        {
            Name = "AddToCartInput";
            Field<StringGraphType>("code");
            Field<DecimalGraphType>("quantity");
        }
    }
}