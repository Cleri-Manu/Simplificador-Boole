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
    class Terms {
        private List<Term> _TermsIniciales;                                                     //Los terms con los que comienza el proceso
        public List<Term> TermsIniciales {
            get { return this._TermsIniciales; }
            set { this._TermsIniciales = value; }
        }

        private List<Term> _TermsFinales;                                                       //Los terms con los que finaliza el proceso
        public List<Term> TermsFinales {
            get { return this._TermsFinales; }
            set { this._TermsFinales = value; }
        }

        private List<List<Term>> _GruposDe1s;                                                   //Los terms separados en grupos segun el nº de 1s que tengan
        public List<List<Term>> GruposDe1s {
            get { return this._GruposDe1s; }
            set { this._GruposDe1s = value; }
        }

        private List<String> _TermsBase;
        public List<String> TermsBase {
            get { return this._TermsBase; }
            set { this._TermsBase = value; }
        }

        public List<String> TermsBaseCopy;

        private char _TipoTerm;
        public char TipoTerm {                                                                  //Maxterms --> 0 Minterms --> 1
            get { return this._TipoTerm; }
            set { this._TipoTerm = value; }
        }

        private int _NumeroBits;
        public int NumeroBits {                                                                 //Nº de bits que se utilizan
            get { return this._NumeroBits; }
            set { this._NumeroBits = value; }
        }

        private String ABCD = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";                                     //Utilizado para diferentes funciones

        public string tempS;                                                                    //Variable temporal para comrpobar resultados         

        public Terms() {

        }

        public Terms(string textoFuncionInicial, string textoNONI, int nBits, string tipoFuncion, string tipoEntrada) {
            NumeroBits = nBits;
            TermsIniciales = new List<Term>();
            TermsFinales = new List<Term>();
            TermsBase = new List<String>();
            List<String> debugTerms = new List<String>();



            if (tipoFuncion.Equals("Letras")) {                                                 //Si la funcion es tipo Letras obtenemos la lista de terms con su numero a partir de ellas
                if (textoFuncionInicial.Contains('*') && textoFuncionInicial.Contains('+')) {                                        
                    textoFuncionInicial = textoFuncionInicial.Replace("+", "");                 //Maxterms
                    textoFuncionInicial = textoFuncionInicial.Replace(" ", "");
                    textoFuncionInicial = textoFuncionInicial.Replace(")", "");
                    textoFuncionInicial = textoFuncionInicial.Replace("(", "");
                    TipoTerm = '0';
                    TermsBase = new List<string>(textoFuncionInicial.Split('*'));
                    TermsBase = CalcularTerminos(TermsBase);
                } else if (textoFuncionInicial.Contains('+')) {                                  //Minterms
                    TipoTerm = '1';
                    TermsBase = new List<string>(textoFuncionInicial.Split('+'));
                    textoFuncionInicial = textoFuncionInicial.Replace(" ", "");
                    textoFuncionInicial = textoFuncionInicial.Replace(")", "");
                    textoFuncionInicial = textoFuncionInicial.Replace("(", "");
                    TermsBase = CalcularTerminos(TermsBase);
                }
            } else {
                TermsBase = new List<string>(textoFuncionInicial.Split(','));                   //Si no, separamos los terms directamente con las comas obteniendo sus numeros
                if(tipoEntrada.Equals("Minterms")) {                                            //Y asignamos le tipo de Term que usamos
                    TipoTerm = '1';
                } else {
                    TipoTerm = '0';
                }
            }

            foreach(String termino in textoNONI.Split(',')) {                                   //Añadimos todos los terms NONI para operar
                if(!termino.Equals(""))
                    TermsIniciales.Add(new Term(termino, NumeroBits, TipoTerm));
            }

            foreach (String termino in TermsBase) {                                             //A continuacion añadimos todos los terms a nuestra lista, creándolos y consiguiendo así su valor en bits
                TermsIniciales.Add(new Term(termino, NumeroBits, TipoTerm));
            }
            TermsBaseCopy = new List<String>(TermsBase);

            ContarUnos();                                                                       //Posteriormente se separan los terminos en grupos según su nº de unos
            bool continuar = true;
            while (continuar) {                                                                 //Ahora buscamos todas las simplifciaciones posibles
                continuar = Simplificar();
            }

            foreach (List<Term> t in GruposDe1s) {                                              //Ahora copiamos los terms simplificados que nos han quedado a la lista de termsFinales
                foreach (Term termino in t) {
                    TermsFinales.Add(termino);
                }
            }

            SeleccionarTermsUnicos();//
            TermNecesario();
        }

        private List<String> RellenarLetras(List<String> termsBase, List<int> longitudIni) {                               //Rellena los terminos en formato letras (AB; AC, Ce, D...) con '-' donde falte algo
            List<String> termsRellenados = new List<string>();
            foreach(string termino in termsBase) {                                          //Para cada termino
                //if (termino.Length != NumeroBits) {                                         //Si no está completo
                    longitudIni.Add(termino.Length);                                        //Guardamos la cantidad inicial de bits que nos dan
                    string nuevoTermino = "";
                    int j = 0;
                    for(int i = 0; i < NumeroBits; i++) {                                   //Lo comparamos con las letras del abecedario, donde falte una ponemos un '-' y donde coincide ponemos un 1 o un 0 segun corresponda
                        if( j < termino.Length && termino[j] == ABCD[i]) {
                            if (TipoTerm == '1') {                                         
                                nuevoTermino = nuevoTermino + "1";                         //Si es minterms
                            } else {
                                nuevoTermino = nuevoTermino + "0";                         //Si es maxterms
                            }
                            j++;
                        } else if (j < termino.Length &&  termino[j] == Char.ToLower(ABCD[i])) {
                            if (TipoTerm == '1') {
                                nuevoTermino = nuevoTermino + "0";                         //Si es minterms
                            } else {
                                nuevoTermino = nuevoTermino + "1";                         //Si es maxterms
                            }
                            j++;
                        } else {
                            nuevoTermino = nuevoTermino + '-';
                        }
                    }
                    termsRellenados.Add(nuevoTermino);                                      //Lo añadimos a la lista de terminos rellenados
                //} else {
                 //   termsRellenados.Add(termino);                                           //Si está completo lo añadimos directamente
                //}
            }
            return termsRellenados;
        }

        private List<String> CalcularTerminos(List<String> termsBase) {
            List<String> terminosCompletos = new List<string>();
            List<int> longitudIni = new List<int>();
            int count = 0;
            termsBase = RellenarLetras(termsBase, longitudIni).ToList();                    //Rellenamos los terminos
            foreach(string termino in termsBase) {                                          //Si encontramos un termino con un '-' significa que está simplificado, por lo que habrá que calcular los terminos base de los caules se ha deducido
                if (termino.Contains('-')) {                                                //Reemplazamos todos loas guiones por los posibles n valores (n=2^[numbits-texto.length])
                    for (int i =  0; i < Math.Pow(2, NumeroBits - longitudIni[count]); i++) { //Asi creamos los m terminos base de los que sale el actuial y los añadimos a la lista
                        string rellenoActual = Convert.ToString(i, 2);
                        int l = 0;
                        char[] auxTermino = termino.ToCharArray();
                        for (int j = 0; j < NumeroBits; j++) {
                            if (auxTermino[j] == '-') {
                                auxTermino[j] = rellenoActual[l];
                                l++;
                            }
                        }
                        if(!terminosCompletos.Contains(BitsATerm(new String(auxTermino)))) {
                            terminosCompletos.Add(BitsATerm(new String(auxTermino)));                      //Lo añadimos a la lista
                        }

                    }
                    count++;
                } else {
                    if (!terminosCompletos.Contains(BitsATerm(termino))) {
                        terminosCompletos.Add(BitsATerm(termino));                                         //Si ya está completo lo añadimos directamente a la lista
                    }
                    
                }
            }
            return terminosCompletos;

        }

        private void ContarUnos() {                                                             //Añade los terms al grupo que le corresponde dentro de la lista de grupos segun su nº de unos
            GruposDe1s = new List<List<Term>>();

            for (int i = 0; i < NumeroBits + 1; i++) {                                          //Creamos grupos para todos los casos posibles
                GruposDe1s.Add(new List<Term>());
            }

            foreach(Term t in TermsIniciales) {                                                 //Añadimos cada Termino al grupo que corresponde (si tiene 0 unos va al grupo 0, si tiene 5 unos va al grupo cinco, etc)
                GruposDe1s[t.BitsTerm.Count(f => f == '1')].Add(t);
            }
            
        }

        private Boolean Simplificar() {
            int i = 0;
            foreach(List<Term> t in GruposDe1s) {
                i += t.Count();
            }

            if(i <= 1) {
                return false;
            }

            bool simplificadoAlgo = false;
            int grupoSig = 0;
            for (int grupoN = 0; grupoN < GruposDe1s.Count; grupoN++) {                         //Recorremos todos los grupos menos el ultimo para buscar simplificaciones
                if (grupoN == GruposDe1s.Count - 1) {
                    grupoSig = grupoN - 1;
                } else {
                    grupoSig = grupoN + 1;
                }
                foreach (Term t1 in GruposDe1s[grupoN].Reverse<Term>()) {                       //Comparamos los del grupo actual con los del siguiente
                    foreach (Term t2 in GruposDe1s[grupoSig].Reverse<Term>()) {                 //Trabajo con las listas empezando por el final para poder modificarlas mientras las uso sin que esto afecte a la ejecución
                        int posicion = -1;                                                      //De esta manera si el tamaño de la lista es 4 empiezo por el 4º term de la lista, luego el 3º, etc, y puedo añadir un 5º, 6º... si que ocurra nada en este momento
                        if ((posicion = t1.Simplificable(t2)) != -1) {                          //Si los terminos se pueden simplificar
                            simplificadoAlgo = true;                                            //Si hemos simplificado algo ponemos esto a true para indicarlo (si no simplificamos nada significará que hemos acabado)
                            StringBuilder sb = new StringBuilder(t1.BitsTerm);                  //Cojemos el valor en bits del primer Term
                            sb[posicion] = '-';                                                 //Y cambiamos el lugar donde se puede simplicar por un '-' (lo simplificamos)
                            t1.Simplificado = true;                                             //Marcamos ambos como simplificados y luego añadimos el nuevo term simplificado a la lista de terms
                            t2.Simplificado = true;                                             //Luego creamos el nuevo term y lo añadimos al grupo correspondiente
                            Term tempTerm = new Term(t1.NumeroTerm, t2.NumeroTerm, NumeroBits, sb.ToString(), TipoTerm);
                            int tempNUnos= tempTerm.BitsTerm.Count(f => f == '1');
                            if(!BuscarIgual(GruposDe1s[tempNUnos], tempTerm)) {                 //Si no hay un term que sea igual en cuanto a valor se añade
                                GruposDe1s[tempNUnos].Add(tempTerm);
                            }
                        }
                    }                                                                           //Una vez hemos acabado el term se elimina si se ha simplificado
                    if(t1.Simplificado) {
                        GruposDe1s[t1.BitsTerm.Count(f => f == '1')].Remove(t1);
                    }
                }
            }
            return simplificadoAlgo;
        }

        private bool BuscarIgual(List<Term> lista, Term termino) {                              //Busca un term que tenga el mismo valor en bits en una lista dada, a partir de un term dado
            foreach(Term t in lista) {
                if (t.Equals(termino)) {
                    return true;
                }
            }
            return false;
        }

        private void SeleccionarTermsUnicos() {                                                 //Simplificado == unico/necesario en este caso
            foreach(Term t in TermsFinales) {
                if (t.Simplificado = TermUnico(t)) {
                    foreach (String s in t.NumeroTerm) {                                        //Si es unico borramos todos los terms que tiene de la lista de los que nos hacen falta
                        if(TermsBase.Contains(s)) {
                            TermsBase.Remove(s);
                        }
                    }
                }
            }
        }

        private bool TermUnico(Term termino) {                                                  //Busca los terminos unicos                                      
            foreach (String s in termino.NumeroTerm) {
                if(TermsBase.Contains(s)) {                                                     //Si el que miramos no es un NO/NI
                    if (!TermRepetido(termino, s))
                        return true;
                }
            }
            return false;
        }

        private bool TermRepetido(Term termino, string valor) {                                 //Indica si hay mas terminos que él mismo que esten derivados del termino "valor" (es decir, si valor es '5' indica si el termino contiene '5')(indica si es unico para ese valor)
            bool repetido = false;
            for (int i = 0; i < TermsFinales.Count; i++) {
                if (!termino.Equals(TermsFinales[i])) {
                    if (TermsFinales[i].NumeroTerm.Contains(valor)) {
                        repetido = true;
                    }
                }
            }
            return repetido;
        }

        private void TermNecesario() {                                                          //En caso de que todavía se hayan conseguido todos los terms busca los más patos para ser parte de la solucion
            Term tNecesario = TermsFinales[0];
            if(TermsBase.Count != 0) {
                foreach (String termB in TermsBase.Reverse<String>()) {                         //Comprobamos si el term aporta alguno de los que nos faltan
                    foreach (Term termF in TermsFinales) {
                        if (termF.NumeroTerm.Contains(termB)) {
                            termF.Heuristica += 4;                                              //Por cada uno que aporta aumenta su heurisitca en 4
                            if(tNecesario.Heuristica < termF.Heuristica) {                      //Seleccionamos el term con mayor heuristica
                                tNecesario = termF;
                            }
                        }
                    } 
                }
                tNecesario.Simplificado = true;
                foreach (String s in tNecesario.NumeroTerm) {                                   //Si es necesario borramos todos los terms que tiene de la lista de los que nos hacen falta
                    if (TermsBase.Contains(s)) {
                        TermsBase.Remove(s);
                    }
                }
                TermNecesario();                                                                //Repetimos recursivamente el rpoceso hasta que acabemos
            }
        }

        private int CalcularHeuristica() {
            return 0;
        }

        public String TermsALetras() {
            String texto = " ";
            if (TipoTerm == '1') {//Minterm
                foreach (Term t in TermsFinales) {
                    if(t.Simplificado) 
                        texto += t.TermALetras() + " + ";    
                }
            } else {
                foreach (Term t in TermsFinales) {
                    if(t.Simplificado)
                        texto += t.TermALetras() + " * ";
                }
            }


            return texto.Remove(texto.Length - 3); ;
        }

    private String BitsATerm(string ValorEnBits) {
            return Convert.ToInt32(ValorEnBits, 2).ToString();
        }
    }
}
