public class TrabajadorDto
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = null!;
    public string ApPaterno { get; set; } = null!;
    public string ApMaterno { get; set; } = null!;
    public DateTime FechaIngreso { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string Genero { get; set; } = null!;
    public bool Estado { get; set; }
    public List<HijoDto> Hijos { get; set; } = new();
}

