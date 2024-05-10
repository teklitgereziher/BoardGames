using AutoMapper;

namespace BoardGames.RestApi.AutoMapper
{
  public class AutoMapperConfig
  {
    private static readonly Lazy<IMapper> LazyMapper;
    private static readonly Lazy<MapperConfiguration> LazyConfiguration;

    static AutoMapperConfig()
    {
      LazyConfiguration = new Lazy<MapperConfiguration>(() =>
      {
        return new MapperConfiguration(cfg =>
        {
          cfg.AllowNullCollections = true;
          cfg.AddProfile<AccountMapperProfile>();
        });
      });

      // LazyMapper = new Lazy<IMapper>(() => LazyConfiguration.Value.CreateMapper());
      LazyMapper = new Lazy<IMapper>(LazyConfiguration.Value.CreateMapper);
    }

    public static MapperConfiguration Configuration => LazyConfiguration.Value;
    public static IMapper Mapper => LazyMapper.Value;
  }
}
