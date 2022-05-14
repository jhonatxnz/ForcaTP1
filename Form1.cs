//Jhonatan Willian dos Santos Silva 21686
//Matheus Henrique Pedrozo Traiba 21254
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _21254_21686_Proj3
{
    public partial class FrmForca : Form
    {
        VetorDicionario osDic;          //instanciamos a classe para podermos utilizar seus métodos
        int posicaoDeInclusao;
        Dicionario palavraSorte;        //instanciamos a classe para podermos utilizar seus métodos
        int temporizador = 60;          //60 segundos
        int acertos = 1;                //váriavel referente aos acertos
        int erros = 1;                  //váriavel referente aos erros

        public FrmForca()
        {
            InitializeComponent();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmForca_Load(object sender, EventArgs e)
        {
            stData.Text = DateTime.Now.ToString();       //quando carregar o form mostramos a data e hora ao usário

            foreach (var botao in pnButoes.Controls)     //percorremos os botoes dentro do painel
            {                                            //se ele ja tiver sido clicado desabilitamos ele                        
                var b = (Button)botao;
                b.Enabled = false;
            }
            int indice = 0;
            tsBotoes.ImageList = imlBotoes;
            foreach (ToolStripItem item in tsBotoes.Items)
                if (item is ToolStripButton) // se não é separador:
                    (item as ToolStripButton).ImageIndex = indice++;
            osDic = new VetorDicionario(100); // instancia com vetor dados com 100 posições

            if (dlgAbrir.ShowDialog() == DialogResult.OK)
            {
                var arquivo = new StreamReader(dlgAbrir.FileName);
                while (!arquivo.EndOfStream)
                {
                    Dicionario dadoLido = new Dicionario();
                    dadoLido.LerDados(arquivo); // método da classe Dicionario
                    osDic.Incluir(dadoLido);   // método de VetorDicionario – inclui ao final
                    osDic.ExibirDados(dataGridView1);
                }
                arquivo.Close();
                osDic.PosicionarNoPrimeiro(); // posiciona no 1o registro a visitação nos dados
                AtualizarTela();               // mostra na tela as informações do registro visitado agora 
                if (cbSerial.Checked)
                {
                    
                }
            }
        }
        private void AtualizarTela()
        {
            if (!osDic.EstaVazio)
            {
                int indice = osDic.PosicaoAtual;
                txtPalavra.Text = osDic[indice].Palavra + "";
                txtDica.Text = osDic[indice].Dica;
                TestarBotoes();
                stlbMensagem.Text = "Registro " + (osDic.PosicaoAtual + 1) +
                "/" + osDic.Tamanho;
            }
        }
        private void TestarBotoes()
        {
            btnInicio.Enabled = true;
            btnAnterior.Enabled = true;
            btnProximo.Enabled = true;
            btnUltimo.Enabled = true;
            if (osDic.EstaNoInicio)
            {
                btnInicio.Enabled = false;
                btnAnterior.Enabled = false;
            }

            if (osDic.EstaNoFim)
            {
                btnProximo.Enabled = false;
                btnUltimo.Enabled = false;
            }
        }
        private void LimparTela()
        {
            txtPalavra.Clear();
            txtDica.Clear();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            osDic.SituacaoAtual = Situacao.incluindo;
            LimparTela();
            txtPalavra.ReadOnly = false;
            txtPalavra.Focus();
            stlbMensagem.Text = "Digite a palavra.";
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            osDic.PosicionarNoPrimeiro();
            AtualizarTela();
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            osDic.RetrocederPosicao();
            AtualizarTela();
        }

        private void btnProximo_Click(object sender, EventArgs e)
        {
            osDic.AvancarPosicao();
            AtualizarTela();
        }

        private void btnUltimo_Click(object sender, EventArgs e)
        {
            osDic.PosicionarNoUltimo();
            AtualizarTela();
        }

        private void btnProcurar_Click(object sender, EventArgs e)
        {
            osDic.SituacaoAtual = Situacao.pesquisando;  // entramos no modo de busca
            LimparTela();
            txtPalavra.ReadOnly = false;
            txtPalavra.Focus();
            stlbMensagem.Text = "Digite a palavra procurada.";
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            osDic.SituacaoAtual = Situacao.editando;
            txtPalavra.Focus();
            stlbMensagem.Text = "Digite a nova dica e pressione [Salvar].";
            btnSalvar.Enabled = true;
            txtPalavra.ReadOnly = true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            osDic.SituacaoAtual = Situacao.navegando;
            AtualizarTela();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (osDic.SituacaoAtual == Situacao.incluindo)  // só guarda novo funcionário no vetor se estiver incluindo
            {
                // criamos objeto com o registro do novo funcionário digitado no formulário
                var novaPalavra = new Dicionario(txtPalavra.Text, txtDica.Text);
                osDic.Incluir(novaPalavra, posicaoDeInclusao);
                osDic.SituacaoAtual = Situacao.navegando;  // voltamos ao mode de navegação
                osDic.PosicaoAtual = posicaoDeInclusao;
                AtualizarTela();
            }
            else
            if (osDic.SituacaoAtual == Situacao.editando)
            {
                osDic[osDic.PosicaoAtual].Palavra = txtPalavra.Text;
                osDic[osDic.PosicaoAtual].Dica = txtDica.Text;
                osDic.SituacaoAtual = Situacao.navegando;
            }
            btnSalvar.Enabled = false;    // desabilita pois a inclusão terminou
            txtPalavra.ReadOnly = true;
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja realmente excluir?", "Exclusão",
          MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                osDic.Excluir(osDic.PosicaoAtual);
                if (osDic.PosicaoAtual >= osDic.Tamanho)
                    osDic.PosicionarNoUltimo();
                AtualizarTela();
            }
        }

        private void FrmForca_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dlgAbrir.FileName != "")  // foi selecionado um arquivo com dados
                osDic.GravarDados(dlgAbrir.FileName);
        }

        private void txtPalavra_Leave(object sender, EventArgs e)
        {
            if (osDic.SituacaoAtual == Situacao.incluindo ||
          osDic.SituacaoAtual == Situacao.pesquisando)
                if (txtPalavra.Text == "")
                {
                    MessageBox.Show("Digite uma palavra válida!");
                    txtPalavra.Focus();
                }
                else  // temos um valor digitado no txtPalavra
                {
                    string PalavraProcurada = txtPalavra.Text;
                    int posicao;
                    bool achouRegistro = osDic.ExisteSemOrdem(PalavraProcurada, out posicao);
                    switch (osDic.SituacaoAtual)
                    {
                        case Situacao.incluindo:
                            if (achouRegistro)
                            {
                                MessageBox.Show("palavra repetida! Inclusão cancelada.");
                                osDic.SituacaoAtual = Situacao.navegando;
                                AtualizarTela(); // exibe novamente o registro que estava na tela antes de esta ser limpa
                            }
                            else  // a matrícula não existe e podemos incluí-la no índice ondeIncluir
                            {     // incluí-la no índice ondeIncluir do vetor interno dados de osFunc
                                txtDica.Focus();
                                btnSalvar.Enabled = true;  // habilita quando é possível incluir
                                posicaoDeInclusao = posicao;  // guarda índice de inclusão em variável global
                            }
                            break;

                        case Situacao.pesquisando:
                            if (achouRegistro)
                            {
                                // a variável posicao contém o índice do funcionário que se buscou
                                osDic.PosicaoAtual = posicao;   // reposiciona o índice da posição visitada
                                AtualizarTela();
                            }
                            else
                            {
                                MessageBox.Show("palavra digitada não foi encontrada.");
                                AtualizarTela();  // reexibir o registro que aparecia antes de limparmos a tela
                            }

                            osDic.SituacaoAtual = Situacao.navegando;
                            txtPalavra.ReadOnly = true;
                            break;
                    }
                }

        }
        
        private void A_Click(object sender, EventArgs e)    //todos os botões das letras possuem o mesmo evento
        {
            char letra = (sender as Button).Text[0];
            (sender as Button).Enabled = false;             //se o usário já clicou uma vez no botão ele fica enable
            if (palavraSorte.ExisteLetra(letra))            //verifica  se a letra existe
            {
                (sender as Button).BackColor = Color.Green; //se ele clicou no botão correto o fundo da palavra fica verde,indicando acerto da letra
                lbPontos.Text = "" + acertos++;             //somamos 1 aos pontos que o jogador obteve 
                palavraSorte.Revelar(dgvPalavra);           //escreve a letra na posição correta
            }
            else
            {
                (sender as Button).BackColor = Color.Red;   //se ele clicou no botão correto o fundo da palavra fica vermelho,indicando erro da letra
                lbErros.Text = "" + erros++;                //somamos 1 aos erros que o jogador obteve
                ExibirLedDoErro(erros);                     
                AparecerImagensDeErros();                   //chamamos as imagens de erros a cada erro
            }
            if (palavraSorte.Ganhou())                      //se encontrou todas as letras da palavra que está em jogo 
            {                                               //paramos o jogo e exibimos vitória   
                GanhouOjogo();
                if (MessageBox.Show(" Ganhou! \n Pontos: " + (acertos - 1) + "\n Deseja jogar novamente?", "Parabens", MessageBoxButtons.YesNo) == DialogResult.Yes)   //exibimos os pontos que o  usuário fez e perguntamo se ele quer jogar novamente
                {
                    ResetarTudo();  //método que reseta tudo
                    foreach (var botao in pnButoes.Controls)    //botoes ficam com enable false até clicar o botao iniciar
                    {
                        var b = (Button)botao;
                        b.Enabled = false;
                    }
                }
                else
                {
                    Close();    //fechamos o programa
                }
            }
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            foreach (var botao in pnButoes.Controls)                     //se o usário clicar no botão iniciar habilitamos os botões  
            {
                var b = (Button)botao;
                b.Enabled = true;
            }
            temporizador = 60;                                           //60 segundos
            Random SorteioDePalavra = new Random();                      //iniciamos o método que irá sortear a palavra para o jogo
            int sorteio = SorteioDePalavra.Next(osDic.Tamanho - 1);      //sorteia a palavra -1 por conta do tamanho do vetor
            palavraSorte = osDic.ValorDe(sorteio);                       //retorna resultado do sorteio
            lbDica.Text = " ";                                           //se o usuário jogou uma vez com dica e apertou para jogar novamente sem dica,limpamos o label da dica para não arrumar confusões
            lbTempo.Text = " ";                                          //o mesmo vale para o tempo
            lbPontos.Text = " ";                                         //para os pontos
            lbErros.Text = " ";                                          //e para os erros
            palavraSorte.LimparDgv(dgvPalavra);
            dgvPalavra.ColumnCount = palavraSorte.Palavra.Trim().Length; //dgv fica com o número de colunas da palavra procurada 
            if (Dica())
            {
                lbDica.Text = palavraSorte.Dica;    //exibe a dica no lbDica
                tmrTempo.Interval = 1000;
                tmrTempo.Start();                   //inicia o temporizador
                                                    //se o checkBox está true colocamos um temporizador para o usuário
            }
            lbEstado.Text = "Estado: A forca Iniciou!";             //complemento para o usuário saber que o jogo começou 
        }
        bool Dica()
        {
            return cbDica.Checked;      //se o checkBox estvier selecionado
        }


        private void tmrTempo_Tick(object sender, EventArgs e)
        {
            lbTempo.Text = +temporizador + " segundos"; //exibe os segundos restantes no label
            temporizador--;                             //subtrair do temporizador
            if (temporizador == -1)                     //temporizador chega a zero o usuário perde
            {
                tmrTempo.Stop();
                if (MessageBox.Show("Perdeu por tempo! \n Pontos: " + (acertos - 1) + "\n Deseja jogar novamente?", "Perdedor ", MessageBoxButtons.YesNo) == DialogResult.Yes)   //exibimos os pontos que o  usuário fez e perguntamos se ele quer jogar novamente
                {
                    ResetarTudo();                      //método que reseta tudo
                    foreach (var botao in pnButoes.Controls)    //botoes ficam com enable false até clicar o botao iniciar
                    {
                        var b = (Button)botao;
                        b.Enabled = false;
                    }
                }
                else
                {
                    Close();                            //fechamos o programa
                }
            }
        }
        public void AparecerImagensDeErros()             //a cada erro que o usário comete,uma imagem do homenzinho fica visível
        {
            switch (erros)
            {
                case 2:pbCabeca.Visible = true; break;
                case 3:pbQueixo.Visible = true;break;
                case 4:pbCorpo.Visible = true;break;
                case 5:pbEsquerdo.Visible = true;break;
                case 6:pbDireito.Visible = true;break;
                case 7:pbQuadril.Visible = true;break;
                case 8:pbPEsquerda.Visible = true;break;
                case 9:
                    pbPDireita.Visible = true;          //imagem da perna direita fica visível
                    PerdeuOjogo();                      //erros igual a 8 chamamos o método perdeu 
                    break;
            }
        }
        public void PerdeuOjogo()
        {
            pbCabecaMorta.Visible = true;    //imagem da cabecaMorta fica visível
            pbCabecaMorta.BringToFront();    //colocamos a imagem da cabecaMorta na frente da cabeca viva(que está na forca) 
            pbMorto.Visible = true;          //mostramos o anjinho
            if (MessageBox.Show(" Perdeu! \n Pontos: " + (acertos - 1)+ "\n Deseja jogar novamente?", "Perdedor", MessageBoxButtons.YesNo) == DialogResult.Yes)   //exibimos os pontos que o  usuário fez e perguntamos se ele quer jogar novamente
            {
                ResetarTudo();  //método que reseta tudo
                foreach (var botao in pnButoes.Controls)    //botoes ficam com enable false até clicar o botao iniciar
                {
                    var b = (Button)botao;
                    b.Enabled = false;
                }
            }
            else
            {
                Close();       //fechamos o programa
            }     
        }
        public void GanhouOjogo()
        {

            pbCabeca.Visible = false;        //erros ficam invisíveis
            pbQueixo.Visible = false;       
            pbCorpo.Visible = false;        
            pbEsquerdo.Visible = false;      
            pbDireito.Visible = false;      
            pbQuadril.Visible = false;      
            pbPEsquerda.Visible = false;     
            pbPDireita.Visible = false;     

            pbForca1.Visible = false;       //forca fica invisível
            pbForca2.Visible = false;
            pbForca3.Visible = false;
            pbForca4.Visible = false;
            pbForca5.Visible = false;
            pbForca6.Visible = false;
            pbForca7.Visible = false;
            /////////////////////////////////////////////////////////////////////////

            pbCabeca2.Visible = true;        //imagem do homem vencedor fica visível
            pbQueixo2.Visible = true;        
            pbBarriga2.Visible = true;       
            pbMaoPalito.Visible = true;      
            pbPalito2.Visible = true;        
            pbPalito3.Visible = true;        
            pbDireita2.Visible = true;       
            pbDireito2.Visible = true;       
            pbQuadril2.Visible = true;       
            pbEsquerda2.Visible = true;      
            pbDireita2.Visible = true;       

            pbCabeca2.BringToFront();        //mandamos a imagem do homem vencedor para frente 
            pbQueixo2.BringToFront();        
            pbBarriga2.BringToFront();       
            pbMaoPalito.BringToFront();      
            pbPalito2.BringToFront();        
            pbPalito3.BringToFront();       
            pbDireita2.BringToFront();       
            pbDireito2.BringToFront();       
            pbQuadril2.BringToFront();       
            pbEsquerda2.BringToFront();      
            pbDireita2.BringToFront();       
        }
        public void ResetarImagens() //se o usário perder ou ganhar tiramos todas as imagens da tela
        {
            pbCabeca.Visible = false;
            pbQueixo.Visible = false;
            pbCorpo.Visible = false;
            pbEsquerdo.Visible = false;
            pbDireito.Visible = false;
            pbQuadril.Visible = false;
            pbPEsquerda.Visible = false;
            pbPDireita.Visible = false;
            pbCabecaMorta.Visible = false;
            pbMorto.Visible = false;
            /////////////////////////////////////////////////////////////////////////
            pbCabeca2.Visible = false;
            pbQueixo2.Visible = false;
            pbBarriga2.Visible = false;
            pbMaoPalito.Visible = false;
            pbPalito2.Visible = false;
            pbPalito3.Visible = false;
            pbDireito2.Visible = false;
            pbQuadril2.Visible = false;
            pbEsquerda2.Visible = false;
            pbDireita2.Visible = false;
            /////////////////////////////////////////////////////////////////////////
            pbForca1.Visible = true; //a forca ficará visível para o próximo jogo
            pbForca2.Visible = true;
            pbForca3.Visible = true;
            pbForca4.Visible = true;
            pbForca5.Visible = true;
            pbForca6.Visible = true;
            pbForca7.Visible = true;
        }
        public void ResetarBotoes()
        {
            foreach (var botao in pnButoes.Controls)    //percorremos os botoes dentro do painel
            {                                           //se ele ja tiver sido clicado na partida passada voltamos ele ao normal(botão)
                var b = (Button)botao;
                b.Enabled = true;
                b.BackColor = Color.White;
            }
        }
        public void ResetarTudo() //reseta tudo que havia sido colocado no jogo passado e limpamos o dgv
        {
            ResetarImagens();
            ResetarBotoes();
            lbErros.Text = "";
            lbPontos.Text = "";
            acertos = 1;
            erros = 1;
            tmrTempo.Stop();
            lbTempo.Text = 0.ToString();
            temporizador = 0;
            palavraSorte.LimparDgv(dgvPalavra);
        }

        private void cbSerial_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSerial.Checked)
            {
                /* Muda o nome porta e  Abre a porta serial */
                spArduino.PortName = txtCom.Text;
                try
                {
                    spArduino.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao abrir porta serial ...");
                    cbSerial.Checked = false;
                    return;
                }
            }
            else
            {
                spArduino.Close();  //fechar porta serial
            }
            HabilitaControles(cbSerial.Checked);
        }
        private void HabilitaControles(bool estado)
        {
            btnArduino.Enabled = estado;
            txtCom.Enabled = !estado;
        }

        void ExibirLedDoErro(int erros)
        {
            if (cbSerial.Checked)   //aprendemos isso na aula de tira dúvidas do dia 10/06 
            {
                string str = "";
                if (erros == 2)
                    str = "A";
                if (erros == 3)
                    str = "B";
                if (erros == 4)
                    str = "C";
                if (erros == 5)
                    str = "D";
                if (erros == 6)
                    str = "E";
                if (erros == 7)
                    str = "F";
                if (erros == 8)
                    str = "G";
                if (erros == 9)
                    str = "H";
                if (erros == 0)
                    str = "I";
                spArduino.Write(str);
            }
        }
    }
}

