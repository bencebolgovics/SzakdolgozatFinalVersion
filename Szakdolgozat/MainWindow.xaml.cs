using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.ML;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Szakdolgozat.Services.DatabaseServices;
using Szakdolgozat.Services.RecommendationService;
using Szakdolgozat.Services.Scraping;
using Unity;

namespace Szakdolgozat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
