using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API.Entities;

public partial class HISTAFF_SQL_DEVContext : DbContext
{
    public HISTAFF_SQL_DEVContext()
    {
    }

    public HISTAFF_SQL_DEVContext(DbContextOptions<HISTAFF_SQL_DEVContext> options)
        : base(options)
    {
    }

    public virtual DbSet<REPORT_DATA_STAFF_PROFILE> REPORT_DATA_STAFF_PROFILEs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Password=MatKhau@123;User ID=sa;Initial Catalog=HISTAFF_SQL_DEV;Data Source=192.168.60.22,1433;TrustServerCertificate=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<REPORT_DATA_STAFF_PROFILE>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("REPORT_DATA_STAFF_PROFILE");

            entity.Property(e => e.AVATAR).HasMaxLength(1000);
            entity.Property(e => e.FULL_NAME).HasMaxLength(100);
            entity.Property(e => e.GENDER).HasMaxLength(400);
            entity.Property(e => e.ID_NO).HasMaxLength(20);
            entity.Property(e => e.OTHER_NAME).HasMaxLength(150);
        });
        modelBuilder.HasSequence("seq_at_request");
        modelBuilder.HasSequence("seq_employee_details").IncrementsBy(2);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
