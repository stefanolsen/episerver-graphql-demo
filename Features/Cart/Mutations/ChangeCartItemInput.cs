using GraphQL.Relay.Types;
using GraphQL.Types;

namespace EPiServer.Reference.Commerce.Site.Features.Cart.Mutations
{
    public class ChangeCartItemInput : MutationInputGraphType
    {
        public ChangeCartItemInput()
        {
            Name = "ChangeCartItemInput";

            Field<IntGraphType>("shipmentId");
            Field<StringGraphType>("code");
            Field<DecimalGraphType>("quantity");
            Field<StringGraphType>("size");
            Field<StringGraphType>("newSize");
            Field<StringGraphType>("displayName");
        }
    }
}