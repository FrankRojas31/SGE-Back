using Base.Application.Service.Interfaces.Contracts.Seguridad;
using Base.Domain.DTO.Security;
using Base.Domain.Entidades.Seguridad;
using Base.Domain.ViewModels.Seguridad;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Base.API.Controllers.Seguridad
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IUserAccountService userAccountService) : ControllerBase
    {

        [HttpPost("refresh")]
        public async Task<ActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            if (string.IsNullOrEmpty(request?.RefreshToken))
                return BadRequest(new { message = "Refresh token is required" });

            try
            {
                var tokens = await userAccountService.RefreshToken(request.RefreshToken);
                return Ok(tokens);
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            var response = await userAccountService.CreateAccount(userDTO);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var response = await userAccountService.LoginAccount(loginDTO);
            return Ok(response);
        }

        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRol(RoleDTO role)
        {
            var response = await userAccountService.CreateRol(role.Name);
            return Ok(response);
        }

        [HttpPut]
        [Route("UpdateRole")]
        public async Task<IActionResult> UpdateRol(RoleDTO role)
        {
            var response = await userAccountService.UpdateRol(role);
            return Ok(response);
        }

        [HttpDelete]
        [Route("DeleteRole")]
        public async Task<IActionResult> DeleteRol(string id)
        {
            var response = await userAccountService.DeleteRol(id);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            var response = await userAccountService.GetRoles();
            return Ok(response);
        }

        [HttpGet]
        [Route("GetRoleByName/{name}")]
        public async Task<IActionResult> GetRolByName(string name)
        {
            var response = await userAccountService.GetRolByName(name);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetRoleById/{id}")]
        public async Task<IActionResult> GetRolById(string id)
        {
            var response = await userAccountService.GetRolById(id);
            return Ok(response);
        }

        [HttpPut]
        [Route("UpdateAccount")]
        public async Task<IActionResult> UpdateAccount(UserDTO user)
        {
            var response = await userAccountService.UpdateAccount(user);
            return Ok(response);
        }

        [HttpPut]
        [Route("UpdateUserData")]
        public async Task<IActionResult> UpdateUserData(UserDTO user)
        {
            var response = await userAccountService.UpdateUserData(user);
            return Ok(response);
        }

        [HttpPut]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassowrd(ChangePasswordViewModel vm)
        {
            var response = await userAccountService.ChangePassword(vm);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var response = await userAccountService.GetUsers();
            return Ok(response);
        }

        [HttpGet]
        [Route("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var response = await userAccountService.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetUserByEmail/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var response = await userAccountService.GetByEmail(email);
            return Ok(response);
        }
        [HttpDelete]
        [Route("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var response = await userAccountService.DeleteUser(id);
            return Ok(response);
        }
    }
}
