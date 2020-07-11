using System;
using System.Windows.Threading;
using Jarloo.Sojurn.Helpers;
using Jarloo.Sojurn.ViewModels;

namespace Jarloo.Sojurn
{
    public static class ErrorManager
    {
        public static Dispatcher Dispatcher { get; set; }

        public static void Log(Exception ex)
        {
            Log(ex.ToString());
        }

        public static void Log(string msg)
        {
            try
            {
                if (Dispatcher == null) return;

                Dispatcher.InvokeAsync(() =>
                {
                    try
                    {
                        var vm = ViewModelManager.GetFirstViewModelByType<ErrorViewModel>();

                        if (vm == null)
                        {
                            vm = ViewModelManager.Create<ErrorViewModel>();
                            vm.Show();
                        }

                        vm.AddEntry(msg);
                    }
                    catch
                    {
                        //supress
                    }
                }, DispatcherPriority.Background);
            }
            catch
            {
                //supress
            }
        }
    }
}