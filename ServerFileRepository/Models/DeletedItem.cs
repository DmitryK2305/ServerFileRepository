using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerFileRepository.Models
{
    public class DeletedItem
    {
        public string Path { get; set; }
        public DateTime Time { get; set; }
    }
}
