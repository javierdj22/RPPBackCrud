using Rpp.Examen.JavierSalazar.Application.DTO;
namespace Rpp.Examen.JavierSalazar.Application.Interfaces
{
    public interface ILibeyUserAggregate
    {
        LibeyUserResponse FindResponse(string documentNumber);
        void Create(UserUpdateorCreateCommand command);
    }
}