using System;

class MainClass {
  public static void Main (string[] args) {
    try {
      Console.WriteLine("Informe um inteiro: ");
      int inteiro = int.Parse(Console.ReadLine());
      Console.WriteLine("\nPares encontrados: ");
      for(int i = 0; i <= inteiro; i++) {
          if (i % 2 == 0)  
              { Console.Write ("{0} ", i); }
          if ((i % 20 == 0) && (i > 0)) 
              { Console.Write ("\n"); }
      }
    } catch(Exception) { Console.WriteLine("Inteiro invalido"); }
  }
}