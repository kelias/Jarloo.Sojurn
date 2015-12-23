using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using Jarloo.Sojurn.ViewModels;

namespace Jarloo.Sojurn
{
    public class SojurnBootstrapper : Bootstrapper<MainViewModel>
    {
        private CompositionContainer container;

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            //Prevent Multiple Copies
            var processName = Process.GetCurrentProcess().ProcessName;
            var processes = Process.GetProcesses();

            if (processes.Count(w => w.ProcessName == processName) > 1)
            {
                Application.Shutdown();
            }

            base.OnStartup(sender, e);
        }

        protected override void Configure()
        {
            container =
                new CompositionContainer(
                    new AggregateCatalog(
                        AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()));

            var batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new AppWindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(container);

            container.Compose(batch);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            var contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var exports = container.GetExportedValues<object>(contract);

            if (exports.Any())
            {
                return exports.First();
            }

            throw new Exception($"Could not locate any instances of contract {contract}.");
        }
    }
}