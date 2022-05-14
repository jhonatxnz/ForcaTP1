//Jhonatan Willian dos Santos Silva 21686
//Matheus Henrique Pedrozo Traiba 21254
using System;
using System.IO;
using System.Windows.Forms;
class Dicionario
{
    const int iniPalavra = 0,
              tamPalavra = 15,
              iniDica = iniPalavra + tamPalavra,
              tamDica = 100;

    string palavra,
           dica;
    bool[] acertou = new bool[15];
    //para marcarmos true na posição onde o jogador acertou uma letra

    public void LimparAcertos()
    {
        for (int i = 0; i < acertou.Length; i++)
            acertou[i] = false;
    }
    public void Acertou()
    {
        for (int i = 0; i < acertou.Length; i++)
            acertou[i] = false;

    }
    public Dicionario(string palavra, string dica)
    {
        Palavra = palavra;
        Dica = dica;
    }

    public Dicionario()
    {
        Palavra = " ";
        Dica = " ";
    }

    public string Palavra
    {
        get => palavra;
        set
        {
            if (value.Length > tamPalavra)
                value = value.Substring(0, tamPalavra);
            palavra = value.PadRight(tamPalavra, ' ');
        }
    }
    public string Dica
    {
        get => dica;
        set
        {
            if (value.Length > tamDica)
                value = value.Substring(0, tamDica);
            dica = value.PadRight(tamDica, ' ');
        }
    }
    public void LerDados(StreamReader arq)
    {
        if (!arq.EndOfStream)
        {
            String linha = arq.ReadLine();
            Palavra = linha.Substring(iniPalavra, tamPalavra);
            Dica = linha.Substring(iniDica, tamDica);
        }
    }
    public String FormatoDeArquivo()
    {
        return Palavra.PadLeft(tamPalavra, ' ') +
               Dica.PadLeft(tamDica, ' ');

        //return $"{Palavra.PadLeft(tamPalavra)}  {Dica.PadLeft(tamDica)}";
    }
    public bool ExisteLetra(char palavraProcurada)  // onde --> posicao onde achou ou onde deveria estar (inclusão)
    {
        bool achou = false;
        for (int i = 0; i < palavra.Length; i++) // condição
        {
            if (palavra[i] == palavraProcurada) // verifica se a letra da palavra é igual a letra procurada
            {
                acertou[i] = true;
                achou = true; // achou a letra
            }
        }
        return achou;   
    }
    public void Revelar(DataGridView dgvRevelar)    //método que coloca a letra na posição correta
    {
        for (int i = 0; i < palavra.Length; i++)
        {
            if (acertou[i]) //se acertou na posição procurada
            {
                dgvRevelar[i, 0].Value = palavra[i];    //coloca a letra na posiçao correta
            }
        }
    }
    public bool Ganhou()    //método que indica que todas as letras foram encontradas
    {
        for (int i = 0; i < palavra.Trim().Length; i++) // usamos o trim para deixar o tamanho da palavra "correta"
        {
            if (acertou[i] == false)
            {
                return false;
            }
        }
        return true;
    }
    public void LimparDgv(DataGridView dgvRevelar) //método que irá limpar o dgv
    {
        for (int i = 0; i < palavra.Length; i++)
        {
            if (acertou[i])
            {
                dgvRevelar[i, 0].Value = " ";   //recebe cadeia vazia para não ficar com as letra do jogo passado
            }
        }
    }
}

