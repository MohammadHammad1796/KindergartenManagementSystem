namespace KindergartenManagementSystem.Core.Services
{
    public interface IMapper
    {
        /// <summary>
        /// Execute a mapping from the source object to a new destination object.
        /// </summary>
        public TDestination Map<TDestination>(object source) where TDestination : class;

        /// <summary>
        /// Execute a mapping from the source object to a new destination object.
        /// </summary>
        public TDestination Map<TSource, TDestination>(TSource source) where TDestination : class;

        /// <summary>
        /// Execute a mapping from the source object to the existing destination object.
        /// </summary>
        public void Map<TSource, TDestination>(TSource source, TDestination destination) where TDestination : class;
    }
}