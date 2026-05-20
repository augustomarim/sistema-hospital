using System;
using System.Collections.Generic;
using System.Text;

using HospitalSystem.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Data.Repositories;

public class ConsultaRepository : Repository<Consulta>
{
    public ConsultaRepository(HospitalDbContext context) : base(context) { }

    public async Task<IEnumerable<Consulta>> BuscarPorPeriodoAsync(DateTime inicio, DateTime fim)
    {
        var ids = new List<int>();
        var connection = _context.Database.GetDbConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT Id FROM Consultas WHERE DataHora BETWEEN @inicio AND @fim";
        command.Parameters.Add(new SqliteParameter("@inicio", inicio.ToString("yyyy-MM-dd")));
        command.Parameters.Add(new SqliteParameter("@fim", fim.ToString("yyyy-MM-dd")));

        using var result = await command.ExecuteReaderAsync();
        while (await result.ReadAsync())
            ids.Add(result.GetInt32(0));

        return await _context.Consultas
            .Include(c => c.Paciente)
            .Include(c => c.Medico)
            .Where(c => ids.Contains(c.Id))
            .ToListAsync();
    }
}
