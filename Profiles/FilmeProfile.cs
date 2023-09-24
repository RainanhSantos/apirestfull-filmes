using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apifilmes.Data.Dtos;
using apifilmes.Models;
using AutoMapper;

namespace apifilmes.Profiles;

public class FilmeProfile : Profile
{
    public FilmeProfile()
    {
        //Mapeando um DTO => Filme
        CreateMap<CreateFilmeDto, Filme>();
        CreateMap<UpdateFIlmeDto, Filme>();
        CreateMap<Filme, UpdateFIlmeDto>();
        CreateMap<Filme, ReadFilmeDto>();
    }
}