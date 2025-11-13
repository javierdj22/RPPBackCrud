
using Rpp.Examen.JavierSalazar.Domain.Entities;
using Rpp.Examen.JavierSalazar.Domain.Interfaces;

namespace Rpp.Examen.JavierSalazar.Application.Interfaces
{
    public class TrabajadorService
    {
        private readonly ITrabajadorRepository _repo;
        public TrabajadorService(ITrabajadorRepository repo) => _repo = repo;

        public Task<IEnumerable<TrabajadorDto>> ListarAsync(string? nombre, DateTime? inicio, DateTime? fin, string? genero, string? hijos) =>
            _repo.ListarAsync(nombre, inicio, fin, genero, hijos);

        public Task<TrabajadorDto?> ObtenerAsync(int id) => _repo.ObtenerAsync(id);
        public Task<Trabajador> CrearAsync(Trabajador t) => _repo.CrearAsync(t);
        public Task<bool> ActualizarAsync(Trabajador t) => _repo.ActualizarAsync(t);
        public Task<bool> EliminarAsync(int id) => _repo.EliminarAsync(id);
    }

}