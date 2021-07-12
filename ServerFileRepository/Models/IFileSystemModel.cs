using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerFileRepository.Models
{
    public interface IFileSystemModel
    {
        string RepositoryPath { get; }

        Task<UserFileSystemModel> GetUserModelAsync(string user);
        UserFileSystemModel GetUserModel(string user);
    }
}
