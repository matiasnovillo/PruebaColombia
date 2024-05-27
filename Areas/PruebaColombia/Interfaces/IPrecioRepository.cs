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
    public interface IPrecioRepository
    {
        IQueryable<Precio> AsQueryable();

        #region Queries
        int Count();

        Precio? GetByPrecioId(int precioId);

        List<Precio?> GetAll();

        paginatedPrecioDTO GetAllByValorPaginated(string textToSearch,
            bool strictSearch,
            int pageIndex,
            int pageSize);
        #endregion

        #region Non-Queries
        bool Add(Precio precio);

        bool Update(Precio precio);

        bool DeleteByPrecioId(int precio);
        #endregion

        #region Other methods
        DataTable GetAllInDataTable();
        #endregion
    }
}
