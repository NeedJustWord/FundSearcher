using System;
using System.IO;
using System.Linq;

namespace Fund.Core.Helpers
{
    public static class DirectoryHelper
    {
        /// <summary>
        /// 确保目录存在
        /// </summary>
        /// <param name="path"></param>
        public static void Ensure(string path)
        {
            Ensure(new DirectoryInfo(path));
        }

        /// <summary>
        /// 确保目录存在
        /// </summary>
        /// <param name="dir"></param>
        public static void Ensure(DirectoryInfo dir)
        {
            if (dir.Exists) return;

            if (dir.Parent.Exists == false)
            {
                Ensure(dir.Parent);
            }
            dir.Create();
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="path"></param>
        /// <param name="deleteSelf">指定目录是否删除</param>
        /// <param name="error">删除失败时的错误消息</param>
        /// <returns></returns>
        public static bool Delete(string path, bool deleteSelf, out string error)
        {
            var result = true;
            error = null;
            if (Directory.Exists(path))
            {
                try
                {
                    if (deleteSelf)
                    {
                        Directory.Delete(path, true);
                    }
                    else
                    {
                        foreach (var subDir in Directory.GetDirectories(path))
                        {
                            Directory.Delete(subDir, true);
                        }
                        foreach (var file in Directory.GetFiles(path))
                        {
                            File.Delete(file);
                        }
                    }
                }
                catch (Exception e)
                {
                    error = e.Message;
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取带单位的文件大小
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetDirSizeWithUnit(string path)
        {
            return FormatFileSize(GetDirSize(path));
        }

        /// <summary>
        /// 获取文件大小，单位字节
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static long GetDirSize(string path)
        {
            var dir = new DirectoryInfo(path);
            return dir.Exists ? dir.GetFiles("*.*", SearchOption.AllDirectories).Sum(t => t.Length) : 0;
        }

        /// <summary>
        /// 格式化文件大小
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string FormatFileSize(long length)
        {
            string[] units = { "B", "KB", "MB", "GB", "TB" };
            double len = length;
            int order = 0;

            while (len >= 1024 && order < units.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return $"{len:0.##} {units[order]}";
        }
    }
}
