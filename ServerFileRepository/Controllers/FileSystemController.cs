using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ServerFileRepository.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using File = ServerFileRepository.Models.File;

namespace ServerFileRepository.Controllers
{
    public class FileSystemController : Controller
    {
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly string repositoryPath;

        public FileSystemController(IWebHostEnvironment webHost)
        {
            hostEnvironment = webHost;

            repositoryPath = Path.Combine(hostEnvironment.ContentRootPath, "FileRepository");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            var curUserPath = Path.Combine(repositoryPath, User.Identity.Name);
            if (!Directory.Exists(curUserPath))
            {
                Directory.CreateDirectory(curUserPath);
            }

            IEnumerable<IFileSystemItem> items = Directory.GetDirectories(curUserPath).Select(t => new Folder() { Name = Path.GetDirectoryName(t) });
            items = items.Concat(Directory.GetFiles(curUserPath).Select(t => new File() { Name = Path.GetFileName(t) }));            

            return View(items);
        }

        
        public ActionResult OpenDirectory(string name)
        {
            //Заглушка
            var model = new List<IFileSystemItem>();
            model.Add(new File() { Name = "File3.txt" });
            model.Add(new File() { Name = "File4.dat" });

            return View("Index", model);
        }
    }
}
