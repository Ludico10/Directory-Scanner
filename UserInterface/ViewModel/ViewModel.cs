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
using UserInterface.Model;

namespace UserInterface.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string? path = null;
        private DirScanner? scanner;
        private int threadCnt = 5;
        private bool inWork = false;
        private ModelNode? root;

        public Command ChooseCommand { get; }
        public Command SearchCommand { get; }
        public Command StopCommand { get;  }

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

        public bool NotInWork { get { return !inWork; } set { inWork = !value; OnPropertyChanged("NotInWork"); } }
        public ModelNode? Root { get { return root; } set { root = value; OnPropertyChanged("Root"); } }
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
                    root = ModelTree.TreeConvert(scanner.Scan(path));
                    inWork = false;
                }
            }));

            StopCommand = new Command(obj =>
            {
                if (inWork && scanner != null)
                {
                    scanner.Stop();
                    inWork = false;
                }
            });
        }
    }
}
