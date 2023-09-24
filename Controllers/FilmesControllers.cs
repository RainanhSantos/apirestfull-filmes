using System.Collections;
using System.Collections.Generic;
using System.Linq;
using apifilmes.Data;
using apifilmes.Data.Dtos;
using apifilmes.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace apifilmes.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmesControllers : ControllerBase
{
    //
    private readonly FilmeContext _context;
    private readonly IMapper _mapper;

    public FilmesControllers(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    //Metodo para inserir dados 
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    //A anotação "FromBody" inplicita que as informações serão enviadas no corpo da requisição

    //IActionResult substitui a "List<>" e é o mais recomentado
    public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
    {
        //Convertendo DTO recebido para um Filme
        Filme filme = _mapper.Map<Filme>(filmeDto);
        //Adicionando um filme no banco de dados
        _context.Filmes.Add(filme);
        //Salvando as alterações no banco de dados
        _context.SaveChanges();
        //Mostra no Body quando realizamos um POST os itens que inserimos
        return CreatedAtAction(nameof(RecuperaFilmePorId), 
        new { id = filme.Id},
        filme);
    }

    /// <summary>
    /// Mostra todos os filmes no banco de dados e a data/hora da pesquisa
    /// </summary>
    /// <param name="skip">Ignora um número especificado de elementos em uma sequência e retorna os elementos restantes</param>
    /// <param name="take">Retorna um número especificado de elementos contínuos do início de uma sequência</param>
    /// <returns>IEnumerable</returns>
    /// <response code="201">Caso a leitura seja concluida</response>
    //Metodo para retornar dados
    [HttpGet]
    public IEnumerable<ReadFilmeDto> LerFilmes([FromQuery] int skip = 0, [FromQuery] int take = 5)
    {
        return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take));
    }

    /// <summary>
    /// Pesquisa no banco de dados um filme por um id específico
    /// </summary>
    /// <param name="id">Código identificador do filme no banco de dados</param>
    /// <returns>IActionResult</returns>
    //Recuperar filme por Id
    [HttpGet("{id}")]
    //IActionResult Resultado de uma ação que foi executada (NoContent/NotFound)
    public IActionResult RecuperaFilmePorId(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        //Se o filme encontrado for igual a NULL retorna NotFound caso contratio retorna Ok.Substitui o erro 204 No Content para o 404 Not Found
        if(filme == null ) return NotFound();
        //Retornando um DTO de filme por id com o horário da consulta
        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
        return Ok(filmeDto);
    }

    /// <summary>
    /// Atualiza as informações no banco de dados utilizando todas as propriedades
    /// </summary>
    /// <param name="id">Código identificador do filme no banco de dados</param>
    /// <returns>IActionResult</returns>
    /// <param name="filmeDto">DTO do filme, que será atualizada no banco de dados</param>
    /// <returns>IActionResult</returns>
    //Metodo para atualizar filme
    //Recebe o Id do filme a ser alterado e o campo que será alterado
    [HttpPut("{id}")]
    public IActionResult AtualizaFilme(int id, [FromBody] UpdateFIlmeDto filmeDto)
    {
        //Retorna o primeiro elemento de uma sequência ou um valor padrão se a sequência não contém elementos.
        var filme = _context.Filmes.FirstOrDefault(
            filme => filme.Id == id
        );
        if(filme == null) return NotFound();
        //Atualizando/Mapeando o filme com as atualizações do filme novo
        _mapper.Map(filmeDto, filme);
        //Salvando alterações
        _context.SaveChanges();
        //Retornando Status Code de atualização no banco
        return NoContent();
    }

    /// <summary>
    /// Pesquisa no banco de dados um filme por um id específico
    /// </summary>
    /// <param name="id">Código identificador do filme no banco de dados</param>
    /// <returns>IActionResult</returns>
    /// <param name="patch">Atualiza parcialmente informações no</param>
    /// <returns>IActionResult</returns>
    //Realiza atualizações parciais dos dados
    [HttpPatch("{id}")]
        public IActionResult AtualizaFilmeParcial(int id, JsonPatchDocument<UpdateFIlmeDto> patch)
    {
        //Retorna o primeiro elemento de uma sequência ou um valor padrão se a sequência não contém elementos.
        var filme = _context.Filmes.FirstOrDefault(
            filme => filme.Id == id
        );
        if(filme == null) return NotFound();

        //Converte o filme do banco para um UpdateFIlmeDto
        //Aplicando as regras de validação e alterando o DTO
        //Caso o DTO esteja valido, ele será convertido de volta para um filme
        var filmeParaAtualizar = _mapper.Map<UpdateFIlmeDto>(filme);

        patch.ApplyTo(filmeParaAtualizar, ModelState);
        if(!TryValidateModel(filmeParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }


        _mapper.Map(filmeParaAtualizar, filme);
        //Salvando alterações
        _context.SaveChanges();
        //Retornando Status Code de atualização no banco
        return NoContent();
    }

    /// <summary>
    /// Deleta um filme do banco de dados
    /// </summary>
    /// <param name="id">Código identificador do filme no banco de dados</param>
    /// <returns>IActionResult</returns>
    //Metodo para exluir dados por id
    [HttpDelete("{id}")]
    public IActionResult DeletaFilme(int id)
    {
        //Retorna o primeiro elemento de uma sequência ou um valor padrão se a sequência não contém elementos.
        var filme = _context.Filmes.FirstOrDefault(
            filme => filme.Id == id
        );
        if(filme == null) return NotFound();
        //Remove o filme com o id informado
        _context.Remove(filme);
        //Salva as alterações
        _context.SaveChanges();
        return NoContent();
    }
}