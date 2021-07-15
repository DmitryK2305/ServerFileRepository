using ServerFileRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerFileRepository.ViewModels
{
    public class DeletedItemsViewModel
    {
        public IEnumerable<DeletedItem> Items { get; set; }
    }
}
