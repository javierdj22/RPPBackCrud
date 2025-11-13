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
                FechaNacimiento = t.Persona.FechaNacimiento,
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
            // Cargar el trabajador existente con sus relaciones
            var existente = await _context.Trabajador
                .Include(t => t.Persona)
                .Include(t => t.Hijos)
                .ThenInclude(h => h.Persona) // si tienes relación Hijo → Persona en el modelo EF
                .FirstOrDefaultAsync(t => t.Id == trabajador.Id);

            if (existente == null)
                return false;

            // ✅ Actualizar datos del trabajador
            existente.FechaIngreso = trabajador.FechaIngreso;
            existente.Estado = trabajador.Estado;
            // otros campos de trabajador si los hay

            // ✅ Actualizar Persona del trabajador
            if (trabajador.Persona != null)
            {
                existente.Persona.ApPaterno = trabajador.Persona.ApPaterno;
                existente.Persona.ApMaterno = trabajador.Persona.ApMaterno;
                existente.Persona.Nombres = trabajador.Persona.Nombres;
                existente.Persona.Genero = trabajador.Persona.Genero;
                existente.Persona.FechaNacimiento = trabajador.Persona.FechaNacimiento;
                existente.Persona.Estado = trabajador.Persona.Estado;
            }

            // ✅ Actualizar hijos (tabla Hijos + tabla Persona)
            if (trabajador.Hijos != null)
            {
                // Eliminar relaciones de hijos que ya no están
                var hijosAEliminar = existente.Hijos
                    .Where(h => !trabajador.Hijos.Any(n => n.Id == h.Id))
                    .ToList();
                _context.Hijos.RemoveRange(hijosAEliminar);

                foreach (var hijo in trabajador.Hijos)
                {
                    if (hijo.IdHijo == 0)
                    {
                        // 🧩 Crear nueva Persona para el hijo
                        var nuevaPersonaHijo = new Persona
                        {
                            Nombres = hijo.Persona.Nombres,
                            ApPaterno = hijo.Persona.ApPaterno,
                            ApMaterno = hijo.Persona.ApMaterno,
                            Estado = true,
                            FechaRegistro = DateTime.Now
                        };
                        _context.Personas.Add(nuevaPersonaHijo);
                        await _context.SaveChangesAsync(); // Para obtener el IdPersona generado

                        // Crear relación en Hijos
                        var nuevoHijo = new Hijo
                        {
                            IdTrabajador = existente.Id,
                            IdHijo = nuevaPersonaHijo.Id,
                            Estado = true
                        };
                        _context.Hijos.Add(nuevoHijo);
                    }
                    else
                    {
                        // 🧩 Si el hijo ya existe, actualizar la Persona del hijo
                        var personaHijo = await _context.Personas.FirstOrDefaultAsync(p => p.Id == hijo.IdHijo);
                        if (personaHijo != null)
                        {
                            personaHijo.Nombres = hijo.Persona.Nombres;
                            personaHijo.ApPaterno = hijo.Persona.ApPaterno;
                            personaHijo.ApMaterno = hijo.Persona.ApMaterno;
                            personaHijo.Estado = hijo.Estado;
                        }
                    }
                }
            }

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
