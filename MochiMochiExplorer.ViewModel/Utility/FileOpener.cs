using MochiMochiExplorer.Model;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MochiMochiExplorer.ViewModel.Wpf.Utility
{
    internal class FileOpener
    {
        private FileOpener()
        { }

        public static FileOpener Instance { get; } = new FileOpener();

        public void OpenFile(string inFilepath)
        {
            Process ps = new Process();
            ps.StartInfo.UseShellExecute = true;

            var extension = Path.GetExtension(inFilepath);
            if (extension.StartsWith('.'))
                extension = extension.Substring(1);

            var opener = FileOpenOption.Instance.FileOpenPrograms.FirstOrDefault(p => p.TargetExtension.Value == extension);
            if (opener is null)
            {
                ps.StartInfo.FileName = inFilepath;
            }
            else
            {
                ps.StartInfo.FileName = opener.ProgramFilepath.Value;
                ps.StartInfo.Arguments = $"\"{inFilepath}\"";
            }            

            ps.Start();
        }
    }
}
