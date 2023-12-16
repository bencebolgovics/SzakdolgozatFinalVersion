using GalaSoft.MvvmLight.Command;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Szakdolgozat.FormatServices.Epub.Format;
using Szakdolgozat.Models;
using Szakdolgozat.Models.Constants;
using Szakdolgozat.Services.EventManager;
using Szakdolgozat.Services.Extensions;
using Szakdolgozat.Services.RecommendationService;
using Szakdolgozat.Services.Scraping;
using Szakdolgozat.ViewModels.ControlViewModels;
using Windows.Globalization.DateTimeFormatting;

namespace Szakdolgozat.ViewModels
{
    public class BookSidebarViewModel : ViewModelBase
    {
        const string COMBOBOX_DEFAULT = "Atleast 3 character to search";

        private SearchResultDTO _selectedItem;
        private readonly BookJsonExtensions bookJsonExtensions;
        private MLContext _context;

        public ObservableCollection<BookControlViewModel> LastReadBook { get; set; }
        public ObservableCollection<SearchResultDTO> SearchResults { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand OpenBrowserCommand { get; set; }
        public string SearchText { get; set; }
        public SearchResultDTO SelectedItem {
            get => _selectedItem;
            set 
            {
                SetProperty(ref _selectedItem, value);
                OnSelectedBook(_selectedItem);
            } 
        }

        private string _recommendedBookOne;
        public string RecommendedBookOne
        {
            get => _recommendedBookOne;
            set => SetProperty(ref _recommendedBookOne, value);
        }

        private string _recommendedBookTwo;
        public string RecommendedBookTwo
        {
            get => _recommendedBookTwo;
            set => SetProperty(ref _recommendedBookTwo, value);

        }

        private string _recommendedBookThree;
        public string RecommendedBookThree
        {
            get => _recommendedBookThree;
            set => SetProperty(ref _recommendedBookThree, value);

        }

        private string _bookOneUri;
        public string BookOneUri
        {
            get => _bookOneUri;
            set => SetProperty(ref _bookOneUri, value);
        }

        private string _bookTwoUri;
        public string BookTwoUri
        {
            get => _bookTwoUri;
            set => SetProperty(ref _bookTwoUri, value);
        }

        private string _bookThreeUri;
        public string BookThreeUri
        {
            get => _bookThreeUri;
            set => SetProperty(ref _bookThreeUri, value);
        }

        public BookSidebarViewModel()
        {
            SearchText = COMBOBOX_DEFAULT;

            bookJsonExtensions = new BookJsonExtensions();

            LastReadBook = new ObservableCollection<BookControlViewModel>();
            SearchResults = new ObservableCollection<SearchResultDTO>();
            FilterCommand = new RelayCommand(OnFilterChange);
            OpenBrowserCommand = new RelayCommand<string>(OnLinkClick);

            _context = new MLContext();

            //update last read book from json
            UpdateLastReadBook();
            Initialize();
        }

        private void OnLinkClick(string uri)
        {
        }

        private async void Initialize()
        {
            WeakEventManager<Events, EventArgs>.AddHandler(
                source: Events.Instance,
                eventName: nameof(Events.JsonChanged),
                HandleJsonChange);

            await Recommendation();
        }

        private async Task Recommendation()
        {
            GutenbergEbookMetadataProvider provider = new();

            var trendingBooks = await provider.GetTop30Books();

            var recommendedBooks = GetRecommendedBooks(trendingBooks);

            SetRecommendedBooks(recommendedBooks, provider);
        }

        private async void SetRecommendedBooks(List<float> recommendedBooks, GutenbergEbookMetadataProvider provider)
        {
            List<BookMetadata> bookMetadatas = new();

            foreach (var book in recommendedBooks)
            {
                bookMetadatas.Add(await provider.GetBookMetadata(book.ToString()));
            }

            RecommendedBookOne = bookMetadatas[0].Title;
            RecommendedBookTwo = bookMetadatas[1].Title;
            RecommendedBookThree = bookMetadatas[2].Title;

            SetLinks(bookMetadatas);
        }

        private void SetLinks(List<BookMetadata> bookMetadatas)
        {
            var gutenbergUriBase = "https://gutenberg.org/ebooks/";

            BookOneUri = gutenbergUriBase + bookMetadatas[0].BookCode.ToString();
            BookTwoUri = gutenbergUriBase + bookMetadatas[1].BookCode.ToString();
            BookThreeUri = gutenbergUriBase + bookMetadatas[2].BookCode.ToString();
        }

        private List<float> GetRecommendedBooks(List<int> bookIds)
        {
            var data = BookRecommendation.LoadModel(_context);
            var recommendedBooks = BookRecommendation.UseModelForRanking(_context, data, bookIds);

            return recommendedBooks;
        }

        #region SEARCHING
        private void OnFilterChange()
        {
            if (SearchText == null || SearchText.Length < 3)
                return;

            List<SearchResultDTO> searchResults = SearchResultProvider.GetSearchResult(SearchText).ToList();

            SetSearchResult(searchResults);
        }

        private void SetSearchResult(List<SearchResultDTO> searchResults)
        {
            SearchResults.Clear();

            foreach(SearchResultDTO result in searchResults)
            {
                SearchResults.Add(result);
            }
        }
        private void OnSelectedBook(SearchResultDTO result)
        {
            var selectedItem = SelectedItem;

            string popupText = "Would you like to download " + result.Title + " from " + result.Author;

            MessageBoxResult dialogResult = MessageBox.Show(popupText, "Download", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (dialogResult == MessageBoxResult.Yes)
            {
                GutenbergDownloader gutenbergDownloader = new(result);
                gutenbergDownloader.ImportBook();
            }
        }

        #endregion SEARCHING

        #region UPDATEJSON
        private void HandleJsonChange(object? sender, EventArgs args)
        {
            UpdateLastReadBook();
        }

        private void UpdateLastReadBook()
        {
            Book? book = bookJsonExtensions.GetLastReadBookFromJson();

            LastReadBook.Clear();

            if (book.BookCode == -1)
            {
                LastReadBook.Add(new BookControlViewModel(new Book())
                {
                    IsButtonEnabled = false,
                    Title = string.Empty,
                    Author = string.Empty,
                    CoverSource = Models.Constants.Constants.NO_IMAGE_PATH
                });
            }
            else
            {
                LastReadBook.Add(new BookControlViewModel(book)
                {
                    IsButtonEnabled = true,
                    Title = book.Title != string.Empty ? book.Title : "<Title>",
                    Author = book.Author,
                    CoverSource = book.CoverId
                });
            }
        }

        #endregion UPDATEJSON
    }
}
