using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ServerFileRepository.Models;
using ServerFileRepository.ViewModels;
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
        private readonly string repositoryPath;
        private FileSystemModel fileSystemModel;

        public FileSystemController(IWebHostEnvironment webHost)
        { 
            repositoryPath = Path.Combine(webHost.ContentRootPath, "FileRepository");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            fileSystemModel = new FileSystemModel(repositoryPath, User.Identity.Name);
            var viewModel = new FileSystemViewModel() { Items = fileSystemModel.Items };                        

            return View(viewModel);
        }
        
        public ActionResult OpenDirectory(string name)
        {
            fileSystemModel.OpenFolder(name);
            var viewModel = new FileSystemViewModel() { Items = fileSystemModel.Items };

            return View("Index", viewModel);
        }
    }
}
