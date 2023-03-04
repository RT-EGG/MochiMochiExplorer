using Reactive.Bindings;

namespace MochiMochiExplorer.Model
{
    public class FileInformation
    {
        public FileInformation(string inFilepath)
        {
            Filepath = inFilepath;
        }        

        public readonly string Filepath;
        public ReactiveProperty<DateTime> LastAccessTime { get; } = new ReactiveProperty<DateTime>();
    }
}
