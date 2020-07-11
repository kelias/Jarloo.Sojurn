using System;
using System.Collections.Generic;
using System.Linq;
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
        public static T GetFirstViewModelByType<T>() where T : ViewModel
        {
            return ViewModels.Where(vm => typeof(T) == vm.GetType()).Cast<T>().FirstOrDefault();
        }
    }
}