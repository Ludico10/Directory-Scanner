namespace DirectoryScanner
{
    public class FileTree
    {
        public TreeNode root;

        public FileTree(string dirName)
        {
            root = new TreeNode(true, dirName, 0);
        }
    }
}
