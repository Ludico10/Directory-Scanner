namespace DirectoryScanner
{
    public class TreeNode
    {
        public string name;
        public long size;
        public double relativeSize;
        public List<TreeNode>? children = null;

        public TreeNode(bool isDir, string name, long size)
        {
            if (isDir) children = new List<TreeNode>();
            this.name = name;
            this.size = size;
        }

        public long Resize()
        {
            if (children != null)
            {
                size = 0;
                foreach (TreeNode cild in children)
                    size += cild.Resize();
                foreach (TreeNode cild in children)
                    cild.relativeSize = cild.size / size;
            }
            return size;
        }
    }
}
