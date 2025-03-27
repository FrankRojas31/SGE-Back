using Base.Application.Service.Interfaces.Contracts.Seguridad;
using Base.Domain.DTO.Security;
using Base.Domain.Entidades.Seguridad;
using Base.Domain.ViewModels;
using Base.Domain.ViewModels.Seguridad;
using Base.Infraestructura.Data.Repositorios.Contrato.Seguridad;
using Base.Infraestructura.Datos.ContextoBD;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static Base.Common.Enumeraciones.Enums;

namespace Base.Application.Services.Interfaces.Implementacion.Seguridad
{
    public class UserAccountService(IAccountRepository accountRepository, IApplicationUserRepository _repository, IConfiguration _config, UserManager<ApplicationUser> _userManager, DataBaseContext _database) : IUserAccountService
    {

        public async Task<TokenModel> RefreshToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                throw new ArgumentNullException(nameof(refreshToken), "Refresh token cannot be null or empty");

            var user = await _database.Users
                .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken)
                ?? throw new SecurityTokenException("Invalid refresh token");

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new SecurityTokenException("Refresh token has expired");

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "ADMIN";

            var userSession = new UserSession(
                Id: user.Id,
                Name: user.UserName,
                Email: user.Email,
                Role: role
            );

            var newAccessToken = accountRepository.GenerateAccessToken(userSession);
            var newRefreshToken = await _repository.GenerateRefreshToken();

