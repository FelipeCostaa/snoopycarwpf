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
    public partial class ProdutosWindow : Window
    {
        private Produto _produtoSelecionado;

        public ProdutosWindow()
        {
            InitializeComponent();
            ProdutosDataGrid.ItemsSource = DataStore.Produtos;
        }

        private void ProdutosDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _produtoSelecionado = ProdutosDataGrid.SelectedItem as Produto;
            if (_produtoSelecionado != null)
            {
                NomeProdutoTextBox.Text = _produtoSelecionado.Nome;
                QuantidadeTextBox.Text = _produtoSelecionado.Quantidade.ToString();
            }
        }

        private void BtnAdicionarProduto_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(QuantidadeTextBox.Text, out int quantidade))
            {
                MessageBox.Show("A quantidade deve ser um número válido.", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var novoProduto = new Produto
            {
                Nome = NomeProdutoTextBox.Text,
                Quantidade = quantidade
            };
            DataStore.Produtos.Add(novoProduto);
            LimparCamposProduto();
        }

        private void BtnSalvarProduto_Click(object sender, RoutedEventArgs e)
        {
            if (_produtoSelecionado != null)
            {
                if (!int.TryParse(QuantidadeTextBox.Text, out int quantidade))
                {
                    MessageBox.Show("A quantidade deve ser um número válido.", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                _produtoSelecionado.Nome = NomeProdutoTextBox.Text;
                _produtoSelecionado.Quantidade = quantidade;
                MessageBox.Show("Produto atualizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                LimparCamposProduto();
            }
        }

        private void BtnExcluirProduto_Click(object sender, RoutedEventArgs e)
        {
            if (_produtoSelecionado != null)
            {
                var resultado = MessageBox.Show($"Tem certeza que deseja excluir '{_produtoSelecionado.Nome}'?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.Yes)
                {
                    DataStore.Produtos.Remove(_produtoSelecionado);
                    LimparCamposProduto();
                }
            }
        }

        private void BtnLimparProduto_Click(object sender, RoutedEventArgs e)
        {
            LimparCamposProduto();
        }

        private void LimparCamposProduto()
        {
            NomeProdutoTextBox.Clear();
            QuantidadeTextBox.Clear();
            ProdutosDataGrid.SelectedItem = null;
            _produtoSelecionado = null;
            NomeProdutoTextBox.Focus();
        }
    }
}
