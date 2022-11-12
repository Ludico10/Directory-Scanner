using DirectoryScanner;
using System.Collections.Generic;

namespace UserInterface.Model
{
    public class ModelTree
    {
        public static ModelNode TreeConvert(TreeNode node)
        {
            ModelNode res = new ModelNode();
            res.Name = node.name;
            res.AbsoluteSize = node.size;
            res.RelativeSize = node.relativeSize * 100;
            if (node.children != null)
            {
                res.ImagePath = "Resources/Dir.png";
                res.Children = new List<ModelNode>();
                foreach (TreeNode childNode in node.children)
                    res.Children.Add(TreeConvert(childNode));
            }
            else res.ImagePath = "Resources/File.png";
            return res;
        }
    }
}
