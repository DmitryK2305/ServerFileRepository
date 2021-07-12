using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ServerFileRepository.Models
{
    public class FileSystemModel : IFileSystemModel
    {
        public string RepositoryPath { get; private set; }
        private ConcurrentDictionary<string, UserFileSystemModel> users { get; set; }

        //По возможности убрать вебхост из параметров
        public FileSystemModel(IWebHostEnvironment webHost)
        {
            RepositoryPath = Path.Combine(webHost.ContentRootPath, "FileRepository");
            users = new ConcurrentDictionary<string, UserFileSystemModel>();
        }

        public async Task<UserFileSystemModel> GetUserModelAsync(string user)
        {
            return await Task.Run(() => GetUserModel(user));
            
        }

        public UserFileSystemModel GetUserModel(string user)
        {
            if (users.Keys.Contains(user))
            {
                return users[user];
            }
            else
            {
                var userModel = new UserFileSystemModel(this, user);
                users[user] = userModel;
                return userModel;
            }
        }
    }
}
