using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API.Entities;

public partial class VEAM_SQL_DEVContext : DbContext
{
    public VEAM_SQL_DEVContext()
    {
    }

    public VEAM_SQL_DEVContext(DbContextOptions<VEAM_SQL_DEVContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SYS_MIGRATION_LOG> SYS_MIGRATION_LOGs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Password=MatKhau@123;User ID=sa;Initial Catalog=VEAM_SQL_DEV;Data Source=192.168.60.22,1433;TrustServerCertificate=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SYS_MIGRATION_LOG>(entity =>
        {
            entity.ToTable("SYS_MIGRATION_LOG");

            entity.Property(e => e.CREATED_BY)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.NEW_STRING_ID)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.OLD_STRING_ID)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.TABLE_NAME)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UPDATED_BY)
                .HasMaxLength(36)
                .IsUnicode(false);
        });
        modelBuilder.HasSequence("seq_at_request");
        modelBuilder.HasSequence("seq_employee_details").IncrementsBy(2);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
