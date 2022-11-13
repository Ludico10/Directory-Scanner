using DirectoryScanner;
using Xunit;

namespace DirScannerTests
{
    public class Tests
    {
        private void NodeCmp(TreeNode node1, TreeNode node2)
        {
            Assert.Equal(node1.name, node2.name);
            Assert.Equal(node1.size, node2.size);
            Assert.Equal(node1.relativeSize, node2.relativeSize);
            if (node1.children != null)
            {
                Assert.NotNull(node2.children);
                if (node2.children != null)
                    Assert.Equal(node1.children.Count, node2.children.Count);
            }
            else Assert.Null(node2.children);
        }

        [Fact]
        public void SimpleDirTest()
        {
            string path = "C:\\Users\\eugen\\source\\repos\\DirectoryScanner\\DirScannerTests\\testDirs";
            DirScanner scanner = new DirScanner(10);
            TreeNode creatorRoot = DirCreator.CreateOneLayerDir(path);
            TreeNode scannerRoot = scanner.Scan(path);
            NodeCmp(scannerRoot, creatorRoot);
        }
    }
}