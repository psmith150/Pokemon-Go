using Autofac;
using Pokemon_Go_Database.Base.AbstractClasses;
using Pokemon_Go_Database.Services;
using Pokemon_Go_Database.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Pokemon_Go_Database.IOC
{
    public static class IoCContainer
    {
        private static IContainer container;
        private static readonly Dictionary<Type, Type> screenToViewModelMapping = new Dictionary<Type, Type>();

        public static void Build()
        {
            if (container == null)
            {
                var builder = new ContainerBuilder();

                // Register components/services
                builder.RegisterType<SessionService>().SingleInstance();
                builder.RegisterType<NavigationService>().SingleInstance();
                builder.RegisterType<MessageViewer>().As<MessageViewerBase>().SingleInstance();

                // Register windows explicitly
                builder.RegisterType<MainWindow>().SingleInstance();

                var uiAssemblyTypes = Assembly.GetAssembly(typeof(App)).GetTypes();

                // Register view models
                builder.RegisterTypes(uiAssemblyTypes.Where(t => typeof(BaseViewModel).IsAssignableFrom(t) && !t.IsAbstract).ToArray())
                       .SingleInstance();

                // Register Screens
                builder.RegisterTypes(uiAssemblyTypes.Where(t => typeof(UserControl).IsAssignableFrom(t) && t.Namespace.EndsWith(".Screens")).ToArray())
                       .SingleInstance();

                // Register Popups
                builder.RegisterTypes(uiAssemblyTypes.Where(t => typeof(UserControl).IsAssignableFrom(t) && t.Namespace.EndsWith(".Popups")).ToArray());

                container = builder.Build();

                GenerateScreenToViewModelMappings();
            }
        }

        public static BaseViewModel ResolveViewModel(Type viewModelType)
        {
            return container.Resolve(viewModelType) as BaseViewModel;
        }

        public static BaseViewModel ResolveViewModel<TViewModel>() where TViewModel : BaseViewModel
        {
            return ResolveViewModel(typeof(TViewModel));
        }

        public static Window ResolveWindow(Type windowType)
        {
            return container.Resolve(windowType) as Window;
        }

        public static UserControl ResolveScreen(Type viewModelType)
        {
            var screenType = screenToViewModelMapping.FirstOrDefault(p => p.Value == viewModelType).Key;
            var screen = container.Resolve(screenType) as UserControl;
            return screen;
        }

        private static void GenerateScreenToViewModelMappings()
        {
            screenToViewModelMapping.Clear();

            // get all screen types
            var screenTypes = container.ComponentRegistry.Registrations.Where(r => typeof(UserControl).IsAssignableFrom(r.Activator.LimitType)).Select(r => r.Activator.LimitType);
            foreach (var screenType in screenTypes)
            {
                var screen = container.Resolve(screenType) as UserControl;
                var viewModel = screen?.DataContext;

                if (!(viewModel is BaseViewModel))
                {
                    throw new NotSupportedException("Screen data context should only be of type BaseViewModel");
                }

                screenToViewModelMapping.Add(screenType, viewModel.GetType());
            }
        }
    }
}
