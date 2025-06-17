using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using MiMangaBot.Domain;
using MiMangaBot.Services;
using MiMangaBot.Domain.Filters;

namespace MiMangaBot.Controllers
{
    /// <summary>
    /// Controlador de API para gestionar las operaciones relacionadas con los mangas.
    /// Provee endpoints para obtener, buscar, anadir, actualizar y eliminar mangas.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")] // La ruta base sera /api/Manga
    public class MangaController : ControllerBase
    {
        private readonly MangaServices _servicioDeMangas;

        /// <summary>
        /// Constructor del controlador. Recibe MangaServices a traves de inyeccion de dependencias.
        /// </summary>
        /// <param name="mangaServices">El servicio de logica de negocio para mangas.</param>
        public MangaController(MangaServices mangaServices)
        {
            _servicioDeMangas = mangaServices;
        }

        /// <summary>
        /// Obtiene una lista completa de todos los mangas disponibles.
        /// Endpoint: GET /api/Manga
        /// </summary>
        /// <returns>Una lista de objetos Manga.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Manga>), 200)]
        public ActionResult<List<Manga>> ObtenerTodosLosMangas()
        {
            return Ok(_servicioDeMangas.ObtenerTodosLosMangas());
        }

        /// <summary>
        /// Busca mangas especificos aplicando varios criterios de filtro (titulo, autor, genero, ano de publicacion o calificacion).
        /// Endpoint: GET /api/Manga/buscar
        /// </summary>
        /// <param name="filtros">Objeto que contiene los criterios de busqueda.</param>
        /// <returns>Una coleccion de mangas que coinciden con los filtros.</returns>
        [HttpGet("buscar")]
        [ProducesResponseType(typeof(IEnumerable<Manga>), 200)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<Manga>> BuscarMangas([FromQuery] MangaFilter filtros)
        {
            try
            {
                var mangasFiltrados = _servicioDeMangas.FiltrarMangas(filtros);
                return Ok(mangasFiltrados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene los detalles de un manga especifico utilizando su identificador unico.
        /// Endpoint: GET /api/Manga/{id}
        /// </summary>
        /// <param name="id">El identificador unico del manga.</param>
        /// <returns>El objeto Manga si se encuentra, de lo contrario, null.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Manga), 200)]
        [ProducesResponseType(404)]
        public ActionResult<Manga> ObtenerMangaPorIdentificador([FromRoute] string id)
        {
            var manga = _servicioDeMangas.BuscarMangaPorId(id);
            if (manga == null)
            {
                return NotFound($"Manga con ID '{id}' no encontrado.");
            }
            return Ok(manga);
        }

        /// <summary>
        /// Permite a la aplicacion cliente enviar los datos de un nuevo manga para que sea guardado en el sistema.
        /// Endpoint: POST /api/Manga
        /// </summary>
        /// <param name="mangaNuevo">El objeto Manga a anadir.</param>
        /// <returns>El manga recien creado con su ID.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Manga), 201)]
        [ProducesResponseType(400)]
        public ActionResult RegistrarManga([FromBody] Manga mangaNuevo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _servicioDeMangas.RegistrarNuevoManga(mangaNuevo);
            return CreatedAtAction(nameof(ObtenerMangaPorIdentificador), new { id = mangaNuevo.IdentificadorUnico }, mangaNuevo);
        }

        /// <summary>
        /// Permite modificar la informacion de un manga ya existente.
        /// Endpoint: PUT /api/Manga/{id}
        /// </summary>
        /// <param name="id">El identificador unico del manga a actualizar.</param>
        /// <param name="mangaActualizado">El objeto Manga con los datos modificados.</param>
        /// <returns>Un 204 No Content si la actualizacion fue exitosa, o un 400 Bad Request.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult ActualizarManga(string id, [FromBody] Manga mangaActualizado)
        {
            if (id != mangaActualizado.IdentificadorUnico)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del manga en el cuerpo de la peticion.");
            }

            var mangaExistente = _servicioDeMangas.BuscarMangaPorId(id);
            if (mangaExistente == null)
            {
                return NotFound($"Manga con ID '{id}' no encontrado para actualizacion.");
            }

            _servicioDeMangas.ActualizarInformacionManga(id, mangaActualizado);
            return NoContent();
        }

        /// <summary>
        /// Permite borrar un manga del sistema.
        /// Endpoint: DELETE /api/Manga/{id}
        /// </summary>
        /// <param name="id">El identificador unico del manga a borrar.</param>
        /// <returns>Un 204 No Content si la eliminacion fue exitosa, o un 404 Not Found.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public ActionResult EliminarManga(string id)
        {
            var mangaExistente = _servicioDeMangas.BuscarMangaPorId(id);
            if (mangaExistente == null)
            {
                return NotFound($"Manga con ID '{id}' no encontrado para eliminacion.");
            }

            _servicioDeMangas.RemoverManga(id);
            return NoContent();
        }
    }
}