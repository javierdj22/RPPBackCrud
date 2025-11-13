namespace Rpp.Examen.JavierSalazar.Domain.Entities
{
    public class Trabajador
    {
        public int Id { get; set; }
        public int IdPersona { get; set; }
        public DateTime FechaIngreso { get; set; }
        public bool Estado { get; set; }

        // Navegación
        public Persona Persona { get; set; } = null!;
        public ICollection<Hijo>? Hijos { get; set; }
    }
}