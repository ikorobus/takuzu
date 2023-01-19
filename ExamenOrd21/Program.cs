// Cynthia Tristán Álvarez
// Laboratorio:    Puesto:   

//*******************

namespace takuzu
{
    class Program
    {
        const string FICHERO = @"takuzu6"; // ./bin/Debug/takuzu6               // Sentencias de clase
        const bool DEBUG = false;
        static bool continuar = true;

        static void Main(string[] args)                                         // Programa principal
        {
            int N = 4; //*                                                      // Sentencias iniciales
            char[,] tab;
            tab = new char[4, 4] {    // tablero del ejemplo   
                 {'.','1','.','0'},  // fila 0 
                 {'.','.','0','.'},  // fila 1
                 {'.','0','.','.'},  // etc
                 {'1','1','.','0'} };

            if (File.Exists(FICHERO))
            {
                Console.WriteLine("Pulsa X para recuperar la partida.\n" +          // Bloque de lectura de archivo
                                  "Pulsa cualquier otro botón para continuar.");
                string dir = Console.ReadKey(true).Key.ToString();
                if (dir == "X")
                {
                    LeeArchivo(FICHERO, out tab);
                    N = tab.GetLength(0);
                }
                while (Console.KeyAvailable) Console.ReadKey();
            }

            bool[,] fijas = new bool[N, N]; // matriz de posiciones fijas
            InicializaFijas(tab, ref fijas, out int fil, out int col);          // Bloque de inialización
            Renderiza(tab, fijas, fil, col);

            while (continuar)                                                   // Bucle principal
            {
                continuar = !TabLleno(tab);
                ProcesaInput(LeeInput(), ref tab, fijas, ref fil, ref col);
                Renderiza(tab, fijas, fil, col);
            }
            Console.Clear();                                                    // Finalización del programa
            MuestraResultado(tab);
            Console.Write("El juego ha finalizado.");
            while (true) ;
        }

        public static void InicializaFijas(char[,] tab, ref bool[,] fijas, out int fil, out int col)
        {
            int i, j;
            for (j = 0; j < tab.GetLength(1); j++)
            {
                for (i = 0; i < tab.GetLength(0); i++)
                {
                    if (tab[i, j] == '.')
                        fijas[i, j] = false;
                    else
                        fijas[i, j] = true;
                }
            }
            fil = 0;
            col = 0;
        }

        public static void Renderiza(char[,] tab, bool[,] fijas, int fil, int col)
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkMagenta;

            int i, j;
            for (j = 0; j < tab.GetLength(1); j++)
            {
                for (i = 0; i < tab.GetLength(0); i++)
                {
                    if (fijas[j, i] == true)
                        Console.ForegroundColor = ConsoleColor.Blue;
                    else
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(tab[j, i] + " ");
                }
                Console.WriteLine();
            }

            Console.ResetColor();
            Console.WriteLine("TAKUZU");
            if (DEBUG)
            {
                Console.Write("\nTAB: ");
                foreach (char c in tab)
                    Console.Write(c);
                Console.Write("\nFIJAS: ");
                foreach (bool b in fijas)
                    Console.Write(b + ",");
                Console.WriteLine("\nfil: " + fil);
                Console.WriteLine("fil/2: " + fil / 2);
                Console.WriteLine("col: " + col);
            }
            Console.SetCursorPosition(fil, col);
        }

        public static void ProcesaInput(char c, ref char[,] tab, bool[,] fijas, ref int fil, ref int col)
        {
            switch (c)
            {
                case 'u':
                    if (col > 0) col--; // mov up
                    break;
                case 'd':
                    if (col < tab.GetLength(1) - 1) col++; // mov down
                    break;
                case 'l':
                    if (fil / 2 > 0) fil -= 2; // mov left
                    break;
                case 'r':
                    if (fil / 2 < tab.GetLength(0) - 1) fil += 2; // mov right
                    break;
                case '0':
                    if (fijas[col, fil / 2] != true)
                        tab[col, fil / 2] = '0';
                    break;
                case '1':
                    if (fijas[col, fil / 2] != true)
                        tab[col, fil / 2] = '1';
                    break;
                case '.':
                    if (fijas[col, fil / 2] != true)
                        tab[col, fil / 2] = '.';
                    break;
                case 'q':
                    continuar = false;
                    break;
                default:
                    break;
            }
        }

        public static bool TabLleno(char[,] tab)
        {
            int i = 0;
            foreach (char c in tab)
                if (c != '.')
                    i++;
            if (i >= tab.Length)
                return true;
            else
                return false;
        }

        public static void SacaFilCol(int k, char[,] tab, out char[] filk, out char[] colk)
        {
            int i;
            filk = new char[tab.GetLength(0)];
            colk = new char[tab.GetLength(1)];

            for (i = 0; i < filk.Length; i++)
                filk[i] = tab[k, i];
            for (i = 0; i < colk.Length; i++)
                colk[i] = tab[i, k];
        }

        public static bool TresSeguidos(char[] v)
        {
            bool seguidos = false;
            int i;
            for (i = 0; i + 2 < v.Length; i++)
                if (v[i] == v[i + 1] && v[i] == v[i + 2])
                    seguidos = true;
            return seguidos;
        }

        public static bool IgCerosUnos(char[] v)
        {
            bool iguales = false;
            int nZ = 0, // número de 0s
                nU = 0, // número de 1s
                i;
            for (i = 0; i < v.Length; i++)
            {
                if (v[i] == '0')
                    nZ++;
                else if (v[i] == '1')
                    nU++;
            }
            if (nU == nZ)
                iguales = true;
            return iguales;
        }

        public static void MuestraResultado(char[,] tab)
        {
            int i;
            for (i = 0; i < tab.GetLength(0); i++)
            {
                SacaFilCol(i, tab, out char[] filk, out char[] colk);
                if (TresSeguidos(filk))
                    Console.WriteLine("Tres números seguidos iguales en la fila " + i + ".");
                if (TresSeguidos(colk))
                    Console.WriteLine("Tres números seguidos iguales en la columna " + i + ".");
                if (!IgCerosUnos(filk))
                    Console.WriteLine("No mismo número de 0s que de 1s en la fila " + i + ".");
                if (!IgCerosUnos(colk))
                    Console.WriteLine("No mismo número de 0s que de 1s en la columna " + i + ".");
            }
        }

        public static void LeeArchivo(string file, out char[,] tab)
        {
            StreamReader sr = new(file);
            int N = int.Parse(sr.ReadLine()!); // *
            string input = sr.ReadToEnd();
            tab = new char[N, N];
            int i = 0,
                j = 0;
            foreach (char c in input)
            {
                if (c == '\n')
                {
                    j++;
                    i = 0;
                }
                else if (i < N)
                {
                    tab[j, i] = c;
                    i++;
                }
            }
            sr.Close();
        }

        static char LeeInput()
        {
            char d = ' ';
            while (d == ' ')
            {
                if (Console.KeyAvailable)
                {
                    string tecla = Console.ReadKey(true).Key.ToString();
                    switch (tecla)
                    {
                        case "LeftArrow": d = 'l'; break;
                        case "UpArrow": d = 'u'; break;
                        case "RightArrow": d = 'r'; break;
                        case "DownArrow": d = 'd'; break;
                        case "D0": d = '0'; break;  // dígito 0
                        case "D1": d = '1'; break;  // dígito 1                
                        case "Spacebar": d = '.'; break;  // casilla vacia 
                        case "Escape": d = 'q'; break;  // terminar					
                        default: d = ' '; break;
                    }
                }
            }
            return d;
        } // LeeInput
    }
}
