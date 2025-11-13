using Rpp.Examen.JavierSalazar.Application.DTO;
using Rpp.Examen.JavierSalazar.Application.Domain;

namespace Rpp.Examen.JavierSalazar.Application.Interfaces
{
    public interface ILibeyUserRepository
    {
        LibeyUserResponse FindResponse(string documentNumber);
        void Create(LibeyUser libeyUser);
    }
}
