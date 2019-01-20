using Autofac;
using Autofac.Integration.WebApi;
using RestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace RestApi.DependencyInjection
{
    public class Dependency
    {
        public static IContainer SetDependencies(bool useLocal = default(bool))
        {
            //var diContainer = new ApplicationDependencyContainer(Assembly.GetExecutingAssembly(), context);
            //diContainer.Build();

            //// Set Dependency Resolver
            //GlobalConfiguration.Configuration.DependencyResolver = diContainer.WebApiDependencyResolver;
            //return diContainer.Container;


            var builder = new ContainerBuilder();
            // Register Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            // Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);

            // register types
            if (useLocal)
            {
                builder.RegisterType<InMemoryPatientContext>().AsImplementedInterfaces();
            }
            else
            {
                builder.RegisterType<PatientContext>().AsImplementedInterfaces();
            }

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            return container;
        }
    }
}