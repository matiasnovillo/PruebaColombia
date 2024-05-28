using PruebaColombia.Areas.PruebaColombia.Entities;

namespace PruebaColombia.Areas.PruebaColombia.DTOs
{
    public class resultadoVisualizacionDeSurtidoresDTO
    {
        public Precio Precio { get; set; }
        public Dispensador Dispensador { get; set; }
        public DispensadorManguera DispensadorManguera { get; set; }
        public Producto Producto { get; set; }
    }
}
