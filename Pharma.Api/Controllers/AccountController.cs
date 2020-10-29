using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using IPharmaAuth0.Helpers;
using IPharmaAuth0.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pharma.Api.Models;
using Pharma.Api.Services;
using Pharma.Common.Auth0;
using Pharma.Common.Model;

namespace Pharma.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IJwtGenerateToken _jwtGenerateToken;

        public AccountController(
            IAccountService accountService,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IJwtGenerateToken jwtGenerateToken)
        {
            _accountService = accountService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _jwtGenerateToken = jwtGenerateToken;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateModel model)
        {
            var account = _accountService.Authenticate(model.Username, model.Password);

            if (account is null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var token = await _jwtGenerateToken.GenerateToken();

            return Ok(new
            {
                account.Id,
                account.Username,
                account.FirstName,
                account.LastName,
                Token = token
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            var account = _mapper.Map<Account>(model);

            try
            {
                _accountService.Create(account, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var accounts = _accountService.GetAll();
            var model = _mapper.Map<IList<AccountModel>>(accounts);
            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var account = _accountService.GetById(id);
            var model = _mapper.Map<AccountModel>(account);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateModel model)
        {
            // map model to entity and set id
            var account = _mapper.Map<Account>(model);
            account.Id = id;

            try
            {
                // update account 
                _accountService.Update(account, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _accountService.Delete(id);
            return Ok();
        }
    }
}
