using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationNewJob.Models;

namespace WebApplicationNewJob.Services.Http
{
    public interface IHttpService
    {

        Task<string> PostAsync(object obj, string url);

        Task<string> GetAsync(string url);
        Task<List<T>> ListFromApiAsync<T>(T model, string endpoint);
    }
}
