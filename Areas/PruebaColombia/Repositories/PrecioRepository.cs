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
    public class PrecioRepository : IPrecioRepository
    {
        protected readonly PruebaColombiaContext _context;

        public PrecioRepository(PruebaColombiaContext context)
        {
            _context = context;
        }

        public IQueryable<Precio> AsQueryable()
        {
            try
            {
                return _context.Precio.AsQueryable();
            }
            catch (Exception) { throw; }
        }

        #region Queries
        public int Count()
        {
            try
            {
                return _context.Precio.Count();
            }
            catch (Exception) { throw; }
        }

        public Precio? GetByPrecioId(int precioId)
        {
            try
            {
                return _context.Precio
                            .FirstOrDefault(x => x.PrecioId == precioId);
            }
            catch (Exception) { throw; }
        }

        public List<Precio?> GetAll()
        {
            try
            {
                return _context.Precio.ToList();
            }
            catch (Exception) { throw; }
        }

        public paginatedPrecioDTO GetAllByValorPaginated(string textToSearch,
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

                int TotalPrecio = _context.Precio.Count();

                var query = from precio in _context.Precio
                            join userCreation in _context.User on precio.UserCreationId equals userCreation.UserId
                            join userLastModification in _context.User on precio.UserLastModificationId equals userLastModification.UserId
                            join producto in _context.Producto on precio.ProductoId equals producto.ProductoId
                            select new { Producto = producto, Precio = precio, UserCreation = userCreation, UserLastModification = userLastModification };

                // Extraemos los resultados en listas separadas
                List<Precio> lstPrecio = query.Select(result => result.Precio)
                        .Where(x => strictSearch ?
                            words.All(word => x.Valor.ToString().Contains(word)) :
                            words.Any(word => x.Valor.ToString().Contains(word)))
                        .OrderByDescending(p => p.DateTimeLastModification)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
                List<Producto> lstProducto = query.Select(result => result.Producto).ToList();
                List<User> lstUserCreation = query.Select(result => result.UserCreation).ToList();
                List<User> lstUserLastModification = query.Select(result => result.UserLastModification).ToList();

                return new paginatedPrecioDTO
                {
                    lstPrecio = lstPrecio,
                    lstUserCreation = lstUserCreation,
                    lstUserLastModification = lstUserLastModification,
                    lstProducto = lstProducto,
                    TotalItems = TotalPrecio,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
            }
            catch (Exception) { throw; }
        }
        #endregion

        #region Non-Queries
        public bool Add(Precio precio)
        {
            try
            {
                _context.Precio.Add(precio);
                return _context.SaveChanges() > 0;
            }
            catch (Exception) { throw; }
        }

        public bool Update(Precio precio)
        {
            try
            {
                _context.Precio.Update(precio);
                return _context.SaveChanges() > 0;
            }
            catch (Exception) { throw; }
        }

        public bool DeleteByPrecioId(int precioId)
        {
            try
            {
                AsQueryable()
                        .Where(x => x.PrecioId == precioId)
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
                List<Precio> lstPrecio = _context.Precio.ToList();

                DataTable DataTable = new();
                DataTable.Columns.Add("PrecioId", typeof(string));
                DataTable.Columns.Add("Active", typeof(string));
                DataTable.Columns.Add("DateTimeCreation", typeof(string));
                DataTable.Columns.Add("DateTimeLastModification", typeof(string));
                DataTable.Columns.Add("UserCreationId", typeof(string));
                DataTable.Columns.Add("UserLastModificationId", typeof(string));
                DataTable.Columns.Add("Valor", typeof(string));
                DataTable.Columns.Add("ProductoId", typeof(string));
                

                foreach (Precio precio in lstPrecio)
                {
                    DataTable.Rows.Add(
                        precio.PrecioId,
                        precio.Active,
                        precio.DateTimeCreation,
                        precio.DateTimeLastModification,
                        precio.UserCreationId,
                        precio.UserLastModificationId,
                        precio.Valor,
                        precio.ProductoId
                        
                        );
                }

                return DataTable;
            }
            catch (Exception) { throw; }
        }
        #endregion
    }
}
