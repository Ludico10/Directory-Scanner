using System.Collections.Generic;

namespace UserInterface.Model
{
    public class ModelNode
    {
        public string Name { get; set; } = "";
        public long AbsoluteSize { get; set; } = 0;
        public double RelativeSize { get; set; } = 100;
        public string ImagePath { get; set; } = "";
        public List<ModelNode>? Children { get; set; } = null;
    }
}
