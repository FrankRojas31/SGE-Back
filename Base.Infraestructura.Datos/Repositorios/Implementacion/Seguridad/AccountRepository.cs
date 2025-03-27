using Base.Domain.DTO.Security;
using Base.Domain.Entidades.Seguridad;
using Base.Domain.ViewModels.Seguridad;
using Base.Infraestructura.Data.Repositorios.Contrato.Seguridad;
using Base.Infraestructura.Datos.ContextoBD;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Base.Infraestructura.Data.Repositorios.Implementacion.Seguridad
{
    public class AccountRepository(
        DataBaseContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration config)
        : IAccountRepository
    {
        public async Task<bool> CreateAccount(UserDTO userDTO)
        {
            
            var checkUser = await userManager.FindByEmailAsync(userDTO.Email);
            if (checkUser != null)
            {
                return false;
            }

            var checkrol = await roleManager.FindByNameAsync(userDTO.Rol);
            if (checkrol == null)
            {
                return false;
            }

            var newUser = new ApplicationUser()
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                NormalizedEmail = userDTO.Email.ToUpper(),
                UserName = userDTO.Email,
                NormalizedUserName = userDTO.Email.ToUpper(),
                PasswordHash = userDTO.Password,
                EstatusUsuario = userDTO.EstatusUsuario,
                RowVersion = DateTime.Now,
            };
            
            var createUser = await userManager.CreateAsync(newUser!, userDTO.Password);
            if (!createUser.Succeeded)
            {
                return false;
            }else
            {
                await userManager.AddToRoleAsync(newUser, userDTO.Rol);
            }
            
            return true;
        }

        public async Task<bool> UpdateAccount(UserDTO userDTO)
        {
            var checkUser = await userManager.FindByEmailAsync(userDTO.Email);
            if (checkUser != null && checkUser.Id.CompareTo(userDTO.Id) != 0)
            {
                return false;
            }

            var checkrol = await roleManager.FindByNameAsync(userDTO.Rol);
            if (checkrol == null)
            {
                return false;
            }

            var dbUser = await this.UpdateUser(userDTO);
            if (dbUser == null)
            {
                return false;
            }
            else
            {
                if (!userDTO.Password.IsNullOrEmpty()) { 
                    //Remover contraseña anterior
                    var removePass = await userManager.RemovePasswordAsync(dbUser);
                    //Asignar la nueva contraseña
                    var addPass = await userManager.AddPasswordAsync(dbUser, userDTO.Password);
                }

                //Obtener viejos roles
                var dbRoles = await userManager.GetRolesAsync(dbUser);
                //Eliminar viejos roles
                var removeRoles = await userManager.RemoveFromRolesAsync(dbUser, dbRoles);
                //Agregar al nuevo rol
                var addRoles = await userManager.AddToRoleAsync(dbUser, userDTO.Rol);
            }

            return true;
        }

        public async Task<bool> UpdateUserData(UserDTO userDTO)
        {
            var checkUser = await userManager.FindByEmailAsync(userDTO.Email);
            if (checkUser != null && checkUser.Id.CompareTo(userDTO.Id) != 0)
            {
                return false;
            }

            var dbUser = await this.UpdateUser(userDTO);
            if (dbUser == null)
            {
                return false;
            }

            return true;
        }

        private async Task<ApplicationUser> UpdateUser(UserDTO userDTO)
        {
            var dbUser = await userManager.FindByIdAsync(userDTO.Id);
            dbUser.Name = userDTO.Name;
            dbUser.Email = userDTO.Email;
            dbUser.UserName = userDTO.Email;
            dbUser.NormalizedEmail = userDTO.Email.ToUpper();
            dbUser.NormalizedUserName = userDTO.Email.ToUpper();
            dbUser.EstatusUsuario = userDTO.EstatusUsuario;
            dbUser.RowVersion = DateTime.Now;

            var updateUser = await userManager.UpdateAsync(dbUser);
            if (!updateUser.Succeeded)
            {
                return null;
            }

            return dbUser;
        }

        public async Task<bool> ChangePassword(ChangePasswordViewModel vm)
        {
            var checkUser = await userManager.FindByIdAsync(vm.Id);
            if (checkUser == null)
            {
                return false;
            }

            var changePass = await userManager.ChangePasswordAsync(checkUser, vm.CurrentPassword, vm.NewPassword);
            if (!changePass.Succeeded)
            {
                return false;
            }
            return true;
        }

        public async Task<ApplicationUser> FindByIdAsync(string id)
        {
            return await userManager.FindByIdAsync(id);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<UserDTO> GetDTOByIdAsync(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);
            IList<string> roles = await userManager.GetRolesAsync(user);

            UserDTO dto = new UserDTO
            {
                Name = user.Name,
                Email = user.Email,
                Password = "",
                ConfirmPassword = "",
                EstatusUsuario = user.EstatusUsuario,
            };
            if (roles.Count > 0) dto.Rol = roles.First();
            
            return dto;
        }

        public async Task<UserDTO> GetDTOByEmailAsync(string email)
        {
            ApplicationUser user = await userManager.FindByEmailAsync(email);
            IList<string> roles = await userManager.GetRolesAsync(user);

            UserDTO dto = new UserDTO
            {
                Name = user.Name,
                Email = user.Email,
                Password = "",
                ConfirmPassword = "",
                EstatusUsuario = user.EstatusUsuario,
            };
            if (roles.Count > 0) dto.Rol = roles.First();

            return dto;
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, LoginDTO loginDTO)
        {
            return await userManager.CheckPasswordAsync(user, loginDTO.Password);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await userManager.GetRolesAsync(user);
        }

        public string GenerateAccessToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //ROLES
        public async Task<bool> CreateRol(string rol)
        {
            bool success = false;
            var checkAdmin = await roleManager.FindByNameAsync(rol);
            if (checkAdmin is null)
            {
                var result = await roleManager.CreateAsync(new IdentityRole() { Name = rol });

                success = result.Succeeded;
            }

            return success;
        }

        public async Task<bool> UpdateRol(IdentityRole rol)
        {
            bool success = false;
            var checkAdmin = await roleManager.FindByNameAsync(rol.Name);
            if (checkAdmin is null || checkAdmin.Id.Equals(rol.Id))
            {
                IdentityRole bdrol = await roleManager.FindByIdAsync(rol.Id);
                bdrol.Name = rol.Name;
                bdrol.NormalizedName = rol.Name.ToUpper();
                var result = await roleManager.UpdateAsync(bdrol);

                success = result.Succeeded;
            }

            return success;
        }

        public async Task<bool> DeleteRol(IdentityRole rol)
        {
            bool success = false;
            
            //Se valida que el rol no este relacionado a algun usuario
            string sqlValidation = @"SELECT count(*) FROM AspNetRoles as r right join AspNetUserRoles as ur on(r.id = ur.RoleId) where r.id = @roleId";
            var users = await context.Database.GetDbConnection().QueryFirstOrDefaultAsync<int>(sqlValidation, new { roleId = rol.Id });

            if (users == 0)
            {
                var result = await roleManager.DeleteAsync(rol);
                success = result.Succeeded;
            }

            return success;
        }

        public async Task<List<IdentityRole>> GetAllRolesAsync()
        {
            List<IdentityRole> roles = new List<IdentityRole>();

            roles = roleManager.Roles.ToList();

            return roles;
        }

        public async Task<IdentityRole> GetRolByNameAsync(string name)
        {
            IdentityRole rol = await roleManager.FindByNameAsync(name);

            return rol;
        }

        public async Task<IdentityRole> GetRolByIdAsync(string id)
        {
            IdentityRole rol = await roleManager.FindByIdAsync(id);

            return rol;
        }

        //USUARIOS
        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            List<ApplicationUser> users = [.. userManager.Users.Where(p => p.IsDeleted == false)];
            return users;
        }

        public bool AnyRoles()
        {
            return context.Roles.Any();
        }

        public bool AnyUsers()
        {
            return context.Users.Any();
        }

        public async Task<bool> DeleteUser(ApplicationUser user)
        {
            bool success = false;
            var checkUser = await userManager.FindByIdAsync(user.Id);
            if (checkUser != null)
            {
                checkUser.IsDeleted = true;
                var result = await userManager.UpdateAsync(checkUser);
                success = result.Succeeded;
            }

            return success;
        }
    }
}
