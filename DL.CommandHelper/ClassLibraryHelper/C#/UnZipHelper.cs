using System.IO;
using System.Reflection;

namespace ClassLibraryHelper.C_
{
    public class UnZipHelper
    {
        private readonly object _zpackage;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public UnZipHelper(object zpackage)
        {
            _zpackage = zpackage;
        }

        //public static UnZipHelper OpenFile(string path)
        //{
        //    var type = typeof(Packaging.Package).Assembly.GetType("MS.Internal.IO.Zip.ZipArchive");
        //    var method = type.GetMethod("OpenFile", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        //    return new UnZipHelper(method.Invoke(null, new object[] { path, FileMode.Open, FileAccess.Read, FileShare.Read, false }));
        //}

        public static void UnZip(string zipPath, string destFolder)
        {
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }
            
        }
    }
}
