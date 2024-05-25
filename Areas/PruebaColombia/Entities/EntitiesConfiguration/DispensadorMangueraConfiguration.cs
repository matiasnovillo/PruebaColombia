using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

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

namespace PruebaColombia.Areas.PruebaColombia.Entities.EntitiesConfiguration
{
    public class DispensadorMangueraConfiguration : IEntityTypeConfiguration<DispensadorManguera>
    {
        public void Configure(EntityTypeBuilder<DispensadorManguera> entity)
        {
            try
            {
                //DispensadorMangueraId
                entity.HasKey(e => e.DispensadorMangueraId);
                entity.Property(e => e.DispensadorMangueraId)
                    .ValueGeneratedOnAdd();

                //Volumen
                entity.Property(e => e.Volumen)
                    .HasColumnType("numeric(18, 2)")
                    .IsRequired(true);

                //Active
                entity.Property(e => e.Active)
                    .HasColumnType("tinyint")
                    .IsRequired(true);

                //DateTimeCreation
                entity.Property(e => e.DateTimeCreation)
                    .HasColumnType("datetime")
                    .IsRequired(true);

                //DateTimeLastModification
                entity.Property(e => e.DateTimeLastModification)
                    .HasColumnType("datetime")
                    .IsRequired(true);

                //UserCreationId
                entity.Property(e => e.UserCreationId)
                    .HasColumnType("int")
                    .IsRequired(true);

                //UserLastModificationId
                entity.Property(e => e.UserLastModificationId)
                    .HasColumnType("int")
                    .IsRequired(true);

                //Nombre
                entity.Property(e => e.Nombre)
                    .HasColumnType("varchar(100)")
                    .IsRequired(true);

                //DispensadorId
                entity.Property(e => e.DispensadorId)
                    .HasColumnType("int")
                    .IsRequired(true);

                //ProductoId
                entity.Property(e => e.ProductoId)
                    .HasColumnType("int")
                    .IsRequired(true);

                
            }
            catch (Exception) { throw; }
        }
    }
}
