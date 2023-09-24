using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace apifilmes.Data.Dtos;

public class CreateFilmeDto
{
    //Data Annotation -> Validações em tempo de execução
    [Required(ErrorMessage = "O título do filme é obrigatório")]
    public string? Titulo { get; set; }

    [Required(ErrorMessage = "O Gênero do filme é obrigatório")]
    //O StringLength não aloca memória dentro do BD
    [StringLength(50, ErrorMessage = "O tamanho do gênero não pode exceder 50 caracteres")]
    public string? Genero { get; set; }

    [Required]
    [Range(70, 600, ErrorMessage = "A duração deve ter entre 70 e 600 minutos")]
    public int Duracao { get; set; }
}