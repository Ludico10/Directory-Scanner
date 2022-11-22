using DirectoryScanner;
using System.IO;

namespace DirScannerTests
{
    public class DirCreator
    {
        public static void CreateFile(string path, int size)
        {
            using var fileStream = new FileStream(path, FileMode.Create);
            fileStream.Write(new byte[size]);
        }

        public static TreeNode CreateEmptyDir(string path)
        {
            path = Path.GetFullPath(path);
            if (Directory.Exists(path)) Directory.Delete(path, true);
            Directory.CreateDirectory(path);

            return new TreeNode(true, path, 0);
        }

        public static TreeNode CreateOneLayerDir(string path)
        {
            path = Path.GetFullPath(path);
            if (Directory.Exists(path)) Directory.Delete(path, true);
            Directory.CreateDirectory(path);

            TreeNode res = new TreeNode(true, path, 0);
            int fileCnt = 20;
            int size = 50;
            for (var i = 0; i < fileCnt; i++)
            {
                var filePath = $"{path}\\{i}.txt";
                var fileSize = i * size;
                CreateFile(filePath, fileSize);
                if (res.children != null)
                    res.children.Add(new TreeNode(false, filePath, fileSize));
            }
            return res;
        }

        public static TreeNode CreateMultilayerDir(string path, int layer, int limit)
        {
            path = Path.GetFullPath(path);
            if (Directory.Exists(path)) Directory.Delete(path, true);
            Directory.CreateDirectory(path);

            if (layer < limit)
            {
                TreeNode res = new TreeNode(true, path, limit);
                if (res.children != null)
                    for (int i = 0; i < layer; i++)
                        res.children.Add(CreateMultilayerDir($"{path}\\{i}", layer + 1, limit));
                return res;
            }
            else return CreateOneLayerDir(path);
        }

        public static TreeNode CreateDirWithTarget(string path)
        {
            path = Path.GetFullPath(path);
            if (Directory.Exists(path)) Directory.Delete(path, true);
            Directory.CreateDirectory(path);

            string filePath = $"{path}\\file.txt";
            int fileSize = 500;
            CreateFile(filePath, fileSize);
            Directory.CreateSymbolicLink($"{path}\\dirlink", path);

            TreeNode res = new TreeNode(true, path, 0);
            if (res.children != null) res.children.Add(new TreeNode(false, filePath, fileSize));
            return res;
        }
    }
}
