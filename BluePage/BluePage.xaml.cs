// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using Microsoft.UI.Xaml.Controls;
using System;
using System.IO;
using System.Reflection;

namespace BluePage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BluePage : Page
    {
        public BluePage()
        {
            this.InitializeComponentCustom();
        }

        public void InitializeComponentCustom()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;

            var codeBase = Assembly.GetExecutingAssembly().Location;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string resourcepath = $"ms-appx:///{Path.GetDirectoryName(path).Replace("\\", "/")}/{this.ToString().Replace(".", "/")}.xaml";
            var resourceLocator = new Uri(resourcepath);
            Microsoft.UI.Xaml.Application.LoadComponent(this, resourceLocator, Microsoft.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
        }

        public static string Title => "Blue Page";
        public static string Description => "This is the blue page";
        public static string Icon => "\xe13d";
    }
}
