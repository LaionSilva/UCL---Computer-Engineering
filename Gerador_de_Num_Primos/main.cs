
using System;
using System.Collections.Generic;

class MainClass {
  public static List<int> nPrimos = new List<int>();

  public static void Main (string[] args) {
    string in_inf;
    int limite;
    Console.Write("GERADOR DE NÚMEROS PRIMOS\n\nInforme o range da busca: ");

    in_inf = Console.ReadLine();
    if(CheckInput(in_inf, "double")) {
      limite = int.Parse(in_inf);
      if(limite > 1) 
        { Calcular(limite); } 
      else { Console.WriteLine("\nNenhum número primo encontrado."); }
    } else { Console.WriteLine("Valor invalido."); }
    Console.WriteLine("\nCÁLCULOS FINALIZADOS!");
    Console.ReadKey(); 
  }

  public static void Calcular(int limite) {
    int cont = 0;
    for(int i = 3; i <= limite; i += 2) {
      bool check = true;
      int j, unid;
      unid = i % 10;
      if((unid != 5)||(i == 5)) {
        for(j = 0; j < cont; j++) {
          if(i % nPrimos[j] == 0) { check = false; break; }
          if(nPrimos[j] > Math.Sqrt(i)) { break; }
        }
        if(check) { 
          Console.WriteLine("{0} - Divisões Realizadas: {1}", i, j + 1); 
          nPrimos.Add(i); cont++;
        }
      }
      if(i % 30 == 0) { System.Threading.Thread.Sleep(1); }
    }
  }

  public static bool CheckInput ( string input, string type = "int" ) {
    if(type == "double") {
      for(int i = 0; i < input.Length; i++) { 
        if ( !(((input[i] >= 48) && (input[i] <= 57)) || (input[i] == 44) || (input[i] == 46)) ) 
          { return false; }
      }
    } return true;
  }

} 
