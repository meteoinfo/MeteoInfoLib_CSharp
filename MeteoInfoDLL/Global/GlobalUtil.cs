using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteoInfoC.Global
{
    /// <summary>
    /// Global class
    /// </summary>
    public class GlobalUtil
    {
        /// <summary>
        /// Get relative path of the file using project file path
        /// </summary>
        /// <param name="fileName">file path</param>
        /// <param name="projFile">project file path</param>
        /// <returns>relative path</returns>
        public string GetRelativePath(string fileName, string projFile)
        {
            string RelativePath = "";

            if (Path.GetPathRoot(fileName).ToLower() != Path.GetPathRoot(projFile).ToLower())
            {
                RelativePath = fileName;
            }
            else
            {
                List<string> aList = new List<string>();
                aList.Add(Path.GetFullPath(fileName));
                do
                {
                    aList.Add("");
                    try
                    {
                        aList[aList.Count - 1] = Directory.GetParent(aList[aList.Count - 2]).FullName.ToLower();
                    }
                    catch
                    {
                        break;
                    }
                }
                while (aList[aList.Count - 1] != "");

                List<string> bList = new List<string>();
                bList.Add(Path.GetFullPath(projFile));
                do
                {
                    bList.Add("");
                    try
                    {
                        bList[bList.Count - 1] = Directory.GetParent(bList[bList.Count - 2]).FullName.ToLower();
                    }
                    catch
                    {
                        break;
                    }
                }
                while (bList[bList.Count - 1] != "");

                bool ifExist = false;
                int offSet = 0;
                for (int i = 0; i < aList.Count; i++)
                {
                    for (int j = 0; j < bList.Count; j++)
                    {
                        if (aList[i] == bList[j])
                        {
                            for (int k = 1; k < j; k++)
                            {
                                RelativePath = RelativePath + @"..\";
                            }
                            if (aList[i].EndsWith(@"\"))
                            {
                                offSet = 0;
                            }
                            else
                            {
                                offSet = 1;
                            }
                            RelativePath = RelativePath + fileName.Substring(aList[i].Length + offSet);
                            ifExist = true;
                            break;
                        }
                    }
                    if (ifExist)
                    {
                        break;
                    }
                }
            }

            if (RelativePath == "")
            {
                RelativePath = fileName;
            }
            return RelativePath;
        }

        /// <summary>
        /// Get all special files by folder using file filter
        /// </summary>
        /// <param name="folderName">The folder name</param>
        /// <param name="fileFilter">The file filter</param>
        /// <param name="isContainSubFolder">Is contain sub folder</param>
        /// <returns>The file list</returns>
        public static List<string> GetAllFilesByFolder(string folderName, string fileFilter, bool isContainSubFolder)
        {
            List<string> resArray = new List<string>();
            string[] files = Directory.GetFiles(folderName, fileFilter);
            for (int i = 0; i < files.Length; i++)
            {
                resArray.Add(files[i]);
            }
            if (isContainSubFolder)
            {
                string[] folders = Directory.GetDirectories(folderName);
                for (int j = 0; j < folders.Length; j++)
                {                    
                    List<string> temp = GetAllFilesByFolder(folders[j], fileFilter, isContainSubFolder);
                    resArray.AddRange(temp);
                }
            }
            return resArray;
        }
    }
}
