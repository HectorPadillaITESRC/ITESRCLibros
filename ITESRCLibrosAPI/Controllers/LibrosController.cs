using ITESRCLibrosAPI.Models.DTOs;
using ITESRCLibrosAPI.Models.Entities;
using ITESRCLibrosAPI.Models.Validators;
using ITESRCLibrosAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop.Infrastructure;

namespace ITESRCLibrosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        public LibrosController(LibrosRepository repository)
        {
            Repository = repository;
        }

        public LibrosRepository Repository { get; }

        [HttpPost]
        public IActionResult Post(LibroDTO dto)
        {
            //Validar

            LibroValidator validator = new();
            var resultados = validator.Validate(dto);
            if (resultados.IsValid)
            {
                //Mapear: Convertir DTO a Entidad
                Libros entity = new()
                {
                    Id = 0,
                    Eliminado = false,
                    FechaActualizacion = DateTime.UtcNow,
                    Autor = dto.Autor,
                    Portada = dto.Portada,
                    Titulo = dto.Titulo,
                };
                Repository.Insert(entity);
                return Ok();
            }

            return BadRequest(resultados.Errors.Select(x => x.ErrorMessage));

        }

        [HttpGet("{fecha?}/{hora?}/{minutos?}")]
        public IActionResult Get(DateTime? fecha, int hora = 0, int minutos = 0)
        {
            if (fecha != null)
            {
                fecha = new DateTime(fecha.Value.Year, fecha.Value.Month, fecha.Value.Day,
                     hora, minutos, 0);
            }
            var libros = Repository.GetAll()
                .Where(x => fecha == null || x.FechaActualizacion > fecha)
                .OrderBy(x => x.Titulo)
                .Select(x => new LibroDTO
                {
                    Id = x.Id,
                    Titulo = x.Titulo,
                    Autor = x.Autor,
                    Eliminado = x.Eliminado,
                    Portada = x.Portada,
                    Fecha = x.FechaActualizacion
                });

            return Ok(libros);
        }

        [HttpPut("{Id}")]
        public IActionResult Put(LibroDTO dto)
        {
            //Validar

            LibroValidator validator = new();
            var resultados = validator.Validate(dto);
            if (resultados.IsValid)
            {
                var entidadLibro = Repository.Get(dto.Id ?? 0);

                if (entidadLibro == null || entidadLibro.Eliminado)
                {
                    return NotFound();
                }
                else
                {
                    entidadLibro.Autor = dto.Autor;
                    entidadLibro.Titulo = dto.Titulo;
                    entidadLibro.Portada = dto.Portada;
                    entidadLibro.FechaActualizacion = DateTime.UtcNow;

                    Repository.Update(entidadLibro);

                    return Ok();
                }

            }

            return BadRequest(resultados.Errors.Select(x => x.ErrorMessage));

        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entidadLibro = Repository.Get(id);

            if (entidadLibro == null || entidadLibro.Eliminado)
            {
                return NotFound();
            }

            entidadLibro.Eliminado = true;
            entidadLibro.FechaActualizacion = DateTime.UtcNow;
            Repository.Update(entidadLibro);

            return Ok();
        }



    }
}
