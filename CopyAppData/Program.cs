using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace CopyAppData
{
   class Program
    {
       static string sourcePath =string.Empty;
       static string destinationPath = string.Empty;
        static void Main(string[] args)
        {
            if(args.Length == 2)
            {
                sourcePath = args[0];
                destinationPath = args[1];
            }
            var destFileName = Path.Combine(destinationPath, @"Intusoft-runtime.json");
            var srcFileName = Path.Combine(sourcePath, @"Intusoft-runtime.json");
            if (!File.Exists(destFileName))
            {
                File.Copy(srcFileName, destFileName);
                Console.WriteLine($"Destination File copy ={destFileName}");

            }
            GetDirFiles("ReportTemplates");
            GetDirFiles("ImageResources");
            Console.WriteLine("Completed ...");
        }
        static void GetDirFiles(string DirectoryName)
        {
            DirectoryInfo srcDirectoryInfo = new DirectoryInfo(Path.Combine(sourcePath, DirectoryName));
            
            DirectoryInfo destDirectoryInfo = new DirectoryInfo(Path.Combine(destinationPath, DirectoryName));
            Console.WriteLine($"Source Directory ={srcDirectoryInfo.FullName}");
            Console.WriteLine($"Destination Directory ={destDirectoryInfo.FullName}");
            if (!destDirectoryInfo.Exists)
                destDirectoryInfo.Create();
            FileInfo[] fileInfos = srcDirectoryInfo.GetFiles();
            if(fileInfos.Any())
            {
                FileInfo[] destfileInfos = destDirectoryInfo.GetFiles();
                foreach (var item in fileInfos)
                {
                    Console.WriteLine($"Source File ={item.FullName}");

                    var destFileName = Path.Combine(destDirectoryInfo.FullName, item.Name);
                    if (!File.Exists(destFileName))
                    {
                        File.Copy(item.FullName, destFileName);
                        Console.WriteLine($"Destination File copy ={destFileName}");

                    }
                }
            }
            else
            {
                DirectoryInfo[] srcDirectoryInfos = srcDirectoryInfo.GetDirectories();

                foreach (var item in srcDirectoryInfos)
                {
                    GetDirFiles( Path.Combine(DirectoryName, item.Name));
                }
            }
        }
    }
}
