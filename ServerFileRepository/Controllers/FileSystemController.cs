using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerFileRepository.Contexts;
using ServerFileRepository.Contexts.Enums;
using ServerFileRepository.Models;
using ServerFileRepository.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using File = ServerFileRepository.Models.File;

namespace ServerFileRepository.Controllers
{
    public class FileSystemController : Controller
    {
        private readonly IFileSystem fileSystemModel;
        private readonly UserContext db;

        public FileSystemController(IFileSystem model, UserContext userContext)
        { 
            fileSystemModel = model;            
            db = userContext;
        }

        [Authorize]
        public ActionResult Index()
        { 
            var userModel = fileSystemModel.GetUserModel(User.Identity.Name);
            
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
            var userName = User.Identity.Name;
            var userModel = fileSystemModel.GetUserModel(userName);
            var success = userModel.DeleteFile(name);
            if (success)
            {
                db.Events.Add(new Contexts.Tables.FileSystemEvent()
                {
                    Type = FileSystemEventType.Delete,
                    Time = DateTime.Now,
                    UserLogin = userName,
                    FilePath = Path.Combine(userModel.CurrentDir, name),
                    IsFolder = false
                });
                db.SaveChanges();
            }

            var viewModel = new FileSystemViewModel() { Items = userModel.Items };

            return View("Index", viewModel);
        }

        [Authorize]
        public ActionResult DeleteDirectory(string name)
        {
            var userName = User.Identity.Name;
            var userModel = fileSystemModel.GetUserModel(userName);
            var success = userModel.DeleteDirectory(name);
            if (success)
            {
                db.Events.Add(new Contexts.Tables.FileSystemEvent()
                {
                    Type = FileSystemEventType.Delete,
                    Time = DateTime.Now,
                    UserLogin = userName,
                    FilePath = Path.Combine(userModel.CurrentDir, name),
                    IsFolder = true
                });
                db.SaveChanges();
            }

            var viewModel = new FileSystemViewModel() { Items = userModel.Items };

            return View("Index", viewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddDirectory(string name)
        {
            var userName = User.Identity.Name;
            var userModel = fileSystemModel.GetUserModel(userName);
            var success = userModel.AddDirectory(name);
            if (success)
            {
                db.Events.Add(new Contexts.Tables.FileSystemEvent()
                {
                    Type = FileSystemEventType.Create,
                    Time = DateTime.Now,
                    UserLogin = userName,
                    FilePath = Path.Combine(userModel.CurrentDir, name),
                    IsFolder = true
                });
                db.SaveChanges();
            }

            var viewModel = new FileSystemViewModel() { Items = userModel.Items };

            return View("Index", viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> UploadFile(IFormFile file)
        {
            var userName = User.Identity.Name;
            var userModel = fileSystemModel.GetUserModel(User.Identity.Name);            
            var success = await userModel.UploadFileAsync(file);
            if (success)
            {
                db.Events.Add(new Contexts.Tables.FileSystemEvent()
                {
                    Type = FileSystemEventType.Create,
                    Time = DateTime.Now,
                    UserLogin = userName,
                    FilePath = Path.Combine(userModel.CurrentDir, file.FileName),
                    IsFolder = false
                });
                db.SaveChanges();
            }

            var viewModel = new FileSystemViewModel() { Items = userModel.Items };

            return View("Index", viewModel);
        }
        
        [Authorize]
        public FileResult DownloadFile(string name)
        {            
            var userModel = fileSystemModel.GetUserModel(User.Identity.Name);
            var filePath = userModel.GetFilePath(name);

            var cd = new ContentDisposition
            {
                FileName = Path.GetFileName(filePath),
                Inline = false
            };
            
            HttpContext.Response.Headers.Add("Content-Disposition", cd.ToString());
            return this.File(new FileStream(filePath, FileMode.Open), MediaTypeNames.Application.Octet);
        }
    }
}
