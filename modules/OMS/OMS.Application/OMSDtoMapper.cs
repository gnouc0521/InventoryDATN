using AutoMapper;

namespace bbk.netcore.mdl.OMS.Application
{
  internal static class OMSDtoMapper
    {
        private static volatile bool _mappedBefore;
        private static readonly object SyncObj = new object();

        public static void CreateMappings(IMapperConfigurationExpression mapper)
        {
            lock (SyncObj)
            {
                if (_mappedBefore)
                {
                    return;
                }

                CreateMappingsInternal(mapper);

                _mappedBefore = true;
            }
        }

        public static void CreateMappingsInternal(IMapperConfigurationExpression configuration)
        {
            //configuration.CreateMap<Document, DocEditDto>();
            //configuration.CreateMap<Document, DocListDto>();

            //User
            //configuration.CreateMap<User, UserEditDto>()
            //    .ForMember(dto => dto.Password, options => options.Ignore())
            //    .ReverseMap()
            //    .ForMember(user => user.Password, options => options.Ignore());
            //configuration.CreateMap<User, UserLoginInfoDto>();
            //configuration.CreateMap<User, UserListDto>();
            //configuration.CreateMap<User, ChatUserDto>();
            //configuration.CreateMap<User, OrganizationUnitUserListDto>();
            //configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            //configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */

        }
    }
}