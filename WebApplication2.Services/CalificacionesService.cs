using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.Core.Common;
using WebApplication2.Core.Models;
using WebApplication2.Data.DbContexts;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Services
{
    public class CalificacionesService : ICalificacionesService
    {
        private readonly ApplicationDbContext _dbContext;

        public CalificacionesService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CalificacionParcial> AbrirParcial(CalificacionParcial newActa)
        {
            await _dbContext.CalificacionesParciales.AddAsync(newActa);
            await _dbContext.SaveChangesAsync();

            return newActa;
        }

        public async Task CambiarEstadoParcial(int calificacionParcialId, string nuevoEstado, string usuario)
        {
            throw new NotImplementedException();
        }

        public async Task<(IList<CalificacionDetalle>, decimal CalificacionFinal)> GetConcentradoAlumno(int inscripcionId)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<(int InscripcionId, decimal AporteParcial)>> GetConcentradoGrupoParcial(int grupoMateriaId, int parcialId)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<CalificacionDetalle>> GetDetalles(int grupoMateriaId, int parcialId, int inscripcionId, int tipoEvaluacionEnum, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<CalificacionParcial> GetParcialById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CalificacionParcial>> GetParcialesPorGrupo(int grupoMateriaId, int parcialId)
        {
            throw new NotImplementedException();
        }

        public async Task UpsertDetalle(CalificacionDetalle detalle, string applicationUserName)
        {
            throw new NotImplementedException();
        }
    }
}
