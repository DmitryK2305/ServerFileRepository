using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerFileRepository.Models
{
    public interface IFileSystem
    {
        string RepositoryPath { get; }

        Task<UserFileSystem> GetUserModelAsync(string user);
        UserFileSystem GetUserModel(string user);
    }
}
