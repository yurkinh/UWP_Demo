using System.Collections.Generic;
using System.Threading.Tasks;
using UWPDemo.Models;

namespace UWPDemo.Interfaces
{
    public interface IApiService
    {
        Task<IEnumerable<ValueModel>> GetValueModelsAsync();
    }
}
