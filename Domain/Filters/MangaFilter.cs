using MiMangaBot.Domain;
using static FuncExtensions; // Importa los metodos de extension directamente

namespace MiMangaBot.Domain.Filters;

/// <summary>
/// super duper comands
/// Define los criterios de filtrado para buscar mangas.
/// </summary>
public class MangaFilter
{
    public string? TituloABuscar { get; set; }
    public string? AutorABuscar { get; set; }
    public string? GeneroABuscar { get; set; }
    public int? AnoDePublicacionExacto { get; set; }
    public int? CalificacionMinima { get; set; }
    public int? CalificacionMaxima { get; set; }
    public bool? SoloCompletos { get; set; } // Nuevo filtro

    /// <summary>
    /// Construye y devuelve un predicado (funcion booleana) que aplica todos los filtros configurados.
    /// </summary>
    /// <returns>Una funcion Lambda que filtra objetos Manga.</returns>
    public Func<Manga, bool> BuildFilter()
    {
        Func<Manga, bool> filter = m => true; // Filtro inicial que acepta todos los mangas

        if (!string.IsNullOrEmpty(TituloABuscar))
        {
            filter = filter.And(m => m.TituloPrincipal.Contains(TituloABuscar, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(AutorABuscar))
        {
            filter = filter.And(m => m.AutorPrincipal.Contains(AutorABuscar, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(GeneroABuscar))
        {
            filter = filter.And(m => m.GeneroPrincipal.Contains(GeneroABuscar, StringComparison.OrdinalIgnoreCase));
        }

        if (AnoDePublicacionExacto.HasValue)
        {
            filter = filter.And(m => m.AnoDePublicacion == AnoDePublicacionExacto.Value);
        }

        if (CalificacionMinima.HasValue)
        {
            filter = filter.And(m => m.CalificacionEstrellas >= CalificacionMinima.Value);
        }

        if (CalificacionMaxima.HasValue)
        {
            filter = filter.And(m => m.CalificacionEstrellas <= CalificacionMaxima.Value);
        }

        if (SoloCompletos.HasValue)
        {
            filter = filter.And(m => m.EstaCompleto == SoloCompletos.Value);
        }

        return filter;
    }
}