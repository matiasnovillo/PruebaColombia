using PruebaColombia.Areas.PruebaColombia.Entities;

namespace PruebaColombia.Areas.PruebaColombia.DTOs
{
    public class visualizacionDeDispensadoresDTO
    {
        public List<Dispensador> lstDispensador { get; set; } = [];

        public List<DispensadorManguera> lstDispensadorManguera { get; set; } = [];
        public List<Producto> lstProducto { get; set; } = [];
        public List<Precio> lstPrecio { get; set; } = [];
    }
}
