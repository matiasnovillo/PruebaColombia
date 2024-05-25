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
    public interface IDispensadorMangueraRepository
    {
        IQueryable<DispensadorManguera> AsQueryable();

        #region Queries
        int Count();

        DispensadorManguera? GetByDispensadorMangueraId(int dispensadormangueraId);

        List<DispensadorManguera?> GetAll();

        paginatedDispensadorMangueraDTO GetAllByDispensadorMangueraIdPaginated(string textToSearch,
            bool strictSearch,
            int pageIndex,
            int pageSize);
        #endregion

        #region Non-Queries
        bool Add(DispensadorManguera dispensadormanguera);

        bool Update(DispensadorManguera dispensadormanguera);

        bool DeleteByDispensadorMangueraId(int dispensadormanguera);
        #endregion

        #region Other methods
        DataTable GetAllInDataTable();
        #endregion
    }
}
