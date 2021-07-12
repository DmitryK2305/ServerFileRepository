using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerFileRepository.Models
{
    public interface IFileSystemItem
    {
        string Name { get; set; }
    }
}
