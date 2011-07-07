namespace Conf.ObjectModel
{
    static class Constants
    {
        /// <summary>
        /// /data/data/com.confapp.monospace11/files
        /// </summary>
        public static string DocumentsFolder
        {
            get
            { 
                return System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            }
        }
    }
}