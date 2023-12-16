namespace Szakdolgozat.Services.TranslationServices
{
    public sealed class TranslatorProvider
    {
        private static object _myLock = new object();
        private static TranslatorAPI _apiSingleton = null;

        private TranslatorProvider() { }

        public static TranslatorAPI GetInstance()
        {
            if (_apiSingleton is null)
            {
                lock (_myLock)
                {
                    if (_apiSingleton is null)
                    {
                        _apiSingleton = new TranslatorAPI();
                    }
                }
            }

            return _apiSingleton;
        }
    }
}
