using System;
using System.IO;

namespace Luna {

    public static class Config {

        public const char DOUBLE_QUOTE = '\"';
        public const char SINGLE_QUOTE = '\'';

        const string FILES_FOLDER = "Luna";

        public static string RootFolder { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), FILES_FOLDER); } }

        public static string GetFilePath(string file) {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), FILES_FOLDER, file);
        }
    }
}
