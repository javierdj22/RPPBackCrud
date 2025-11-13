namespace Rpp.Examen.JavierSalazar.Domain.Entities
{
    public class Hijo
    {
        public int Id { get; set; }
        public int IdTrabajador { get; set; }
        public int IdHijo { get; set; }
        public bool Estado { get; set; }

        // Navegación
        public Trabajador Trabajador { get; set; } = null!;
        public Persona Persona { get; set; } = null!;
    }
}