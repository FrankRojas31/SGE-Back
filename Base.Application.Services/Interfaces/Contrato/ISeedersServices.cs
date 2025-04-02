using Base.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Application.Services.Interfaces.Contrato
{
    public interface ISeedersServices
    {
        Task<ResponseHelper> Seeders();
    }
}
