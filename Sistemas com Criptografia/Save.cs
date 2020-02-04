using System;
using System.Collections.Generic;
using System.IO;

namespace logistica {

  public class Save{
    string fileUsers = "usuarios.txt;";

    public void CheckArquivo() { //  Verifica a existencia de um arquivo.txt, caso não exista é criado um
      System.Threading.Thread.Sleep(5);
      if (!System.IO.File.Exists(fileUsers)){ 
        using (StreamWriter Salvar = File.AppendText(fileUsers)) {}
      }
      System.Threading.Thread.Sleep(5);
    }


    public void GuardarUsers(string codMatriz = "", string usuario = "", string senha = ""){
      try {
        using (StreamWriter Salvar = File.AppendText(fileUsers)) { 
          Salvar.WriteLine(codMatriz);
          Salvar.WriteLine(usuario); 
          Salvar.WriteLine(senha); 
        }
      }
      catch (Exception) {}
    }


    public string[] CarregarUsers(){
      string codMatriz = "", usuario = "", senha = "";

      using(Stream FileIn = File.Open(fileUsers, FileMode.Open)){
        using(StreamReader Carregar = new StreamReader(FileIn)){
          codMatriz = Carregar.ReadLine();
          usuario = Carregar.ReadLine(); 
          senha = Carregar.ReadLine(); 
        }
      }

      string[] dados = new string[3];
      dados[0] = codMatriz;
      dados[1] = usuario;
      dados[2] = senha;

      return dados;
    }
    

    public void ResetUsers(){
      System.IO.File.Delete(fileUsers);
      CheckArquivo();
    }

  }
}