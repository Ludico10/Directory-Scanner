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
    }
}
