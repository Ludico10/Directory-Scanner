namespace DirectoryScanner
{
    public class TreeNode
    {
        public string name;
        public long size;
        public double relativeSize = 1;
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
                foreach (TreeNode child in children)
                    size += child.Resize();
                foreach (TreeNode child in children)
                    if (size > 0) child.relativeSize = (double)child.size / (double)size;
                    else child.relativeSize = 1;
            }
            return size;
        }
    }
}
