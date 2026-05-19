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

public partial class PacientesView : Window
{
    private readonly PacienteController _controller;

    public PacientesView(PacienteController controller)
    {
        InitializeComponent();
        _controller = controller;
        _ = CarregarGridAsync();
    }

    private async Task CarregarGridAsync()
    {
        var pacientes = await _controller.CarregarTodosAsync();
        dgPacientes.ItemsSource = pacientes.ToList();
    }

    private async void btnSalvar_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var paciente = new Paciente
            {
                Nome = txtNome.Text.Trim(),
                CPF = txtCPF.Text.Trim(),
                Telefone = txtTelefone.Text.Trim(),
                TipoSanguineo = (cmbTipoSanguineo.SelectedItem as ComboBoxItem)?.Content.ToString() ?? string.Empty
            };

            await _controller.SalvarAsync(paciente);
            await CarregarGridAsync();
            LimparFormulario();
            MessageBox.Show("Paciente salvo com sucesso!", "Sucesso",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Validação",
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private async void btnExcluir_Click(object sender, RoutedEventArgs e)
    {
        if (dgPacientes.SelectedItem is not Paciente selecionado) return;

        var confirm = MessageBox.Show("Confirmar exclusão?", "Excluir",
            MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (confirm == MessageBoxResult.Yes)
        {
            await _controller.ExcluirAsync(selecionado.Id);
            await CarregarGridAsync();
        }
    }

    private void btnLimpar_Click(object sender, RoutedEventArgs e)
        => LimparFormulario();

    private void LimparFormulario()
    {
        txtNome.Clear();
        txtCPF.Clear();
        txtTelefone.Clear();
        cmbTipoSanguineo.SelectedIndex = -1;
    }
}
