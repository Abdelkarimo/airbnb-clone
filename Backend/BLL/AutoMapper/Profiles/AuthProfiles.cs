namespace BLL.AutoMapper.Profiles
{
    public class AuthProfiles : Profile
    {
        public AuthProfiles()
        { // User to UserVM mapping
            CreateMap<User, UserVM>();
            CreateMap<RegisterVM, User>()
              .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
              .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
              .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
              .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
              .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.UtcNow))
              .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        }
    }
}
