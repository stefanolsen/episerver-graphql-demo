using System;
using System.Web.Http;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using GraphQL;
using GraphQL.Execution;
using GraphQL.Types;
using GraphQL.Validation;
using GraphQL.Validation.Complexity;

namespace EPiServer.Reference.Commerce.Site.Infrastructure.GraphQL
{
    [InitializableModule]
    [ModuleDependency(typeof(ServiceContainerInitialization))]
    public class GraphQLInitialization : IConfigurableModule
    {
        public void Initialize(InitializationEngine context)
        {
            
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            var services = context.Services;

            services.AddSingleton<ServiceAccessor<HttpContextBase>>(locator => locator.GetInstance<HttpContextBase>);

            services.AddSingleton<IComplexityAnalyzer, ComplexityAnalyzer>();
            services.AddSingleton<IDocumentBuilder, GraphQLDocumentBuilder>();
            services.AddSingleton<IDocumentValidator, DocumentValidator>();
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddTransient<Func<Type, IGraphType>>(c =>
            {
                return t => c.GetInstance(t) as IGraphType;
            });

            services.AddSingleton<ISchema, InjectedSchema>();
        }
    }
}