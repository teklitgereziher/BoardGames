using AutoMapper;
using BoardGames.DataContract.Models;
using BoardGames.RestApi.DTOs;

namespace BoardGames.RestApi.AutoMapper
{
  public class AccountMapperProfile : Profile
  {
    public AccountMapperProfile()
    {
      CreateMap<RegisterDTO, BoardGameUser>()
        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
    }
  }
}
