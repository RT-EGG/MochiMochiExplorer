using Reactive.Bindings;

namespace MochiMochiExplorer.Model
{
    public class FileOpenProgram
    {
        public FileOpenProgram(string inTargetExtension, string inProgramFilepath)
        {
            TargetExtension.Value = inTargetExtension;
            ProgramFilepath.Value = inProgramFilepath;
        }

        public ReactiveProperty<string> TargetExtension { get; } = new ReactiveProperty<string>(string.Empty);
        public ReactiveProperty<string> ProgramFilepath { get; } = new ReactiveProperty<string>(string.Empty);
    }

    public class FileOpenOption
    {
        public static FileOpenOption Instance { get; } = new FileOpenOption();

        public Utility.ReactiveCollection<FileOpenProgram> FileOpenPrograms { get; } = new Utility.ReactiveCollection<FileOpenProgram>();
    }
}
