using GrowDay.Domain.DTO;
using GrowDay.Domain.Helpers;
using GrowDay.Domain.ViewModels;
using Microsoft.AspNetCore.Http;

namespace GrowDay.Application.Services
{
    public interface IAccountService
    {
        Task<Result<int>>UpdateAccount(string username,UpdateAccountDTO updateAccountDTO,HttpResponse httpResponse);
        Task<Result<GetAccountData>> GetAccountData(string username);
        Task<Result<int>>DeleteAccount(string username);

    }

}
