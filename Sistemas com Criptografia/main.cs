using System;
using System.Collections.Generic;

namespace logistica {
  public class MainClass {

    private static Seguranca seguranca = new Seguranca();

    public static void Main(){
      bool loop = false, loopMain = true;

      //Tools.Pause();
      Tools.fachada();   

      
      for(int i = 0; i < 1500; i++)
        { Console.WriteLine((char)i + " " + i); }
      
      
      while(loopMain){
        Tools.titulo();
        Console.WriteLine("\nL - Login | N - Novo usuário | SAIR - Desconectar");
        Console.Write("\n>> ");

        switch (Console.ReadLine().ToUpper()) {
          case "L": 
            bool statusLogin = seguranca.Login();
            MsgCadastroLogin("login", statusLogin);
            loop = statusLogin;
            break;
          case "N": MsgCadastroLogin("cadastro", seguranca.CadastrarUsuario()); break;
          case "S": loopMain = false; break;
          case "SAIR": loopMain = false; break;
          default: ErroOpcoes(); break;
        }

        while(loop){
          Tools.titulo();
          Console.WriteLine("\nO - Secretaria | SAIR - Desconectar");
          Console.Write("\nEscolha o setor desejado... \nSetor: ");

          switch (Console.ReadLine().ToUpper()) { 
            case "A": ; break;
            case "S": loop = false; break;
            case "SAIR": loop = false; break;
            default: ErroOpcoes(); break;
          }
        }
      }
        
      Tools.fimProcesso();
    }


    public static void ErroOpcoes() {
      Tools.titulo();
      Console.WriteLine("\nComando inválido!\n"); 
      Tools.Pause(); 
      Tools.titulo();
    }


    public static void MsgCadastroLogin(string acao = "", bool sucesso = false){
      if(acao == "cadastro"){
        if(sucesso){
          Console.Write("Cadastro efetuado com sucesso\n");
          Tools.Pause();
        } else { 
          Console.Write("Falha ao cadastrar\n");
          Tools.Pause(); 
        }
      } else if(acao == "login"){
        if(sucesso){
          Console.Write("Login efetuado com sucesso\n");
          Tools.Pause();
        } else { 
          Console.Write("Falha ao Logar\n");
          Tools.Pause(); 
        }
      }
      
    }
    
  }
}