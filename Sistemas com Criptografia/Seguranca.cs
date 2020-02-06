using System;
using System.Collections.Generic;

namespace logistica {
  public class Seguranca {
    private static Save save = new Save(GerarCodMatriz());


    public Seguranca(){
      RenovarCriptografia();
    }


    private string LerSenha(string usuario = "", string msg = ""){
      string senha = "";
      msg = "\n" + msg;

      Tools.titulo();
      Console.WriteLine(msg + "\nUsuário: " + usuario);
      Console.Write("Senha:");

      while(true){
        string mascara = "";        
        char c = Console.ReadKey().KeyChar;

        if(c == '\n'){
          break;
        }
        else if(c == 0){
          senha = senha.Remove(senha.Length - 1);
        }
        else {
          senha += c;
        }
        for(int i = 0; i < senha.Length; i++){
          mascara += "*";
        }
        
        Tools.Clear();
        Tools.titulo();
        Console.WriteLine(msg + "\nUsuário: " + usuario);
        Console.Write("Senha: ");
        Console.Write(mascara);
      }
      return senha;
    }


    public bool Login() {
      Tools.titulo();
      string msg = "LOGIN\n\nInforme suas credenciais:";

      RenovarCriptografia();
      Console.WriteLine("\n" + msg);      
      Console.Write("Usuário: ");
        string usuario = Console.ReadLine();
        string senha = LerSenha(usuario, msg);
      bool userValido = ValidarUsers(usuario, senha);
      RenovarCriptografia();

      return userValido;
    }


    public bool CadastrarUsuario() {
      Tools.titulo();
      string msg = "CADASTRAR NOVO USUÁRIO\n\nInforme suas credenciais para realizar o novo cadastro:";

      RenovarCriptografia();
      Console.WriteLine("\n" + msg);      
      Console.Write("Usuário: ");
        string usuario = Console.ReadLine();
      
      Users users = save.CarregarUsers();
      foreach(string u in users.usuarios){
        if(Criptografia(users.codMatriz, u, true) == usuario){
          Tools.titulo();
          Console.Write("\nUsuário já cadastrado.\n");
          return false;
        }
      }

      string senha = LerSenha(usuario, msg);
      bool userValido = GuardarUsers(usuario, senha);
      RenovarCriptografia();

      return userValido;
    }


    private bool GuardarUsers(string usuario, string senha){
      if(usuario.Length <= 0 || senha.Length <= 0) {
        return false;
      }

      Users dadosUsers = save.CarregarUsers();
      return save.GuardarUsers(
        Criptografia(dadosUsers.codMatriz, usuario), 
        Criptografia(dadosUsers.codMatriz, senha)
      );
    }


    public bool ValidarUsers(string usuario, string senha){
      Users dadosUsers = save.CarregarUsers();
      bool valido = false;

      foreach(string u in dadosUsers.usuarios){
        if(Criptografia(dadosUsers.codMatriz, u, true) == usuario){
          string senhaUser = dadosUsers.senhas[ dadosUsers.usuarios.IndexOf(u) ];
          if(Criptografia(dadosUsers.codMatriz, senhaUser, true) == senha){
            valido = true;
          }
        }
      }

      return valido;
    }


    public string Criptografia(string codMatriz = "", string codigo = "", bool traduzir = false){
      int auxi = 0, contMatriz = 0;
      string segredo = "";
      char[] matriz = new char[codMatriz.Length]; 

      foreach(char c in codMatriz){
        matriz[auxi] = (char)(c % 20);
        auxi++;
      }
      auxi = 0;

      if(traduzir){
        foreach(char c in codigo){
          auxi = c + 170 + (102 - contMatriz * 10) * matriz[contMatriz];
          if(auxi > 1024) { auxi -= 1024; }
          segredo += (char)auxi;
          contMatriz++;
          if (contMatriz > codMatriz.Length - 1) { contMatriz = 0; }
        }
      }
      else {
        foreach(char c in codigo){
          if(c < 102 * matriz[contMatriz]) { auxi = c + 1024; }
          auxi = auxi - 170 - ((102 - contMatriz * 10) * matriz[contMatriz]);
          segredo += (char)auxi;
          contMatriz++;
          if (contMatriz > codMatriz.Length - 1) { contMatriz = 0; }
        }
      }
      
      return segredo;
    }


    private static string GerarCodMatriz(){
      Random codMatriz = new Random();
      string codigo, segredo = "";
      
      codigo = String.Format("{0:00000}", codMatriz.Next(0, 99999)) + String.Format("{0:00000}", codMatriz.Next(0, 99999));
      foreach(char c in codigo){
        segredo = segredo += (char)(c + 20 * (c - 48));
      }
      return segredo;
    }


    public void RenovarCriptografia(){
      Users dados = save.CarregarUsers(); 
      Users aux = new Users();
      Random rand = new Random();
      int i = 0;

      foreach(string u in dados.usuarios){
        aux.usuarios.Add( Criptografia(dados.codMatriz, u, true) );
      }
      foreach(string s in dados.senhas){
        aux.senhas.Add( Criptografia(dados.codMatriz, s, true) );
      }
      save.ResetUsers(GerarCodMatriz());
      
      while(aux.usuarios.Count > 0){
        i = rand.Next(0, aux.usuarios.Count - 1);
        GuardarUsers(aux.usuarios[i], aux.senhas[i]);
        aux.usuarios.Remove( aux.usuarios[i] );
        aux.senhas.Remove( aux.senhas[i] );
      }
    }


  }
}