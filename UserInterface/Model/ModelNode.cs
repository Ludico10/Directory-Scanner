using DirectoryScanner;
using System.Collections.Generic;
using System.IO;

namespace UserInterface.Model
{
    public class ModelNode
    {
        public string Name { get; set; } = "";
        public long AbsoluteSize { get; set; } = 0;
        public double RelativeSize { get; set; } = 100;
        public string ImagePath { get; set; } = "";
        public List<ModelNode>? Children { get; set; } = null;

        private ModelNode(TreeNode node)
        {
            string[] pathParts = node.name.Split('\\', System.StringSplitOptions.RemoveEmptyEntries);
            Name = pathParts[pathParts.Length - 1];
            AbsoluteSize = node.size;
            RelativeSize = node.relativeSize * 100;
            if (node.children != null)
            {
                ImagePath = Path.GetFullPath("Resources\\Dir.png");
                Children = new List<ModelNode>();
            }
            else ImagePath = Path.GetFullPath("Resources\\File.png");
        }

        public static ModelNode TreeConvert(TreeNode node)
        {
            ModelNode res = new ModelNode(node);
            if ((node.children != null) && (res.Children != null))
            {
                foreach (TreeNode childNode in node.children)
                    res.Children.Add(TreeConvert(childNode));
            }
            return res;
        }
    }
}
