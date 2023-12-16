using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Szakdolgozat.Models;
using Szakdolgozat.Models.Enum;
using Szakdolgozat.Services;
using Szakdolgozat.Services.BookExtractorStrategies;
using Szakdolgozat.Services.DatabaseServices;
using Szakdolgozat.Services.EventManager;
using Szakdolgozat.Services.Interfaces;
using Szakdolgozat.Services.Strategies;
using Szakdolgozat.ViewModels.ControlViewModels;

namespace Szakdolgozat.ViewModels
{
    class BookBrowserViewModel : ObservableObject, IDragAndDropTarget
    {
        private DbAdapter _adapter;

        public ObservableCollection<BookControlViewModel> Books { get; set; }

        public ICommand DragCommand;

        public BookBrowserViewModel()
        {
            _adapter = DbProvider.GetInstance();

            Books = new ObservableCollection<BookControlViewModel>();

            SetBookControls();
            Initialize();
        }

        public void Initialize()
        {
            _adapter.GetContext().SavedChanges += OnBookChanged;

            WeakEventManager<Events, StringEventArgs>.AddHandler(
                source: Events.Instance,
                eventName: nameof(Events.SearchBarChanged),
                OnSearchBarChanged);
        }

        public void OnFileDrop(string[] paths)
        {
            if (!paths.Any())
                return;

            foreach (var path in paths)
            {
                string ext = path.Split('.').Last();

                var strategy = ExtractorContext.GetStrategy(ext.GetFormatEnum());

                strategy.ExtractingAndSaving(path);
            }
        }

        private void OnSearchBarChanged(object? sender, StringEventArgs args)
        {
            SetBookControls();

            string searchText = args.Text.ToLower();

            var orderedBooks = Books.Where(x => x.Title.ToLower().Contains(searchText))
                .OrderBy(x => x.Title.ToLower().StartsWith(searchText))
                .Select(x => x.Book)
                .ToList();

            RefreshBookControls(orderedBooks);
        }

        private void OnBookChanged(object? sender, SavedChangesEventArgs? e)
        {
            SetBookControls();
        }

        private void SetBookControls()
        {
            Books.Clear();

            foreach (var book in _adapter.GetBooks())
            {
                SetBookControl(book);
            }
        }

        private void RefreshBookControls(IEnumerable<Book> orderedBooks)
        {
            Books.Clear();

            foreach (var book in orderedBooks)
            {
                SetBookControl(book);
            }
        }

        private void SetBookControl(Book book)
        {
            if (book == null)
                return;

            Books.Add(new BookControlViewModel(book)
            {
                IsButtonEnabled = true,
                Title = book.Title != string.Empty ? book.Title : "<Title>",
                Author = book.Author,
                CoverSource = book.CoverId
            });
        }
    }
}
