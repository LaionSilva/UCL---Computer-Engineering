using System;

class MainClass {
  public static void Main () {
    int inInt = -1, melhor = 0;
    int[] votacao = new int[24];
    float percentual = 0.2f;

    Console.WriteLine ("VOTAÇÃO\n");

    do {
        Console.Write("Jogador: ");
        try {
            inInt = int.Parse(Console.ReadLine());
            if((inInt > 0) && (inInt <= 23)) {
                votacao[inInt - 1] ++;
                votacao[23]++;
                Console.WriteLine ("Voto computado: Jogador {0}\n", inInt);
            }
            else if(inInt != 0) { Console.WriteLine ("Jogador {0} não reconhecido!\n", inInt); }
        } 
        catch { Console.WriteLine (inInt < 0? "Jogador não reconhecido!\n":  "Jogador" + inInt + " não reconhecido!\n"); }
    } while (inInt != 0);

    Console.Write("\nResultado:\n ");
    for(int i = 0; i < 23; i++) {
        percentual = ((float)votacao[i] / votacao[23] * 100);
        if(votacao[i] > votacao[melhor]) { melhor = i; }
        Console.WriteLine("Jogador {0} ganhou {1} votos. {2:f}%", i, votacao[i], percentual);
    }
    percentual = ((float)votacao[melhor] / votacao[23] * 100);
    Console.WriteLine("\nO melhor jogador é o camisa {0} com {1} votos ({2:f}%).", melhor, votacao[melhor], percentual);

    Console.ReadKey(); 
  } 
}