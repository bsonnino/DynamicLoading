using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DynamicLoading
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            LoadDynamicPages();
        }

        private void LoadDynamicPages()
        {
            string dllFolder = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory +
                "..\\..\\..\\..\\..\\..\\..\\DynamicModules");
            if (!Directory.Exists(dllFolder))
            {
                Directory.CreateDirectory(dllFolder);
            }

            var dlls = Directory.GetFiles(dllFolder, "*.dll");

            foreach (var dll in dlls)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(dll);
                    var types = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Page)));

                    foreach (var type in types)
                    {
                        var title = type.GetProperty("Title")?.GetValue(null) as string ?? "";
                        var icon = type.GetProperty("Icon")?.GetValue(null) as string ?? "";
                        var description = type.GetProperty("Description")?.GetValue(null) as string ?? "";

                        var item = new NavigationViewItem
                        {
                            Content = title ?? type.Name,
                            Icon = new FontIcon() { Glyph = icon },
                            Tag = type,
                        };
                        ToolTipService.SetToolTip(item, description);
                        MainNav.MenuItems.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading {dll}: {ex.Message}");
                }
            }
            if (MainNav.MenuItems.Count > 0 && MainNav.MenuItems[0] is NavigationViewItem it && it.Tag is Type tp)
            {
                NavigateToPage(tp);
            }
        }

        private void NavigateToPage(Type type)
        {
            //MainFrame.Navigate(type);
            var page = Activator.CreateInstance(type) as Page;
            MainContent.Content = page;
        }

        private void MainNav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem navItem && navItem.Tag is Type type)
            {
                NavigateToPage(type);
            }
        }
    }
}
