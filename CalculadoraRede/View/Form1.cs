using CalculadoraRede.Controler;
using CalculadoraRede.Entidade;
using CalculadoraRede.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculadoraRede
{
    public partial class Form1 : Form
    {
        
        Controle controle = new Controle();
        public Form1()
        {
            InitializeComponent();
            toolTip1.SetToolTip(txtQuantidade, "Valores inválidos serão alterados para o valor mais próximo possível!");
        }

        private void btnProsseguir_Click(object sender, EventArgs e)
        {
            controle.adicionarBase(txtIp.Text);

            btnProsseguir.Enabled = false;

            txtIp.Enabled = false;

            if (controle.adicionarBase(txtIp.Text) == "Maior"){

                MessageBox.Show("Somente endereços Classe A, B e C permitidos!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else if (controle.adicionarBase(txtIp.Text) == "Invalido"){

                MessageBox.Show("Endereço IP Inválido!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }else{

                richTextBox1.Height = 289;
                richTextBox1.Location = new System.Drawing.Point(0, 287);
            }
        }

        public void Preencher()
        {

            List<Host> lista = controle.retornarListaHost();
            var preecherTabela = lista.Select(hosts => new
            {
                Hosts = hosts.Hosts,
                Mais2 = hosts.Mais2,
                Total = hosts.Total
            }).ToList();

            tabela.DataSource = preecherTabela;
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            controle.adicionarHost(Convert.ToInt32(txtQuantidade.Value));

            Preencher();
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            btnCalcular.Enabled = false;
            btnAdd.Enabled = false;
            tabela.ReadOnly = true;

            if (controle.verificarQuantidadeHost() == false)
            {
                MessageBox.Show("Número de Host excedidos pela Classe " + controle.retornarClasse() + " .Remova hosts da lista", "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }else{
                mostrarTela();
            }
        }

       private void mostrarTela()
        {
            richTextBox1.SelectionColor = Color.MediumPurple;

            richTextBox1.AppendText("Utilizando IP base: \t" + controle.retornarIpBase() + Environment.NewLine);

            richTextBox1.SelectionColor = Color.MediumPurple;

            richTextBox1.AppendText("Endereço Classe: \t" + controle.retornarClasse() + Environment.NewLine);

            richTextBox1.SelectionColor = Color.MediumPurple;

            richTextBox1.AppendText("Máscara Padrão: \t" + controle.retornarMascaraPadrao() + "\n\n");

            richTextBox1.SelectionColor = Color.Salmon;

            richTextBox1.AppendText(controle.converterBinario(controle.retornarIpBase())+"\n");

            richTextBox1.SelectionColor = Color.Salmon;

            richTextBox1.AppendText(controle.converterBinario(controle.retornarMascaraPadrao())+"\n\n");

            List<Subrede> lista = controle.retornarSubrede();
            for (int i = 0; i < lista.Count; i++)
            {
                richTextBox1.SelectionColor = Color.Green;

                richTextBox1.AppendText("Subnet "+lista[i].NumeroSubrede +": ");

                richTextBox1.SelectionColor = Color.Blue;

                richTextBox1.AppendText(lista[i].IdRede + "\t/ " + lista[i].MascaraCustomizada);

                richTextBox1.SelectionColor = Color.Purple;

                richTextBox1.AppendText("\nAcrescentando "+lista[i].TotalHost+ " hosts ao endereço de base\n\n");

                richTextBox1.SelectionColor = Color.Purple;

                richTextBox1.AppendText(controle.converterBinario(lista[i].IdRede) + "\n");

                richTextBox1.SelectionColor = Color.Purple;

                richTextBox1.AppendText(controle.converterBinario(lista[i].MascaraCustomizada) + "\n");

                richTextBox1.SelectionColor = Color.MediumVioletRed;

                richTextBox1.AppendText("Broadcast: "+lista[i].Broadcast + "\n");

                richTextBox1.SelectionColor = Color.MediumVioletRed;

                richTextBox1.AppendText(controle.converterBinario(lista[i].Broadcast) + "\n\n");

            }
            
        }

        private void Tabela_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            controle.excluir(tabela.CurrentRow.Index);
            Preencher();
        }

        private void BtnRecomecar_Click(object sender, EventArgs e)
        {
            controle.limpar();

            richTextBox1.Clear();

            richTextBox1.Height = 497;

            richTextBox1.Location = new System.Drawing.Point(0, 64);

            btnProsseguir.Enabled = true;

            btnAdd.Enabled = true;

            btnCalcular.Enabled = true;

            txtIp.Enabled = true;

            Preencher();
        }
    }
}
