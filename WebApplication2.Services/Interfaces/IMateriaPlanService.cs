using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.Core.Common;
using WebApplication2.Core.DTOs;
using WebApplication2.Core.Requests.NewFolder;
using WebApplication2.Core.Models;
using WebApplication2.Core.Common;
using WebApplication2.Core.Models;

namespace WebApplication2.Services.Interfaces
{
    public interface IMateriaPlanService
    {
        Task<PagedResult<MateriaPlan>> GetMateriaPlanes(int page, int pageSize);
        Task<MateriaPlan> GetMateriaPlanDetalle(int id);
        Task<MateriaPlan> CrearMateriaPlan(MateriaPlan materiaPlan);
        Task<MateriaPlan> ActualizarMateriaPlan(MateriaPlan newMateriaPlan);

    }
}
