using ServerFileRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerFileRepository.ViewModels
{
    public class FileSystemViewModel
    {
        public IEnumerable<IFileSystemItem> Items { get; set; }
    }
}
