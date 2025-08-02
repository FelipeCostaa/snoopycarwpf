using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SnoopyCarWPF.Data;
using SnoopyCarWPF.Models;

namespace SnoopyCarWPF
{
    public partial class ClientesWindow : Window
    {
        private Cliente _clienteSelecionado;

        public ClientesWindow()
        {
            InitializeComponent();
            // Conecta o DataGrid ao DataStore central
            ClientesDataGrid.ItemsSource = DataStore.Clientes;
        }

        private void ClientesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _clienteSelecionado = ClientesDataGrid.SelectedItem as Cliente;
            if (_clienteSelecionado != null)
            {
                NomeTextBox.Text = _clienteSelecionado.Nome;
                TelefoneTextBox.Text = _clienteSelecionado.Telefone;
                EnderecoTextBox.Text = _clienteSelecionado.Endereco;
                VeiculoTextBox.Text = _clienteSelecionado.Veiculo;
            }
        }

        private void BtnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NomeTextBox.Text))
            {
                MessageBox.Show("O campo 'Nome' é obrigatório.", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var novoCliente = new Cliente
            {
                Nome = NomeTextBox.Text,
                Telefone = TelefoneTextBox.Text,
                Endereco = EnderecoTextBox.Text,
                Veiculo = VeiculoTextBox.Text
            };
            DataStore.Clientes.Add(novoCliente);
            LimparCampos();
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (_clienteSelecionado != null)
            {
                _clienteSelecionado.Nome = NomeTextBox.Text;
                _clienteSelecionado.Telefone = TelefoneTextBox.Text;
                _clienteSelecionado.Endereco = EnderecoTextBox.Text;
                _clienteSelecionado.Veiculo = VeiculoTextBox.Text;
                MessageBox.Show("Cliente atualizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                LimparCampos();
            }
        }

        private void BtnExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (_clienteSelecionado != null)
            {
                var resultado = MessageBox.Show($"Tem certeza que deseja excluir '{_clienteSelecionado.Nome}'?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.Yes)
                {
                    DataStore.Clientes.Remove(_clienteSelecionado);
                    LimparCampos();
                }
            }
        }

        private void BtnLimpar_Click(object sender, RoutedEventArgs e)
        {
            LimparCampos();
        }

        private void LimparCampos()
        {
            NomeTextBox.Clear();
            TelefoneTextBox.Clear();
            EnderecoTextBox.Clear();
            VeiculoTextBox.Clear();
            ClientesDataGrid.SelectedItem = null;
            _clienteSelecionado = null;
            NomeTextBox.Focus();
        }
    }
}
