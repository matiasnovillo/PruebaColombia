using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using PruebaColombia.Areas.CMSCore.Entities;
using PruebaColombia.Areas.PruebaColombia.Entities;
using PruebaColombia.Areas.PruebaColombia.DTOs;
using PruebaColombia.Areas.PruebaColombia.Interfaces;
using System.Data;
using PruebaColombia.DBContext;

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

namespace PruebaColombia.Areas.PruebaColombia.Repositories
{
    public class ProductoUnidadRepository : IProductoUnidadRepository
    {
        protected readonly PruebaColombiaContext _context;

        public ProductoUnidadRepository(PruebaColombiaContext context)
        {
            _context = context;
        }

        public IQueryable<ProductoUnidad> AsQueryable()
        {
            try
            {
                return _context.ProductoUnidad.AsQueryable();
            }
            catch (Exception) { throw; }
        }

        #region Queries
        public int Count()
        {
            try
            {
                return _context.ProductoUnidad.Count();
            }
            catch (Exception) { throw; }
        }

        public ProductoUnidad? GetByProductoUnidadId(int productounidadId)
        {
            try
            {
                return _context.ProductoUnidad
                            .FirstOrDefault(x => x.ProductoUnidadId == productounidadId);
            }
            catch (Exception) { throw; }
        }

        public List<ProductoUnidad?> GetAll()
        {
            try
            {
                return _context.ProductoUnidad.ToList();
            }
            catch (Exception) { throw; }
        }

        public paginatedProductoUnidadDTO GetAllByProductoUnidadIdPaginated(string textToSearch,
            bool strictSearch,
            int pageIndex, 
            int pageSize)
        {
            try
            {
                //textToSearch: "novillo matias  com" -> words: {novillo,matias,com}
                string[] words = Regex
                    .Replace(textToSearch
                    .Trim(), @"\s+", " ")
                    .Split(" ");

                int TotalProductoUnidad = _context.ProductoUnidad.Count();

                var query = from productounidad in _context.ProductoUnidad
                            join userCreation in _context.User on productounidad.UserCreationId equals userCreation.UserId
                            join userLastModification in _context.User on productounidad.UserLastModificationId equals userLastModification.UserId
                            select new { ProductoUnidad = productounidad, UserCreation = userCreation, UserLastModification = userLastModification };

                // Extraemos los resultados en listas separadas
                List<ProductoUnidad> lstProductoUnidad = query.Select(result => result.ProductoUnidad)
                        .Where(x => strictSearch ?
                            words.All(word => x.ProductoUnidadId.ToString().Contains(word)) :
                            words.Any(word => x.ProductoUnidadId.ToString().Contains(word)))
                        .OrderByDescending(p => p.DateTimeLastModification)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
                List<User> lstUserCreation = query.Select(result => result.UserCreation).ToList();
                List<User> lstUserLastModification = query.Select(result => result.UserLastModification).ToList();

                return new paginatedProductoUnidadDTO
                {
                    lstProductoUnidad = lstProductoUnidad,
                    lstUserCreation = lstUserCreation,
                    lstUserLastModification = lstUserLastModification,
                    TotalItems = TotalProductoUnidad,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
            }
            catch (Exception) { throw; }
        }
        #endregion

        #region Non-Queries
        public bool Add(ProductoUnidad productounidad)
        {
            try
            {
                _context.ProductoUnidad.Add(productounidad);
                return _context.SaveChanges() > 0;
            }
            catch (Exception) { throw; }
        }

        public bool Update(ProductoUnidad productounidad)
        {
            try
            {
                _context.ProductoUnidad.Update(productounidad);
                return _context.SaveChanges() > 0;
            }
            catch (Exception) { throw; }
        }

        public bool DeleteByProductoUnidadId(int productounidadId)
        {
            try
            {
                AsQueryable()
                        .Where(x => x.ProductoUnidadId == productounidadId)
                        .ExecuteDelete();

                return _context.SaveChanges() > 0;
            }
            catch (Exception) { throw; }
        }
        #endregion

        #region Other methods
        public DataTable GetAllInDataTable()
        {
            try
            {
                List<ProductoUnidad> lstProductoUnidad = _context.ProductoUnidad.ToList();

                DataTable DataTable = new();
                DataTable.Columns.Add("ProductoUnidadId", typeof(string));
                DataTable.Columns.Add("Active", typeof(string));
                DataTable.Columns.Add("DateTimeCreation", typeof(string));
                DataTable.Columns.Add("DateTimeLastModification", typeof(string));
                DataTable.Columns.Add("UserCreationId", typeof(string));
                DataTable.Columns.Add("UserLastModificationId", typeof(string));
                DataTable.Columns.Add("Nombre", typeof(string));
                

                foreach (ProductoUnidad productounidad in lstProductoUnidad)
                {
                    DataTable.Rows.Add(
                        productounidad.ProductoUnidadId,
                        productounidad.Active,
                        productounidad.DateTimeCreation,
                        productounidad.DateTimeLastModification,
                        productounidad.UserCreationId,
                        productounidad.UserLastModificationId,
                        productounidad.Nombre
                        
                        );
                }

                return DataTable;
            }
            catch (Exception) { throw; }
        }
        #endregion
    }
}
