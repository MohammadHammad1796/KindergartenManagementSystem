using KindergartenManagementSystem.Core.Services;

namespace KindergartenManagementSystem.Mapping
{
    public class ApplicationMapper : IMapper
    {
        private readonly AutoMapper.IMapper _mapper;

        public ApplicationMapper(AutoMapper.IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination Map<TDestination>(object source) where TDestination : class
        {
            // new instance of destination
            return _mapper.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source) where TDestination : class
        {
            // new instance of destination
            return _mapper.Map<TSource, TDestination>(source);
        }

        public void Map<TSource, TDestination>(TSource source, TDestination destination) where TDestination : class
        {
            // existing destination
            _mapper.Map(source, destination);
        }
    }
}