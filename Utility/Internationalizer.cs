namespace Utility
{
    public interface IInternationalizer
    {
        string Internationalize(string inValue);
    }

    public class Internationalizer : IInternationalizer
    {
        private Internationalizer()
        { }

        public static void CreateInstance()
        {
            Instance = new Internationalizer();
        }
        public static Internationalizer? Instance { get; private set; } = null;

        public string Internationalize(string inValue) => inValue;
    }
}
