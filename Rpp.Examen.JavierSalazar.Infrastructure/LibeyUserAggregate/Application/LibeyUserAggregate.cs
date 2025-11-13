using Rpp.Examen.JavierSalazar.LibeyUserAggregate.Application.DTO;
using Rpp.Examen.JavierSalazar.LibeyUserAggregate.Application.Interfaces;
using Rpp.Examen.JavierSalazar.LibeyUserAggregate.Domain;
namespace Rpp.Examen.JavierSalazar.LibeyUserAggregate.Application
{
    public class LibeyUserAggregate : ILibeyUserAggregate
    {
        private readonly ILibeyUserRepository _repository;
        public LibeyUserAggregate(ILibeyUserRepository repository)
        {
            _repository = repository;
        }
        public void Create(UserUpdateorCreateCommand command)
        {
            throw new NotImplementedException();
        }
        public LibeyUserResponse FindResponse(string documentNumber)
        {
            var row = _repository.FindResponse(documentNumber);
            return row;
        }
    }
}