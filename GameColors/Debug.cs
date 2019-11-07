using System;
using System.IO;

namespace GameColors
{
    public static class Debug
    {
        public static void Log(string msg)
        {
            using (StreamWriter streamWriter = new StreamWriter("Log.txt", true))
            {
                streamWriter.WriteLine("[" + DateTime.Now.ToString("h:mm:ss tt") + "]  " + msg);
            }
        }

        public static void Delete()
        {
            if (File.Exists("Log.txt"))
            {
                File.Delete("Log.txt");
            }
        }
    }
}
