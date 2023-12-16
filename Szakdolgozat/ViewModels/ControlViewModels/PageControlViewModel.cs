using Aspose.Words;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using System.Windows.Input;
using Szakdolgozat.Models;
using Szakdolgozat.Models.Constants;
using Szakdolgozat.Models.Enum;
using Szakdolgozat.Services.FormatServices.Epub;

namespace Szakdolgozat.ViewModels.ControlViewModels
{
    public class PageControlViewModel : ViewModelBase
    {
        private readonly Book _book;
        private string _text;

        public ICommand TranslateCommand;

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public PageControlViewModel(Book book)
        {
            _book = book;
            string ext = book.FilePath.Split('.').LastOrDefault() ?? "";
            Text = GetText(_book, ext.GetFormatEnum());
            TranslateCommand = new RelayCommand(Translate);
        }

        public PageControlViewModel() 
        {
            TranslateCommand = new RelayCommand(Translate);
        }

        private void Translate()
        {
            
        }

        public string GetText(Book book, Format ext)
        {
            switch (ext)
            {
                case Format.Epub:
                    return GetTextForEpub(book);
                case Format.Mobi:
                    return GetTextForMobi(book);
                default:
                    return "";
            }
        }

        public string GetTextForEpub(Book book)
        {
            Epub epubBook = EpubReader.Read(book.FilePath);

            return epubBook.ToText();
        }

        public string GetTextForMobi(Book book)
        {
            Document mobiBook = new Document(book.FilePath);

            return mobiBook.GetText();
        }

    }
}
