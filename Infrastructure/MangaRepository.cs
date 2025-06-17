using MiMangaBot.Domain;
using MiMangaBot.Domain.Filters;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;

namespace MiMangaBot.Infrastructure;

/// <summary>
/// Repositorio encargado de la persistencia de los objetos Manga en un archivo JSON.
/// Simula una base de datos con operaciones CRUD basicas.
/// </summary>
public class MangaRepository
{
    private readonly List<Manga> _coleccionMangas;
    private readonly string _rutaArchivoJson;

    /// <summary>
    /// Constructor del repositorio. Carga los datos del JSON al iniciar.
    /// </summary>
    /// <param name="configuration">Configuracion de la aplicacion para obtener la ruta del archivo.</param>
    public MangaRepository(IConfiguration configuration)
    {
        var nombreArchivo = configuration.GetValue<string>("RutaDatosMangas") ?? "javerage.library.data.json";
        _rutaArchivoJson = Path.Combine(AppContext.BaseDirectory, "Infrastructure", "data", nombreArchivo);

        _coleccionMangas = CargarDatos();
    }

    /// <summary>
    /// Carga los datos de los mangas desde el archivo JSON.
    /// Si el archivo o el directorio no existen, los crea y devuelve una lista vacia.
    /// </summary>
    /// <returns>Una lista de objetos Manga.</returns>
    private List<Manga> CargarDatos()
    {
        var directorio = Path.GetDirectoryName(_rutaArchivoJson);
        if (!string.IsNullOrEmpty(directorio) && !Directory.Exists(directorio))
        {
            Directory.CreateDirectory(directorio);
        }

        if (File.Exists(_rutaArchivoJson))
        {
            var jsonData = File.ReadAllText(_rutaArchivoJson);
            return JsonSerializer.Deserialize<List<Manga>>(jsonData) ?? new List<Manga>();
        }

        File.WriteAllText(_rutaArchivoJson, "[]");
        return new List<Manga>();
    }

    /// <summary>
    /// Guarda la lista actual de mangas en el archivo JSON.
    /// </summary>
    private void GuardarDatos()
    {
        var opcionesSerializacion = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(_rutaArchivoJson, JsonSerializer.Serialize(_coleccionMangas, opcionesSerializacion));
    }

    /// <summary>
    /// Obtiene todos los mangas disponibles en el repositorio.
    /// </summary>
    /// <returns>Una coleccion enumerable de objetos Manga.</returns>
    public IEnumerable<Manga> ObtenerTodosLosMangas()
    {
        return _coleccionMangas;
    }

    /// <summary>
    /// Obtiene un manga especifico por su identificador unico.
    /// </summary>
    /// <param name="id">El identificador unico del manga.</param>
    /// <returns>El objeto Manga si se encuentra, de lo contrario, null.</returns>
    public Manga? ObtenerMangaPorId(string id)
    {
        return _coleccionMangas.Find(m => m.IdentificadorUnico == id);
    }

    /// <summary>
    /// Agrega un nuevo manga al repositorio y guarda los cambios.
    /// </summary>
    /// <param name="nuevoManga">El objeto Manga a agregar.</param>
    public void AnadirManga(Manga nuevoManga)
    {
        _coleccionMangas.Add(nuevoManga);
        GuardarDatos();
    }

    /// <summary>
    /// Actualiza un manga existente en el repositorio y guarda los cambios.
    /// </summary>
    /// <param name="id">El identificador unico del manga a actualizar.</param>
    /// <param name="mangaActualizado">El objeto Manga con los datos actualizados.</param>
    public void ActualizarManga(string id, Manga mangaActualizado)
    {
        var mangaExistente = ObtenerMangaPorId(id);
        if (mangaExistente != null)
        {
            mangaExistente.TituloPrincipal = mangaActualizado.TituloPrincipal;
            mangaExistente.Sinopsis = mangaActualizado.Sinopsis;
            mangaExistente.URL_Portada = mangaActualizado.URL_Portada;
            mangaExistente.AutorPrincipal = mangaActualizado.AutorPrincipal;
            mangaExistente.GeneroPrincipal = mangaActualizado.GeneroPrincipal;
            mangaExistente.AnoDePublicacion = mangaActualizado.AnoDePublicacion;
            mangaExistente.CalificacionEstrellas = mangaActualizado.CalificacionEstrellas;
            mangaExistente.CantidadDeCapitulos = mangaActualizado.CantidadDeCapitulos;
            mangaExistente.EstaCompleto = mangaActualizado.EstaCompleto;

            GuardarDatos();
        }
    }

    /// <summary>
    /// Elimina un manga del repositorio por su identificador unico y guarda los cambios.
    /// </summary>
    /// <param name="id">El identificador unico del manga a eliminar.</param>
    public void EliminarManga(string id)
    {
        var manga = ObtenerMangaPorId(id);
        if (manga != null)
        {
            _coleccionMangas.Remove(manga);
            GuardarDatos();
        }
    }

    /// <summary>
    /// Busca mangas aplicando los filtros especificados.
    /// </summary>
    /// <param name="filtro">El objeto MangaFilter con los criterios de busqueda.</param>
    /// <returns>Una coleccion enumerable de mangas que coinciden con los filtros.</returns>
    public IEnumerable<Manga> BuscarMangas(MangaFilter filtro)
    {
        return _coleccionMangas.Where(filtro.BuildFilter());
    }
}