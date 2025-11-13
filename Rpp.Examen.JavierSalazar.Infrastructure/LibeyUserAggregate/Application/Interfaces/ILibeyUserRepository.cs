using Rpp.Examen.JavierSalazar.LibeyUserAggregate.Application.DTO;
using Rpp.Examen.JavierSalazar.LibeyUserAggregate.Domain;

namespace Rpp.Examen.JavierSalazar.LibeyUserAggregate.Application.Interfaces
{
    public interface ILibeyUserRepository
    {
        LibeyUserResponse FindResponse(string documentNumber);
        void Create(LibeyUser libeyUser);
    }
}
