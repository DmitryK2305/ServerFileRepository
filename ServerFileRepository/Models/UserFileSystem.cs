using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ServerFileRepository.Models
{
    public class UserFileSystem
    {
        private readonly FileSystem owner;
        private string userName;
        public string CurrentDir { get; private set; }
        public string CurrentDirGlobalPath => Path.Combine(owner.RepositoryPath, userName, CurrentDir);

        public UserFileSystem(FileSystem mainModel, string userName)
        {
            owner = mainModel;
            this.userName = userName;
            CurrentDir = "";

            var curUserPath = Path.Combine(owner.RepositoryPath, userName);
            if (!System.IO.Directory.Exists(curUserPath))
            {
                System.IO.Directory.CreateDirectory(curUserPath);
            }
        }

        public bool OpenDirectory(string folderName)
        {
            var folderPath = Path.Combine(CurrentDirGlobalPath, folderName);
            if (System.IO.Directory.Exists(folderPath))
            {
                CurrentDir = Path.Combine(CurrentDir, folderName);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool BackDirectory()
        {
            if (string.IsNullOrEmpty(CurrentDir))
            {
                return false;
            }
            else
            {
                CurrentDir = Path.GetDirectoryName(CurrentDir);
                return true;
            }
        }

        public IEnumerable<IFileSystemItem> Items {
            get {
                IEnumerable<IFileSystemItem> items = System.IO.Directory.GetDirectories(CurrentDirGlobalPath).Select(t => new Directory() { Name = Path.GetFileName(t) });
                items = items.Concat(System.IO.Directory.GetFiles(CurrentDirGlobalPath).Select(t => new File() { Name = Path.GetFileName(t) }));

                if (!string.IsNullOrEmpty(CurrentDir))
                    items = new IFileSystemItem[] { new BackFolder()}.Concat(items);
                return items;
            }
        }

        public void Reset()
        {
            CurrentDir = "";
        }

        public bool DeleteFile(string name)
        {
            try
            {
                System.IO.File.Delete(Path.Combine(CurrentDirGlobalPath, name));
                return true;
            }
            catch { }
            return false;
        }

        public bool DeleteDirectory(string name)
        {
            try
            {
                System.IO.Directory.Delete(Path.Combine(CurrentDirGlobalPath, name), true);
                return true;
            }
            catch { }
            return false;
        }

        public bool AddDirectory(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(Path.Combine(CurrentDirGlobalPath, name));
                    return true;
                }
                catch { }
            }
            return false;
        }

        //По возможности заменить IFormFile
        //Проверить словится ли исключение из FileStream
        public async Task<bool> UploadFileAsync(IFormFile file)
        {
            if (file is not null)
            {
                try
                {
                    using (var fileStream = new FileStream(Path.Combine(CurrentDirGlobalPath, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return true;
                }
                catch { }
            }
            return false;
        }

        public string GetFilePath(string name)
        {
            return Path.Combine(CurrentDirGlobalPath, name);
        }
    }
}
