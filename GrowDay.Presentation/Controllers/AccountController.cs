using GrowDay.Application.Services;
using GrowDay.Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GrowDay.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        protected readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("GetAccountData")]
        public async Task<IActionResult> GetAccountData()
        {
            var user = User.FindFirstValue(ClaimTypes.Name);
            var result = await _accountService.GetAccountData(user!);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpPut("UpdateAccount")]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountDTO updateAccountDTO)
        {
            var user = User.FindFirstValue(ClaimTypes.Name);
            var result = await _accountService.UpdateAccount(user!, updateAccountDTO, Response);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpDelete("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = User.FindFirstValue(ClaimTypes.Name);
            var result = await _accountService.DeleteAccount(user!);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}