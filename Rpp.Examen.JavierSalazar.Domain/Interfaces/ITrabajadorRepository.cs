using Rpp.Examen.JavierSalazar.Domain;

namespace Rpp.Examen.JavierSalazar.Domain.Interfaces
{
    using Rpp.Examen.JavierSalazar.Domain.Entities;
    using System.Linq.Expressions;
    public interface ITrabajadorRepository
    {
        Task<IEnumerable<TrabajadorDto>> ListarAsync(
            string? nombreCompleto,
            DateTime? fechaInicio,
            DateTime? fechaFin,
            string? genero,
            string? tieneHijos
        );
        Task<TrabajadorDto?> ObtenerAsync(int id);
        Task<Trabajador> CrearAsync(Trabajador trabajador);
        Task<bool> ActualizarAsync(Trabajador trabajador);
        Task<bool> EliminarAsync(int id);
    }
}