            using var transaction = await _database.Database.BeginTransactionAsync();
            try
            {
                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

                _database.Users.Update(user);
                await _database.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

            return new TokenModel
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                AccessTokenExpiration = DateTime.UtcNow.AddDays(1)
            };
        }

        public async Task<ResponseHelper> CreateAccount(UserDTO userDTO)
        {
            ResponseHelper response = new ResponseHelper();
            try
            {
                if (userDTO is null) return new ResponseHelper() { Success = false, Message = "Sin datos." };

                var user = await accountRepository.FindByEmailAsync(userDTO.Email);
                if (user is not null) return new ResponseHelper() { Success = false, Message = "El usuario ya se encuentra registrado." };

                var creationSuccess = await accountRepository.CreateAccount(userDTO);
                if (!creationSuccess) return new ResponseHelper() { Success = false, Message = "Ocurrió un error." };

                response.Success = true;
                response.Message = "Cuenta creada con éxito.";
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            return response;
        }

        public async Task<ResponseHelper> UpdateAccount(UserDTO userDTO)
        {
            ResponseHelper response = new ResponseHelper();
            try
            {
                if (userDTO is null) return new ResponseHelper() { Success = false, Message = "Sin datos." };

                var user = await accountRepository.FindByEmailAsync(userDTO.Email);
                if (user is not null && user.Id.CompareTo(userDTO.Id) != 0) return new ResponseHelper() { Success = false, Message = "El correo ya se encuentra registrado." };

                var updateSuccess = await accountRepository.UpdateAccount(userDTO);
                if (!updateSuccess) return new ResponseHelper() { Success = false, Message = "Ocurrió un error." };

                response.Success = true;
                response.Message = "Cuenta actualizada con éxito.";
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ResponseHelper> UpdateUserData(UserDTO userDTO)
        {
            ResponseHelper response = new ResponseHelper();
            try
            {
                if (userDTO is null) return new ResponseHelper() { Success = false, Message = "Sin datos." };

                var user = await accountRepository.FindByEmailAsync(userDTO.Email);
                if (user is not null && user.Id.CompareTo(userDTO.Id) != 0) return new ResponseHelper() { Success = false, Message = "El correo ya se encuentra registrado." };

                var updateSuccess = await accountRepository.UpdateUserData(userDTO);
                if (!updateSuccess) return new ResponseHelper() { Success = false, Message = "Ocurrió un error." };

                response.Success = true;
                response.Message = "Cuenta actualizada con éxito.";
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ResponseHelper> ChangePassword(ChangePasswordViewModel vm)
        {
            ResponseHelper response = new ResponseHelper();
            try
            {
                if (vm is null || vm.NewPassword == null || vm.CurrentPassword == null)
                {
                    return new ResponseHelper() { Success = false, Message = "Sin datos." };
                }

                if (vm.NewPassword.CompareTo(vm.ConfirmPassword) != 0)
                {
                    return new ResponseHelper() { Success = false, Message = "La confirmación no coincide." };
                }

                var changePasswordSuccess = await accountRepository.ChangePassword(vm);
                if (!changePasswordSuccess) return new ResponseHelper() { Success = false, Message = "Ocurrió un error al actualizar la contraseña, revise que la contraseña anterior sea correcta." };

                response.Success = true;
                response.Message = "Contraseña actualizada con éxito.";
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ResponseHelper> GetUsers()
        {
            ResponseHelper response = new ResponseHelper();
            try
            {
                var roles = await accountRepository.GetAllUsersAsync();
                response.Success = true;
                response.Data = roles;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ResponseHelper> LoginAccount(LoginDTO loginDTO)
        {
            ResponseHelper response = new ResponseHelper();
            try
            {
                if (loginDTO == null)
                    return new ResponseHelper() { Success = false, Message = "NO se encuentran los datos" };

                var getUser = await accountRepository.FindByEmailAsync(loginDTO.Email);
                if (getUser is null)
                    return new ResponseHelper() { Success = false, Message = "Usuario no encontrado" };

                if (getUser.IsDeleted == true || getUser.EstatusUsuario != EstatusUsuario.ACTIVO)
                    return new ResponseHelper() { Success = false, Message = "Usuario Inactivo o esta eliminado" };

                bool checkUserPasswords = await accountRepository.CheckPasswordAsync(getUser, loginDTO);
                if (!checkUserPasswords)
                    return new ResponseHelper() { Success = false, Message = "Usuario o contraseña incorrectos" };

                var getUserRole = await accountRepository.GetRolesAsync(getUser);
                var userSession = new UserSession(getUser.Id, getUser.Name, getUser.Email, getUserRole.First());

                string accessToken = accountRepository.GenerateAccessToken(userSession);
                string refreshToken = await _repository.GenerateRefreshToken();

                _ = int.TryParse(_config["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
                getUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                bool tokenValidation = accessToken == "" || getUser.RefreshToken == "";
                if (tokenValidation)
                    return new ResponseHelper() { Success = false, Message = "Error al iniciar sesión (Tokens). Comuníquese con el administrador" };

                getUser.RefreshToken = refreshToken;
                int result = await _repository.UpdateAsync(getUser);
                if (result == 0)
                    return new ResponseHelper() { Success = false, Message = "Error al iniciar sesión (Refresh). Comuníquese con el administrador" };

                JwtViewModel jwtVM = new JwtViewModel
                {
                    Id = getUser.Id,
                    Email = getUser.Email,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    AccessTokenExpiration = DateTime.UtcNow.AddDays(1),
                    Role = getUserRole.First()
                };
                response.Success = true;
                response.Message = "Se ha iniciado sesión correctamente";
                response.Data = jwtVM;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            return response;
        }

        public async Task<ResponseHelper> GetRoles()
        {
            ResponseHelper response = new ResponseHelper();

            try
            {
                var roles = await accountRepository.GetAllRolesAsync();
                response.Success = true;
                response.Data = roles;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ResponseHelper> GetRolByName(string name)
        {
            ResponseHelper response = new ResponseHelper();

            try
            {
                var rol = await accountRepository.GetRolByNameAsync(name);
                response.Success = true;
                response.Data = rol;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ResponseHelper> GetRolById(string id)
        {
            ResponseHelper response = new ResponseHelper();

            try
            {
                var rol = await accountRepository.GetRolByIdAsync(id);
                response.Success = true;
                response.Data = rol;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ResponseHelper> GetById(string id)
        {
            ResponseHelper response = new ResponseHelper();

            try
            {
                var rol = await accountRepository.GetDTOByIdAsync(id);
                response.Success = true;
                response.Data = rol;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ResponseHelper> GetByEmail(string email)
        {
            ResponseHelper response = new ResponseHelper();

            try
            {
                var rol = await accountRepository.GetDTOByEmailAsync(email);
                response.Success = true;
                response.Data = rol;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ResponseHelper> CreateRol(string rol)
        {
            ResponseHelper response = new ResponseHelper();
            try
            {
                var result = await accountRepository.CreateRol(rol);

                if (result)
                {
                    response.Success = true;
                    response.Message = $"Rol {rol} creado con éxito";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Error al crear el rol";
                }
            }
            catch (Exception e)
            {
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ResponseHelper> UpdateRol(RoleDTO rolDto)
        {
            ResponseHelper response = new ResponseHelper();
            try
            {
                IdentityRole rol = new IdentityRole()
                {
                    Id = rolDto.Id,
                    Name = rolDto.Name
                };

                var result = await accountRepository.UpdateRol(rol);

                if (result)
                {
                    response.Success = true;
                    response.Message = $"Rol {rol.Name} actualizado con éxito";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Error al actualizar el rol";
                }
            }
            catch (Exception e)
            {
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ResponseHelper> DeleteRol(string id)
        {
            ResponseHelper response = new ResponseHelper();
            try
            {
                IdentityRole rol = await accountRepository.GetRolByIdAsync(id);
                var result = await accountRepository.DeleteRol(rol);

                if (result)
                {
                    response.Success = true;
                    response.Message = $"Rol {rol.Name} eliminado con éxito";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Error al eliminar el rol";
                }
            }
            catch (Exception e)
            {
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ResponseHelper> DeleteUser(string id)
        {
            ResponseHelper response = new ResponseHelper();
            try
            {
                ApplicationUser user = await accountRepository.FindByIdAsync(id);
                var result = await accountRepository.DeleteUser(user);

                if (result)
                {
                    response.Success = true;
                    response.Message = $"Usuario {user.Name} eliminado con éxito";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Error al eliminar el usuario";
                }
            }
            catch (Exception e)
            {
                response.Message = e.Message;
            }

            return response;
        }
    }
}
