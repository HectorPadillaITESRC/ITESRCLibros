namespace ITESRCLibrosAPI.Models.DTOs
{
    public class LibroDTO
    {

        public string Titulo { get; set; } = null!;
        public string Autor { get; set; } = null!;
        public string Portada { get; set; } = null!;

        public DateTime Fecha { get; set; }
        public int? Id { get; set; }
        public bool Eliminado { get; set; }
    }
}
