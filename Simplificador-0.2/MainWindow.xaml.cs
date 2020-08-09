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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Simplificador_0._2 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }
        private void Text_GotFocus(object sender, RoutedEventArgs e) {
            System.Windows.Controls.TextBox tb = (System.Windows.Controls.TextBox)sender;
            tb.Text = tb.Text != string.Empty ? string.Empty : tb.Name;
        }

        private void Text_LostFocus(object sender, RoutedEventArgs e) {
            System.Windows.Controls.TextBox tb = (System.Windows.Controls.TextBox)sender;
            tb.Text = tb.Text == string.Empty ? tb.Name : tb.Text;
        }
        private void B_Simplificar_Click(object sender, RoutedEventArgs e) {
            Terms nuevo = new Terms();
            string textoNoni = "";
            string textoFuncionInicial = FuncionInicial.Text;



            if (NONI.Text.Equals("NONI") || NONI.Text.Equals("Introduce los valores NO/NI") || NONI.Text.Equals("")) {
                textoFuncionInicial = textoFuncionInicial.Replace(" ", "");
                textoFuncionInicial = textoFuncionInicial.Replace(")", "");
                textoFuncionInicial = textoFuncionInicial.Replace("(", "");
                if(textoFuncionInicial.Equals("a+b") || textoFuncionInicial.Equals("A*B")) {
                    TextoSalida1.Text = "a+b";
                    TextoSalida2.Text = "A*B";
                    TextoSalida2.Foreground = Brushes.Green;
                    TextoSalida2.FontWeight = FontWeights.Bold;
                    TextoSalida1.Foreground = Brushes.Green;
                    TextoSalida1.FontWeight = FontWeights.Bold;
                    return;;
                } else if(textoFuncionInicial.Equals("a+B") || textoFuncionInicial.Equals("A*b")) {
                    TextoSalida1.Text = "a+B";
                    TextoSalida2.Text = "A*b";
                    TextoSalida2.Foreground = Brushes.Green;
                    TextoSalida2.FontWeight = FontWeights.Bold;
                    TextoSalida1.Foreground = Brushes.Green;
                    TextoSalida1.FontWeight = FontWeights.Bold;
                    return;;
                } else if (textoFuncionInicial.Equals("A+b") || textoFuncionInicial.Equals("a*B")) {
                    TextoSalida1.Text = "a+b";
                    TextoSalida2.Text = "a*B";
                    TextoSalida2.Foreground = Brushes.Green;
                    TextoSalida2.FontWeight = FontWeights.Bold;
                    TextoSalida1.Foreground = Brushes.Green;
                    TextoSalida1.FontWeight = FontWeights.Bold;
                    return;;
                } else if (textoFuncionInicial.Equals("A+B") || textoFuncionInicial.Equals("a*b")) {
                    TextoSalida1.Text = "a+b";
                    TextoSalida2.Text = "a*b";
                    TextoSalida2.Foreground = Brushes.Green;
                    TextoSalida2.FontWeight = FontWeights.Bold;
                    TextoSalida1.Foreground = Brushes.Green;
                    TextoSalida1.FontWeight = FontWeights.Bold;
                    return;;
                } else if (textoFuncionInicial.Equals("a") || textoFuncionInicial.Equals("A")) {
                    TextoSalida1.Text = "a";
                    TextoSalida2.Text = "A";
                    TextoSalida2.Foreground = Brushes.Green;
                    TextoSalida2.FontWeight = FontWeights.Bold;
                    TextoSalida1.Foreground = Brushes.Green;
                    TextoSalida1.FontWeight = FontWeights.Bold;
                    return;;
                } else if (textoFuncionInicial.Equals("A") || textoFuncionInicial.Equals("a")) {
                    TextoSalida1.Text = "A";
                    TextoSalida2.Text = "a";
                    TextoSalida2.Foreground = Brushes.Green;
                    TextoSalida2.FontWeight = FontWeights.Bold;
                    TextoSalida1.Foreground = Brushes.Green;
                    TextoSalida1.FontWeight = FontWeights.Bold;
                    return;;
                }
            } else {
                textoNoni = NONI.Text;
            }

            //4,5,6,7,8,9,11 (9,14,15) M-->0,1,2,3,10,12,13
            //aBc+Abc+AbD+aBD+aBC
            //0,2,3,5,7,8,10,13 11,15
            //abcd+abCd+aBcD+aBCD+Abcd+AbCd+ABcD+abCD  A+b+C+D*A+B+C+D
            //0,2,6,10,12,30,28,20,1,3,7,11,15,29,21  14,9,23
            //0,1,2,3,6,7,10,11,12,15,20,21,28,29,30  9,14,23
            //00000 00010 00110 01010 01100 11110 11100 10100 00001 00011 00111 01011 01111 11101 10101 
            //01110 01001 10111

            if (FuncionInicial.Text.Equals("") || FuncionInicial.Text.Equals(" ") || NumBits.Text.Equals("") || NumBits.Text.Equals(" ") || NONI.Text.Equals("") || NONI.Text.Equals(" ")) return;;
            nuevo = new Terms(FuncionInicial.Text, textoNoni, int.Parse(NumBits.Text), tipoInput.Text, tipoSeleccionado.Text); //"a*B*c,A*b*c,A*b*D,a*B*D,a*B*C"

            string temp = "";
            nuevo.TermsBase.Sort();
            /*
            foreach(List<Term> t in nuevo.GruposDe1s) { //temp += t.BitsTerm + "-";
                foreach (Term termino in t) {
                    temp += termino.BitsTerm + "|";
                }

            }*/
            /*
            foreach(String s in nuevo.TermsBase) {
                temp += s + "|";
            }*/

            temp = nuevo.TermsALetras();
            TextoSalida1.Text = temp;

            List<String> funIni2 = new List<String>();
            for(int i = 0; i < Math.Pow(2, int.Parse(NumBits.Text)); i++) {
                funIni2.Add(i.ToString());
            }
            string nuevaFuncion2 = "";
            foreach (String s in nuevo.TermsBaseCopy) {
                if (funIni2.Contains(s)) {
                    funIni2.Remove(s);
                }
            }

            foreach (string s in NONI.Text.Split(',')) {
                if (funIni2.Contains(s)) {
                    funIni2.Remove(s);
                }
            }
            if(!(funIni2.Count == 0)) {
                foreach (String s in funIni2) {
                    nuevaFuncion2 += s + ",";
                }
                nuevaFuncion2 = nuevaFuncion2.Remove(nuevaFuncion2.Length - 1); //borramos la última ','

                Terms nuevo2 = new Terms();

                if (nuevo.TipoTerm == '0') {
                    nuevo2 = new Terms(nuevaFuncion2, textoNoni, int.Parse(NumBits.Text), "Minterms", "Numeros");
                } else {
                    nuevo2 = new Terms(nuevaFuncion2, textoNoni, int.Parse(NumBits.Text), "Maxterms", "Numeros");
                }

                String temp2 = nuevo2.TermsALetras();
                TextoSalida2.Text = temp2;

                SeleccionarMejor(temp, temp2);
            }



        }

        private void TipoSeleccionado_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }

        private void TipoInput_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (tipoInput.Text.Equals("Numeros")) {
                tipoSeleccionado.Visibility = Visibility.Hidden;
            } else {
                tipoSeleccionado.Visibility = Visibility.Visible;
            }

        }

        public String BitsATerm(string ValorEnBits) {
            return Convert.ToInt32(ValorEnBits, 2).ToString();
        }

        public void SeleccionarMejor(string texto, string texto2) {
            if ((texto2.Count(f => f == '+') + texto2.Count(f => f == '*')) < (texto.Count(f => f == '+') + texto.Count(f => f == '*'))) {
                TextoSalida2.Foreground = Brushes.Green;
                TextoSalida2.FontWeight = FontWeights.Bold;
                TextoSalida1.Foreground = Brushes.Black;
                TextoSalida1.FontWeight = FontWeights.Normal;
            } else if((texto2.Count(f => f == '+') + texto2.Count(f => f == '*')) > (texto.Count(f => f == '+') + texto.Count(f => f == '*'))) {
                TextoSalida2.Foreground = Brushes.Black;
                TextoSalida2.FontWeight = FontWeights.Normal;
                TextoSalida1.Foreground = Brushes.Green;
                TextoSalida1.FontWeight = FontWeights.Bold;
            } else if((texto2.Count(f => f == '+') + texto2.Count(f => f == '*')) == (texto.Count(f => f == '+') + texto.Count(f => f == '*'))) {
                TextoSalida2.Foreground = Brushes.Green;
                TextoSalida2.FontWeight = FontWeights.Bold;
                TextoSalida1.Foreground = Brushes.Green;
                TextoSalida1.FontWeight = FontWeights.Bold;
            }
        }

    }
}
