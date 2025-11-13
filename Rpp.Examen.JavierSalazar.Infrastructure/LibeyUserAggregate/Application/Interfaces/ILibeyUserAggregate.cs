using Rpp.Examen.JavierSalazar.LibeyUserAggregate.Application.DTO;
namespace Rpp.Examen.JavierSalazar.LibeyUserAggregate.Application.Interfaces
{
    public interface ILibeyUserAggregate
    {
        LibeyUserResponse FindResponse(string documentNumber);
        void Create(UserUpdateorCreateCommand command);
    }
}