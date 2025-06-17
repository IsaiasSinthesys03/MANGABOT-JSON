using MiMangaBot.Domain;
using MiMangaBot.Domain.Filters;
using MiMangaBot.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MiMangaBot.Services;

/// <summary>
/// Provee la logica de negocio para las operaciones relacionadas con los mangas.
/// Actua como intermediario entre el controlador y el repositorio de datos.
/// </summary>
public class MangaServices
{
    private readonly MangaRepository _gestorDeMangas;

    /// <summary>
    /// Constructor que recibe una instancia de MangaRepository mediante inyeccion de dependencias.
    /// </summary>
    /// <param name="mangaRepository">El repositorio de datos para mangas.</param>
    public MangaServices(MangaRepository mangaRepository)
    {
        _gestorDeMangas = mangaRepository;
    }

    /// <summary>
    /// Obtiene una lista de todos los mangas disponibles.
    /// </summary>
    /// <returns>Una lista de objetos Manga.</returns>
    public List<Manga> ObtenerTodosLosMangas()
    {
        return _gestorDeMangas.ObtenerTodosLosMangas().ToList();
    }

    /// <summary>
    /// Busca y recupera un manga especifico por su identificador unico.
    /// </summary>
    /// <param name="id">El identificador unico del manga.</param>
    /// <returns>El objeto Manga si se encuentra, de lo contrario, null.</returns>
    public Manga? BuscarMangaPorId(string id)
    {
        return _gestorDeMangas.ObtenerMangaPorId(id);
    }

    /// <summary>
    /// Agrega un nuevo manga al sistema.
    /// </summary>
    /// <param name="mangaNuevo">El objeto Manga a anadir.</param>
    public void RegistrarNuevoManga(Manga mangaNuevo)
    {
        _gestorDeMangas.AnadirManga(mangaNuevo);
    }

    /// <summary>
    /// Actualiza la informacion de un manga existente.
    /// </summary>
    /// <param name="id">El identificador unico del manga a actualizar.</param>
    /// <param name="mangaModificado">El objeto Manga con los datos actualizados.</param>
    public void ActualizarInformacionManga(string id, Manga mangaModificado)
    {
        _gestorDeMangas.ActualizarManga(id, mangaModificado);
    }

    /// <summary>
    /// Elimina un manga del sistema por su identificador unico.
    /// </summary>
    /// <param name="id">El identificador unico del manga a eliminar.</param>
    public void RemoverManga(string id)
    {
        _gestorDeMangas.EliminarManga(id);
    }

    /// <summary>
    /// Realiza una busqueda de mangas aplicando diversos criterios de filtro.
    /// </summary>
    /// <param name="filtroDeBusqueda">El objeto MangaFilter con los criterios deseados.</param>
    /// <returns>Una lista de mangas que coinciden con los criterios del filtro.</returns>
    public List<Manga> FiltrarMangas(MangaFilter filtroDeBusqueda)
    {
        return _gestorDeMangas.BuscarMangas(filtroDeBusqueda).ToList();
    }
}
