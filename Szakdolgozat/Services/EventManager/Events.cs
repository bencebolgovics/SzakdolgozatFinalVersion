using System;

namespace Szakdolgozat.Services.EventManager
{
    public sealed class Events
    {
        public static Events Instance { get; } = new Events();

        public event EventHandler<EventArgs>? JsonChanged;
        public event EventHandler<StringEventArgs>? SearchBarChanged;

        private Events() { }

        public static void RaiseJsonChanged(object? sender = null)
            => Instance.JsonChanged?.Invoke(sender ?? Instance, EventArgs.Empty);

        public static void RaiseSearchBarChanged(StringEventArgs args, object? sender = null)
            => Instance.SearchBarChanged?.Invoke(sender ?? Instance, args);
    }
}
