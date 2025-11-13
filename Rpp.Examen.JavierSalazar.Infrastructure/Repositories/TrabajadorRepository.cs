using Microsoft.EntityFrameworkCore;
using Rpp.Examen.JavierSalazar.Domain.Entities;
using Rpp.Examen.JavierSalazar.Domain.Interfaces;
using Rpp.Examen.JavierSalazar.Infrastructure;

namespace Rpp.Examen.JavierSalazar.EFCore
{
    public class TrabajadorRepository : ITrabajadorRepository
    {
        private readonly AppDbContext _context;
        public TrabajadorRepository(AppDbContext context) => _context = context;

        // Listar Trabajadores con filtros y DTOs
        public async Task<IEnumerable<TrabajadorDto>> ListarAsync(
            string? nombreCompleto,
            DateTime? fechaInicio,
            DateTime? fechaFin,
            string? genero,
            string? tieneHijos)
        {
            var query = _context.Trabajador
                .Include(t => t.Persona)
                .Include(t => t.Hijos)
                    .ThenInclude(h => h.Persona)
                .AsQueryable();

            if (!string.IsNullOrEmpty(nombreCompleto))
                query = query.Where(t =>
                    (t.Persona.Nombres + " " + t.Persona.ApPaterno + " " + t.Persona.ApMaterno)
                    .Contains(nombreCompleto));

            if (fechaInicio.HasValue)
                query = query.Where(t => t.Persona.FechaNacimiento >= fechaInicio.Value);

            if (fechaFin.HasValue)
                query = query.Where(t => t.Persona.FechaNacimiento <= fechaFin.Value);

            if (!string.IsNullOrEmpty(genero))
                query = query.Where(t => t.Persona.Genero.ToString() == genero);

            if (!string.IsNullOrEmpty(tieneHijos))
            {
                if (tieneHijos.ToUpper() == "S")
                    query = query.Where(t => t.Hijos.Any());
                else if (tieneHijos.ToUpper() == "N")
                    query = query.Where(t => !t.Hijos.Any());
            }

            return await query
                .Select(t => new TrabajadorDto
                {
                    Id = t.Id,
                    NombreCompleto = t.Persona.Nombres + " " + t.Persona.ApPaterno + " " + t.Persona.ApMaterno,
                    FechaIngreso = t.FechaIngreso,
                    Genero = t.Persona.Genero.ToString(),
                    Estado = t.Estado,
                    Hijos = t.Hijos.Select(h => new HijoDto
                    {
                        Id = h.Id,
                        NombreCompleto = h.Persona.Nombres + " " + h.Persona.ApPaterno + " " + h.Persona.ApMaterno,
                        Estado = h.Estado
                    }).ToList()
                })
                .ToListAsync();
        }

        // Obtener por Id con DTO
        public async Task<TrabajadorDto?> ObtenerAsync(int id)
        {
            var t = await _context.Trabajador
                .Include(t => t.Persona)
                .Include(t => t.Hijos)
                    .ThenInclude(h => h.Persona)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (t == null) return null;

            return new TrabajadorDto
            {
                Id = t.Id,
                NombreCompleto = t.Persona.Nombres,
                ApPaterno = t.Persona.ApPaterno,
                ApMaterno = t.Persona.ApMaterno,
                FechaIngreso = t.FechaIngreso,
                Genero = t.Persona.Genero.ToString(),
                Estado = t.Estado,
                Hijos = t.Hijos.Select(h => new HijoDto
                {
                    Id = h.Id,
                    NombreCompleto = h.Persona.Nombres,
                    ApPaterno = t.Persona.ApPaterno,
                    ApMaterno = t.Persona.ApMaterno,
                    Estado = h.Estado
                }).ToList()
            };
        }

        // Crear trabajador
        public async Task<Trabajador> CrearAsync(Trabajador trabajador)
        {
            // Asignar valores automáticos
            trabajador.Persona.FechaRegistro = DateTime.Now;       
            trabajador.Persona.Estado = true;           

            // Si viene con una Persona asociada, la agregamos primero
            if (trabajador.Persona != null)
            {
                _context.Personas.Add(trabajador.Persona);
            }

            // Agregar el trabajador (Entity Framework maneja la relación automáticamente)
            _context.Trabajador.Add(trabajador);

            // Guardar cambios
            await _context.SaveChangesAsync();

            return trabajador;
        }


        // Actualizar trabajador
        public async Task<bool> ActualizarAsync(Trabajador trabajador)
        {
            _context.Trabajador.Update(trabajador);
            return await _context.SaveChangesAsync() > 0;
        }

        // Eliminar trabajador (cambiar estado a 0)
        public async Task<bool> EliminarAsync(int id)
        {
            var entity = await _context.Trabajador.FindAsync(id);
            if (entity == null) return false;

            // Cambiar estado a inactivo (false)
            entity.Estado = false;

            _context.Trabajador.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
