using challenge.Models;
using System;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation GetCompensationById(String id);
        Compensation CreateCompensation(Compensation compensation);
        Task SaveAsync();
    }
}