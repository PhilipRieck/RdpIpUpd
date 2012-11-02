using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Autofac;
using RdpIpUpd.Config;

namespace RdpIpUpd
{
    internal class AppBootstrap : Bootstrapper
    {
        private IContainer _container;
        private NotifyViewModel _shell;

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            var modelFactory = _container.Resolve<Func<NotifyViewModel>>();
            var manager = _container.Resolve<IWindowManager>();

            _shell = modelFactory();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            _shell.Stop();
            base.OnExit(sender, e);
        }
        
        protected override void Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
                .Where(t => t.Name.EndsWith("ViewModel"))
                .Where(t => t.GetInterface("INotifyPropertyChanged", false) != null)
                .AsSelf()
                .InstancePerDependency();


            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
                .Where(t => t.Name.EndsWith("View"))
                .AsSelf()
                .InstancePerDependency();

            builder.RegisterType<NotifyWindowManager>()
                .As<IWindowManager>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EventAggregator>()
                .As<IEventAggregator>()
                .InstancePerLifetimeScope();

            //auto sub to event aggregators?

            //More?
            builder.Register(c=> Settings.GetSettings())
                .As<Settings>()
                .InstancePerLifetimeScope();

            builder.RegisterType<NetworkListener>()
                .AsSelf()
                .InstancePerDependency();

            builder.RegisterType<IpHandler>()
                .AsSelf()
                .InstancePerDependency();

            _container = builder.Build();
        }

        protected override object GetInstance(Type service, string key)
        {
            object instance;
            if (string.IsNullOrWhiteSpace(key))
            {
                if (_container.TryResolve(service, out instance))
                {
                    return instance;
                }
            }
            if (_container.TryResolveNamed(key, service, out instance))
            {
                return instance;
            }
            throw new NotImplementedException(string.Format("Service not implemented: {0}", key ?? service.Name));
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.Resolve(typeof (IEnumerable<>).MakeGenericType(service)) as IEnumerable<object>;
        }

        protected override void BuildUp(object instance)
        {
            _container.InjectUnsetProperties(instance);
        }
    }
}
