using System.Windows;
using System.Windows.Controls;
using Szakdolgozat.Services.TranslationServices;

namespace Szakdolgozat.Controls
{
    /// <summary>
    /// Need to implement this in here because the contextmenu is not in the visual tree
    /// later gonna change to the solution with the BindingProxy
    /// see: https://thomaslevesque.com/2011/03/21/wpf-how-to-bind-to-data-when-the-datacontext-is-not-inherited/
    /// </summary>
    public partial class PageControl : UserControl
    {
        public PageControl()
        {
            InitializeComponent();
        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var translator = TranslatorProvider.GetInstance();
            var obj = (MenuItem)sender;
            var translated = await translator.TranslateAndSaveToDictionary(selection.SelectedText);

            translatedText.Header = translated;
            translatedText.Visibility = Visibility.Visible;
        }

        private void menuItem_LostFocus(object sender, RoutedEventArgs e)
        {
            translatedText.Header = "";
            translatedText.Visibility = Visibility.Collapsed;
        }
    }
}
