using Data.Class;

namespace Infra.Business
{
    public interface IBusinessBase
    {
        IUnitOfWork _unitOfWork { get; set; }

        void Dispose();
    }
}