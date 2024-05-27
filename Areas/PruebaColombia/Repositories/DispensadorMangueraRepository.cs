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
    public class DispensadorMangueraRepository : IDispensadorMangueraRepository
    {
        protected readonly PruebaColombiaContext _context;

        public DispensadorMangueraRepository(PruebaColombiaContext context)
        {
            _context = context;
        }

        public IQueryable<DispensadorManguera> AsQueryable()
        {
            try
            {
                return _context.DispensadorManguera.AsQueryable();
            }
            catch (Exception) { throw; }
        }

        #region Queries
        public int Count()
        {
            try
            {
                return _context.DispensadorManguera.Count();
            }
            catch (Exception) { throw; }
        }

        public DispensadorManguera? GetByDispensadorMangueraId(int dispensadormangueraId)
        {
            try
            {
                return _context.DispensadorManguera
                            .FirstOrDefault(x => x.DispensadorMangueraId == dispensadormangueraId);
            }
            catch (Exception) { throw; }
        }

        public List<DispensadorManguera?> GetAll()
        {
            try
            {
                return _context.DispensadorManguera.ToList();
            }
            catch (Exception) { throw; }
        }

        public paginatedDispensadorMangueraDTO GetAllByNamePaginated(string textToSearch,
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

                int TotalDispensadorManguera = _context.DispensadorManguera.Count();

                var query = from dispensadormanguera in _context.DispensadorManguera
                            join userCreation in _context.User on dispensadormanguera.UserCreationId equals userCreation.UserId
                            join userLastModification in _context.User on dispensadormanguera.UserLastModificationId equals userLastModification.UserId
                            join producto in _context.Producto on dispensadormanguera.ProductoId equals producto.ProductoId
                            join dispensador in _context.Dispensador on dispensadormanguera.DispensadorId equals dispensador.DispensadorId
                            select new { Producto = producto, Dispensador = dispensador, DispensadorManguera = dispensadormanguera, UserCreation = userCreation, UserLastModification = userLastModification };

                // Extraemos los resultados en listas separadas
                List<DispensadorManguera> lstDispensadorManguera = query.Select(result => result.DispensadorManguera)
                        .Where(x => strictSearch ?
                            words.All(word => x.Nombre.Contains(word)) :
                            words.Any(word => x.Nombre.Contains(word)))
                        .OrderByDescending(p => p.DateTimeLastModification)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
                List<Producto> lstProducto = query.Select(result => result.Producto).ToList();
                List<Dispensador> lstDispensador = query.Select(result => result.Dispensador).ToList();
                List<User> lstUserCreation = query.Select(result => result.UserCreation).ToList();
                List<User> lstUserLastModification = query.Select(result => result.UserLastModification).ToList();

                return new paginatedDispensadorMangueraDTO
                {
                    lstDispensadorManguera = lstDispensadorManguera,
                    lstUserCreation = lstUserCreation,
                    lstUserLastModification = lstUserLastModification,
                    lstProducto = lstProducto,
                    lstDispensador = lstDispensador,
                    TotalItems = TotalDispensadorManguera,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
            }
            catch (Exception) { throw; }
        }
        #endregion

        #region Non-Queries
        public bool Add(DispensadorManguera dispensadormanguera)
        {
            try
            {
                _context.DispensadorManguera.Add(dispensadormanguera);
                return _context.SaveChanges() > 0;
            }
            catch (Exception) { throw; }
        }

        public bool Update(DispensadorManguera dispensadormanguera)
        {
            try
            {
                _context.DispensadorManguera.Update(dispensadormanguera);
                return _context.SaveChanges() > 0;
            }
            catch (Exception) { throw; }
        }

        public bool DeleteByDispensadorMangueraId(int dispensadormangueraId)
        {
            try
            {
                AsQueryable()
                        .Where(x => x.DispensadorMangueraId == dispensadormangueraId)
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
                List<DispensadorManguera> lstDispensadorManguera = _context.DispensadorManguera.ToList();

                DataTable DataTable = new();
                DataTable.Columns.Add("DispensadorMangueraId", typeof(string));
                DataTable.Columns.Add("Volumen", typeof(string));
                DataTable.Columns.Add("Active", typeof(string));
                DataTable.Columns.Add("DateTimeCreation", typeof(string));
                DataTable.Columns.Add("DateTimeLastModification", typeof(string));
                DataTable.Columns.Add("UserCreationId", typeof(string));
                DataTable.Columns.Add("UserLastModificationId", typeof(string));
                DataTable.Columns.Add("Nombre", typeof(string));
                DataTable.Columns.Add("DispensadorId", typeof(string));
                DataTable.Columns.Add("ProductoId", typeof(string));
                

                foreach (DispensadorManguera dispensadormanguera in lstDispensadorManguera)
                {
                    DataTable.Rows.Add(
                        dispensadormanguera.Volumen,
                        dispensadormanguera.Active,
                        dispensadormanguera.DateTimeCreation,
                        dispensadormanguera.DateTimeLastModification,
                        dispensadormanguera.UserCreationId,
                        dispensadormanguera.UserLastModificationId,
                        dispensadormanguera.Nombre,
                        dispensadormanguera.DispensadorId,
                        dispensadormanguera.ProductoId
                        
                        );
                }

                return DataTable;
            }
            catch (Exception) { throw; }
        }
        #endregion
    }
}
