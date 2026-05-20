using System;
using System.Collections.Generic;
using System.Text;

using HospitalSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Data;

public class HospitalDbContext : DbContext
{
    public DbSet<Paciente> Pacientes => Set<Paciente>();
    public DbSet<FichaMedica> FichasMedicas => Set<FichaMedica>();
    public DbSet<Medico> Medicos => Set<Medico>();
    public DbSet<Enfermeiro> Enfermeiros => Set<Enfermeiro>();
    public DbSet<Departamento> Departamentos => Set<Departamento>();
    public DbSet<Consulta> Consultas => Set<Consulta>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=hospital.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Funcionario>()
            .HasDiscriminator<string>("Tipo")
            .HasValue<Medico>("Medico")
            .HasValue<Enfermeiro>("Enfermeiro");

        modelBuilder.Entity<Paciente>()
            .HasOne(p => p.FichaMedica)
            .WithOne(f => f.Paciente)
            .HasForeignKey<FichaMedica>(f => f.PacienteId);

        modelBuilder.Entity<Funcionario>()
            .HasOne(f => f.Departamento)
            .WithMany(d => d.Funcionarios)
            .HasForeignKey(f => f.DepartamentoId)
            .IsRequired(false);

        modelBuilder.Entity<Consulta>()
            .HasOne(c => c.Paciente)
            .WithMany(p => p.Consultas)
            .HasForeignKey(c => c.PacienteId);

        modelBuilder.Entity<Consulta>()
            .HasOne(c => c.Medico)
            .WithMany(m => m.Consultas)
            .HasForeignKey(c => c.MedicoId);
    }
}