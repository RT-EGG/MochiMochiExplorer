using MochiMochiExplorer.ViewModel.Wpf.ApplicationViewModel;
using MochiMochiExplorer.ViewModel.Wpf.FileInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MochiMochiExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            TargetApplication.InitializeApplication(this);
            ViewModel.Wpf.TargetApplicationBinder.InitializeForApplication(TargetApplication.Instance!);

            InitializeComponent();

            this.Loaded += (_, _) => TargetApplication.Instance.AddFrameworkElement(this);
            this.Unloaded += (_, _) =>
            {
                TargetApplication.Instance.RemoveFrameworkElement(this);
                if (DataContext is IDisposable d)
                    d.Dispose();
            };

            this.ContentRendered += MainWindow_ContentRendered;
        }

        private void MainWindow_ContentRendered(object? sender, EventArgs e)
        {
            var model = new Model.Application();
            var viewmodel = DataContext as ApplicationViewModel;
            viewmodel?.BindModel(model);

            this.ContentRendered -= MainWindow_ContentRendered;
        }
    }
}
