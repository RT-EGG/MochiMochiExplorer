using MochiMochiExplorer.Model.Json;
using Newtonsoft.Json;
using Reactive.Bindings;
using System.Reactive.Concurrency;

namespace MochiMochiExplorer.Model
{
    public class FileInformationList : Utility.ReactiveCollection<FileInformation>
    {
        public FileInformationList(string inName = "")
        { 
            Name.Value = inName;
        }

        public ReactiveProperty<string> Name { get; } = new ReactiveProperty<string>(string.Empty);
    }
}
