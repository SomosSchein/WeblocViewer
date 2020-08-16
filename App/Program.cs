using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace App
{
    abstract class Program
    {
        static void Main(string[] args)
        {
            var done = false;
#if !DEBUG
                //with args(user open file with the program)
                if (args != null && args.Length > 0)
                {
                    done = TryProcessWebloc(args);
                }
#else
            done = TryProcessWebloc(new string[]{"./Test/test_file.webloc","./Test/test_file.webloc"});
            if (!done)
            {
                Console.Write("Invalid link");
                Console.ReadKey();
            }
#endif
        }

        private static bool TryProcessWebloc(string[] args)
        {
            bool processed = true;
            foreach (var fileName in args)
            {
                //Check file exists
                if (File.Exists(fileName))
                {
                    try
                    {
                        PList plist = new PList();
                        plist.Load(fileName);
                        foreach (var url in plist.Values)
                        {
                            OpenBrowser(url);
                        }
                    }
                    catch (Exception)
                    {
                        processed = false;
                    }
                }
            }
            return processed;
        }

        public static void OpenBrowser(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }); // Works ok on windows
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);  // Works ok on linux
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url); // Not tested
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}