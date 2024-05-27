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
    public class ProductoRepository : IProductoRepository
    {
        protected readonly PruebaColombiaContext _context;

        public ProductoRepository(PruebaColombiaContext context)
        {
            _context = context;
        }

        public IQueryable<Producto> AsQueryable()
        {
            try
            {
                return _context.Producto.AsQueryable();
            }
            catch (Exception) { throw; }
        }

        #region Queries
        public int Count()
        {
            try
            {
                return _context.Producto.Count();
            }
            catch (Exception) { throw; }
        }

        public Producto? GetByProductoId(int productoId)
        {
            try
            {
                return _context.Producto
                            .FirstOrDefault(x => x.ProductoId == productoId);
            }
            catch (Exception) { throw; }
        }

        public List<Producto?> GetAll()
        {
            try
            {
                return _context.Producto.ToList();
            }
            catch (Exception) { throw; }
        }

        public paginatedProductoDTO GetAllByProductoIdPaginated(string textToSearch,
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

                int TotalProducto = _context.Producto.Count();

                var query = from producto in _context.Producto
                            join userCreation in _context.User on producto.UserCreationId equals userCreation.UserId
                            join userLastModification in _context.User on producto.UserLastModificationId equals userLastModification.UserId
                            join productoTipo in _context.ProductoTipo on producto.ProductoTipoId equals productoTipo.ProductoTipoId
                            join productoUnidad in _context.ProductoUnidad on producto.ProductoUnidadId equals productoUnidad.ProductoUnidadId
                            select new { Producto = producto, ProductoTipo = productoTipo, ProductoUnidad = productoUnidad, UserCreation = userCreation, UserLastModification = userLastModification };

                // Extraemos los resultados en listas separadas
                List<Producto> lstProducto = query.Select(result => result.Producto)
                        .Where(x => strictSearch ?
                            words.All(word => x.Name.Contains(word)) :
                            words.Any(word => x.Name.Contains(word)))
                        .OrderByDescending(p => p.DateTimeLastModification)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
                List<ProductoTipo> lstProductoTipo = query.Select(result => result.ProductoTipo).ToList();
                List<ProductoUnidad> lstProductoUnidad = query.Select(result => result.ProductoUnidad).ToList();
                List<User> lstUserCreation = query.Select(result => result.UserCreation).ToList();
                List<User> lstUserLastModification = query.Select(result => result.UserLastModification).ToList();

                return new paginatedProductoDTO
                {
                    lstProducto = lstProducto,
                    lstUserCreation = lstUserCreation,
                    lstUserLastModification = lstUserLastModification,
                    lstProductoTipo = lstProductoTipo,
                    lstProductoUnidad = lstProductoUnidad,
                    TotalItems = TotalProducto,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
            }
            catch (Exception) { throw; }
        }
        #endregion

        #region Non-Queries
        public bool Add(Producto producto)
        {
            try
            {
                _context.Producto.Add(producto);
                return _context.SaveChanges() > 0;
            }
            catch (Exception) { throw; }
        }

        public bool Update(Producto producto)
        {
            try
            {
                _context.Producto.Update(producto);
                return _context.SaveChanges() > 0;
            }
            catch (Exception) { throw; }
        }

        public bool DeleteByProductoId(int productoId)
        {
            try
            {
                AsQueryable()
                        .Where(x => x.ProductoId == productoId)
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
                List<Producto> lstProducto = _context.Producto.ToList();

                DataTable DataTable = new();
                DataTable.Columns.Add("ProductoId", typeof(string));
                DataTable.Columns.Add("Active", typeof(string));
                DataTable.Columns.Add("DateTimeCreation", typeof(string));
                DataTable.Columns.Add("DateTimeLastModification", typeof(string));
                DataTable.Columns.Add("UserCreationId", typeof(string));
                DataTable.Columns.Add("UserLastModificationId", typeof(string));
                DataTable.Columns.Add("Name", typeof(string));
                DataTable.Columns.Add("ProductoTipoId", typeof(string));
                DataTable.Columns.Add("ProductoUnidadId", typeof(string));
                

                foreach (Producto producto in lstProducto)
                {
                    DataTable.Rows.Add(
                        producto.ProductoId,
                        producto.Active,
                        producto.DateTimeCreation,
                        producto.DateTimeLastModification,
                        producto.UserCreationId,
                        producto.UserLastModificationId,
                        producto.Name,
                        producto.ProductoTipoId,
                        producto.ProductoUnidadId
                        
                        );
                }

                return DataTable;
            }
            catch (Exception) { throw; }
        }
        #endregion
    }
}
