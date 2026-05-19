using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalSystem.Models;

public class FichaMedica
{
    public int Id { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public string HistoricoAlergias { get; set; } = string.Empty;

    public int PacienteId { get; set; }
    public Paciente? Paciente { get; set; }
}
