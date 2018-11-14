using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LexicoWF
{
    public partial class Lexico : Form //mudar nome da classe
    {

        public Tokens token = new Tokens(); //Def.token = TipoToken
        int terminaPrograma = 0;
        int indiceTexto = 0;
        int linhaTexto = 1;
        string conteudo;
        public Lexico()
        {
            InitializeComponent();
        }
 

        private void abrirArquivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName); //Abre arquivo fonte

                richTextBox1.Text = sr.ReadToEnd();

                sr.Close();
            }
        }


        public void SetTerminaPrograma(int terminaProg)
        {
            terminaPrograma = terminaProg;
        }
        public Tokens AnalisadorLexical(string conteudo)
        {
            //salvar código do rich em arquivo (fazer)
            //MessageBox.Show("c");
            if (terminaPrograma == 0)
            {

                do
                {
                    int j = 0;

                    char caracter;
                    //MessageBox.Show(indiceTexto.ToString());

                    //MessageBox.Show("e");

                    caracter = conteudo[indiceTexto];   //ler caracter
                    if (caracter == '\n')
                    {
                        linhaTexto++;
                        //MessageBox.Show(linhaTexto.ToString());
                    }



                    //MessageBox.Show("1"+caracter.ToString(), indiceTexto.ToString());
                    //indiceTexto++;


                    while ((char.IsWhiteSpace(caracter) || caracter == '\n') && indiceTexto + 1 < conteudo.Length)
                    {
                        //MessageBox.Show("vazio: "+ caracter.ToString(), indiceTexto.ToString());

                        indiceTexto++;
                        caracter = conteudo[indiceTexto];

                        if (caracter == '\n')
                        {
                            linhaTexto++;
                            //MessageBox.Show(linhaTexto.ToString());
                        }
                        //MessageBox.Show(caracter.ToString(), indiceTexto.ToString());


                    }

                    while (j < conteudo.Length)
                    {

                        if (caracter == '{' || caracter == ' ')
                        {

                            if (caracter == '{')
                            {
                                int achouChave = 0;
                                while (caracter != '}' && terminaPrograma != 1) //&& terminaPrograma != 1
                                {
                                    if (caracter == '\n')
                                    {
                                        linhaTexto++;
                                       // MessageBox.Show(linhaTexto.ToString());
                                    }

                                    if (indiceTexto + 1 < conteudo.Length)
                                    {
                                        indiceTexto++;
                                        caracter = conteudo[indiceTexto];
                                        if (caracter == '\n')
                                        {
                                            linhaTexto++;
                                            //MessageBox.Show(linhaTexto.ToString());
                                        }
                                        if (caracter == '}')
                                        {

                                            achouChave = 1;
                                        }

                                    }

                                    if (indiceTexto == conteudo.Length - 1)
                                    {
                                        terminaPrograma = 1;
                                    }

                                }

                                if (achouChave == 0)
                                {
                                    linhaTexto--;
                                    TrataErro('{');
                                    terminaPrograma = 1;
                                }

                                if (achouChave == 1 && terminaPrograma == 0)
                                {
                                    indiceTexto++;
                                    caracter = conteudo[indiceTexto];

                                    if (caracter == '\n')
                                    {
                                        linhaTexto++;
                                        //MessageBox.Show(linhaTexto.ToString());
                                    }

                                }
                                //indiceTexto++;
                                //caracter = conteudo[indiceTexto];
                            }

                            while ((char.IsWhiteSpace(caracter) || caracter == '\n') && indiceTexto + 1 < conteudo.Length)
                            {

                                indiceTexto++;
                                caracter = conteudo[indiceTexto];
                                if (caracter == '\n')
                                {
                                    linhaTexto++;
                                    //MessageBox.Show(linhaTexto.ToString());
                                }
                            }
                        }
                        j++;
                    }


                    if (caracter == '\n')
                    {
                        //indiceTexto++;
                        linhaTexto++;
                    }

                    if (terminaPrograma != 1)
                    {
                        indiceTexto++;
                        return PegaToken(caracter, conteudo);

                    }

                } while (indiceTexto < conteudo.Length && terminaPrograma == 0);

                return token;
            }
            else
            {
                return token;
            }
        }



        private Tokens PegaToken(char caracter, string conteudo)
        {
            if (Char.IsDigit(caracter))
            {
                return TrataDigito(caracter, conteudo);
                //indiceTexto--;
            }
            else if (Char.IsLetter(caracter))
            {
                //MessageBox.Show(caracter.ToString());

                return TrataId_Palavra(caracter, conteudo);
               //indiceTexto--; //descrementavamos o indice depois de adicionar na lista

            }
            else if (caracter == ':')
            {
                return TrataAtribuicao(caracter, conteudo);
            }
            else if (caracter == '+' || caracter == '-' || caracter == '*')
            {
                return TrataOperadorAritmetico(caracter, conteudo);
            }
            else if (caracter == '<' || caracter == '>' || caracter == '=' || caracter == '!')
            {
                return TrataOperadorRelacional(caracter, conteudo);
            }
            else if (caracter == ';' || caracter == ',' || caracter == '(' || caracter == ')' || caracter == '.')
            {
                return TrataPontuacao(caracter, conteudo);
            }
            else
            {
                //erro
                TrataErro(caracter);
            }
            return token;
        }

        private void TrataErro(char caracter)
        {
            if (!Char.IsWhiteSpace(caracter) || caracter == '!' || caracter == '.')
            {
                dataGridView2.Rows.Add(new object[] { linhaTexto, "Caracter não reconhecido: " + caracter });
                terminaPrograma = 1;
            }
        }

        private Tokens TrataPontuacao(char caracter, string conteudo)
        {

            if (caracter == '.')
            {
                token.lexema = ".";
                token.simbolo = "sponto";
                token.numLinha = linhaTexto;
                dataGridView1.Rows.Add(new object[] { linhaTexto, "sponto", '.' });
                terminaPrograma = 1;
                return token;

            }
            else if (caracter == ';')
            {
                token.lexema = ";";
                token.simbolo = "sponto_virgula";
                token.numLinha = linhaTexto;
                dataGridView1.Rows.Add(new object[] { linhaTexto, "sponto_virgula", ';' });
                return token;

            }
            else if (caracter == ',')
            {

                token.lexema = ",";
                token.simbolo = "svirgula";
                token.numLinha = linhaTexto;
                dataGridView1.Rows.Add(new object[] { linhaTexto, "svirgula", ',' });
                return token;

            }
            else if (caracter == '(')
            {
                token.lexema = "(";
                token.simbolo = "sabre_parenteses";
                token.numLinha = linhaTexto;
                dataGridView1.Rows.Add(new object[] { linhaTexto, "sabre_parenteses", '(' });
                return token;
            }
            else if (caracter == ')')
            {
                token.lexema = ")";
                token.simbolo = "sfecha_parenteses";
                token.numLinha = linhaTexto;
                dataGridView1.Rows.Add(new object[] { linhaTexto, "sfecha_parenteses", ')' });
                return token;
            }

            return token;

        }
        
        private Tokens TrataOperadorRelacional(char caracter, string conteudo)
        {


            if (caracter == '=')
            {
                token.lexema = "=";
                token.simbolo = "sig";
                token.numLinha = linhaTexto;
                dataGridView1.Rows.Add(new object[] { linhaTexto, "sig", '=' });
                return token;


            }
            else if (caracter == '>')
            {

                indiceTexto++;
                caracter = conteudo[indiceTexto-1];
                if (caracter == '=')
                {
                    token.lexema = ">=";
                    token.simbolo = "smaiorig";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "smaiorig", ">=" });
                    return token;
                }
                else
                {
                    token.lexema = ">";
                    token.simbolo = "smaior";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "smaior", '>' });
                    indiceTexto--;
                    return token;

                    
                }

            }
            else if (caracter == '<')
            {

                    indiceTexto++;
                    caracter = conteudo[indiceTexto-1];
                

                if (caracter == '=')
                {
                    token.lexema = "<=";
                    token.simbolo = "smenorig";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "smenorig", "<=" });
                    return token;
                }
                else
                {
                    token.lexema = "<";
                    token.simbolo = "smenor";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "smenor", '<' });
                    indiceTexto--;
                    return token;
                    
                }
            }
            else if (caracter == '!')
            {
                if (indiceTexto + 1 < conteudo.Length)
                {
                    indiceTexto++;
                    caracter = conteudo[indiceTexto-1];
                }
                    if (caracter == '=')
                    {
                        token.lexema = "!=";
                        token.simbolo = "sdif";
                        token.numLinha = linhaTexto;
                        dataGridView1.Rows.Add(new object[] { linhaTexto, "sdif", "!=" });
                        return token;
                    }
                    else
                    {
                        TrataErro('!');
                    }
                
            }

            return token;

        }

        private Tokens TrataOperadorAritmetico(char caracter, string conteudo)
        {


            if (caracter == '+')
            {
                token.lexema = "+";
                token.simbolo = "smais";
                token.numLinha = linhaTexto;
                dataGridView1.Rows.Add(new object[] { linhaTexto, "smais", '+' });
                return token;

            }
            else if (caracter == '-')
            {
                token.lexema = "-";
                token.simbolo = "smenos";
                token.numLinha = linhaTexto;
                dataGridView1.Rows.Add(new object[] { linhaTexto, "smenos", '-' });
                return token;

            }
            else if (caracter == '*')
            {
                token.lexema = "*";
                token.simbolo = "smult";
                token.numLinha = linhaTexto;
                dataGridView1.Rows.Add(new object[] { linhaTexto, "smult", '*' });
                return token;

            }

            return token;

        }

        private Tokens TrataAtribuicao(char caracter, string conteudo)
        {
            string atr = "";

            atr = atr + caracter;

            /*if (indiceTexto < conteudo.Length - 1)
            {


            }*/

            indiceTexto++;
            caracter = conteudo[indiceTexto-1];
   
            if (caracter == '=')
            {
                atr = atr + caracter;

                token.lexema = atr;
                token.simbolo = "satribuicao";
                token.numLinha = linhaTexto;
                dataGridView1.Rows.Add(new object[] { linhaTexto, "satribuicao", atr });
                
                return token;

            }
            else
            {
                token.lexema = atr;
                token.simbolo = "sdoispontos";
                token.numLinha = linhaTexto;
                dataGridView1.Rows.Add(new object[] { linhaTexto, "sdoispontos", atr });
                return token;
            }


        }
        private Tokens TrataDigito(char caracter, string conteudo)
        {
            string num = "";
            num = num + caracter;

           // indiceTexto++;
            caracter = conteudo[indiceTexto];

            /*if (indiceTexto < conteudo.Length)
            {
                caracter = conteudo[indiceTexto];
                MessageBox.Show("2 Trata Digito: " + caracter.ToString());
            }*/
            while (Char.IsDigit(caracter) && indiceTexto < conteudo.Length)
            {

                num = num + caracter;


                indiceTexto++;
                caracter = conteudo[indiceTexto];

                /*if (indiceTexto < conteudo.Length)
                {
                    caracter = conteudo[indiceTexto];
                    MessageBox.Show("3 Trata Digito: " + caracter.ToString());
                }*/
            }


            token.lexema = num;
            token.simbolo = "snumero";
            token.numLinha = linhaTexto;
            dataGridView1.Rows.Add(new object[] { linhaTexto, "snumero", num });
            return token;

        }

        private Tokens TrataId_Palavra(char caracter, string conteudo)
        {
            string id = "";
            id = id + caracter;
            //MessageBox.Show("2 " + id);

            //indiceTexto--;
            if (indiceTexto < conteudo.Length)
            {
                caracter = conteudo[indiceTexto];
              //  MessageBox.Show("Entou " + caracter.ToString());
            }

            while ((Char.IsLetterOrDigit(caracter) || caracter == '_') && indiceTexto < conteudo.Length)
            {
                id = id + caracter;
                indiceTexto++;
               // MessageBox.Show("3 " + id);

                if (indiceTexto < conteudo.Length)
                {
                    caracter = conteudo[indiceTexto];
                }
            }
            //MessageBox.Show("4 " + id);

            switch (id)
            {
                case "programa":
                    token.lexema = id;
                    token.simbolo = "sprograma";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sprograma", id });
                    return token;

                case "se":

                    token.lexema = id;
                    token.simbolo = "sse";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sse", id });
                    return token;

                case "entao":

                    token.lexema = id;
                    token.simbolo = "sentao";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sentao", id });
                    return token;

                case "senao":

                    token.lexema = id;
                    token.simbolo = "ssenao";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "ssenao", id });
                    return token;

                case "enquanto":

                    token.lexema = id;
                    token.simbolo = "senquanto";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "senquanto", id });
                    return token;


                case "faca":

                    token.lexema = id;
                    token.simbolo = "sfaca";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sfaca", id });
                    return token;

                case "inicio":

                    token.lexema = id;
                    token.simbolo = "sinicio";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sinicio", id });
                    return token;

                case "fim":

                    token.lexema = id;
                    token.simbolo = "sfim";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sfim", id });
                    return token;

                case "escreva":

                    token.lexema = id;
                    token.simbolo = "sescreva";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sescreva", id });
                    return token;

                case "leia":

                    token.lexema = id;
                    token.simbolo = "sleia";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sleia", id });
                    return token;


                case "var":

                    token.lexema = id;
                    token.simbolo = "svar";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "svar", id });
                    return token;

                case "inteiro":

                    token.lexema = id;
                    token.simbolo = "sinteiro";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sinteiro", id });
                    return token;

                case "booleano":

                    token.lexema = id;
                    token.simbolo = "sbooleano";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sbooleano", id });
                    return token;

                case "nteiro": //VER NA PROXIMA SPRINT

                    token.lexema = id;
                    token.simbolo = "sinteiro";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sinteiro", id });
                    return token;

                case "ooleano": //VER NA PROXIMA SPRINT

                    token.lexema = id;
                    token.simbolo = "sbooleano";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sbooleano", id });
                    return token;

                case "verdadeiro":

                    token.lexema = id;
                    token.simbolo = "sverdadeiro";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sverdadeiro", id });
                    return token;

                case "falso":

                    token.lexema = id;
                    token.simbolo = "sfalso";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sfalso", id });
                    return token;

                case "procedimento":

                    token.lexema = id;
                    token.simbolo = "sprocedimento";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sprocedimento", id });
                    return token;

                case "funcao":

                    token.lexema = id;
                    token.simbolo = "sfuncao";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sfuncao", id });
                    return token;

                case "div":

                    token.lexema = id;
                    token.simbolo = "sdiv";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sdiv", id });
                    return token;

                case "e":

                    token.lexema = id;
                    token.simbolo = "se";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "se", id });
                    return token;

                case "ou":

                    token.lexema = id;
                    token.simbolo = "sou";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sou", id });
                    return token;

                case "nao":

                    token.lexema = id;
                    token.simbolo = "snao";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "snao", id });
                    return token;

                default:

                    token.lexema = id;
                    token.simbolo = "sidentificador";
                    token.numLinha = linhaTexto;
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sidentificador", id });
                    return token;

                    /*if (indiceTexto < conteudo.Length)
                        analisadorLexical();*/


                    
            }

        }
        
        private void GravaNoArquivo()
        {
            // Create a SaveFileDialog to request a path and file name to save to.
            SaveFileDialog saveFile1 = new SaveFileDialog();

            // Initialize the SaveFileDialog to specify the RTF extension for the file.
            saveFile1.DefaultExt = "*.txt";
            saveFile1.Filter = "TXT Files|*.txt";

            // Determine if the user selected a file name from the saveFileDialog.
            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
               saveFile1.FileName.Length > 0)
            {
                // Save the contents of the RichTextBox into the file.
                richTextBox1.SaveFile(saveFile1.FileName, RichTextBoxStreamType.PlainText);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            //Tokens token;
            Sintatico analiseSint = new Sintatico();
            dataGridView2.Rows.Clear();
            //indiceTexto = 0;
            terminaPrograma = 0;
            linhaTexto = 1;
            if (richTextBox1.Text.Length > 0 )
            {
                //MessageBox.Show(richTextBox1.Text);
                conteudo = richTextBox1.Text;
                File.WriteAllText("CodigoGerado.txt", String.Empty);

                //MessageBox.Show(conteudo);
                analiseSint.Main(conteudo);
                //token = AnalisadorLexical(conteudo);
              //System.Diagnostics.Debug.WriteLine("simbolo = {0}, lexema = {1}", token.simbolo, token.lexema);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            GravaNoArquivo();
            
        }
    }
    public class Tokens
    {
        public string lexema { set; get;}
        public string simbolo { set; get; }
        public int numLinha { set; get; }

        

    }

    public class Simbolo
    {
        public string lexema { set; get; } //nome do id
        public string tipo { set; get; }    //padrão do id
        public int nivel { set; get; }  // nivel de declaração
        public int memoria { set; get; }    //endereço de memória alocado
        //public int rotulo { set; get; }

    }

    public  class Sintatico
    {
        private Tokens token;
        public Lexico analiseLex = new Lexico();
        // public DataGridView dt_erros;

        public int rotulo; //geracod
        public int ultimoalloc = 0; //geracod
        public int totalVar = 0; //geracod
        public int espacoMemoria = 0; //geracod
        public int checaProcedimento = 0; //geracod
        public List <string> auxExpressao = new List<string>(); //geracod
        string codigoGerado = @"CodigoGerado.txt"; //geracod

        // Semantico - INICIO
        public List<Simbolo> tabelaSimbolos = new List<Simbolo>(); //semantico
        public int nivelEscopo = 0;

        // Semantico - FIM

        private int terminaProgramaSintatico = 0;
        public void Main(string conteudo)
        {
            //dt_erros = dt2;
                rotulo = 1; //geracod
                token = analiseLex.AnalisadorLexical(conteudo);

                if (string.Equals(token.simbolo, "sprograma"))
                {
                 Gera("", "START", "", ""); //geracod
                token = analiseLex.AnalisadorLexical(conteudo);

                    if (string.Equals(token.simbolo, "sidentificador"))
                    {

                    Insere_tabela(token.lexema, "programa", nivelEscopo, 0);
                    token = analiseLex.AnalisadorLexical(conteudo);

                        if (string.Equals(token.simbolo, "sponto_virgula"))
                        {
                            Analisa_bloco(conteudo);
                            //token = analiseLex.AnalisadorLexical(conteudo);
                            if (string.Equals(token.simbolo, "sponto"))
                            {
                                TrataErroSintatico("Programa Terminou");
                                Gera("", "HLT", "", ""); //geracod
                            }
                            else
                            {
                                TrataErroSintatico("Erro: esperado '.'");

                                //MessageBox.Show("Erro: Faltando ponto");
                            }
                        }
                        else
                        {
                            TrataErroSintatico("Erro: esperado ';'");
                            // MessageBox.Show("Erro: Faltando ponto_virgula");
                        }
                    }
                    else
                    {
                        TrataErroSintatico("Erro: esperado 'identificador'");
                        // MessageBox.Show("Erro: Faltando identificador");
                    }
                }
                else
                {
                    TrataErroSintatico("Erro: esperado 'programa'");

                    //MessageBox.Show("Erro: Faltando programa");
                }
            
        }

        //Metodos Semanticos - INICIO

        private void Insere_tabela(string slexema, string stipo, int snivel, int smemoria)
        {
            tabelaSimbolos.Add(new Simbolo() { lexema = slexema, tipo = stipo, nivel = snivel, memoria = smemoria });
            System.Console.WriteLine("Lexema {0}, Memoria {1}, Nivel {2}, Tipo {3} \n", tabelaSimbolos[tabelaSimbolos.Count - 1].lexema,tabelaSimbolos[tabelaSimbolos.Count - 1].memoria,tabelaSimbolos[tabelaSimbolos.Count - 1].nivel,tabelaSimbolos[tabelaSimbolos.Count - 1].tipo);
        }


        public bool Pesquisa_duplicvar_tabela(string slexema)
        {
            if (PesquisaLocal(slexema))
            {
                return true; //duplicado
            }
            else
            {
                return false;
            }
        }

        public bool PesquisaLocal(string slexema)
        {
            for (int i = tabelaSimbolos.Count - 1; i > 0; i--)
            {
                if (tabelaSimbolos[i].nivel == 1)
                {
                    return false;
                }
                else if (tabelaSimbolos[i].lexema == slexema && tabelaSimbolos[i].nivel == 0)
                {
                    return true; //duplicado
                }
            }
            return false;
        }

        public void TrataErroSemantico(string mensagem)
        {
            if (terminaProgramaSintatico == 0)
            {
                MessageBox.Show(mensagem, "linha " + token.numLinha);
                terminaProgramaSintatico = 1;
                //analiseLex.SetTerminaPrograma(terminaProgramaSintatico);

            }
        }

        public void Coloca_tipo_tabela(string slexema)
        {
            //Procurar na tabela de simbolos variaveis com o "tipo variavel" e substituir pelo seu tipo

            for (int i = tabelaSimbolos.Count - 1; i > 0; i--)
            {
                if (tabelaSimbolos[i].tipo == "variavel")
                {
                    tabelaSimbolos[i].tipo = slexema; 

                }
                //System.Console.WriteLine("Apos Colocar tipo: Lexema {0}, Memoria {1}, Nivel {2}, Tipo {3} \n", tabelaSimbolos[i].lexema, tabelaSimbolos[i].memoria, tabelaSimbolos[i].nivel, tabelaSimbolos[i].tipo);

            }

        }

        public bool Pesquisa_declvar_tabela(string slexema)
        {
            //Pesquisa elementos que possuam tipo inteiro ou booleano, e compara o lexema desse tipo com o lexema a se comparar
            for (int i = tabelaSimbolos.Count - 1; i > 0; i--)
            {

                if(tabelaSimbolos[i].lexema == slexema)
                {
                    if ((tabelaSimbolos[i].tipo == "inteiro" || tabelaSimbolos[i].tipo == "booleano"))
                    {
                        return true;

                    }
                    System.Console.WriteLine("Pesquisa declaracao de var na tabela: Lexema {0}, Memoria {1}, Nivel {2}, Tipo {3} \n", tabelaSimbolos[i].lexema, tabelaSimbolos[i].memoria, tabelaSimbolos[i].nivel, tabelaSimbolos[i].tipo);
                }


               


            }
            return false;
        }


        //Metodos Semanticos - FIM



        //Geração de Código - INICIO

        private void Gera(string s1, string s2, string s3, string s4)
        {
            string comando;

            comando = s1 + ' ' + s2 + ' ' + s3 + ' ' + s4;

            if (!File.Exists(codigoGerado))
            {
                File.Create(codigoGerado);
                TextWriter tw = new StreamWriter(codigoGerado);
                tw.WriteLine(comando + "\n");
                tw.Close();
            }
            else if (File.Exists(codigoGerado))
            {
                using (var tw = new StreamWriter(codigoGerado, true))
                {
                    tw.WriteLine(comando + "\n");
                }
            }
        }
               


        //Geração de Código - FIM

        private void TrataErroSintatico(string mensagem)
        {
            if (terminaProgramaSintatico == 0)
            {
                MessageBox.Show(mensagem,"linha "+token.numLinha);
                terminaProgramaSintatico = 1;
                //analiseLex.SetTerminaPrograma(terminaProgramaSintatico);

            }
           //dt_erros.Rows.Add(new object[] { token.numLinha, mensagem });

        }


        private void Analisa_bloco(string conteudo)
        {

            token = analiseLex.AnalisadorLexical(conteudo);
            if (terminaProgramaSintatico == 0)
                Analisa_et_variaveis(conteudo);
            if(terminaProgramaSintatico==0)
                Analisa_subrotinas(conteudo);
            if (terminaProgramaSintatico == 0)
                Analisa_comandos(conteudo);
            
        }

        private void Analisa_et_variaveis(string conteudo)
        {
            if (string.Equals(token.simbolo, "svar") && terminaProgramaSintatico == 0)
            {
                totalVar = 0;   //geracod

                token = analiseLex.AnalisadorLexical(conteudo);

                if (string.Equals(token.simbolo, "sidentificador") && terminaProgramaSintatico == 0)
                {
                    while (string.Equals(token.simbolo, "sidentificador") && terminaProgramaSintatico == 0)
                    {
                        Analisa_variaveis(conteudo);

                        if (string.Equals(token.simbolo, "sponto_virgula") && terminaProgramaSintatico == 0)
                        {
                            Gera("", "ALLOC", espacoMemoria.ToString(), totalVar.ToString()); //geracod
                            ultimoalloc = totalVar;
                            espacoMemoria = espacoMemoria + totalVar; //lembrar de fazer "menos" com DALLOC
                            totalVar = 0;
                            token = analiseLex.AnalisadorLexical(conteudo);
                        }
                        else
                        {
                            TrataErroSintatico("Erro: esperado ';'");
                           // MessageBox.Show("Erro (Analisa_et_variaveis): Faltando Ponto e Virgula");
                        }
                    }
                }
                else
                {
                    TrataErroSintatico("Erro: esperado 'identificador'");
                    //MessageBox.Show("Erro (Analisa_et_variaveis): Esperado Identificador");
                }

            }
        }


        private void Analisa_variaveis(string conteudo)
        {
            do
            {
                if (string.Equals(token.simbolo, "sidentificador") && terminaProgramaSintatico == 0)
                {
                    if (!Pesquisa_duplicvar_tabela(token.lexema))
                    {
                        Insere_tabela(token.lexema, "variavel", 0, 0);
                        totalVar++;  //geracod
                        token = analiseLex.AnalisadorLexical(conteudo);

                        if (string.Equals(token.simbolo, "svirgula") || string.Equals(token.simbolo, "sdoispontos") && terminaProgramaSintatico == 0)
                        {
                            if (string.Equals(token.simbolo, "svirgula") && terminaProgramaSintatico == 0)
                            {
                                token = analiseLex.AnalisadorLexical(conteudo);

                                if (string.Equals(token.simbolo, "sdoispontos") && terminaProgramaSintatico == 0)
                                {
                                    TrataErroSintatico("Erro: esperado ':' após ','");
                                    //MessageBox.Show("Erro (Analisa_variaveis): Doispontos após a virgula");
                                }
                            }
                        }
                        else
                        {
                            TrataErroSintatico("Erro: esperado ':' ou ','");
                            // MessageBox.Show("Erro (Analisa_variaveis): Esperado doispontos ou virgula");
                        }
                    }
                    else
                    {
                        TrataErroSemantico("Erro: Variavel duplicada encontrada"); //Inserir o lexema
                    }
                }
                else
                {
                    TrataErroSintatico("Erro: esperado 'identificador'");
                   // MessageBox.Show("Erro (Analisa_variaveis): Esperado identificador");
                }
            } while (!(string.Equals(token.simbolo, "sdoispontos")) && terminaProgramaSintatico==0);

            token = analiseLex.AnalisadorLexical(conteudo);
            Analisa_tipo(conteudo);
        }

        private void Analisa_tipo(string conteudo)
        {
            if (!(string.Equals(token.simbolo, "sinteiro")) && !(string.Equals(token.simbolo, "sbooleano")) && terminaProgramaSintatico == 0)
            {
                TrataErroSintatico("Erro: esperado 'inteiro' ou 'booleano'");
                //MessageBox.Show("Erro (Analisa_tipo): Necessario tipo ser inteiro ou booleano");
            }
            else
            {
                
                Coloca_tipo_tabela(token.lexema);
                token = analiseLex.AnalisadorLexical(conteudo);
            }
        }

        private void Analisa_comandos(string conteudo)
        {
            if (string.Equals(token.simbolo, "sinicio") && terminaProgramaSintatico == 0)
            {
                if (checaProcedimento == 0)
                {
                    Gera("0", "NULL", "", ""); //geracod, marca inicio main
                }
                token = analiseLex.AnalisadorLexical(conteudo);
                Analisa_comando_simples(conteudo);

                while(!(string.Equals(token.simbolo, "sfim")) && terminaProgramaSintatico == 0)
                {
                    if (string.Equals(token.simbolo, "sponto_virgula") && terminaProgramaSintatico == 0)
                    {
                        //Gera("", "DALLOC", ultimoalloc.ToString(), ""); //geracod
                        token = analiseLex.AnalisadorLexical(conteudo);
                        checaProcedimento = 0; //geracod acabou procedimento
                        if (!(string.Equals(token.simbolo, "sfim")) && terminaProgramaSintatico == 0)
                        {
                            Analisa_comando_simples(conteudo);
                        }
                    }
                    else
                    {
                        //token = analiseLex.AnalisadorLexical(conteudo);
                        TrataErroSintatico("Erro: esperado ';'");
                        //MessageBox.Show("Erro (Analisa_comandos): Faltando ponto_virgula. " + token.simbolo, token.lexema);
                    }
                }
                token = analiseLex.AnalisadorLexical(conteudo);
            }
            else
            {
                TrataErroSintatico("Erro: esperado 'inicio'");

                //MessageBox.Show("Erro (Analisa_comandos): Faltando inicio");
            }

        }

        private void Analisa_comando_simples(string conteudo)
        {
            //MessageBox.Show("1 Analisa_comando_simples: " + token.simbolo, token.lexema);

            if (string.Equals(token.simbolo, "sidentificador") && terminaProgramaSintatico == 0)
            {
                Analisa_atrib_chprocedimento(conteudo);
            }
            else if (string.Equals(token.simbolo, "sse") && terminaProgramaSintatico == 0)
            {
                checaProcedimento = 1;
                Analisa_se(conteudo);
            }
            else if (string.Equals(token.simbolo, "senquanto") && terminaProgramaSintatico == 0)
            {
                checaProcedimento = 1;
                Analisa_enquanto(conteudo);
            }
            else if (string.Equals(token.simbolo, "sleia") && terminaProgramaSintatico == 0)
            {
                checaProcedimento = 1;
                Analisa_leia(conteudo);
            }
            else if (string.Equals(token.simbolo, "sescreva") && terminaProgramaSintatico == 0)
            {
                checaProcedimento = 1;
                Analisa_escreva(conteudo);
            }
            else
            {
                if(terminaProgramaSintatico == 0)
                    Analisa_comandos(conteudo);
            }
        }

        private void Analisa_atrib_chprocedimento(string conteudo)
        {
            string aux;
            aux = token.lexema;
            token = analiseLex.AnalisadorLexical(conteudo);
            if (string.Equals(token.simbolo, "satribuicao") && terminaProgramaSintatico == 0)
            {
                Analisa_atribuicao(conteudo);
                Gera("", "STR", aux, "");
            }
            else
            {
                Gera("", "CALL", rotulo.ToString(), ""); //geracod, usar rotulo no paremetro do CALL?
                //Chamada_procedimento(conteudo) //esse que nao existe? 
            }
        }

        private void Analisa_leia(string conteudo)
        {
            token = analiseLex.AnalisadorLexical(conteudo);
            if (string.Equals(token.simbolo, "sabre_parenteses") && terminaProgramaSintatico == 0)
            {
                token = analiseLex.AnalisadorLexical(conteudo);
                if (string.Equals(token.simbolo, "sidentificador") && terminaProgramaSintatico == 0)
                {

                    if (Pesquisa_declvar_tabela(token.lexema))
                    {

                        token = analiseLex.AnalisadorLexical(conteudo);

                        if (string.Equals(token.simbolo, "sfecha_parenteses") && terminaProgramaSintatico == 0)
                        {
                            token = analiseLex.AnalisadorLexical(conteudo);
                        }
                        else
                        {
                            TrataErroSintatico("Erro: esperado ')'");

                            //MessageBox.Show("Erro (Analisa_leia): Esperado fecha_parenteses");
                        }
                    }
                    else
                    {
                        TrataErroSemantico("Erro: Variavel não declarada");
                    }
                }
                else
                {
                    TrataErroSintatico("Erro: esperado 'identificador'");

                    //MessageBox.Show("Erro (Analisa_leia): Esperado identificador");
                }
            }
            else
            {
                TrataErroSintatico("Erro: esperado '('");
               // MessageBox.Show("Erro(Analisa_leia): Esperado abre_parenteses");
            }
        }


        private void Analisa_escreva(string conteudo)
        {
            token = analiseLex.AnalisadorLexical(conteudo);

            if (string.Equals(token.simbolo, "sabre_parenteses") && terminaProgramaSintatico == 0)
            {
                token = analiseLex.AnalisadorLexical(conteudo);
                if (string.Equals(token.simbolo, "sidentificador") && terminaProgramaSintatico == 0)
                {
                    token = analiseLex.AnalisadorLexical(conteudo);
                    if (string.Equals(token.simbolo, "sfecha_parenteses") && terminaProgramaSintatico == 0)
                    {
                        token = analiseLex.AnalisadorLexical(conteudo);
                    }
                    else
                    {
                        TrataErroSintatico("Erro: esperado ')'");
                        //MessageBox.Show("Erro (Analisa_escreva): Esperado fecha_parenteses");
                    }
                }
                else
                {
                    TrataErroSintatico("Erro: esperado 'identificador'");

                  // MessageBox.Show("Erro (Analisa_escreva): Esperado identificador");
                }
            }
            else
            {
                TrataErroSintatico("Erro: esperado '('");

                //MessageBox.Show("Erro (Analisa_escreva): Esperado abre_parenteses");
            }

        }

        private void Analisa_enquanto(string conteudo)
        {
            int auxrot1, auxrot2;          //geracod
            auxrot1 = rotulo;           //geracod
            Gera(rotulo.ToString(), "NULL", "", ""); //geracod
            rotulo++;   //geracod
            token = analiseLex.AnalisadorLexical(conteudo);
            Analisa_expressao(conteudo);


            if (string.Equals(token.simbolo, "sfaca") && terminaProgramaSintatico == 0)
            {
                auxrot2 = rotulo; //geracod
                Gera("", "JMPF", rotulo.ToString(), ""); //geracod
                rotulo++; //geracod

                token = analiseLex.AnalisadorLexical(conteudo);

                Analisa_comando_simples(conteudo);

                Gera("", "JMP", auxrot1.ToString(), ""); //geracod

                Gera(auxrot2.ToString(), "NULL", "", ""); //geracod

            }
            else
            {
                TrataErroSintatico("Erro: esperado 'faca'");

              //  MessageBox.Show("Erro (Analisa_enquanto): Esperado faça");
            }
        }


        static void convertePost(ref List<string> infix)
        {
            Stack<string> s = new Stack<string>();
            List<string> outputList = new List<string>();
            int n;
            char g;
            foreach (string c in infix)
            {
                
                if (int.TryParse(c.ToString(), out n) || char.TryParse(c.ToString(), out g))
                {
                    if (!isOperator(c) && c != "(" && c != ")")
                    {
                        outputList.Add(c);
                    }
                }
                if (c == "(")
                {
                    s.Push(c);
                }
                if (c == ")")
                {
                    while (s.Count != 0 && s.Peek() != "(")
                    {
                        outputList.Add(s.Pop());
                    }
                    s.Pop();
                }
                if (isOperator(c) == true)
                {
                    while (s.Count != 0 && Priority(s.Peek()) >= Priority(c))
                    {
                        outputList.Add(s.Pop());
                    }
                    s.Push(c);
                }
            }
            while (s.Count != 0)//if any operators remain in the stack, pop all & add to output list until stack is empty 
            {
                outputList.Add(s.Pop());
            }
            for (int i = 0; i < outputList.Count; i++)
            {
                //SWITCH 2200 REAIS CASE
                System.Console.WriteLine("{0}", outputList[i]);
            }
        }

        static int Priority(string c)
        {
            if (c == "*" || c == "div")
            {
                return 4;
            }
            else if (c == "+" || c == "-")
            {
                return 3;
            }
            else if (c == ">" || c == "<" || c == "<=" || c == ">=" || c == "=" || c == "!=")
            {
                return 2;
            }
            else if (c == "e")
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        static bool isOperator(string ch)
        {
            if (ch == "+" || ch == "-" || ch == "nao" || ch == "*" || ch == "div" || ch == ">" || ch == "<" || ch == "<=" || ch == ">=" || ch == "=" || ch == "!=" || ch == "e" || ch == "ou")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*static bool converte(ref List<string> infix)
        {
            int prio = 0;
            List<string> postfix = new List<string>();
            Stack<String> s1 = new Stack<String>();

            for (int i = 0; i < infix.Count(); i++)
            {
                string ch = infix[i];
                if(ch == "+" || ch == "-" || ch == "nao" || ch == "*" || ch == "div" || ch == ">" || ch == "<" || ch == "<=" || ch == ">=" || ch == "=" || ch == "!=" || ch == "e" || ch == "ou" || ch == "(" || ch == ")")
                {
                    if (s1.Count <= 0)
                    {
                        s1.Push(ch);
                    }
                    else
                    {
                        if (ch == "(")
                        {
                            s1.Push(ch);
                        }
                        if(ch == ")")
                        {
                            for (int j = 0; j < s1.Count && infix[i] != "("; j++)
                            {
                                postfix.Add(s1.Pop());
                            }
                            s1.Pop();
                        }

                    }

                 
                }
            }
        }*/

        private void Analisa_se(string conteudo)
        {
            string infix = "";
            string postfix = "";
            token = analiseLex.AnalisadorLexical(conteudo);
            Analisa_expressao(conteudo);
            for (int index = 0; index < auxExpressao.Count(); index++)
            {
                //MessageBox.Show(auxExpressao[index]);
                infix = infix + auxExpressao[index];
            }

            convertePost(ref auxExpressao);

            //token = analiseLex.AnalisadorLexical(conteudo);
            //token = analiseLex.AnalisadorLexical(conteudo);
            //token = analiseLex.AnalisadorLexical(conteudo);
            //MessageBox.Show("Analisa_se: " + token.simbolo, token.lexema);

            if (string.Equals(token.simbolo, "sentao") && terminaProgramaSintatico == 0)
            {
                Gera("", "JMPF", rotulo.ToString(), ""); //geracod
                token = analiseLex.AnalisadorLexical(conteudo);
                Analisa_comando_simples(conteudo);

                if (string.Equals(token.simbolo, "ssenao"))
                {
                    Gera("", "JMPF", rotulo.ToString(), ""); //geracod

                    token = analiseLex.AnalisadorLexical(conteudo);
                    Analisa_comando_simples(conteudo);
                }
            }
            else
            {
                TrataErroSintatico("Erro: esperado 'entao'");

               // MessageBox.Show("Erro (Analisa_se): Esperado um entao");
            }
        }

        private void Analisa_subrotinas(string conteudo)
        {
            /*int auxrot, flagSubRotina; //geracod
            //flagSubRotina = 1;  //geracod
            //auxrot = rotulo; //geracod
            if (string.Equals(token.simbolo, "sprocedimento") || string.Equals(token.simbolo, "sfuncao") && terminaProgramaSintatico == 0)
            {
                auxrot = rotulo; //geracod
                Gera("", "JMP", rotulo.ToString(), "");
                rotulo++;
                flagSubRotina = 1;
            }*/



                while (string.Equals(token.simbolo, "sprocedimento") || string.Equals(token.simbolo, "sfuncao") && terminaProgramaSintatico == 0){


                if (string.Equals(token.simbolo, "sprocedimento") && terminaProgramaSintatico == 0)
                {
                    Analisa_declaracao_procedimento(conteudo);
                }
                else
                {
                    Analisa_declaracao_funcao(conteudo);
                }

                //token = analiseLex.AnalisadorLexical(conteudo);
                if (string.Equals(token.simbolo, "sponto_virgula") && terminaProgramaSintatico == 0)
                {
                    token = analiseLex.AnalisadorLexical(conteudo);
                }
                else
                {
                    TrataErroSintatico("Erro: esperado ';'");

                   // MessageBox.Show("Erro (Analisa_subrotinas): Esperado um ponto_virgula ", token.simbolo);
                }
            }

            /*if(flagSubRotina == 1)
            {
                Gera(auxrot.ToString(), "NULL", "", "");
            }*/
        }

        private void Analisa_declaracao_procedimento(string conteudo)
        {
            token = analiseLex.AnalisadorLexical(conteudo);
            if (string.Equals(token.simbolo, "sidentificador") && terminaProgramaSintatico == 0)
            {
                //@to do - Pesquisa declaracao de procedimento

                Insere_tabela(token.lexema, "procedimento", 1, rotulo);
                Gera("", "JMP", "0", ""); //geracod VAI PRO INICIO DO PROGRAMA
                Gera(rotulo.ToString(), "NULL","",""); //geracod
                checaProcedimento = 1;
                rotulo++;//geracod

                token = analiseLex.AnalisadorLexical(conteudo);
                if (string.Equals(token.simbolo, "sponto_virgula") && terminaProgramaSintatico == 0)
                {
                    Analisa_bloco(conteudo);
                }
                else
                {
                    TrataErroSintatico("Erro: esperado ';'");

                    //MessageBox.Show("Erro (Analisa_declaracao_procedimento): Esperado um ponto_virgula");
                }
            }
            else
            {
                TrataErroSintatico("Erro: esperado 'identificador'");

                //MessageBox.Show("Erro (Analisa_declaracao_procedimento): Esperado um identificador");
            }
        }

        private void Analisa_declaracao_funcao(string conteudo)
        {
            token = analiseLex.AnalisadorLexical(conteudo);
            if (string.Equals(token.simbolo, "sidentificador") && terminaProgramaSintatico == 0)
            {
                token = analiseLex.AnalisadorLexical(conteudo);
                if (string.Equals(token.simbolo, "sdoispontos") && terminaProgramaSintatico == 0)
                {
                    token = analiseLex.AnalisadorLexical(conteudo);
                    if (string.Equals(token.simbolo, "sinteiro") || string.Equals(token.simbolo, "sbooleano") && terminaProgramaSintatico == 0)
                    {
                        token = analiseLex.AnalisadorLexical(conteudo);
                        if (string.Equals(token.simbolo, "sponto_virgula"))
                        {
                            Analisa_bloco(conteudo);
                        }
                    }
                    else
                    {
                        TrataErroSintatico("Erro: esperado 'inteiro' ou 'booleano'");

                        //MessageBox.Show("Erro (Analisa_declaracao_funcao): Esperado um inteiro ou booleano");
                    }
                }
                else
                {
                    TrataErroSintatico("Erro: esperado ':'");

                    //MessageBox.Show("Erro (Analisa_declaracao_funcao): Esperado um doispontos");
                }
            }
            else
            {
                TrataErroSintatico("Erro: esperado 'identificador'");

               // MessageBox.Show("Erro (Analisa_declaracao_funcao): Esperado um identificador");
            }
        }

        private void Analisa_expressao(string conteudo)
        {
            //auxExpressao.Add(token.lexema);
            Analisa_expressao_simples(conteudo);

            if (string.Equals(token.simbolo, "smaior") || string.Equals(token.simbolo, "smaiorig") || string.Equals(token.simbolo, "sig") || string.Equals(token.simbolo, "smenor") || string.Equals(token.simbolo, "smenorig") || string.Equals(token.simbolo, "sdif") && terminaProgramaSintatico == 0)
            {
                auxExpressao.Add(token.lexema);
                token = analiseLex.AnalisadorLexical(conteudo);
                Analisa_expressao_simples(conteudo);
            }
        }

        private void Analisa_expressao_simples(string conteudo) //cuidado
        {
            if (terminaProgramaSintatico == 0)
            {


                if (string.Equals(token.simbolo, "smais") || string.Equals(token.simbolo, "smenos") && terminaProgramaSintatico == 0)
                {
                    auxExpressao.Add(token.lexema);
                    token = analiseLex.AnalisadorLexical(conteudo);
                }
                Analisa_termo(conteudo);
                while (string.Equals(token.simbolo, "smais") || string.Equals(token.simbolo, "smenos") || string.Equals(token.simbolo, "sou") && terminaProgramaSintatico == 0 )
                {
                    auxExpressao.Add(token.lexema);
                    token = analiseLex.AnalisadorLexical(conteudo);
                    Analisa_termo(conteudo);
                }
            }
        }

        private void Analisa_termo(string conteudo)
        {
            if (terminaProgramaSintatico == 0)
            {
                Analisa_fator(conteudo);
                while (string.Equals(token.simbolo, "smult") || string.Equals(token.simbolo, "sdiv") || string.Equals(token.simbolo, "se") && terminaProgramaSintatico == 0)
                {
                    auxExpressao.Add(token.lexema);
                    token = analiseLex.AnalisadorLexical(conteudo);
                    Analisa_fator(conteudo);
                }
            }
        }

        private void Analisa_fator(string conteudo)
        {
            //MessageBox.Show("1 Analisa_fator: " + token.simbolo, token.lexema);
            if (terminaProgramaSintatico == 0)
            {
                if (string.Equals(token.simbolo, "sidentificador") && terminaProgramaSintatico == 0)
                {
                    auxExpressao.Add(token.lexema);
                    token = analiseLex.AnalisadorLexical(conteudo); //!!!!

                }
                else if (string.Equals(token.simbolo, "snumero") && terminaProgramaSintatico == 0)
                {
                    auxExpressao.Add(token.lexema);
                    token = analiseLex.AnalisadorLexical(conteudo);
                }
                else if (string.Equals(token.simbolo, "snao") && terminaProgramaSintatico == 0)
                {
                    auxExpressao.Add(token.lexema);
                    token = analiseLex.AnalisadorLexical(conteudo);
                    Analisa_fator(conteudo);
                }
                else if (string.Equals(token.simbolo, "sabre_parenteses") && terminaProgramaSintatico == 0)
                {
                    auxExpressao.Add(token.lexema);
                    token = analiseLex.AnalisadorLexical(conteudo);
                    Analisa_expressao(conteudo);
                    // token = analiseLex.AnalisadorLexical(conteudo);
                    // token = analiseLex.AnalisadorLexical(conteudo);
                    // token = analiseLex.AnalisadorLexical(conteudo);

                    if (string.Equals(token.simbolo, "sfecha_parenteses") && terminaProgramaSintatico == 0)
                    {
                        auxExpressao.Add(token.lexema);
                        token = analiseLex.AnalisadorLexical(conteudo);
                    }
                    else
                    {
                        TrataErroSintatico("Erro: esperado ')'");

                        //  MessageBox.Show("Erro (Analisa_fator): Esperado fecha_parenteses");
                    }
                }
                else if (string.Equals(token.lexema, "verdadeiro") || string.Equals(token.lexema, "falso") && terminaProgramaSintatico == 0)
                {
                    token = analiseLex.AnalisadorLexical(conteudo);
                }
                else
                {
                    TrataErroSintatico("Erro: operador lógico extra");

                    //MessageBox.Show("Erro (Analisa_fator): Possivel erro com operador lógico extra");
                }
            }
        }

        private void Analisa_atribuicao(string conteudo)
        {
            if (terminaProgramaSintatico == 0)
            {
                token = analiseLex.AnalisadorLexical(conteudo);
                Analisa_expressao(conteudo);
            }
        }

    }

}//fim namespace
