using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerFileRepository.Contexts.Tables
{
    public class User
    {        
        [Key, MinLength(3), MaxLength(15)]
        public string Login { get; set; }
        [Required, MinLength(8)]
        public string Password { get; set; }

        public List<FileSystemEvent> Events { get; set; }
    }
}
