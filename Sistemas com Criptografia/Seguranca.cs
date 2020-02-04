using System;
using System.Collections.Generic;

namespace logistica {
  public class Seguranca {
    private static Save save = new Save();

    private string LerSenha(string usuario = ""){
      string senha = "";

      Tools.titulo();
      Console.WriteLine("\nInforme suas credenciais:\n" + "Usuário: " + usuario);
      Console.Write("Senha:");
      while(true){
        string mascara = "";        
        char c = Console.ReadKey().KeyChar;

        if(c == '\n'){
          break;
        }
        else {
          senha += c;
        }
        for(int i = 0; i < senha.Length; i++){
          mascara += "*";
        }
        
        Tools.Clear();
        Tools.titulo();
        Console.WriteLine("\nInforme suas credenciais:\n" + "Usuário: " + usuario);
        Console.Write("Senha: ");
        Console.Write(mascara);
      }
      return senha;
    }


    public bool Login() {
      RenovarCriptografia();
      Console.WriteLine("\nInforme suas credenciais:\n");      
      Console.Write("Usuário: ");
        string usuario = Console.ReadLine();
      Tools.Clear();
        string senha = LerSenha(usuario);
      //GuardarUsers(usuario, senha);
      bool userValido = ValidarUsers(usuario, senha);
      RenovarCriptografia();

      return userValido;
    }


    //TODO: Melhorar método de criptografar
    private void GuardarUsers(string usuario = "", string senha = "", string codMatriz = ""){
      if(codMatriz.Length == 0) { codMatriz = GerarCodMatriz(); }
      if(usuario.Length > 0 && senha.Length > 0 )
        save.GuardarUsers(codMatriz, Criptografia(codMatriz, usuario), Criptografia(codMatriz, senha));
      else 
        Console.WriteLine("\nUsuário ou senha inválidos\n"); 
    }


    public bool ValidarUsers(string usuario = "", string senha = ""){
      string[] dados = save.CarregarUsers();
      bool userValido = false;

      if(Criptografia(dados[0], dados[1], true) == usuario && Criptografia(dados[0], dados[2], true) == senha){
        Console.WriteLine("Login efetuado com sucesso"); 
        userValido = true;
      }
      else{
        Console.WriteLine("\nUsuário ou senha inválidos\n");
        Criptografia(dados[0], dados[1]);
      }
      Tools.Pause();
      return userValido;
    }


    public string Criptografia(string codMatriz = "", string chave = "", bool traduzir = false){
      int auxi = 0, contMatriz = 0;
      string segredo = "";
      char[] matriz = new char[codMatriz.Length]; 

      foreach(char c in codMatriz){
        matriz[auxi] = c;
        auxi++;
      }
      auxi = 0;

      if(traduzir){
        foreach(char c in chave){
          auxi = c + 14 * matriz[contMatriz];
          if(auxi > 126) { auxi -= 126; }
          segredo += (char)auxi;
          contMatriz++;
          if (contMatriz > codMatriz.Length - 1) { contMatriz = 0; }
        }
      }
      else {
        foreach(char c in chave){
          if(c < 14 * matriz[contMatriz]) { auxi = c + 126; }
          auxi = auxi - (14 * matriz[contMatriz]);
          segredo += (char)auxi;
          contMatriz++;
          if (contMatriz > codMatriz.Length - 1) { contMatriz = 0; }
        }
      }
      
      return segredo;
    }


    private string GerarCodMatriz(){
      Random codMatriz = new Random();
      return String.Format("{0:00000}", codMatriz.Next(0, 99999)) + String.Format("{0:00000}", codMatriz.Next(0, 99999));
    }


    public void RenovarCriptografia(){
      save.CheckArquivo();
      string[] dados = new string[3];
      dados[0] = ""; dados[1] = ""; dados[2] = ""; 
      try{ 
        dados = save.CarregarUsers(); 
        save.ResetUsers();

        if(dados[0].Length > 0) { dados[1] = Criptografia(dados[0], dados[1], true); }
        if(dados[0].Length > 0) { dados[2] = Criptografia(dados[0], dados[2], true); }
        if(dados[0].Length > 0) { dados[0] = GerarCodMatriz(); }
        GuardarUsers(dados[1], dados[2], dados[0]);
        
      } catch {}
    }

  }
}