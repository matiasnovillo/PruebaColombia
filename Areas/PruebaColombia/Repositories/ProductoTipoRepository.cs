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
    public class ProductoTipoRepository : IProductoTipoRepository
    {
        protected readonly PruebaColombiaContext _context;

        public ProductoTipoRepository(PruebaColombiaContext context)
        {
            _context = context;
        }

        public IQueryable<ProductoTipo> AsQueryable()
        {
            try
            {
                return _context.ProductoTipo.AsQueryable();
            }
            catch (Exception) { throw; }
        }

        #region Queries
        public int Count()
        {
            try
            {
                return _context.ProductoTipo.Count();
            }
            catch (Exception) { throw; }
        }

        public ProductoTipo? GetByProductoTipoId(int productotipoId)
        {
            try
            {
                return _context.ProductoTipo
                            .FirstOrDefault(x => x.ProductoTipoId == productotipoId);
            }
            catch (Exception) { throw; }
        }

        public List<ProductoTipo?> GetAll()
        {
            try
            {
                return _context.ProductoTipo.ToList();
            }
            catch (Exception) { throw; }
        }

        public paginatedProductoTipoDTO GetAllByProductoTipoIdPaginated(string textToSearch,
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

                int TotalProductoTipo = _context.ProductoTipo.Count();

                var query = from productotipo in _context.ProductoTipo
                            join userCreation in _context.User on productotipo.UserCreationId equals userCreation.UserId
                            join userLastModification in _context.User on productotipo.UserLastModificationId equals userLastModification.UserId
                            select new { ProductoTipo = productotipo, UserCreation = userCreation, UserLastModification = userLastModification };

                // Extraemos los resultados en listas separadas
                List<ProductoTipo> lstProductoTipo = query.Select(result => result.ProductoTipo)
                        .Where(x => strictSearch ?
                            words.All(word => x.ProductoTipoId.ToString().Contains(word)) :
                            words.Any(word => x.ProductoTipoId.ToString().Contains(word)))
                        .OrderByDescending(p => p.DateTimeLastModification)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
                List<User> lstUserCreation = query.Select(result => result.UserCreation).ToList();
                List<User> lstUserLastModification = query.Select(result => result.UserLastModification).ToList();

                return new paginatedProductoTipoDTO
                {
                    lstProductoTipo = lstProductoTipo,
                    lstUserCreation = lstUserCreation,
                    lstUserLastModification = lstUserLastModification,
                    TotalItems = TotalProductoTipo,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
            }
            catch (Exception) { throw; }
        }
        #endregion

        #region Non-Queries
        public bool Add(ProductoTipo productotipo)
        {
            try
            {
                _context.ProductoTipo.Add(productotipo);
                return _context.SaveChanges() > 0;
            }
            catch (Exception) { throw; }
        }

        public bool Update(ProductoTipo productotipo)
        {
            try
            {
                _context.ProductoTipo.Update(productotipo);
                return _context.SaveChanges() > 0;
            }
            catch (Exception) { throw; }
        }

        public bool DeleteByProductoTipoId(int productotipoId)
        {
            try
            {
                AsQueryable()
                        .Where(x => x.ProductoTipoId == productotipoId)
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
                List<ProductoTipo> lstProductoTipo = _context.ProductoTipo.ToList();

                DataTable DataTable = new();
                DataTable.Columns.Add("ProductoTipoId", typeof(string));
                DataTable.Columns.Add("Active", typeof(string));
                DataTable.Columns.Add("DateTimeCreation", typeof(string));
                DataTable.Columns.Add("DateTimeLastModification", typeof(string));
                DataTable.Columns.Add("UserCreationId", typeof(string));
                DataTable.Columns.Add("UserLastModificationId", typeof(string));
                DataTable.Columns.Add("Nombre", typeof(string));
                

                foreach (ProductoTipo productotipo in lstProductoTipo)
                {
                    DataTable.Rows.Add(
                        productotipo.ProductoTipoId,
                        productotipo.Active,
                        productotipo.DateTimeCreation,
                        productotipo.DateTimeLastModification,
                        productotipo.UserCreationId,
                        productotipo.UserLastModificationId,
                        productotipo.Nombre
                        
                        );
                }

                return DataTable;
            }
            catch (Exception) { throw; }
        }
        #endregion
    }
}
