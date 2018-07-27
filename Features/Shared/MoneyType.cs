using GraphQL.Types;
using Mediachase.Commerce;

namespace EPiServer.Reference.Commerce.Site.Features.Shared
{
    public class MoneyType : ObjectGraphType<Money>
    {
        public MoneyType()
        {
            Name = "Money";

            Field(nameof(Money.Amount), x => x.Amount).Description("The amount.");
            Field(nameof(Currency.CurrencyCode), x => x.Currency.CurrencyCode).Description("The currency.");
            Field("formattedAmount", x => x.ToString()).Description("The currency.");
        }
    }
}