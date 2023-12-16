using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Szakdolgozat.Models;
using Szakdolgozat.Models.Enum;
using Szakdolgozat.Services.BookExtractorStrategies;
using Szakdolgozat.Services.EventManager;

namespace Szakdolgozat.ViewModels
{
    public class ToolbarViewModel : ViewModelBase
    {
        private string _searchText;

        public ICommand AddBookCommand { get; }
        public ICommand SearchCommand { get; }
        public string SearchText
        {
            get => _searchText;
            set => SetProperty<string>(ref _searchText, value);
        }

        public ToolbarViewModel()
        {
            SearchText = string.Empty;

            AddBookCommand = new RelayCommand(AddBook);
            SearchCommand = new RelayCommand(Search);
        }

        private void Search()
        {
            Events.RaiseSearchBarChanged(new StringEventArgs(SearchText));
        }

        private void AddBook()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == false)
                return;

            string filePath = openFileDialog.FileName;
            string ext = filePath.Split('.').LastOrDefault() ?? string.Empty;

            var strategy = ExtractorContext.GetStrategy(ext.GetFormatEnum());

            strategy.ExtractingAndSaving(filePath);
        }
    }
}
