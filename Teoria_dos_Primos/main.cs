//Code by: Laion Fernandes - Computer Engineering Student UCL

using System;

class MainClass {
  public static bool[] keys = new bool[5] {true, true, false, false, true};
  public static int[]  time = new int[2] {0, 0}; 

  public static void Main () {
    int limite = 0, memory, combTrue = 0;

    Console.WriteLine("\nTESTANDO A TEORIA: Todo número par pode ser adquirido pela soma de outros\ndois números primos quaisquer.\n");
    
    limite = Iniciar();  // Controle de funções
    if((limite > 1) && (keys[1])) {
        memory = Memory(limite);
        int[] primos = new int[memory];  // Vetor principal semi-dinmico com memória otimizada

        Time(0);  // Temporizador - Inicio
          primos = Primos(memory, limite);  // Gerar números primos
          if(keys[4]) { combTrue = ProcessarPrimos(primos, limite, primos[memory - 1]); }  // Processando vetor de primos
        Time(1);  // Temporizador - Fim
        Relatorio(limite, combTrue, primos[memory - 1]);  // Imprimir relatório

    } else if(keys[1]) { Console.WriteLine("\nNenhum número primo encontrado."); }
    Console.ReadKey(); 
  }

  //Método - Relatório
  private static void Relatorio (int limite, int combTrue, int idEnd) {
    Console.Write(keys[3]?"\nFalha encontrada":"\nSucesso");
    Console.WriteLine(keys[2]? "Intervalo: 0 a {0}": "", limite);
    Console.Write("Primos encontrados: {0}", idEnd);
    Console.WriteLine(keys[4]? "\nCombinações possiveis: {0}": "", combTrue);
    Console.WriteLine("Tempo de processamento: " + (time[1] > 0? time[0] + "m " : "") + "{0}s", time[0] );
  }

  //Método - Controle de funções
  public static int Iniciar () {
    string inInf;
    int inInt = 0; 

    Console.Write("Informe um inteiro par ou digite 'ate' para efetuar uma varredura: ");
    inInf = Console.ReadLine();
    if(inInf.Length == 0) 
        { inInf = " "; }
    if((inInf == "to") || (inInf == "ate") || (inInf == "até") || (inInf == "lenght") || (inInf == "primos") || (inInf == "np")) {
        if((inInf == "lenght") || (inInf == "primos") || (inInf == "np")) 
            { keys[4] = false; }
        Console.Write("Informe range: ");
        inInf = Console.ReadLine();
        keys[2] = true;
    }
    else if((inInf == "exit") || (inInf == "sair") || (inInf == "s")) 
        { keys[1] = false; return 0; }
    try { 
        inInt = int.Parse(inInf); 
        if(inInt % 2 == 1) 
            { Console.WriteLine("Valores ímpares não são permitidos.\n"); return Iniciar(); }
        return inInt;
    } catch {
        Console.WriteLine("Valor invalido."); 
        return Iniciar();
    }
  }

  //Método - Gerador de números primos
  private static int[] Primos (int memory, int limite) {
    int[] primos = new int[memory]; 
    int idEnd = 0;

    Console.Write("\nProcurando primos...");
    primos[0] = 2;  
    for(int i = 3; i <= limite; i += 2) {
        bool check = true;
        if((i % 10 != 5)||(i == 5)) {
            for(int j = 0; j < idEnd; j++) {
                if(i % primos[j] == 0) 
                    { check = false; break; }
                if(primos[j] > Math.Sqrt(i))  // OTM
                    { break; } 
            }
            if(check) { primos[idEnd] = i; idEnd++; }
        }
        //Controle de tempo e fluxo - Inicio >>>
        if(i % 30 == 0) { System.Threading.Thread.Sleep(1); }   // OTM
        if((System.DateTime.Now.Second == 0) && keys[0]) { time[1]++; keys[0] = false; }
        else if(System.DateTime.Now.Second != 0) { keys[0] = true; }
        //Controle do delay do fluxo - Fim <<<
    } 
    primos[memory - 1] = idEnd;
    return primos;
  }

  ////Método - Processamento do vetor de primos
  private static int ProcessarPrimos (int[] primos, int limite, int idEnd) {
    int listaPrimos = idEnd, combTrue = 0, combTrueSaved = 0, kSaved = 0;

    Console.Write(keys[4]? "\nProcurando combinações...\n": "\nProcurando combinações...");
    if(keys[2]) {  // Por varredura
        for(int k = 4; k <= limite; k += 2) {
            combTrueSaved = combTrue;
            if((k == 4) && keys[4]) 
                { Console.WriteLine("2 + 2 = 4"); combTrue++; }
            if(k > 4) {
                for(int i = 0; i <= listaPrimos; i++) {
                    for(int j = 0; ((j <= listaPrimos) && (primos[i] + primos[j] <= k)); j++) {  // OTM
                        if((primos[i] + primos[j] == k) && (kSaved != k)) {
                            if(keys[4]) 
                                { Console.WriteLine("{0} + {1} = {2}", primos[i], primos[j], k); }
                            combTrue++; 
                            kSaved = k;
                            break;
                        }
                    }
                }
                if(combTrueSaved == combTrue) { keys[3] = true; k = limite; }
            }
        }
    }
    else{  // Valor único - limite = objetivo
        if((limite <= 4) && (limite != 2))
            { Console.WriteLine("2 + 2 = 4"); return ++combTrue; }
        for(int i = 0; i <= listaPrimos; i++){
            for(int j = listaPrimos; j >= 0 ; j--){
                if(primos[i] + primos[j] == limite) { 
                    if(keys[4]) 
                        { Console.WriteLine("{0} + {1} = {2}", primos[i], primos[j], limite); }
                    combTrue++; 
                }
                if(primos[i] > limite / 2) {break;}
            }
        }
        if(combTrue == 0) { keys[3] = true; }
    }
    return combTrue;
  }

  //Método - Temporizador
  private static void Time (int id) {
    switch (id) {
        case 0: time[0] = System.DateTime.Now.Second; break;
        case 1: 
            time[0] = System.DateTime.Now.Second - time[0];
            if(time[0] > 60) { time[1]++; time[0] -= 60; }
            else if (time[0] < 0) { time[1]--; time[0] += 60;}
            break;
    }
  } 

  //Método - Otimizar memória de vetor princiapl
  private static int Memory (int limite) {  // OTM
    if(limite <= 1000)       { return limite / 2;  }
    else if(limite <= 10000) { return limite / 5;  }
    else if(limite <= 50000) { return limite / 8;  }
    else                     { return limite / 10; }
  }
} 
