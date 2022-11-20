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

        public RelayCommand ChooseCommand { get; }
        public RelayCommand SearchCommand { get; }
        public RelayCommand StopCommand { get;  }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? property = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public string? Path { get { return path; } set { path = value; OnPropertyChanged(nameof(Path)); } }
        public bool InWork { get { return inWork; } set { inWork = value; OnPropertyChanged(nameof(InWork));} }
        public bool NotInWork { get { return !inWork; } set { inWork = !value; OnPropertyChanged(nameof(NotInWork)); } }
        public ModelNode? Root { get { return root; } set { root = value; OnPropertyChanged(nameof(Root)); } }
        public int ThreadCnt { get { return threadCnt; } set { threadCnt = value; OnPropertyChanged(nameof(ThreadCnt)); } }

        public ViewModel()
        {
            ChooseCommand = new RelayCommand(obj =>
            {
                FolderBrowserDialog folderView = new FolderBrowserDialog();
                if (folderView.ShowDialog() == DialogResult.OK)
                    Path = folderView.SelectedPath;
            });

            SearchCommand = new RelayCommand(obj => Task.Run(() =>
            {
                scanner = new DirScanner(threadCnt);
                if (Path != null)
                {
                    InWork = true;
                    Root = ModelNode.TreeConvert(scanner.Scan(Path));
                    InWork = false;
                }
            }));

            StopCommand = new RelayCommand(obj =>
            {
                if (inWork && scanner != null)
                {
                    scanner.Stop();
                    InWork = false;
                }
            });
        }
    }
}
