namespace Rpp.Examen.JavierSalazar.Domain.Entities
{
    public class Persona
    {
        public int Id { get; set; }
        public string ApPaterno { get; set; } = null!;
        public string ApMaterno { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public char Genero { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool Estado { get; set; }

        // Navegación
        public ICollection<Trabajador>? Trabajadores { get; set; }
        public ICollection<Hijo>? Hijos { get; set; }
    }
}