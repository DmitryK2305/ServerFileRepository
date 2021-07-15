using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerFileRepository.Contexts;
using ServerFileRepository.Contexts.Enums;
using ServerFileRepository.Models;
using ServerFileRepository.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerFileRepository.Controllers
{
    public class DeletedItemsController : Controller
    {
        private readonly UserContext db;

        public DeletedItemsController(UserContext userContext)
        {
            db = userContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            var items = db.Events.Where(t => t.UserLogin == User.Identity.Name && t.Type == FileSystemEventType.Delete)
                .Select(t => new DeletedItem() { Path = t.FilePath, Time = t.Time });
            var viewModel = new DeletedItemsViewModel() { Items = items };
            return View(viewModel);
        }
    }
}
