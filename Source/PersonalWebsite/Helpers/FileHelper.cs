namespace PersonalWebsite.Helpers
{
    public static class FileHelper
    {
        public static bool PrepareDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool DeleteFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return true;
                }

                File.Delete(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
