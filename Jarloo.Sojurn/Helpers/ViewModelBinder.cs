using System;
using Jarloo.Sojurn.ViewModels;
using Jarloo.Sojurn.Views;

namespace Jarloo.Sojurn.Helpers
{
    public static class ViewModelBinder
    {
        public static View GetView(ViewModel vm)
        {
            var vmName = vm.GetType().FullName;
            var i = vmName.LastIndexOf("Model");
            var vName = vmName.Substring(0, i);
            
            if (vName.Contains("ViewModels")) vName = vName.Replace("ViewModels", "Views");

            var type = Type.GetType(vName);
            if (type == null) return null;

            var View = Activator.CreateInstance(type);
            return (View) View;
        }
    }
}