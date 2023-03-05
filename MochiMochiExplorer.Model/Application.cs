using Reactive.Bindings;

namespace MochiMochiExplorer.Model
{
    public class Application
    {
        public ReactiveProperty<FileInformationList> FileInformationList 
            { get; } = new ReactiveProperty<FileInformationList>(new Model.FileInformationList("List"));
    }
}
