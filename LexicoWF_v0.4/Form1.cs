using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        public Tokens AnalisadorLexical(string conteudo)
        {
            //salvar código do rich em arquivo (fazer)
            //MessageBox.Show("c");


            do
            {
                int j = 0;

                char caracter;
                //MessageBox.Show(indiceTexto.ToString());

                //MessageBox.Show("e");

                    caracter = conteudo[indiceTexto];   //ler caracter
 



                //MessageBox.Show("1"+caracter.ToString(), indiceTexto.ToString());
                //indiceTexto++;


                while ((char.IsWhiteSpace(caracter) || caracter == '\n') && indiceTexto + 1 < conteudo.Length)
                {
                    //MessageBox.Show("vazio: "+ caracter.ToString(), indiceTexto.ToString());

                    indiceTexto++;
                    caracter = conteudo[indiceTexto];
                    //MessageBox.Show(caracter.ToString(), indiceTexto.ToString());


                }

                while (j < conteudo.Length)
                {

                    if (caracter == '{' || caracter == ' ')
                    {
                        
                        if (caracter == '{')
                        {
                            int achouChave = 0;
                            while (caracter != '}') //&& terminaPrograma != 1
                            {
                                if (caracter == '\n')
                                {
                                    linhaTexto++;
                                }

                                if (indiceTexto + 1 < conteudo.Length)
                                {
                                    indiceTexto++;
                                    caracter = conteudo[indiceTexto];

                                    if (caracter == '}')
                                    {

                                        achouChave = 1;
                                    }

                                }
                                
                                if(indiceTexto == conteudo.Length-1)
                                {
                                    terminaPrograma = 1;
                                }

                            }

                            if(achouChave == 0)
                            {
                                linhaTexto--;
                                TrataErro('{');
                                //terminaPrograma = 1;
                            }

                            if (achouChave == 1) 
                            {
                                indiceTexto++;
                                caracter = conteudo[indiceTexto];


                            }
                            //indiceTexto++;
                            //caracter = conteudo[indiceTexto];
                        }

                        while ((char.IsWhiteSpace(caracter) || caracter == '\n') && indiceTexto+1 < conteudo.Length)
                        {
                                
                                indiceTexto++;
                                caracter = conteudo[indiceTexto];

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
            if (richTextBox1.Text.Length > 0)
            {
                //MessageBox.Show(richTextBox1.Text);
                conteudo = richTextBox1.Text;
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

    public class Sintatico
    {
        private Tokens token;
        public Lexico analiseLex = new Lexico();

        public void Main(string conteudo)
        {
            token = analiseLex.AnalisadorLexical(conteudo);

            if (string.Equals(token.simbolo, "sprograma"))
            {
                token = analiseLex.AnalisadorLexical(conteudo);

                if (string.Equals(token.simbolo, "sidentificador"))
                {
                    token = analiseLex.AnalisadorLexical(conteudo);

                    if (string.Equals(token.simbolo, "sponto_virgula"))
                    {
                        Analisa_bloco(conteudo);
                        //token = analiseLex.AnalisadorLexical(conteudo);
                        if (string.Equals(token.simbolo, "sponto"))
                        {
                            MessageBox.Show("Acabou programa, SUCESSO");
                        }
                        else
                        {
                            MessageBox.Show("Erro: Faltando ponto");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Erro: Faltando ponto_virgula");
                    }
                }
                else
                {
                    MessageBox.Show("Erro: Faltando identificador");
                }
            }
            else
            {
                MessageBox.Show("Erro: Faltando programa");
            }
        }

        private void Analisa_bloco(string conteudo)
        {
            token = analiseLex.AnalisadorLexical(conteudo);
            Analisa_et_variaveis(conteudo);
            Analisa_subrotinas(conteudo);
            Analisa_comandos(conteudo);
        }

        private void Analisa_et_variaveis(string conteudo)
        {
            if (string.Equals(token.simbolo, "svar"))
            {
                token = analiseLex.AnalisadorLexical(conteudo);

                if (string.Equals(token.simbolo, "sidentificador"))
                {
                    while (string.Equals(token.simbolo, "sidentificador"))
                    {
                        Analisa_variaveis(conteudo);

                        if (string.Equals(token.simbolo, "sponto_virgula"))
                        {
                            token = analiseLex.AnalisadorLexical(conteudo);

                        }
                        else
                        {
                            MessageBox.Show("Erro (Analisa_et_variaveis): Faltando Ponto e Virgula");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Erro (Analisa_et_variaveis): Esperado Identificador");
                }

            }
        }


        private void Analisa_variaveis(string conteudo)
        {
            do
            {
                if (string.Equals(token.simbolo, "sidentificador"))
                {
                    token = analiseLex.AnalisadorLexical(conteudo);

                    if (string.Equals(token.simbolo, "svirgula") || string.Equals(token.simbolo, "sdoispontos"))
                    {
                        if (string.Equals(token.simbolo, "svirgula"))
                        {
                            token = analiseLex.AnalisadorLexical(conteudo);

                            if (string.Equals(token.simbolo, "sdoispontos"))
                            {
                                MessageBox.Show("Erro (Analisa_variaveis): Doispontos após a virgula");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Erro (Analisa_variaveis): Esperado doispontos ou virgula");
                    }
                }
                else
                {
                    MessageBox.Show("Erro (Analisa_variaveis): Esperado identificador");
                }
            } while (!(string.Equals(token.simbolo, "sdoispontos")));

            token = analiseLex.AnalisadorLexical(conteudo);
            Analisa_tipo(conteudo);
        }

        private void Analisa_tipo(string conteudo)
        {
            if (!(string.Equals(token.simbolo, "sinteiro")) && !(string.Equals(token.simbolo, "sbooleano")))
            {
                MessageBox.Show("Erro (Analisa_tipo): Necessario tipo ser inteiro ou booleano");
            }
            else
            {
                token = analiseLex.AnalisadorLexical(conteudo);
            }
        }

        private void Analisa_comandos(string conteudo)
        {
            if (string.Equals(token.simbolo, "sinicio"))
            {
                token = analiseLex.AnalisadorLexical(conteudo);
                //MessageBox.Show("1 Analisa_comandos: " + token.simbolo, token.lexema);
                Analisa_comando_simples(conteudo);
                //MessageBox.Show("2 Analisa_comandos: " + token.simbolo, token.lexema);

                while(!(string.Equals(token.simbolo, "sfim")))
                {
                    if (string.Equals(token.simbolo, "sponto_virgula"))
                    {
                        token = analiseLex.AnalisadorLexical(conteudo);
                        if (!(string.Equals(token.simbolo, "sfim")))
                        {
                            Analisa_comando_simples(conteudo);
                        }
                    }
                    else
                    {
                        //token = analiseLex.AnalisadorLexical(conteudo);
                        MessageBox.Show("Erro (Analisa_comandos): Faltando ponto_virgula. " + token.simbolo, token.lexema);
                    }
                }
                token = analiseLex.AnalisadorLexical(conteudo);
            }
            else
            {
                    MessageBox.Show("Erro (Analisa_comandos): Faltando inicio");
            }

        }

        private void Analisa_comando_simples(string conteudo)
        {
            //MessageBox.Show("1 Analisa_comando_simples: " + token.simbolo, token.lexema);

            if (string.Equals(token.simbolo, "sidentificador"))
            {
                Analisa_atrib_chprocedimento(conteudo);
            }
            else if (string.Equals(token.simbolo, "sse"))
            {
                Analisa_se(conteudo);
            }
            else if (string.Equals(token.simbolo, "senquanto"))
            {
                Analisa_enquanto(conteudo);
            }
            else if (string.Equals(token.simbolo, "sleia"))
            {
                Analisa_leia(conteudo);
            }
            else if (string.Equals(token.simbolo, "sescreva"))
            {
                Analisa_escreva(conteudo);
            }
            else
            {
                Analisa_comandos(conteudo);
            }
        }

        private void Analisa_atrib_chprocedimento(string conteudo)
        {
            token = analiseLex.AnalisadorLexical(conteudo);
            if (string.Equals(token.simbolo, "satribuicao"))
            {
                Analisa_atribuicao(conteudo);
            }
            else
            {

                //Chamada_procedimento(conteudo) //esse que nao existe? 
            }
        }

        private void Analisa_leia(string conteudo)
        {
            token = analiseLex.AnalisadorLexical(conteudo);
            if (string.Equals(token.simbolo, "sabre_parenteses"))
            {
                token = analiseLex.AnalisadorLexical(conteudo);
                if (string.Equals(token.simbolo, "sidentificador"))
                {
                    token = analiseLex.AnalisadorLexical(conteudo);

                    if (string.Equals(token.simbolo, "sfecha_parenteses"))
                    {
                        token = analiseLex.AnalisadorLexical(conteudo);
                    }
                    else
                    {
                        MessageBox.Show("Erro (Analisa_leia): Esperado fecha_parenteses");
                    }
                }
                else
                {
                    MessageBox.Show("Erro (Analisa_leia): Esperado identificador");
                }
            }
            else
            {
                MessageBox.Show("Erro(Analisa_leia): Esperado abre_parenteses");
            }
        }


        private void Analisa_escreva(string conteudo)
        {
            token = analiseLex.AnalisadorLexical(conteudo);

            if (string.Equals(token.simbolo, "sabre_parenteses"))
            {
                token = analiseLex.AnalisadorLexical(conteudo);
                if (string.Equals(token.simbolo, "sidentificador"))
                {
                    token = analiseLex.AnalisadorLexical(conteudo);
                    if (string.Equals(token.simbolo, "sfecha_parenteses"))
                    {
                        token = analiseLex.AnalisadorLexical(conteudo);
                    }
                    else
                    {
                        MessageBox.Show("Erro (Analisa_escreva): Esperado fecha_parenteses");
                    }
                }
                else
                {
                    MessageBox.Show("Erro (Analisa_escreva): Esperado identificador");
                }
            }
            else
            {
                MessageBox.Show("Erro (Analisa_escreva): Esperado abre_parenteses");
            }

        }

        private void Analisa_enquanto(string conteudo)
        {
            //MessageBox.Show("1 Enquanto " + token.simbolo, token.lexema);
            token = analiseLex.AnalisadorLexical(conteudo);
            //MessageBox.Show("2 Enquanto " + token.simbolo, token.lexema);
            Analisa_expressao(conteudo);

            //MessageBox.Show("4 Enquanto " + token.simbolo, token.lexema);

            if (string.Equals(token.simbolo, "sfaca"))
            {
                token = analiseLex.AnalisadorLexical(conteudo);
                //MessageBox.Show("4 Enquanto " + token.simbolo, token.lexema);

                Analisa_comando_simples(conteudo);
                //MessageBox.Show("5 Enquanto " + token.simbolo, token.lexema);

            }
            else
            {
                MessageBox.Show("Erro (Analisa_enquanto): Esperado faça");
            }
        }

        private void Analisa_se(string conteudo)
        {
            token = analiseLex.AnalisadorLexical(conteudo);
            Analisa_expressao(conteudo);
            //token = analiseLex.AnalisadorLexical(conteudo);
            //token = analiseLex.AnalisadorLexical(conteudo);
            //token = analiseLex.AnalisadorLexical(conteudo);
            //MessageBox.Show("Analisa_se: " + token.simbolo, token.lexema);

            if (string.Equals(token.simbolo, "sentao"))
            {
                token = analiseLex.AnalisadorLexical(conteudo);
                Analisa_comando_simples(conteudo);

                if (string.Equals(token.simbolo, "ssenao"))
                {
                    token = analiseLex.AnalisadorLexical(conteudo);
                    Analisa_comando_simples(conteudo);
                }
            }
            else
            {
                MessageBox.Show("Erro (Analisa_se): Esperado um entao");
            }
        }

        private void Analisa_subrotinas(string conteudo)
        {
            while (string.Equals(token.simbolo, "sprocedimento") || string.Equals(token.simbolo, "sfuncao"))
            {

                if (string.Equals(token.simbolo, "sprocedimento"))
                {
                    Analisa_declaracao_procedimento(conteudo);
                }
                else
                {
                    Analisa_declaracao_funcao(conteudo);
                }

                //token = analiseLex.AnalisadorLexical(conteudo);
                if (string.Equals(token.simbolo, "sponto_virgula"))
                {
                    token = analiseLex.AnalisadorLexical(conteudo);
                }
                else
                {
                    MessageBox.Show("Erro (Analisa_subrotinas): Esperado um ponto_virgula ", token.simbolo);
                }
            }
        }

        private void Analisa_declaracao_procedimento(string conteudo)
        {
            token = analiseLex.AnalisadorLexical(conteudo);
            if (string.Equals(token.simbolo, "sidentificador"))
            {
                token = analiseLex.AnalisadorLexical(conteudo);
                if (string.Equals(token.simbolo, "sponto_virgula"))
                {
                    Analisa_bloco(conteudo);
                }
                else
                {
                    MessageBox.Show("Erro (Analisa_declaracao_procedimento): Esperado um ponto_virgula");
                }
            }
            else
            {
                MessageBox.Show("Erro (Analisa_declaracao_procedimento): Esperado um identificador");
            }
        }

        private void Analisa_declaracao_funcao(string conteudo)
        {
            token = analiseLex.AnalisadorLexical(conteudo);
            if (string.Equals(token.simbolo, "sidentificador"))
            {
                token = analiseLex.AnalisadorLexical(conteudo);
                if (string.Equals(token.simbolo, "sdoispontos"))
                {
                    token = analiseLex.AnalisadorLexical(conteudo);
                    if (string.Equals(token.simbolo, "sinteiro") || string.Equals(token.simbolo, "sbooleano"))
                    {
                        token = analiseLex.AnalisadorLexical(conteudo);
                        if (string.Equals(token.simbolo, "sponto_virgula"))
                        {
                            Analisa_bloco(conteudo);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Erro (Analisa_declaracao_funcao): Esperado um inteiro ou booleano");
                    }
                }
                else
                {
                    MessageBox.Show("Erro (Analisa_declaracao_funcao): Esperado um doispontos");
                }
            }
            else
            {
                MessageBox.Show("Erro (Analisa_declaracao_funcao): Esperado um identificador");
            }
        }

        private void Analisa_expressao(string conteudo)
        {
            Analisa_expressao_simples(conteudo);

            if (string.Equals(token.simbolo, "smaior") || string.Equals(token.simbolo, "smaiorig") || string.Equals(token.simbolo, "sig") || string.Equals(token.simbolo, "smenor") || string.Equals(token.simbolo, "smenorig") || string.Equals(token.simbolo, "sdif"))
            {
                token = analiseLex.AnalisadorLexical(conteudo);
                Analisa_expressao_simples(conteudo);
            }
        }

        private void Analisa_expressao_simples(string conteudo) //cuidado
        {
            if (string.Equals(token.simbolo, "smais") || string.Equals(token.simbolo, "smenos"))
            {
                token = analiseLex.AnalisadorLexical(conteudo);
            }
            Analisa_termo(conteudo);
            while (string.Equals(token.simbolo, "smais") || string.Equals(token.simbolo, "smenos") || string.Equals(token.simbolo, "sou"))
            {
                    token = analiseLex.AnalisadorLexical(conteudo);
                    Analisa_termo(conteudo);
            }
        }

        private void Analisa_termo(string conteudo)
        {
            Analisa_fator(conteudo);
            while (string.Equals(token.simbolo, "smult") || string.Equals(token.simbolo, "sdiv") || string.Equals(token.simbolo, "se"))
            {
                token = analiseLex.AnalisadorLexical(conteudo);
                Analisa_fator(conteudo);
            }
        }

        private void Analisa_fator(string conteudo)
        {
            //MessageBox.Show("1 Analisa_fator: " + token.simbolo, token.lexema);

            if (string.Equals(token.simbolo, "sidentificador"))
            {
                token = analiseLex.AnalisadorLexical(conteudo); //!!!!

            }
            else if (string.Equals(token.simbolo, "snumero"))
            {
                token = analiseLex.AnalisadorLexical(conteudo);
            }
            else if (string.Equals(token.simbolo, "snao"))
            {
                token = analiseLex.AnalisadorLexical(conteudo);
                Analisa_fator(conteudo);
            }
            else if (string.Equals(token.simbolo, "sabre_parenteses"))
            {
                token = analiseLex.AnalisadorLexical(conteudo);
                Analisa_expressao(conteudo);
               // token = analiseLex.AnalisadorLexical(conteudo);
               // token = analiseLex.AnalisadorLexical(conteudo);
               // token = analiseLex.AnalisadorLexical(conteudo);

                if (string.Equals(token.simbolo, "sfecha_parenteses"))
                {
                    token = analiseLex.AnalisadorLexical(conteudo);
                }
                else
                {
                    MessageBox.Show("Erro (Analisa_fator): Esperado fecha_parenteses");
                }
            }
            else if (string.Equals(token.lexema, "verdadeiro") || string.Equals(token.lexema, "falso"))
            {
                token = analiseLex.AnalisadorLexical(conteudo);
            }
            else
            {
                MessageBox.Show("Erro (Analisa_fator): Possivel erro com operador lógico extra");
            }
        }

        private void Analisa_atribuicao(string conteudo)
        {
            token = analiseLex.AnalisadorLexical(conteudo);
            Analisa_expressao(conteudo);

        }

    }

}//fim namespace
