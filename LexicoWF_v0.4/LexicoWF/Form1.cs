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
    public partial class Form1 : Form
    {

        List<Tokens> token = new List<Tokens>(); //Def.token = TipoToken
        int terminaPrograma = 0;
        int indiceTexto;
        int linhaTexto = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void abrirArquivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName); //Abre arquivo fonte

                richTextBox1.Text = sr.ReadToEnd();
            }
        }

        private void analisadorLexical()
        {
            //salvar código do rich em arquivo (fazer)
            

            do
            {
                int j = 0;
                //Console.WriteLine("entrou");
                char caracter; 
                caracter = richTextBox1.Text[indiceTexto];   //ler caracter
               // MessageBox.Show(caracter.ToString());
               // Console.WriteLine("caracter {0}", caracter);
                while (j < richTextBox1.Text.Length)
                {   

                    if (caracter == '{' || caracter == ' ')
                    {
                        if (caracter == '{')
                        {
                            while (caracter != '}')
                            {
                                indiceTexto++;
                                caracter = richTextBox1.Text[indiceTexto];
                            }
                            indiceTexto++;
                            caracter = richTextBox1.Text[indiceTexto];
                        }

                        while (char.IsWhiteSpace(caracter))
                        {
                           // MessageBox.Show("espaço");
                                indiceTexto++;
                                caracter = richTextBox1.Text[indiceTexto];
                        }
                    }
                    j++;
                }


                if (caracter == '\n')
                {
                    linhaTexto++;
                }

                pegaToken(caracter);
                indiceTexto++;

            } while (indiceTexto < richTextBox1.Text.Length && terminaPrograma == 0);
        }

        private void pegaToken(char caracter)
        {
            if(Char.IsDigit(caracter))
            {
                trataDigito(caracter);
                indiceTexto--;
                //MessageBox.Show(caracter.ToString());
            }
            else if(Char.IsLetter(caracter))
            {
                trataId_Palavra(caracter);
                indiceTexto--;

            }
            else if(caracter == ':')
            {
                trataAtribuicao(caracter); 
            }
            else if(caracter == '+' || caracter == '-' || caracter == '*')
            {
                trataOperadorAritmetico(caracter); 
            }
            else if(caracter == '<' || caracter == '>' || caracter == '=' || caracter == '!')
            {
                trataOperadorRelacional(caracter); 
            }
            else if(caracter == ';' || caracter == ',' || caracter == '(' || caracter == ')' || caracter == '.')
            {
                trataPontuacao(caracter); 
            }
            else
            {
                //erro
                if (!char.IsWhiteSpace(caracter))
                {
                    dataGridView2.Rows.Add(new object[] { linhaTexto, "Caracter não reconhecido: " + caracter });
                    terminaPrograma = 1;
                }
            }
                
        }

        private void trataPontuacao(char caracter)
        {


            if (caracter == '.')
            {

                token.Add(new Tokens { lexema = ".", simbolo = "sponto", numLinha = 0 });
                dataGridView1.Rows.Add(new object[] { linhaTexto, "sponto", '.' });
                terminaPrograma = 1;

            }
            else if (caracter == ';')
            {
                token.Add(new Tokens { lexema = ";", simbolo = "sponto_virgula", numLinha = 0 });
                dataGridView1.Rows.Add(new object[] { linhaTexto, "sponto_virgula", ';' });
            }
            else if (caracter == ',')
            {
                token.Add(new Tokens { lexema = ",", simbolo = "svirgula", numLinha = 0 });
                dataGridView1.Rows.Add(new object[] { linhaTexto, "svirgula", ',' });
            }
            else if (caracter == '(')
            {
                token.Add(new Tokens { lexema = "(", simbolo = "sabre_parenteses", numLinha = 0 });
                dataGridView1.Rows.Add(new object[] { linhaTexto, "sabre_parenteses", '(' });
            }
            else if (caracter == ')')
            {
                token.Add(new Tokens { lexema = ")", simbolo = "sfecha_parenteses", numLinha = 0 });
                dataGridView1.Rows.Add(new object[] { linhaTexto, "sfecha_parenteses", ')' });
            }

        }

        private void trataOperadorRelacional(char caracter)
        {


            if (caracter == '=')
            {

                token.Add(new Tokens { lexema = "=", simbolo = "sig", numLinha = 0 });
                dataGridView1.Rows.Add(new object[] { linhaTexto, "sig", '=' });

            }
            else if (caracter == '>')
            {
                indiceTexto++;
                caracter = richTextBox1.Text[indiceTexto];
                if (caracter == '=')
                {
                    token.Add(new Tokens { lexema = ">=", simbolo = "smaiorig", numLinha = 0 });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "smaiorig", ">=" });
                }
                else
                {
                    token.Add(new Tokens { lexema = ">", simbolo = "smaior", numLinha = 0 });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "smaior", ">" });
                    indiceTexto--;
                }

            }
            else if (caracter == '<')
            {
                if (indiceTexto+1 < richTextBox1.Text.Length)
                {
                    indiceTexto++;
                    caracter = richTextBox1.Text[indiceTexto];
                }
                else
                {

                }
                if (caracter == '=')
                {
                    token.Add(new Tokens { lexema = "<=", simbolo = "smenorig", numLinha = 0 });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "smenorig", "<=" });
                }
                else
                {
                    token.Add(new Tokens { lexema = "<", simbolo = "smenor", numLinha = 0 });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "smenor", "<" });
                    indiceTexto--;
                }
            }
            else if (caracter == '!')
            {
                indiceTexto++;
                caracter = richTextBox1.Text[indiceTexto];
                if (caracter == '=')
                {
                    token.Add(new Tokens { lexema = "!=", simbolo = "sdif", numLinha = 0 });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sdif", "!=" });
                }
                else
                {
                    //erro  
                }
            }

        }

        private void trataOperadorAritmetico(char caracter)
        {


            if (caracter == '+')
            {

                token.Add(new Tokens { lexema = "+", simbolo = "smais", numLinha = 0 });
                dataGridView1.Rows.Add(new object[] { linhaTexto, "smais", '+' });

            }
            else if(caracter == '-')
            {
                token.Add(new Tokens { lexema = "-", simbolo = "smenos", numLinha = 0 });
                dataGridView1.Rows.Add(new object[] { linhaTexto, "smenos", '-' });
            }
            else if(caracter == '*')
            {
                token.Add(new Tokens { lexema = "*", simbolo = "smult", numLinha = 0 });
                dataGridView1.Rows.Add(new object[] { linhaTexto, "smult", '*' });
            }

        }

        private void trataAtribuicao(char caracter)
        {
            string atr = "";

            atr = atr + caracter;


            if (indiceTexto < richTextBox1.Text.Length - 1)
            {
                indiceTexto++;
                caracter = richTextBox1.Text[indiceTexto];
               
            }


            if (caracter == '=')
            {
                atr = atr + caracter;
                token.Add(new Tokens { lexema = atr, simbolo = "satribuicao", numLinha = 0 });
                //indiceTexto++; //atencao
                dataGridView1.Rows.Add(new object[] { linhaTexto, "satribuicao", atr });

            }
            else
            {
                token.Add(new Tokens { lexema = atr, simbolo = "sdoispontos", numLinha = 0 });

                dataGridView1.Rows.Add(new object[] { linhaTexto, "sdoispontos", atr });
            }

        }
       private void trataDigito(char caracter)
        {
            string num = "";

            num = num + caracter;

            indiceTexto++;
            if (indiceTexto < richTextBox1.Text.Length)
            {
                caracter = richTextBox1.Text[indiceTexto];
            }
                while (Char.IsDigit(caracter) && indiceTexto < richTextBox1.Text.Length)
                {

                        num = num + caracter;
              //  MessageBox.Show(caracter.ToString());

                   // MessageBox.Show(caracter.ToString());

                    indiceTexto++;

                        if (indiceTexto < richTextBox1.Text.Length)
                        {
                            caracter = richTextBox1.Text[indiceTexto];
                         }
                 //MessageBox.Show(caracter.ToString());
            }

            token.Add(new Tokens { lexema = num, simbolo = "snumero", numLinha = 0 });
            dataGridView1.Rows.Add(new object[] { linhaTexto, "snumero", num });
        }

        private void trataId_Palavra(char caracter)
        {
            string id = "";
            id = id + caracter;

            indiceTexto++;
            if (indiceTexto < richTextBox1.Text.Length )
            {
                caracter = richTextBox1.Text[indiceTexto];
            }

                while ((Char.IsLetterOrDigit(caracter) || caracter == '_') && indiceTexto < richTextBox1.Text.Length)
                {
                    id = id + caracter;
                    indiceTexto++;
                if (indiceTexto < richTextBox1.Text.Length)
                {
                    caracter = richTextBox1.Text[indiceTexto];
                }
                //MessageBox.Show(caracter.ToString());
             }

            switch (id)
            {
                case "programa":
                    token.Add(new Tokens { lexema = id, simbolo = "sprograma", numLinha = 0 });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sprograma", id });
                    break;

                case "se":
                    token.Add(new Tokens { lexema = id, simbolo = "sse", numLinha = 0 });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sse", id });
                    break;

                case "entao":
                    token.Add(new Tokens { lexema = id, simbolo = "sentao", numLinha = 0 });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sentao", id });
                    break;

                case "senao":
                    token.Add(new Tokens { lexema = id, simbolo = "ssenao", numLinha = 0 });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "ssenao", id });
                    break;

                case "enquanto":
                    token.Add(new Tokens { lexema = id, simbolo = "senquanto", numLinha = 0 });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "senquanto", id });
                    break;

                case "faca":
                    token.Add(new Tokens { lexema = id, simbolo = "sfaca", numLinha = 0 });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sfaca", id });
                    break;

                case "inicio":
                    token.Add(new Tokens { lexema = id, simbolo = "sinicio", numLinha = 0 });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sinicio", id });
                    break;

                case "fim":
                    token.Add(new Tokens { lexema = id, simbolo = "sfim", numLinha = 0 });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sfim", id });
                    break;

                case "escreva":
                    token.Add(new Tokens { lexema = id, simbolo = "sescreva", numLinha = 0 });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sescreva", id });
                    break;

                case "leia":
                    token.Add(new Tokens { lexema = id, simbolo = "sleia", numLinha = 0 });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sleia", id });
                    break;

                case "var":
                    token.Add(new Tokens { lexema = id, simbolo = "svar", numLinha = 0 });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "svar", id });
                    break;

                case "inteiro":
                    token.Add(new Tokens { lexema = id, simbolo = "sinteiro", numLinha = 0 });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sinteiro", id });
                    break;

                case "booleano":
                    token.Add(new Tokens { lexema = id, simbolo = "sbooleano", numLinha = 0 });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sbooleano", id });
                    break;

                case "verdadeiro":
                    token.Add(new Tokens { lexema = id, simbolo = "sverdadeiro", numLinha = 0 });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sverdadeiro", id });
                    break;

                case "falso":
                    token.Add(new Tokens { lexema = id, simbolo = "sfalso", numLinha = 0 });
                    dataGridView1.Rows.Add(new object[] { 0, "sfalso", id });
                    break;

                case "procedimento":
                    token.Add(new Tokens { lexema = id, simbolo = "sprocedimento", numLinha = linhaTexto });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sprocedimento", id });
                    break;

                case "funcao":
                    token.Add(new Tokens { lexema = id, simbolo = "sfuncao", numLinha = linhaTexto });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sfuncao", id });
                    break;

                case "div":
                    token.Add(new Tokens { lexema = id, simbolo = "sdiv", numLinha = linhaTexto });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sdiv", id });
                    break;

                case "e":
                    token.Add(new Tokens { lexema = id, simbolo = "se", numLinha = linhaTexto });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "se", id });
                    break;

                case "ou":
                    token.Add(new Tokens { lexema = id, simbolo = "sou", numLinha = linhaTexto });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sou", id });
                    break;

                case "nao":
                    token.Add(new Tokens { lexema = id, simbolo = "snao", numLinha = linhaTexto });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "snao", id });
                    break;

                default:
                    token.Add(new Tokens { lexema = id, simbolo = "sidentificador", numLinha = linhaTexto });
                    dataGridView1.Rows.Add(new object[] { linhaTexto, "sidentificador", id });
                    if(indiceTexto< richTextBox1.Text.Length)
                        analisadorLexical();

                
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            indiceTexto = 0;
            terminaPrograma = 0;
            linhaTexto = 0;
            analisadorLexical();
        }
    }
    public class Tokens
    {
        public string lexema { set; get;}
        public string simbolo { set; get; }
        public int numLinha { set; get; }


    }

}
