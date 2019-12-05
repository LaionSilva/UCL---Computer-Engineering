using System;

class MainClass {
  public static void Main () {
    bool fim = true;

    do {
        Console.WriteLine("\nProgramas disponiveis:\n1 - {1}\n2 - {2}\n0 - {0}", "Sair", "MaxBetweenThree", "DigCheckMatricula");
        Console.Write("\nCod: ");

        string inf = Console.ReadLine();
        for(int i = 0; i<inf.Length; i++) {
            if((inf[i] < 48) || (inf[i] > 50)) 
                { Console.WriteLine("Programa não encontrado."); Exit(); }
        }
        switch (inf) {
            case "0": fim = false;          Exit(0);  break;
            case "1": MaxBetweenThree();    Exit(1);  break;
            case "2": DigCheckMatricula();  Exit(1);  break;
        }
    } while (fim);
  }

  ///////////////////////////////////////////////////////////////////////////
  //1 - Entre três valores informar o maior
  private static void MaxBetweenThree() {
    double[] entrada = new double[3];
    double maior = 0;
    string in_inf;
    bool error = false;
    bool[] k = new bool[3];

    Console.WriteLine("Informe três valores:");
    for(int i = 0; i<3; i++) {
        Console.Write("{0}º valor: ", i+1);
        in_inf = Console.ReadLine();
        error = k[i] = !CheckInput( in_inf, "double" );       
        if( !error ) {
            entrada[i] = double.Parse(in_inf);
            if((entrada[i] > maior) || (i == 0)) 
                { maior = entrada[i]; }
        }
    }
    if( !k[0] && !k[1] && !k[2] ) 
        { Console.WriteLine("{3:f2} é o maior valor entre: {0:f2}, {1:f2} e {2:f2}", entrada[0], entrada[1], entrada[2], maior); }
    else { 
        Console.Write("\n");
        if(k[0]) { Console.Write("1º{0}", (k[1] && k[2])? ", " : ( ((!k[1] && k[2]) || (k[1] && !k[2]))? " e " : " " )); }
        if(k[1]) { Console.Write("2º{0}", k[2]? " e " : " "); }
        if(k[2]) { Console.Write("3º "); }
        if(k[0] || k[1] || k[2]) { Console.WriteLine("Valor invalido!"); } 
    }
  }

  ///////////////////////////////////////////////////////////////////////////
  //2 - Gerar e informar dígito de verificação para um código de matrícula
  private static void DigCheckMatricula() {
    string in_inf;
    int [] matricula = new int[8];
    int digCheck = 0;
    bool okCheck = true;
    
    //Recolher e validar código de matricula
    Console.Write("Matrícula: ");
    in_inf = Console.ReadLine();
    if(in_inf.Length == 8) {
        if (CheckInput(in_inf, "int")) { 
            for(int i = 0; i<8; i++) 
                { matricula[i] = in_inf[i] - 48; } 
        }
        else { 
            Console.WriteLine("Dados invalidos."); 
            okCheck = false;
        }for(int i = 0; i<8; i++) { 
            if((in_inf[i] >= 48) && (in_inf[i] <= 57)) 
                { matricula[i] = in_inf[i] - 48; } 
            else { 
                Console.WriteLine("Dados invalidos."); 
                okCheck = false;
            }
        }        
    }
    else { 
        Console.WriteLine("Dados invalidos."); 
        okCheck = false;
    }

    //Gerar dígito de verificação.
    if (okCheck) {
        digCheck += (matricula[0] + matricula[4]) * 2 + (matricula[1] + matricula[3]) * 3 + matricula[2] * 4;
        digCheck += matricula[5] + matricula[6] + matricula[7];
        digCheck = digCheck % 10;

        //Imprimir a matricula com o dígito de verificação
        Console.WriteLine("Sua matrícula agora é: {0}-{1}", in_inf, digCheck);
    }
  }

  ///////////////////////////////////////////////////////////////////////////
  //Finalizar Processos
  private static void Exit(int cod = 0) {
    if(cod == 0) { 
        Console.WriteLine("\nPrograma finalizado."); 
        Console.ReadKey(); 
        Environment.Exit(0); 
    }
    if(cod == 1) { 
        Console.WriteLine("\nOperação finalizado."); 
        Console.ReadKey(); 
    }
  }

  public static bool CheckInput ( string input, string type = "int" ) {
    bool check = true;

    switch (type) {
        case "int":
            for(int i = 0; i < input.Length; i++) { 
                if((input[i] < 48) || (input[i] > 57)) 
                    { return false; }
            } break;
        case "double":
            for(int i = 0; i < input.Length; i++) { 
                if ( !(((input[i] >= 48) && (input[i] <= 57)) || (input[i] == 44) || (input[i] == 46)) ) 
                    { return false; }
            } break;
    }
    return check;
  }
}