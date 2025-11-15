
namespace BLL.AutoMapper
{
    internal class DomainProfile : Profile
    {
        public DomainProfile() 
        {
            // notifications
            CreateMap<Notification, GetNotificationVM>().ReverseMap();
            CreateMap<Notification, CreateNotificationVM>().ReverseMap();
        } 

    }
}
