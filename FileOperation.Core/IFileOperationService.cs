using FileOperation.Core.Models;

namespace FileOperation.Core
{
    public interface IFileOperationService
    {
        SearchResult SearchWord(string FileName, string searchTerm);
    }
}