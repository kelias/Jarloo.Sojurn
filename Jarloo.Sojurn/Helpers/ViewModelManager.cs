using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Jarloo.Sojurn.ViewModels;

namespace Jarloo.Sojurn.Helpers
{
    public static class ViewModelManager
    {
        private static readonly List<ViewModel> ViewModels = new List<ViewModel>();
        
        public static void Register(ViewModel vm)
        {
            ViewModels.Add(vm);
        }

        public static void Deregister(ViewModel vm)
        {
            ViewModels.Remove(vm);
        }

        public static void DeregisterAll()
        {
            ViewModels.Clear();
        }

        public static T Create<T>() where T : ViewModel
        {
            var vm = Activator.CreateInstance<T>();

            return vm;
        }

        public static ViewModel Create(Type t)
        {
            var vm = (ViewModel) Activator.CreateInstance(t);
            return vm;
        }


        public static void CloseAllExcept(ViewModel vm)
        {
            //Don't close main window
            var vms = ViewModels.Where(w => w != vm).ToList();

            //Can't do foreach because each close call, calls Deregister and modifies our collection
            for (var i = vms.Count - 1; i >= 0; i--)
            {
                vms[i].Close();
            }
        }

        public static void CloseAllOfType(Type t)
        {
            var vms = ViewModels.Where(w => w.GetType() == t).ToList();
            for (var i = vms.Count - 1; i >= 0; i--)
            {
                vms[i].Close();
            }
        }
        
        public static T GetFirstViewModelByType<T>() where T : ViewModel
        {
            return ViewModels.Where(vm => typeof (T) == vm.GetType()).Cast<T>().FirstOrDefault();
        }

        public static List<T> GetAllViewModelsByType<T>() where T : ViewModel
        {
            return ViewModels.Where(vm => typeof (T) == vm.GetType()).Cast<T>().ToList();
        }
        
        public static void OpenViewModelExclusive<T>() where T : ViewModel
        {
            var vm = ViewModels.FirstOrDefault(w => w.GetType() == typeof (T));

            if (vm == null)
            {
                Create<T>().Show();
                return;
            }

            vm.View.WindowState = WindowState.Normal;
            vm.View.Activate();
        }
    }
}