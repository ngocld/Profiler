using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Profiler.Helpers
{
    public class clsProcessFile
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        // 0: ko ton tai file
        // 1: file available
        // 2: file lock
        public static int CheckStatusFile(string pathfile)
        {
            var file = new FileInfo(pathfile);
            if (!file.Exists)
                return 0;

            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
                return 1;
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return 2;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
        }
    }
}
