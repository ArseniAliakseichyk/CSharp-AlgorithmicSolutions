using System;
using System.Text;

namespace EncryptionProgram
{
    abstract class Szyfr
    {
        public abstract string Szyfruj(string txt);
        public abstract string Odszyfruj(string txt);
    }

    // Klasa Cezar – szyfruje tekst przez przesunięcie znaków w alfabecie
    class Cezar : Szyfr
    {
        private int _klucz;

        public Cezar(int klucz)
        {
            _klucz = klucz;
        }

        // Szyfruje tekst według klucza
        public override string Szyfruj(string txt)
        {
            return Przetworz(txt, false);
        }

        // Deszyfruje tekst według klucza
        public override string Odszyfruj(string txt)
        {
            return Przetworz(txt, true);
        }

        // Logika szyfrująca i deszyfrująca w jednym miejscu
        private string Przetworz(string txt, bool odwrotnie)
        {
            const string wszystkieZnaki = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789ąćęłńóśźżĄĆĘŁŃÓŚŹŻ!@#$%^&*()_+-=[]{}|;:' ,.<>?/`~";
            int dlugosc = wszystkieZnaki.Length;
            StringBuilder sb = new();

            foreach (char znak in txt)
            {
                int indeks = wszystkieZnaki.IndexOf(znak);
                if (indeks >= 0)
                {
                    int przesuniecie = odwrotnie ? -_klucz : _klucz;
                    int nowyIndeks = (indeks + przesuniecie + dlugosc) % dlugosc;
                    sb.Append(wszystkieZnaki[nowyIndeks]);
                }
                else
                {
                    sb.Append(znak);
                }
            }

            return sb.ToString();
        }
    }

    // Klasa Plot – szyfruje tekst metodą Rail Fence (schodkowo)
    class Plot : Szyfr
    {
        private int _wys;

        public Plot(int wys)
        {
            _wys = wys;
        }

        // Szyfruje tekst metodą płotkową
        public override string Szyfruj(string txt)
        {
            char[,] szyna = new char[_wys, txt.Length];
            bool wDol = false;
            int wiersz = 0;

            for (int i = 0; i < txt.Length; i++)
            {
                szyna[wiersz, i] = txt[i];
                if (wiersz == 0 || wiersz == _wys - 1)
                {
                    wDol = !wDol;
                }
                wiersz += wDol ? 1 : -1;
            }

            StringBuilder sb = new();
            for (int i = 0; i < _wys; i++)
            {
                for (int j = 0; j < txt.Length; j++)
                {
                    if (szyna[i, j] != '\0')
                    {
                        sb.Append(szyna[i, j]);
                    }
                }
            }
            return sb.ToString();
        }

        // Odszyfrowuje tekst metodą płotkową
        public override string Odszyfruj(string txt)
        {
            char[,] szyna = new char[_wys, txt.Length];
            bool wDol = false;
            int wiersz = 0;

            for (int i = 0; i < txt.Length; i++)
            {
                szyna[wiersz, i] = '*';
                if (wiersz == 0 || wiersz == _wys - 1)
                {
                    wDol = !wDol;
                }
                wiersz += wDol ? 1 : -1;
            }

            int idx = 0;
            for (int i = 0; i < _wys; i++)
            {
                for (int j = 0; j < txt.Length; j++)
                {
                    if (szyna[i, j] == '*' && idx < txt.Length)
                    {
                        szyna[i, j] = txt[idx++];
                    }
                }
            }

            StringBuilder sb = new();
            wiersz = 0;
            wDol = false;
            for (int i = 0; i < txt.Length; i++)
            {
                sb.Append(szyna[wiersz, i]);
                if (wiersz == 0 || wiersz == _wys - 1)
                {
                    wDol = !wDol;
                }
                wiersz += wDol ? 1 : -1;
            }
            return sb.ToString();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("/~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("  Witamy w programie do szyfrowania tekstu.");
            Console.ResetColor();
            Console.Write("\n  Wpisz słowo lub zdanie, które chcesz zaszyfrować.\n  Na przykład: ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("Szyfrowanie");
            Console.ResetColor();
            Console.WriteLine();
            Console.Write("\n  Szyfrowanie zostanie wykonane w następujący sposób:\n\n");

            Console.Write("  Pierwszy krok: Szyfr Cezara (przesunięcie o 2)\n");
            Console.Write("  Zaszyfrowane słowo: \"");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Szyfrowanie");
            Console.ResetColor();
            Console.Write("\" => ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Ubahtqycpkg\n\n");
            Console.ResetColor();

            Console.Write("  Drugi krok: Szyfr Rail Fence (wysokość 3)\n");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("  1.      U       t       p");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("  2.        b   h   q   c   k");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("  3.          a       y       g");
            Console.ResetColor();

            Console.Write("\n  Zaszyfrowane słowo: \"Szyfrowanie\" => ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Utp");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("bhqck");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("ayg\n");
            Console.ResetColor();

            Console.WriteLine("\n  Program posiada funkcję dekodera. Aby to zrobić, musisz wstawić \n  słowo lub tekst, który jest już zaszyfrowany, oraz wybrać,\n  jaką wysokość szyfrowania zastosowano w Rail Fence\n  i jakie było przesunięcie Cezara.");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\\~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~/");
            Console.ResetColor();

            int opcja;
            while (true)
            {
                Console.Write("\nWybierz:\n1. Odszyfruj\n2. Szyfruj\n> ");
                if (int.TryParse(Console.ReadLine(), out opcja) && (opcja == 1 || opcja == 2))
                    break;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Błąd: Wpisz 1 (Odszyfruj) lub 2 (Szyfruj).");
                Console.ResetColor();
            }

            string txt;
            do
            {
                Console.Write("\nTekst: ");
                txt = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(txt))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Błąd: Tekst nie może być pusty.");
                    Console.ResetColor();
                }
            } while (string.IsNullOrWhiteSpace(txt));

            int wys;
            while (true)
            {
                if (opcja == 2)
                {
                    Console.Write("\nWysokość Rail Fence\n");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("Zalecenie: długość tekstu powinna być ≥ 2×wys lub min. 4 znaki dla wys < 3.\n");
                    Console.ResetColor();
                    Console.Write(": ");
                }
                else
                {
                    Console.Write("\nWysokość Rail Fence: ");
                }

                if (int.TryParse(Console.ReadLine(), out wys) && wys >= 2)
                    break;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Błąd: Wysokość musi być >= 2.");
                Console.ResetColor();
            }

            int klucz;
            while (true)
            {
                Console.Write("\nKlucz Cezara: ");
                if (int.TryParse(Console.ReadLine(), out klucz) && klucz > 0)
                    break;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Błąd: Klucz musi być liczbą dodatnią.");
                Console.ResetColor();
            }

            Szyfr cezar = new Cezar(klucz);
            Szyfr plot = new Plot(wys);

            string wynik = opcja == 1
                ? cezar.Odszyfruj(plot.Odszyfruj(txt))
                : plot.Szyfruj(cezar.Szyfruj(txt));

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("\nWynik: \"");
            Console.ResetColor();
            Console.Write(wynik);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("\"\n\n");
            Console.ResetColor();
        }
    }
}