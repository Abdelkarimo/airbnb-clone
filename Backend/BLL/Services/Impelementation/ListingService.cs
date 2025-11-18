
namespace BLL.Services.Impelementation
{
    class ListingService /*: IListingService*/
    {
        private readonly IUnitOfWork unitOfWork;

        public ListingService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
       

    }
}
