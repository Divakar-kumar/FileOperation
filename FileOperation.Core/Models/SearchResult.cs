using System;
using System.Collections.Generic;
using System.Text;

namespace FileOperation.Core.Models
{
    public class SearchResult
    {

        public string searchTerm { get; set; }
        public string fileName { get; set; }
        public string fileFormat { get; set; }
        public string fileSize { get; set; }
        public int totalWordOccurences { get; set; }
        public List<SearchWord> countAndLineNumbers { get; set; }

        
    }
}
