namespace MiMangaBot.Domain;

/// <summary>
/// Representa la entidad principal de un Manga con sus caracteristicas clave.
/// super duper comands
/// </summary>
public class Manga
{
    // Un identificador unico para cada manga. Se genera automaticamente.
    public string IdentificadorUnico { get; set; }

    // El nombre oficial del manga.
    public required string TituloPrincipal { get; set; }

    // Una breve descripcion o sinopsis del manga.
    public required string Sinopsis { get; set; }

    // URL de la imagen de la portada del manga.
    public required string URL_Portada { get; set; }

    // El nombre del autor o mangaka principal.
    public required string AutorPrincipal { get; set; }

    // El genero o categoria principal del manga.
    public required string GeneroPrincipal { get; set; }

    // El ano en que fue publicado por primera vez.
    public required int AnoDePublicacion { get; set; }

    // Calificacion del manga, de 1 a 10 estrellas.
    public required int CalificacionEstrellas { get; set; }

    // Numero total de capitulos o volumenes publicados hasta la fecha.
    public int CantidadDeCapitulos { get; set; }

    // Indica si el manga ha sido completado por el autor.
    public bool EstaCompleto { get; set; }

    /// <summary>
    /// Constructor por defecto que inicializa un nuevo manga con un GUID como IdentificadorUnico.
    /// </summary>
    public Manga()
    {
        IdentificadorUnico = Guid.NewGuid().ToString();
    }
}