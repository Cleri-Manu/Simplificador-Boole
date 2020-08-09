using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplificador_0._2 {
    class Term {
        private String ABCD = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";                     //Utilizado para diferentes funciones

        private int NumBits;                                                    //Indica el nº de bits con el que se trabaja

        private List<String> _NumeroTerm;                                       //Indica el nº del term por ejemplo si ers el 0000 sera el 0, si es el 0100 sera el 4, etc. SI esta compuesto por varios indica primero el original yluego el resto pro ejemplo: 2-5-6
        public List<String> NumeroTerm {                                        //Si el numero fuera una combinacion de varios se guardan en las diferentes posiciones
            get { return this._NumeroTerm; }
            set { this._NumeroTerm = value; }
        }

        private string _BitsTerm;                                               //Indica el valor en bits del term
        public string BitsTerm {
            get { return this._BitsTerm; }
            set { this._BitsTerm = value; }
        }

        private bool _Simplificado;                                             //Indica si el term se ha simplificado o no
        public bool Simplificado {
            get { return this._Simplificado; }
            set { this._Simplificado = value; }
        }

        private int _Heuristica;
        public int Heuristica {                                                                 //Heurística para seleccionar el term más apto para la solución
            get { return this._Heuristica; }
            set { this._Heuristica = value; }
        }

        private char _TipoTerm;
        public char TipoTerm {                                                                  //Maxterms --> 0 Minterms --> 1
            get { return this._TipoTerm; }
            set { this._TipoTerm = value; }
        }

        public Term(string numTermIni, int numBitsIni, char tipoT) {
            NumeroTerm = new List<String>();
            NumeroTerm.Add(numTermIni);
            NumBits = numBitsIni;
            ConvertirNumABits(NumeroTerm[0]);
            Simplificado = false;
            Heuristica = BitsTerm.Count(f => f == tipoT);
            TipoTerm = tipoT;
        }

        public Term(List<String> numTermT1, List<String> numTermT2, int numBitsIni, string bitsTermIni, char tipoT) {
            NumeroTerm = new List<String>(numTermT1.Concat(numTermT2));
            NumBits = numBitsIni;
            BitsTerm = bitsTermIni;
            Simplificado = false;
            Heuristica = BitsTerm.Count(f => f == tipoT);
            TipoTerm = tipoT;
        }

        private void ConvertirNumABits(string textoInicial) {
            BitsTerm = Convert.ToString(Convert.ToInt32(textoInicial, 10), 2);  //Convierte a binario el term
            while (this.NumBits - BitsTerm.Length != 0) {                       //Mientras no tenga el nº correcto de bits se le añaden 0s a la izquierda
                BitsTerm = "0" + BitsTerm;
            }
        }

        public int Simplificable(Term otro) {
            int count = 0;
            int pos = -1;
            for (int i = 0; i < NumBits; i++) {                                 //Similar al Equals, solo que devuelve la posicion en la que se puede simplificar (si se puede)
                if (BitsTerm[i] != otro.BitsTerm[i]) {                          //O -1 si no se puede simplificar
                    count++;
                    pos = i;
                }
            }
            if(count == 1) {                                                    //Si hay más de un bit diferente no se puede simplificar
                return pos;
            } else {
                return -1;
            }
        }

        public bool Equals(Term otro) {
            for (int i = 0; i < NumBits; i++) {
                if (BitsTerm[i] != otro.BitsTerm[i]) {
                    return false;
                }
            }
            return true;
        }

        public String TermALetras() {
            char[] array = BitsTerm.ToCharArray();
            String ABCD = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            String text = "";
            if ( TipoTerm == '1') {
                text += "(";
                for (int i = 0; i < array.Length; i++) {
                    if (array[i] == TipoTerm) {
                        text += ABCD[i] + "*";
                    } else if (array[i] == '-') {

                    } else {
                        text += Char.ToLower(ABCD[i]) + "*";
                    }
                }
                text = text.Remove(text.Length - 1); //borramos el ultimo *
                text += ")";
            } else {
                text += "(";
                for (int i = 0; i < array.Length; i++) {
                    if (array[i] == TipoTerm) {
                        text += ABCD[i] + "+";
                    } else if (array[i] == '-') {

                    } else {
                        text += Char.ToLower(ABCD[i]) + "+";
                    }
                }
                text = text.Remove(text.Length - 1); //borramos el ultimo *
                text += ")";
            }


            return text;
        }

        private String BitsATerm() {
            return Convert.ToInt32(BitsTerm, 2).ToString();
        }
    }
}
