using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        private IFileSystemModel fileSystemModel;

        public FileSystemController(IWebHostEnvironment webHost, IFileSystemModel model)
        { 
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
            userModel.OpenDirectory(name);
            var viewModel = new FileSystemViewModel() { Items = userModel.Items };

            return View("Index", viewModel);
        }

        [Authorize]
        public ActionResult GoBack()
        {
            var userModel = fileSystemModel.GetUserModel(User.Identity.Name);
            userModel.BackDirectory();
            var viewModel = new FileSystemViewModel() { Items = userModel.Items };

            return View("Index", viewModel);
        }

        [Authorize]
        public ActionResult DeleteFile(string name)
        {
            var userModel = fileSystemModel.GetUserModel(User.Identity.Name);
            userModel.DeleteFile(name);
            var viewModel = new FileSystemViewModel() { Items = userModel.Items };

            return View("Index", viewModel);
        }

        [Authorize]
        public ActionResult DeleteDirectory(string name)
        {
            var userModel = fileSystemModel.GetUserModel(User.Identity.Name);
            userModel.DeleteDirectory(name);
            var viewModel = new FileSystemViewModel() { Items = userModel.Items };

            return View("Index", viewModel);
        }

        //!Отрабатывает только с Index
        [Authorize]
        public ActionResult AddDirectory(string name)
        {
            var userModel = fileSystemModel.GetUserModel(User.Identity.Name);
            userModel.AddDirectory(name);
            var viewModel = new FileSystemViewModel() { Items = userModel.Items };

            return View("Index", viewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult UploadFile(IFormFile path)
        {
            var userModel = fileSystemModel.GetUserModel(User.Identity.Name);
            //userModel.AddDirectory(name);
            var viewModel = new FileSystemViewModel() { Items = userModel.Items };

            return View("Index", viewModel);
        }
    }
}
