using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace apifilmes.Data.Dtos;

public class ReadFilmeDto
{
public string? Titulo { get; set; }
public string? Genero { get; set; }
public int Duracao { get; set; }
public DateTime HoraDaConsulta { get; set; } = DateTime.Now;
}