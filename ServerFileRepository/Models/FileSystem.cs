using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ServerFileRepository.Models
{
    public class FileSystem : IFileSystem
    {
        public string RepositoryPath { get; private set; }
        private ConcurrentDictionary<string, UserFileSystem> users { get; set; }

        //По возможности убрать вебхост из параметров
        public FileSystem(IWebHostEnvironment webHost)
        {
            RepositoryPath = Path.Combine(webHost.ContentRootPath, "FileRepository");
            users = new ConcurrentDictionary<string, UserFileSystem>();
        }

        public async Task<UserFileSystem> GetUserModelAsync(string user)
        {
            return await Task.Run(() => GetUserModel(user));
            
        }

        public UserFileSystem GetUserModel(string user)
        {
            if (users.Keys.Contains(user))
            {
                return users[user];
            }
            else
            {
                var userModel = new UserFileSystem(this, user);
                users[user] = userModel;
                return userModel;
            }
        }
    }
}
