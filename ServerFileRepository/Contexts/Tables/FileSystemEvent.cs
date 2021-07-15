using ServerFileRepository.Contexts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerFileRepository.Contexts.Tables
{
    public class FileSystemEvent
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserLogin { get; set; }
        public User User { get; set; }
        [Required]
        public FileSystemEventType Type { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public string FilePath { get; set; }
        [Required]
        public bool IsFolder { get; set; }
    }
}
