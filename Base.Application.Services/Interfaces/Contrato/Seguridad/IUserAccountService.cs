using Base.Domain.DTO.Security;
using Base.Domain.Entidades.Seguridad;
using Base.Domain.ViewModels;
using Base.Domain.ViewModels.Seguridad;

namespace Base.Application.Service.Interfaces.Contracts.Seguridad
{
    public interface IUserAccountService
    {
        Task<ResponseHelper> CreateRol(string rol);
        Task<ResponseHelper> UpdateRol(RoleDTO rolDto);
        Task<ResponseHelper> DeleteRol(string id);
        Task<ResponseHelper> GetRoles();
        Task<ResponseHelper> GetRolByName(string name);
        Task<ResponseHelper> GetRolById(string id);

        Task<ResponseHelper> GetUsers();
        Task<ResponseHelper> GetById(string id);
        Task<ResponseHelper> GetByEmail(string id);
        Task<ResponseHelper> CreateAccount(UserDTO userDTO);
        Task<ResponseHelper> UpdateAccount(UserDTO userDTO);
        Task<ResponseHelper> UpdateUserData(UserDTO userDTO);
        Task<ResponseHelper> ChangePassword(ChangePasswordViewModel vm);
        Task<ResponseHelper> LoginAccount(LoginDTO loginDTO);
        Task<ResponseHelper> DeleteUser(string id);
        Task<TokenModel> RefreshToken(string refreshToken);
    }
}
