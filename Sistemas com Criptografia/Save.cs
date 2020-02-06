using System;
using System.Collections.Generic;
using System.IO;

namespace logistica {

  public class Save{
    string fileUsers = "usuarios.txt;";

    public Save(string matrizInicial = ""){
      CheckArquivo(matrizInicial);
    }

    private void CheckArquivo(string inicio = "") { //  Verifica a existencia de um arquivo.txt, caso não exista é criado um
      bool usersVazio = false;

      System.Threading.Thread.Sleep(5);
      if (!System.IO.File.Exists(fileUsers)){
        using (StreamWriter Salvar = File.AppendText(fileUsers)) { 
          if(inicio.Length > 0) { Salvar.WriteLine(inicio); }
        }
      }
      else {
        using(Stream FileIn = File.Open(fileUsers, FileMode.Open)){
          using(StreamReader Carregar = new StreamReader(FileIn)){
            if(FileIn.Length == 0) { 
              usersVazio = true; 
            }
          }
        }
        if(usersVazio) { using (StreamWriter Salvar = File.AppendText(fileUsers)) { Salvar.WriteLine(inicio); } }
      }
      System.Threading.Thread.Sleep(5);
    }


    public bool GuardarUsers(string usuario, string senha){
      try{
        using (StreamWriter Salvar = File.AppendText(fileUsers)) { 
          Salvar.WriteLine(usuario);
          Salvar.WriteLine(senha);
        }
        return true;
      } catch {}
      return false;
    }


    public Users CarregarUsers(){
      Users carga = new Users();

      using(Stream FileIn = File.Open(fileUsers, FileMode.Open)){
        using(StreamReader Carregar = new StreamReader(FileIn)){
          carga.codMatriz = Carregar.ReadLine();
          while(true){
            string usuario = Carregar.ReadLine();
            try{ 
              if(usuario.Length > 0) { carga.usuarios.Add(usuario); } 
            } catch { break; } 
            string senha = Carregar.ReadLine();
            if(senha.Length > 0) { carga.senhas.Add(senha); }
          }
        }
      }
      return carga;
    }
    

    public void ResetUsers(string chaveInicial = ""){
      System.IO.File.Delete(fileUsers);
      CheckArquivo(chaveInicial);
    }

  }
}