using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.File
{
    /// <summary>
    /// 文件操作帮助类
    /// </summary>
    public class FileHelper
    {
        #region 公共方法

        // <summary>
        /// 根据文件路径获取后缀名。 eg.test.txt -> .txt
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string GetSuffix(string path)
        {
            if (string.IsNullOrEmpty(path)) return "";

            return Path.GetExtension(path);
        }
        /// <summary>
        /// 根据文件路径创建目录
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="isDirectory">是否目录</param>
        /// <returns></returns>
        public static bool CreateDirectory(string path, bool isDirectory = false)
        {
            if (string.IsNullOrEmpty(path)) return false;

            path = path.Replace("/", "\\");

            string directoryPath = path;
            if (!isDirectory) directoryPath = path.Substring(0, path.LastIndexOf("\\"));

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            return true;
        }
        /// <summary>
        /// 根据路径删除文件或目录
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="isDirectory">是否目录</param>
        /// <returns></returns>
        public static bool DeleteDirectoryOrFile(string path, bool isDirectory = false)
        {
            if (string.IsNullOrEmpty(path)) return false;
            path = path.Replace("/", "\\");

            string directoryPath = path;
            if (isDirectory) directoryPath = path.Substring(0, path.LastIndexOf("\\"));
            if (Directory.Exists(directoryPath))
            {
                //获取指定路径下所有文件夹
                string[] folderPaths = Directory.GetDirectories(directoryPath);
                //获取指定路径下所有文件
                string[] filePaths = Directory.GetFiles(directoryPath);
                if (folderPaths.Length > 0 || filePaths.Length > 0)
                {
                    Directory.Delete(directoryPath, true);
                }
                else 
                {
                    Directory.Delete(directoryPath, false);
                }
            }

            return true;
        }

        #endregion


       
    }

}
