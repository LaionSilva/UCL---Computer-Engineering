using System;

class MainClass {
  public static void Main (string[] args) {
    int inteiro = 0;

    Console.Write("Informe um inteiro: ");
    try {
        inteiro = int.Parse(Console.ReadLine());
        for(int i = 0; i < inteiro; i++) {  // Linhas
            for(int j = 0; j < inteiro - i -1; j++)  // Espaços vazios
                { Console.Write(" "); }
            for(int j = 0; j < 1 + i * 2; j++)  // Zeros
                { Console.Write("0"); }
            Console.Write("\n");
        }
    } catch { Console.Write("Valor ivalido! "); }

    Console.ReadKey(); 
  }
}