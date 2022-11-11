using DirectoryScanner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace UserInterface.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string? path;
        private DirScanner scanner;
        private int threadCnt = 5;
        private bool inWork = false;
        private DirectoryScanner.TreeNode root;

        private Command ChooseCommand;
        private Command SearchCommand;
        private Command StopCommand;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? property = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public string? Path { get { return path; } set { path = value; OnPropertyChanged("Path"); } }
        public bool InWork { get { return inWork; } set { inWork = value; OnPropertyChanged("InWork");} }
        public DirectoryScanner.TreeNode Root { get { return root; } set { root = value; OnPropertyChanged("Root"); } }
        public int ThreadCnt { get { return threadCnt; } set { threadCnt = value; OnPropertyChanged("ThreadCnt"); } }

        public ViewModel()
        {
            ChooseCommand = new Command(obj =>
            {
                FolderBrowserDialog folderView = new FolderBrowserDialog();
                if (folderView.ShowDialog() == DialogResult.OK)
                    path = folderView.SelectedPath;
            });

            SearchCommand = new Command(obj => Task.Run(() =>
            {
                scanner = new DirScanner(threadCnt);
                if (path != null)
                {
                    inWork = true;
                    root = scanner.Scan(path);
                    inWork = false;
                }
            }));

            StopCommand = new Command(obj =>
            {
                if (inWork && scanner != null)
                {
                    root = scanner.Stop();
                    inWork = false;
                }
            });
        }
    }
}
