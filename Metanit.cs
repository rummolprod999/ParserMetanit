using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace ParserMetanit
{
    public class Metanit
    {
        public Metanit()
        {
        }

        private static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        private string Url => "https://metanit.com";
        private string TargetDir => "metanit";
        private string ZipF => "metanit.zip";
        private string UserAgent =>
            @"""Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.8.1.6) Gecko/20070802 SeaMonkey/1.1.4""";

        public void Parser()
        {
            string fPath = $"{AssemblyDirectory}{Path.DirectorySeparatorChar}{TargetDir}";
            string zipPath = $"{AssemblyDirectory}{Path.DirectorySeparatorChar}{ZipF}";
            CreateDelDir(fPath, zipPath);
            DownloadSite(fPath);
            ZipDir(fPath, zipPath);
            
        }

        private void CreateDelDir(string tempPath, string zipPath)
        {
            if (Directory.Exists(tempPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(tempPath);
                dirInfo.Delete(true);
                Directory.CreateDirectory(tempPath);
            }
            else
            {
                Directory.CreateDirectory(tempPath);
            }

            if (!File.Exists(zipPath)) return;
            FileInfo fileInfo = new FileInfo(zipPath);
            fileInfo.Delete();
        }

        private void DownloadSite(string fPath)
        {
            var myProcess = new Process {StartInfo = new ProcessStartInfo("wget", $"-P {fPath} -U {UserAgent} -r -k -l0 -t 10 -p -E -nc {Url}")};
            myProcess.Start();
            //Console.WriteLine(myProcess.StartInfo.Arguments);
            myProcess.WaitForExit();
        }

        private void ZipDir(string startPath, string zipPath)
        {
            ZipFile.CreateFromDirectory(startPath, zipPath);
        }
    }
}