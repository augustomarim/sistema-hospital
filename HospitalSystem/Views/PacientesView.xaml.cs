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

    private void txtCPF_PreviewTextInput(object sender, TextCompositionEventArgs e)
        => e.Handled = !char.IsDigit(e.Text[0]);

    private void txtCPF_TextChanged(object sender, TextChangedEventArgs e)
    {
        var tb = (TextBox)sender;
        var digits = new string(tb.Text.Where(char.IsDigit).ToArray());

        var formatted = digits.Length switch
        {
            <= 3 => digits,
            <= 6 => $"{digits[..3]}.{digits[3..]}",
            <= 9 => $"{digits[..3]}.{digits[3..6]}.{digits[6..]}",
            _ => $"{digits[..3]}.{digits[3..6]}.{digits[6..9]}-{digits[9..Math.Min(11, digits.Length)]}"
        };

        tb.TextChanged -= txtCPF_TextChanged;
        tb.Text = formatted;
        tb.CaretIndex = formatted.Length;
        tb.TextChanged += txtCPF_TextChanged;
    }

    private void txtTelefone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        => e.Handled = !char.IsDigit(e.Text[0]);

    private void txtTelefone_TextChanged(object sender, TextChangedEventArgs e)
    {
        var tb = (TextBox)sender;
        var digits = new string(tb.Text.Where(char.IsDigit).ToArray());

        var formatted = digits.Length switch
        {
            0 => "",
            <= 2 => $"({digits}",
            <= 7 => $"({digits[..2]}) {digits[2..]}",
            <= 11 => $"({digits[..2]}) {digits[2..7]}-{digits[7..]}",
            _ => tb.Text
        };

        tb.TextChanged -= txtTelefone_TextChanged;
        tb.Text = formatted;
        tb.CaretIndex = formatted.Length;
        tb.TextChanged += txtTelefone_TextChanged;
    }

    private void btnVerFicha_Click(object sender, RoutedEventArgs e)
    {
        if (dgPacientes.SelectedItem is not Paciente selecionado)
        {
            MessageBox.Show("Selecione um paciente.", "Aviso",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        new FichaMedicaView(selecionado, _controller).ShowDialog();
    }
}