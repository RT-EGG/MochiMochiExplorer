namespace Utility
{
    public class DisposableCollection<T> : List<T>, IDisposable where T : IDisposable
    {
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                    this.DisposeItems();

                _isDisposed = true;
            }
        }

        private bool _isDisposed = false;
    }
}
