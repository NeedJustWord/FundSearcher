using System.IO;

namespace Fund.Core.Helpers
{
    public static class DirectoryHelper
    {
        public static void Ensure(string path)
        {
            Ensure(new DirectoryInfo(path));
        }

        public static void Ensure(DirectoryInfo dir)
        {
            if (dir.Exists) return;

            if (dir.Parent.Exists == false)
            {
                Ensure(dir.Parent);
            }
            dir.Create();
        }
    }
}
