using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ConsoleApp1
{
    public class File
    {
        const long SIZE_OF_B = 1;
        const long SIZE_OF_KB = 1024 * SIZE_OF_B;
        const long SIZE_OF_MB = 1024 * SIZE_OF_KB;
        const long SIZE_OF_GB = 1024 * SIZE_OF_MB;
        const long SIZE_OF_TB = 1024 * SIZE_OF_GB;

        [Required]
        public string FileType { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string Extension { get; set; }
        [Required]
        public string SizeString { get; set; }
        public long SizeLong { get; set; }
        public string? Resolution { get; set; }
        public string? Length { get; set; }
        public string? Content { get; set; }
        public string SpecialPath { get; set; }

        public File(string path)
        {

            path = path.Trim();

            int endIndexType = path.IndexOf(':');
            int startSize = path.IndexOf("(")+1;
            int endSize = path.IndexOf(")");
            int endUsualPath = path.IndexOf(";");
            int startExtension = (path.Substring(0, endUsualPath )).LastIndexOf('.')+1;

            FileType = path.Substring(0, endIndexType).ToLower();
            Extension = path.Substring(startExtension, startSize- startExtension-1);
            FileName = path.Substring(endIndexType+1, startExtension- endIndexType -1) +Extension;
            SizeString = path.Substring(startSize, endSize-startSize);
            SizeLong = FormatOfSizeProp(SizeString);
            SpecialPath = path.Substring(endUsualPath+1);

            switch (FileType)
            {
                case "text":
                    Content = SpecialPath;
                    break;
                case "movie":
                    int endResolution = SpecialPath.IndexOf(";");
                    Resolution = SpecialPath.Substring(0, endResolution);
                    Length = SpecialPath.Substring(endResolution+1);
                    break;
                case "image":
                    Resolution = SpecialPath;
                    break;
            }
        }

        private long FormatOfSizeProp(string size)
        {
            long sizeLong = 0;

            for (int i = 0; i < size.Length; i++)
            {
                if (char.IsDigit(size[i]))
                {
                    sizeLong *= 10;
                    sizeLong += long.Parse(size[i].ToString());
                }
                else
                {
                    switch (size[i])
                    {
                        case 'B':
                            sizeLong *= SIZE_OF_B;
                            break;
                        case 'K':
                            sizeLong *= SIZE_OF_KB;
                            break;
                        case 'M':
                            sizeLong *= SIZE_OF_MB;
                            break;
                        case 'G':
                            sizeLong *= SIZE_OF_GB;
                            break;
                        case 'T':
                            sizeLong *= SIZE_OF_TB;
                            break;
                    }
                }
            }
            return sizeLong;
        }
        private static List<File> SortFiles(List<File> list)
        {
            list.Sort((a,b)=> a.SizeLong.CompareTo(b.SizeLong));
           return list;

        }
        private static Dictionary<string, List<File>> GroupeByType(List<File> list)
        {
            Dictionary<string, List<File>> groups = new Dictionary<string, List<File>>();
            foreach (File file in list)
            {
                if (!groups.ContainsKey(file.FileType))
                {
                    groups.Add(file.FileType, new List<File>());
                }
                groups[file.FileType].Add(file);

            }

            foreach (KeyValuePair<string, List<File>> group in groups)
            {
                groups[group.Key] = SortFiles( group.Value);
            }

            return groups;

        }
        private static List<File> TakeData(string paths) 
        {
            string[] pathsList = paths.Split("\r\n");
            List <File> filesList = new List<File>();

            foreach (string path in pathsList)
            {
                filesList.Add(new File(path));
            }
            return filesList;
        }
        private static void DisplayFiles(Dictionary<string, List<File>> dict)
        {
            foreach (KeyValuePair<string, List<File>> fileList in dict)
            {
                if(fileList.Value.Count > 0)
                {
                    if(fileList.Key == "text")
                    {
                        Console.WriteLine("Text files:");
                    }
                    else
                    {
                        Console.WriteLine(char.ToUpper(fileList.Key[0]) + fileList.Key.Substring(1)+"s"+":");
                    }
                    foreach (File? file in fileList.Value)
                    {
                        Console.WriteLine($"\t{file.FileName}");
                        Console.WriteLine($"\t\tExtension: {file.Extension}");
                        Console.WriteLine($"\t\tSize: {file.SizeString}");

                        switch (file.FileType)
                        {
                            case "text":
                                Console.WriteLine("\t\tContent: " + '"'+file.Content+'"');
                                break;
                            case "image":
                                Console.WriteLine($"\t\tResolution: {file.Resolution}");
                                break;
                            case "movie":
                                Console.WriteLine($"\t\tResolution: {file.Resolution}");
                                Console.WriteLine($"\t\tLength: {file.Length}");
                                break;
                        }
                    }
                }
                
            }
        }

        public static void GetDataAboutFilesByPaths(string paths)
        {
            List<File> listOfFiles = TakeData(paths);
            Dictionary<string, List<File>> sortedListOfFiles = GroupeByType(listOfFiles) ;
            DisplayFiles(sortedListOfFiles);
        }
        
    }
}
