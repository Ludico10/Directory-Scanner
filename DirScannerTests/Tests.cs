using DirectoryScanner;
using System.Diagnostics;
using System.Threading;
using Xunit;

namespace DirScannerTests
{
    public class Tests
    {
        private void TreeCmp(TreeNode node1, TreeNode node2)
        {
            Assert.Equal(node1.name, node2.name);
            Assert.Equal(node1.size, node2.size);
            Assert.Equal(node1.relativeSize, node2.relativeSize);
            if (node1.children != null)
            {
                Assert.NotNull(node2.children);
                if (node2.children != null)
                {
                    Assert.Equal(node1.children.Count, node2.children.Count);
                    foreach (TreeNode child1 in node1.children)
                        foreach (TreeNode child2 in node2.children)
                            if (child1.name == child2.name)
                                TreeCmp(child1, child2);
                } 
            }
            else Assert.Null(node2.children);
        }

        [Fact]
        public void CalculateTest()
        {
            TreeNode root = new TreeNode(true, "0", 0);
            if (root.children != null)
            {
                root.children.Add(new TreeNode(true, "1", 0));
                root.children.Add(new TreeNode(false, "2", 500));
                root.children.Add(new TreeNode(true, "3", 0));
                if (root.children[0].children != null)
                {
                    root.children[0].children!.Add(new TreeNode(false, "11", 500));
                    root.children[0].children!.Add(new TreeNode(false, "12", 500));
                }
                if (root.children[2].children != null)
                    root.children[2].children!.Add(new TreeNode(false, "31", 500));

                root.Resize();

                Assert.Equal(2000, root.size);
                Assert.Equal(1000, root.children[0].size);
                Assert.Equal(0.5, root.children[0].relativeSize);
                Assert.Equal(500, root.children[1].size);
                Assert.Equal(0.25, root.children[1].relativeSize);
                Assert.Equal(500, root.children[2].size);
                Assert.Equal(0.25, root.children[2].relativeSize);
            }
        }

        [Fact]
        public void EmptyDirTest()
        {
            string path = "C:\\Users\\eugen\\source\\repos\\DirectoryScanner\\DirScannerTests\\testDirs\\EmptyDirTest";
            DirScanner scanner = new DirScanner(10);
            TreeNode creatorRoot = DirCreator.CreateEmptyDir(path);
            TreeNode scannerRoot = scanner.Scan(path);
            creatorRoot.Resize();
            scannerRoot.Resize();
            TreeCmp(creatorRoot, scannerRoot);
        }

        [Fact]
        public void SimpleDirTest()
        {
            string path = "C:\\Users\\eugen\\source\\repos\\DirectoryScanner\\DirScannerTests\\testDirs\\SimpleDirTest";
            DirScanner scanner = new DirScanner(10);
            TreeNode creatorRoot = DirCreator.CreateOneLayerDir(path);
            TreeNode scannerRoot = scanner.Scan(path);
            creatorRoot.Resize();
            scannerRoot.Resize();
            TreeCmp(scannerRoot, creatorRoot);
        }

        [Fact]
        public void MultiDirTest()
        {
            string path = "C:\\Users\\eugen\\source\\repos\\DirectoryScanner\\DirScannerTests\\testDirs\\MultiDirTest";
            DirScanner scanner = new DirScanner(10);
            TreeNode creatorRoot = DirCreator.CreateMultilayerDir(path, 0, 5);
            TreeNode scannerRoot = scanner.Scan(path);
            creatorRoot.Resize();
            scannerRoot.Resize();
            TreeCmp(creatorRoot, scannerRoot);
        }

        [Fact]
        public void CancelTest()
        {
            string path = "C:\\Users\\eugen\\source\\repos";
            Stopwatch stopwatch = new Stopwatch();
            DirScanner scanner = new DirScanner(10);

            stopwatch.Start();
            scanner.Scan(path);
            stopwatch.Stop();
            double time = stopwatch.Elapsed.TotalMilliseconds;

            stopwatch.Start();
            scanner.Scan(path);
            Thread.Sleep(200);
            scanner.Stop();
            stopwatch.Stop();
            double cancelTime = stopwatch.Elapsed.TotalMilliseconds;

            Assert.InRange(cancelTime, 0, time);
        }
    }
}