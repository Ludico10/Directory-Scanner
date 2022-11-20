using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace DirectoryScanner
{
    public class DirScanner
    {
        private int maxThreadCnt;
        private ConcurrentQueue<Task> queue;
        private SemaphoreSlim semaphore;
        private CancellationTokenSource cts;

        public DirScanner(int threadMax)
        {
            if (threadMax > 0)
            {
                maxThreadCnt = threadMax;
                queue = new ConcurrentQueue<Task>();
                semaphore = new SemaphoreSlim(maxThreadCnt, maxThreadCnt);
                cts = new CancellationTokenSource();
            }
            else throw new ArgumentOutOfRangeException($"Unable to create { threadMax } threds");
        }

        public TreeNode Scan(string source)
        {
            source = Path.GetFullPath(source);
            if (!Directory.Exists(source))
                throw new ArgumentException($"There is not directory { source }");

            TreeNode root = new TreeNode(true, source, 0);
            Task? scanTask = new Task(FileScaner!, root, cts.Token);
            queue.Enqueue(scanTask);
            while (semaphore.CurrentCount != maxThreadCnt || !queue.IsEmpty)
            {
                try
                {
                    if (queue.TryDequeue(out scanTask))
                    {
                        semaphore.Wait(cts.Token);
                        scanTask.Start();
                    }
                }
                catch { }
            }
            root.Resize();
            return root;
        }

        public void Stop()
        {
            cts.Cancel();
        }

        private void FileScaner(object context)
        {
            CancellationToken token = cts.Token;
            token.ThrowIfCancellationRequested();

            try
            {
                TreeNode parent = (TreeNode)context;
                DirectoryInfo rootDirInfo = new DirectoryInfo(parent.name);
                FileSystemInfo[] fileInfos = rootDirInfo.GetFileSystemInfos();
                if (parent.children != null)
                {
                    foreach (FileSystemInfo fileInfo in fileInfos)
                    {
                        if (fileInfo.LinkTarget == null)
                        {
                            if ((fileInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                            {
                                TreeNode dirChild = new TreeNode(true, fileInfo.FullName, 0);
                                parent.children.Add(dirChild);

                                Task task = new Task(FileScaner!, dirChild);
                                queue.Enqueue(task);
                            }
                            else
                            {
                                FileInfo file = (FileInfo)fileInfo;
                                TreeNode fileChild = new TreeNode(false, fileInfo.FullName, file.Length);
                                parent.children.Add(fileChild);
                            }
                        }
                    }
                }
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}