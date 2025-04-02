using Base.Application.Services.Interfaces.Contrato;
using Base.Domain.DTO.Security;
using Base.Domain.ViewModels;
using Base.Infraestructura.Data.Repositorios.Contrato.Seguridad;
using Base.Infraestructura.Datos.ContextoBD;
using static Base.Common.Enumeraciones.Enums;

namespace Base.Application.Services.Interfaces.Implementacion
{
    public class SeedersServices : ISeedersServices
    {
        private readonly DataBaseContext _context;
        private readonly IAccountRepository _accountRepository;
        public SeedersServices(DataBaseContext context, IAccountRepository accountRepository)
        {
            _context = context;
            _accountRepository = accountRepository;
        }

        public async Task<ResponseHelper> Seeders()
        {
            try
            {
                if (!_context.Users.Any())
                {
                    string rolAdmin = "ADMIN";
                    string rolProfesor = "PROFESOR";
                    string rolServiciosEscolares = "SERVICIOS ESCOLARES";

                    await _accountRepository.CreateRol(rolAdmin);
                    await _accountRepository.CreateRol(rolProfesor);
                    await _accountRepository.CreateRol(rolServiciosEscolares);

                    List<UserDTO> users = [];

                    users.Add(new UserDTO
                    {
                        Email = "admin@admin.com",
                        Password = "Adm1nistr@d0r.2025",
                        ConfirmPassword = "Adm1nistr@d0r.2025",
                        EstatusUsuario = EstatusUsuario.ACTIVO,
                        Name = "Administrador",
                        Rol = "ADMIN"
                    });

                    users.Add(new UserDTO
                    {
                        Email = "profesor@profesor.com",
                        Password = "Pr0f3s0r.2025",
                        ConfirmPassword = "Pr0f3s0r.2025",
                        EstatusUsuario = EstatusUsuario.ACTIVO,
                        Name = "Profesor",
                        Rol = "PROFESOR"
                    });

                    users.Add(new UserDTO
                    {
                        Email = "servicios_escolares@servicios_escolares.com",
                        Password = "S3rv1c1os.2025",
                        ConfirmPassword = "S3rv1c1os.2025",
                        EstatusUsuario = EstatusUsuario.ACTIVO,
                        Name = "Servicios Escolares",
                        Rol = "SERVICIOS ESCOLARES"
                    });

                    foreach (UserDTO user in users)
                    {
                        bool response = await _accountRepository.CreateAccount(user);

                        if(response == false)
                        {
                            return new ResponseHelper
                            {
                                Success = false,
                                Message = $"Ocurrio un error con el usuario con nombre... {user.Name}"
                            };
                        }
                    }

                    return new ResponseHelper
                    {
                        Success = true,
                        Message = "¡Correcto... Se ha inicializado el Seeder con Exito",
                        Data = users
                    };
                }

                return new ResponseHelper
                {
                    Success = false,
                    Message = "¡Ya se ha inicializado el seeder.. anteriormente!"
                };
            }
            catch (Exception ex)
            {
                return new ResponseHelper
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
