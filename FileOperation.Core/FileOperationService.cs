using ByteSizeLib;
using FileOperation.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace FileOperation.Core
{
    public class FileOperationService : IFileOperationService
    {
        /// <summary>
        /// Implemented ReadWriteLockSlim to avoid multiple threads trying
        /// to modify while we read the file contents
        /// </summary>
        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
        /// <summary>
        /// Gets fileName and searchTerm as input
        /// Returns fileinfo along with search occurences 
        /// and line number info
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="searchTerm"></param>
        /// <returns>SearchResult</returns>
        public SearchResult SearchWord(string fileName, string searchTerm)
        {
            var searchResult = new SearchResult();
            FileInfo fileInfo = new FileInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), fileName));
            //check if file exists
            if (fileInfo.Exists)
            {
                try
                {
                    _readWriteLock.EnterWriteLock();
                    bool isAllowedToRead = Enum.GetNames(typeof(FileExtensions)).Contains(fileInfo.Extension.Replace(".",""),StringComparer.CurrentCultureIgnoreCase);
                    //check if file is allowed to read 
                    if(isAllowedToRead)
                    {
                        InstantiateResultModel(searchTerm,fileInfo,searchResult);
                        ProcessFile(fileName,searchTerm,searchResult);
                    }

                }
                catch(Exception ex)
                {
                    ConsoleUI.LogExceptions(ex);
                }
                finally
                {
                    //release lock after operation is completed
                    _readWriteLock.ExitWriteLock();
                }
            }
            return searchResult;
        }


        /// <summary>
        /// Instantiate searchResult with FileInfo
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="fileInfo"></param>
        /// <param name="searchResult"></param>
        private void InstantiateResultModel(string searchTerm,FileInfo fileInfo,SearchResult searchResult)
        {
            searchResult.searchTerm = searchTerm;
            searchResult.countAndLineNumbers = new List<SearchWord>();
            searchResult.fileName = fileInfo.Name;
            searchResult.fileFormat = fileInfo.Extension.Replace(".", "");
            //Used ByteSize library to conver byte to readable string format
            searchResult.fileSize = ByteSize.FromBytes(fileInfo.Length).ToString();
            //Gets all the content of file
        }


        /// <summary>
        /// Process File , search for the given 
        /// search term and returns searchResult Model
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="searchTerm"></param>
        /// <param name="searchResult"></param>
        private void ProcessFile(string fileName,string searchTerm,SearchResult searchResult)
        {
            int wordCount = 0;
            int lineCount = 0;
            string fileContent = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), fileName));
            List<string> lines = fileContent.Replace("\r", "").Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (var line in lines)
            {
                lineCount++;
                var words = line.Split(" ");
                int counts = 0;
                SearchWord searchWord = new SearchWord();
                if (words.Length >= 1)
                {
                    foreach (var word in words)
                    {
                        if (word.Equals(searchTerm, StringComparison.CurrentCultureIgnoreCase))
                        {
                            wordCount++;
                            counts++;
                        }
                    }
                    if (counts != 0)
                    {
                        searchWord.noOfWords = counts;
                        searchWord.lineNumber = lineCount;
                        searchResult.countAndLineNumbers.Add(searchWord);
                    }
                }
            }
            searchResult.totalWordOccurences = wordCount;
        }
    }
}
