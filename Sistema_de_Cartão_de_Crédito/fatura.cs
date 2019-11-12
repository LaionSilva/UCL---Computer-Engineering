using System;
using System.Collections.Generic;
using System.IO;

namespace cartao { 
  class Fatura {
    private string usuario;
    private int mes;
    private double totalDivida;
    private bool status;
    private List<double> compras;

    public Fatura(string n, int m){
      usuario = n;
      mes = m;
      totalDivida = 0;
      status = true;
      compras = new List<double>();
      VerificarFatura();
    }

    public bool NovaCompra(double valor) {
      try {
        if (status == true) {
          OutArquivo("saida.txt", String.Format("{0:0.00}", valor));
          return true;
        }
      } 
      catch {}
      return false;
    }

    public bool Pagar(double valor, int mes) { 
      if(valor >= CarregarFatura(mes, false)){
        int nextMes;
        if(getMes() == 11){ nextMes = 0; }
        else{ nextMes = getMes() + 1; }
        OutArquivo("saida.txt", String.Format("==--=="));
        OutArquivo("saida.txt", String.Format("==//=="));
        OutArquivoInt("saida.txt", nextMes);
        OutArquivo("saida.txt", getUsuario());
        Zerar();
        return true;
      } return false;
    }

    public void Fechar(){
      status = false;
    }

    public void Zerar(){
      if(getMes() == 11) { setMes(0); }
      else { setMes(getMes() + 1); }
      setTotalDivida(0);
      setStatus(true);
      compras.Clear();
    }

    public double CarregarFatura(int localMes, bool ler = true){
      string suporte;
      int contMes = 0;
      double total = 0.00;

      using(Stream FileIn = File.Open("saida.txt", FileMode.Open)){
        using(StreamReader Carregar = new StreamReader(FileIn)){
          try{
            do{ suporte = Carregar.ReadLine();
              if(suporte == "==//=="){ 
                contMes = int.Parse(Carregar.ReadLine());
                if(contMes == localMes) { 
                  suporte = Carregar.ReadLine();
                  suporte = Carregar.ReadLine();
                  while(suporte != "==--=="){
                    total += double.Parse(suporte);
                    if(ler){ Console.WriteLine("R$:{0}", String.Format("{0:0.00}", suporte)); }
                    suporte = Carregar.ReadLine();
                  } 
                } 
              }
            } while(suporte.Length > 0);
          } catch {}
        }
      }
      return total;
    }

    public void VerificarFatura(){
      bool existe = false, fechado = false;
      int ultMes = 0;
      string suporte, ultUsuario = "";
      using(Stream FileIn = File.Open("saida.txt", FileMode.Open)){
        using(StreamReader Carregar = new StreamReader(FileIn)){
          try{
            do{
              suporte = Carregar.ReadLine();
              if(suporte == "==//=="){
                existe = true;
                fechado = false;
                ultMes = int.Parse(Carregar.ReadLine()); 
                ultUsuario = Carregar.ReadLine();
              }
              if(suporte == "==--==")
                { fechado = true; }
            } while (suporte.Length > 0);
          } catch {}
        }
      }
      if(!existe){
        OutArquivo("saida.txt", String.Format("==//=="));
        OutArquivo("saida.txt", String.Format("0", mes));
        OutArquivo("saida.txt", getUsuario());
      }
      else{
        if(fechado){
          OutArquivo("saida.txt", String.Format("==//=="));
          OutArquivo("saida.txt", String.Format("0", ultMes==11?0:mes++));
          OutArquivo("saida.txt", getUsuario());
          if(ultMes == 11){ setMes(0); }
          else{ setMes(ultMes + 1); }
        }
        else{
          setMes(ultMes);
        }
      }
      setUsuario(ultUsuario);
      Console.WriteLine(ultMes);
    }

    private void OutArquivo(string file, string dado){
      using (StreamWriter Salvar = File.AppendText(file)) 
        {  Salvar.WriteLine(dado); }
    }
    private void OutArquivoInt(string file, int dado){
      using (StreamWriter Salvar = File.AppendText(file)) 
        {  Salvar.WriteLine(dado); }
    }

    public int getNextMes() { 
      if(getMes() == 11){ return 0; }
      return getMes() + 1; 
    }

    public List<double> getCompras(){ return compras;  }
    public int getMes() { return mes; }
    public bool getStatus() { return status; }
    public double getTotalDivida() { return totalDivida; }
    public string getUsuario() { return usuario; }

    private void setMes(int m) { mes = m; }
    private void setStatus(bool s) { status = s; }
    private void setTotalDivida(double d) { totalDivida = d; }
    private void setUsuario(string u) { usuario = u; }

  }
}