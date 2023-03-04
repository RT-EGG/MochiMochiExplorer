namespace Utility
{
    public static class CollectionDisposement
    {
        public static void DisposeItems<T>(this IEnumerable<T> inItems) where T : IDisposable
        {
            foreach (var item in inItems)
                item?.Dispose();
        }
    }
}
