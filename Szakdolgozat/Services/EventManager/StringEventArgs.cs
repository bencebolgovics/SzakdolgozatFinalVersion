using System;

namespace Szakdolgozat.Services.EventManager
{
    public class StringEventArgs : EventArgs
    {
        private readonly string text;

        public StringEventArgs(string text)
        {
            this.text = text;
        }

        public string Text
        {
            get { return text; }
        }
    }
}
