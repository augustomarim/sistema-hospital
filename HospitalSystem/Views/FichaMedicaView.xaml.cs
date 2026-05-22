using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using HospitalSystem.Controllers;
using HospitalSystem.Models;

namespace HospitalSystem.Views;

public partial class FichaMedicaView : Window
{
    private readonly Paciente _paciente;
    private readonly PacienteController _controller;

    public FichaMedicaView(Paciente paciente, PacienteController controller)
    {
        InitializeComponent();
        _paciente = paciente;
        _controller = controller;
        PreencherDados();
    }

    private void PreencherDados()
    {
        txtNome.Text = _paciente.Nome;
        txtTipoSanguineo.Text = _paciente.TipoSanguineo;
        txtDataCriacao.Text = _paciente.FichaMedica?.DataCriacao.ToString("dd/MM/yyyy") ?? "Não gerada";
        txtAlergias.Text = _paciente.FichaMedica?.HistoricoAlergias ?? string.Empty;
    }

    private async void btnSalvarFicha_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (_paciente.FichaMedica is null)
                _paciente.FichaMedica = new FichaMedica { DataCriacao = DateTime.Now };

            _paciente.FichaMedica.HistoricoAlergias = txtAlergias.Text.Trim();

            await _controller.AtualizarAsync(_paciente);

            MessageBox.Show("Ficha salva com sucesso!", "Sucesso",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Erro",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void btnFechar_Click(object sender, RoutedEventArgs e)
        => Close();
}