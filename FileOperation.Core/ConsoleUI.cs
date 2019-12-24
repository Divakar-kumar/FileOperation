using FileOperation.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileOperation.Core
{
    public static class ConsoleUI
    {
        public static void CreateIntro()
        {
            Console.WriteLine(" =======================================");
            Console.WriteLine("           FILE SEARCH                    ");
            Console.WriteLine(" =======================================");
        }
        public static void WriteResult(SearchResult searchResult)
        {
            Console.WriteLine($"File Name is : {searchResult.fileName}");
            Console.WriteLine($"File Format is : {searchResult.fileFormat}");
            Console.WriteLine($"File Size is : {searchResult.fileSize}");
            Console.WriteLine($"Total occurences of the word \"{searchResult.searchTerm}\" in the file is :{ searchResult.totalWordOccurences}");

            foreach(var occurences in searchResult.countAndLineNumbers)
            {
                Console.WriteLine($"Found {occurences.noOfWords} word in line number {occurences.lineNumber}");
            }
        }
        public static void LogExceptions(Exception ex)
        {
            Console.WriteLine($"Exception occured {ex.Message}");
        }

    }
}
