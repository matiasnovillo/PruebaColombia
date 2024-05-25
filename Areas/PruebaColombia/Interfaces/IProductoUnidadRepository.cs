using PruebaColombia.Areas.PruebaColombia.Entities;
using PruebaColombia.Areas.PruebaColombia.DTOs;
using System.Data;

/*
 * GUID:e6c09dfe-3a3e-461b-b3f9-734aee05fc7b
 * 
 * Coded by fiyistack.com
 * Copyright © 2024
 * 
 * The above copyright notice and this permission notice shall be included
 * in all copies or substantial portions of the Software.
 * 
 */

namespace PruebaColombia.Areas.PruebaColombia.Interfaces
{
    public interface IProductoUnidadRepository
    {
        IQueryable<ProductoUnidad> AsQueryable();

        #region Queries
        int Count();

        ProductoUnidad? GetByProductoUnidadId(int productounidadId);

        List<ProductoUnidad?> GetAll();

        paginatedProductoUnidadDTO GetAllByProductoUnidadIdPaginated(string textToSearch,
            bool strictSearch,
            int pageIndex,
            int pageSize);
        #endregion

        #region Non-Queries
        bool Add(ProductoUnidad productounidad);

        bool Update(ProductoUnidad productounidad);

        bool DeleteByProductoUnidadId(int productounidad);
        #endregion

        #region Other methods
        DataTable GetAllInDataTable();
        #endregion
    }
}
