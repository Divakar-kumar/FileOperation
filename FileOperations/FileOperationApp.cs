using FileOperation.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileOperations
{
    class FileOperationApp
    {
        private readonly IFileOperationService _service;
        public FileOperationApp(IFileOperationService fileOperationService)
        {
            _service = fileOperationService;
        }
        public void Run()
        {
            var filePath = @"big.txt";
            var searchTerm = "the";
            ConsoleUI.CreateIntro();
            var result=_service.SearchWord(filePath, searchTerm);
            ConsoleUI.WriteResult(result);
        }
    }
}
