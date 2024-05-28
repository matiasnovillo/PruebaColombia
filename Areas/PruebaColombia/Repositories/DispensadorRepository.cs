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
    public class DispensadorRepository : IDispensadorRepository
    {
        protected readonly PruebaColombiaContext _context;

        public DispensadorRepository(PruebaColombiaContext context)
        {
            _context = context;
        }

        public IQueryable<Dispensador> AsQueryable()
        {
            try
            {
                return _context.Dispensador.AsQueryable();
            }
            catch (Exception) { throw; }
        }

        #region Queries
        public int Count()
        {
            try
            {
                return _context.Dispensador.Count();
            }
            catch (Exception) { throw; }
        }

        public Dispensador? GetByDispensadorId(int dispensadorId)
        {
            try
            {
                return _context.Dispensador
                            .FirstOrDefault(x => x.DispensadorId == dispensadorId);
            }
            catch (Exception) { throw; }
        }

        public List<Dispensador?> GetAll()
        {
            try
            {
                return _context.Dispensador.ToList();
            }
            catch (Exception) { throw; }
        }

        public visualizacionDeDispensadoresDTO GetAllCustom()
        {
            try
            {
                IQueryable<resultadoVisualizacionDeSurtidoresDTO> query = from dispensador in _context.Dispensador
                            join dispensadormanguera in _context.DispensadorManguera on dispensador.DispensadorId equals dispensadormanguera.DispensadorId
                            join producto in _context.Producto on dispensadormanguera.ProductoId equals producto.ProductoId
                            join precio in _context.Precio on producto.ProductoId equals precio.ProductoId 
                            select new resultadoVisualizacionDeSurtidoresDTO
                            { 
                                Precio = precio,
                                Dispensador = dispensador,
                                DispensadorManguera = dispensadormanguera,
                                Producto = producto
                            };

                // Extraemos los resultados en listas separadas
                List<Dispensador> lstDispensador = query.Select(result => result.Dispensador).ToList();

                List<DispensadorManguera> lstDispensadorManguera = [];
                List<Producto> lstProducto = [];
                List<Precio> lstPrecio = [];

                foreach (Dispensador dispensador in lstDispensador)
                {
                    lstDispensadorManguera.Add(query
                        .Select(result => result.DispensadorManguera)
                        .Where(x => x.DispensadorId == dispensador.DispensadorId)
                        .FirstOrDefault());
                }

                foreach (DispensadorManguera dispensadorManguera in lstDispensadorManguera)
                {
                    lstProducto.Add(query
                        .Select(result => result.Producto)
                        .Where(x => x.ProductoId == dispensadorManguera.ProductoId)
                        .FirstOrDefault());
                }

                foreach (Producto producto in lstProducto)
                {
                    lstPrecio.Add(query
                        .Select(result => result.Precio)
                        .Where(x => x.ProductoId == producto.ProductoId)
                        .FirstOrDefault());
                }

                return new visualizacionDeDispensadoresDTO
                {
                    lstDispensador = lstDispensador,
                    lstDispensadorManguera = lstDispensadorManguera,
                    lstPrecio = lstPrecio,
                    lstProducto = lstProducto
                };
            }
            catch (Exception) { throw; }
        }

        public paginatedDispensadorDTO GetAllByDispensadorIdPaginated(string textToSearch,
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

                int TotalDispensador = _context.Dispensador.Count();

                var query = from dispensador in _context.Dispensador
                            join userCreation in _context.User on dispensador.UserCreationId equals userCreation.UserId
                            join userLastModification in _context.User on dispensador.UserLastModificationId equals userLastModification.UserId
                            select new { Dispensador = dispensador, UserCreation = userCreation, UserLastModification = userLastModification };

                // Extraemos los resultados en listas separadas
                List<Dispensador> lstDispensador = query.Select(result => result.Dispensador)
                        .Where(x => strictSearch ?
                            words.All(word => x.DispensadorId.ToString().Contains(word)) :
                            words.Any(word => x.DispensadorId.ToString().Contains(word)))
                        .OrderByDescending(p => p.DateTimeLastModification)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
                List<User> lstUserCreation = query.Select(result => result.UserCreation).ToList();
                List<User> lstUserLastModification = query.Select(result => result.UserLastModification).ToList();

                return new paginatedDispensadorDTO
                {
                    lstDispensador = lstDispensador,
                    lstUserCreation = lstUserCreation,
                    lstUserLastModification = lstUserLastModification,
                    TotalItems = TotalDispensador,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
            }
            catch (Exception) { throw; }
        }
        #endregion

        #region Non-Queries
        public bool Add(Dispensador dispensador)
        {
            try
            {
                _context.Dispensador.Add(dispensador);
                return _context.SaveChanges() > 0;
            }
            catch (Exception) { throw; }
        }

        public bool Update(Dispensador dispensador)
        {
            try
            {
                _context.Dispensador.Update(dispensador);
                return _context.SaveChanges() > 0;
            }
            catch (Exception) { throw; }
        }

        public bool DeleteByDispensadorId(int dispensadorId)
        {
            try
            {
                AsQueryable()
                        .Where(x => x.DispensadorId == dispensadorId)
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
                List<Dispensador> lstDispensador = _context.Dispensador.ToList();

                DataTable DataTable = new();
                DataTable.Columns.Add("DispensadorId", typeof(string));
                DataTable.Columns.Add("Active", typeof(string));
                DataTable.Columns.Add("DateTimeCreation", typeof(string));
                DataTable.Columns.Add("DateTimeLastModification", typeof(string));
                DataTable.Columns.Add("UserCreationId", typeof(string));
                DataTable.Columns.Add("UserLastModificationId", typeof(string));
                DataTable.Columns.Add("Nombre", typeof(string));
                DataTable.Columns.Add("CantidadDeManguera", typeof(string));
                

                foreach (Dispensador dispensador in lstDispensador)
                {
                    DataTable.Rows.Add(
                        dispensador.DispensadorId,
                        dispensador.Active,
                        dispensador.DateTimeCreation,
                        dispensador.DateTimeLastModification,
                        dispensador.UserCreationId,
                        dispensador.UserLastModificationId,
                        dispensador.Nombre,
                        dispensador.CantidadDeManguera
                        
                        );
                }

                return DataTable;
            }
            catch (Exception) { throw; }
        }
        #endregion
    }
}
