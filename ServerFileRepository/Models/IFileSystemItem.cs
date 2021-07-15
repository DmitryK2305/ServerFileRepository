using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerFileRepository.Models
{
    public interface IFileSystemItem
    {
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
