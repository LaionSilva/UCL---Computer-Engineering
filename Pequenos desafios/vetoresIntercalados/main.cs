using System;

class MainClass {
  public static void Main (string[] args) {
    string in_inf;
    float[] vetor1 = new float[10], vetor2 = new float[10], vetor3 = new float[20];
    
    Console.WriteLine("\nPrimeiro vetor");
    for(int i =0; i<10; i++){
        Console.Write("{0}° Valor: ", i + 1);
            in_inf = Console.ReadLine();
            if(CheckInput(in_inf, "double")){ etor1[i] = float.Parse(in_inf); }
            else { Console.WriteLine("Valor invalido."); i--; }
    }

    Console.WriteLine("\nSegundo vetor");
    for(int i =0; i<10; i++){
        Console.Write("{0}° Valor: ", i + 1);
            in_inf = Console.ReadLine();
            if(CheckInput(in_inf, "double")){ vetor2[i] = float.Parse(in_inf); }
            else { Console.WriteLine("Valor invalido."); i--; }
    }

    Console.WriteLine("\nVetor resultante");
    int j = 0; 
    for(int i =0; i<10; i++){       
        vetor3[j] = vetor1[i]; j++;
        vetor3[j] = vetor2[i]; j++;
    }
    for(int i =0; i<20; i++){       
        Console.WriteLine("{0}° elemento: {1}", i + 1, vetor3[i]);
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
