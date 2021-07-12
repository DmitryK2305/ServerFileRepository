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
        private IFileSystemModel fileSystemModel;

        public FileSystemController(IWebHostEnvironment webHost, IFileSystemModel model)
        { 
            repositoryPath = Path.Combine(webHost.ContentRootPath, "FileRepository");
            fileSystemModel = model;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            var userModel = fileSystemModel.GetUserModel(User.Identity.Name);
            userModel.Reset();
            var viewModel = new FileSystemViewModel() { Items = userModel.Items };
            return View(viewModel);
        }

        [Authorize]
        public ActionResult OpenDirectory(string name)
        {
            var userModel = fileSystemModel.GetUserModel(User.Identity.Name);
            userModel.OpenFolder(name);
            var viewModel = new FileSystemViewModel() { Items = userModel.Items };

            return View("Index", viewModel);
        }
    }
}
