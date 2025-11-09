namespace Farmacia.Core.DTOs
{
    public class ProductFilterDto
    {
        public string? Search { get; set; }
        public int? CategoriaId { get; set; }
        public bool? Activo { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; } = 10;
        // otros filtros si hace falta
    }
}