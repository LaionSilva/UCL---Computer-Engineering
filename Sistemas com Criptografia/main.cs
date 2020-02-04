using System;
using System.Collections.Generic;

namespace logistica {
  public class MainClass {

    private static Seguranca seguranca = new Seguranca();

    public static void Main() {
      //Pause();
      Tools.fachada();   
      Tools.titulo();   

      if(seguranca.Login()){
        Tools.titulo();   
        bool loop = true;

        while(loop) {
          Console.WriteLine("\nO - Secretaria | SAIR - Desconectar");
          Console.Write("\nEscolha o setor desejado... \nSetor: ");
          switch (Console.ReadLine().ToUpper()) { 
            case "A": ; break;
            default: 
              ErroOpcoes();
              break;
          }
        }
      }
      
      Tools.fimProcesso();
    }


    public static void ErroOpcoes() {
      Tools.titulo();
      Console.WriteLine("\nComando inválido!\n"); 
      Console.ReadKey(); 
      Tools.titulo();
    }
  }
}