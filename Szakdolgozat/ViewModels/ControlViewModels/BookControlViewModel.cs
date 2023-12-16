using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Szakdolgozat.Models;
using Szakdolgozat.Services;
using Szakdolgozat.Services.DatabaseServices;
using Szakdolgozat.Services.Extensions;
using Szakdolgozat.Views;
using Microsoft.Win32;
using System.Linq;
using Szakdolgozat.Models.Enum;
using Szakdolgozat.Services.ConverterService;

namespace Szakdolgozat.ViewModels.ControlViewModels
{

    public partial class BookControlViewModel : UserControl
    {

        private DbAdapter _adapter;
        private BookJsonExtensions _jsonExt = new();

        public Book Book;
        public ICommand ButtonClickCommand { get; }
        public ICommand DeleteBookCommand { get; }
        public ICommand ConvertCommand { get; }

        public bool IsButtonEnabled { get; set; }

        public BookControlViewModel(Book book)
        {
            Book = book;

            InitializeComponent();
        }
        
        public BookControlViewModel() 
        {
            _adapter = DbProvider.GetInstance();

            ButtonClickCommand = new RelayCommand<BookControlViewModel>(OnClick);
            DeleteBookCommand = new RelayCommand<BookControlViewModel>(OnDelete);
            ConvertCommand = new RelayCommand<BookControlViewModel>(OnConvert);
        }

        private void OnConvert(BookControlViewModel vm)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = GetFilter(vm.Book);
            saveFileDialog.DefaultExt = GetDefaultExtension(vm.Book);
            saveFileDialog.AddExtension = true;

            if (saveFileDialog.ShowDialog() == true)
                vm.Book.Convert(saveFileDialog.FileName, GetDefaultExtension(vm.Book));

        }

        private string GetDefaultExtension(Book book)
        {
            var format = GetFormat(book);
            var saveFormat = format.GetFormatEnum() == Format.Epub ? "mobi" : "epub";

            return saveFormat;
        }

        private string GetFilter(Book book)
        {
            var format = GetFormat(book);

            string mobiFormat = "Mobi files (*.mobi)|.mobi";
            string epubFormat = "Epub files (*.epub)|.epub";

            var filter = format.GetFormatEnum() == Format.Epub ? mobiFormat : epubFormat;

            return filter;
        }

        private string GetFormat(Book book)
        {
            return book.FilePath.Split(".").Last();
        }

        private async void OnDelete(BookControlViewModel vm)
        {
            _jsonExt.DeleteLastReadBook(vm.Book.BookCode);

            await _adapter.DeleteBook(vm.Book.BookCode);
        }

        private void OnClick(BookControlViewModel vm)
        {
            _jsonExt.SerializeBookToJson(vm.Book);
            BookReaderView win2 = new BookReaderView(vm.Book);
            win2.Show();
        }

        public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(
            name: "Title",
            propertyType: typeof(string),
            ownerType: typeof(BookControlViewModel),
            typeMetadata: new FrameworkPropertyMetadata(defaultValue: ""));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty AuthorProperty =
        DependencyProperty.Register(
            name: "Author",
            propertyType: typeof(string),
            ownerType: typeof(BookControlViewModel),
            typeMetadata: new FrameworkPropertyMetadata(defaultValue: ""));

        public string Author
        {
            get => (string)GetValue(AuthorProperty);
            set => SetValue(AuthorProperty, value);
        }

        public string CoverId
        {
            get => (string)GetValue(CoverIdProperty);
            set => SetValue(CoverIdProperty, value);
        }

        public static readonly DependencyProperty CoverIdProperty =
        DependencyProperty.Register(
            name: "CoverId",
            propertyType: typeof(string),
            ownerType: typeof(BookControlViewModel),
            typeMetadata: new FrameworkPropertyMetadata(defaultValue: ""));

        public string CoverSource
        {
            get => (string)GetValue(CoverSourceProperty);
            set => SetValue(CoverSourceProperty, value);
        }

        public static readonly DependencyProperty CoverSourceProperty =
        DependencyProperty.Register(
            name: "CoverSource",
            propertyType: typeof(string),
            ownerType: typeof(BookControlViewModel),
            typeMetadata: new FrameworkPropertyMetadata(defaultValue: ""));

    }
}
