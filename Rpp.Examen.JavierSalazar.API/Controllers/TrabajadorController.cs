using Microsoft.AspNetCore.Mvc;
using Rpp.Examen.JavierSalazar.Application.Interfaces;
using Rpp.Examen.JavierSalazar.Domain.Entities;

[ApiController]
[Route("api/[controller]")]
public class TrabajadorController : ControllerBase
{
    private readonly TrabajadorService _service;
    public TrabajadorController(TrabajadorService service) => _service = service;

    [HttpPost("listar")]
    public async Task<IActionResult> Listar([FromBody] TrabajadorFilter filter)
    {
        var result = await _service.ListarAsync(
            filter.NombreCompleto,
            filter.FechaNacimientoInicio,
            filter.FechaNacimientoFin,
            filter.Genero,
            filter.TieneHijos
        );
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Obtener(int id)
    {
        var trabajador = await _service.ObtenerAsync(id);
        return trabajador is null ? NotFound() : Ok(trabajador);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] Trabajador trabajador)
    {
        var created = await _service.CrearAsync(trabajador);
        return CreatedAtAction(nameof(Obtener), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] Trabajador trabajador)
    {
        trabajador.Id = id;
        var ok = await _service.ActualizarAsync(trabajador);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var result = await _service.EliminarAsync(id);
        if (!result)
            return NotFound(new { message = "Trabajador no encontrado" });

        return Ok(new { message = "Trabajador inactivado correctamente" });
    }
}

public class TrabajadorFilter
{
    public string? NombreCompleto { get; set; }
    public DateTime? FechaNacimientoInicio { get; set; }
    public DateTime? FechaNacimientoFin { get; set; }
    public string? Genero { get; set; }
    public string TieneHijos { get; set; } = "S";
}
