using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace Szakdolgozat.Views
{
    /// <summary>
    /// Interaction logic for BookSidebarView.xaml
    /// </summary>
    public partial class BookSidebarView : Page
    {
        public BookSidebarView()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            e.Handled = true;
            
            string uri = (sender as Hyperlink).NavigateUri.AbsoluteUri;

            var sInfo = new System.Diagnostics.ProcessStartInfo(uri)
            {
                UseShellExecute = true,
            };
            System.Diagnostics.Process.Start(sInfo);
        }
    }
}
