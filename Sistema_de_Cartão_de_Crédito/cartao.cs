using System;
using System.Collections.Generic;

namespace cartao { 
  class Cartao {
    public Fatura fatura;
    private string usuario;
    private double limite;
    

    public Cartao(string n, int m, double l) {
      usuario = n;
      fatura = new Fatura(n, m);
      limite = l;
      usuario = n;
    }

    //TODO: definir condições especificas para fechamento de fatura
    public void Comprar(double valor){
      if(fatura.NovaCompra(valor) && (valor + fatura.getTotalDivida() <= limite)) { 
        Console.WriteLine("Compra bem sucedida"); 
      }  
      else { 
        if(fatura.getStatus()){
          Console.WriteLine("Compra no valor de {0} rejeitada. Limite disponivel R$:{1}", 
          valor, limite - fatura.getTotalDivida()); 
        } 
        else 
          { Console.WriteLine("Fatura fechada"); }
      }
    }
    
    public void verFatura(int mes){
      string status; //  pendente ou fechada
      if(fatura.getMes() >= mes){
        if(mes == fatura.getMes()) 
          { status = "pendente"; }
        else { status = "fechada"; }
        Console.WriteLine("\nFatura {0} do mes {1} - Usuário: {2}", status, mes + 1, usuario);

        /*foreach(double c in fatura.getCompras()) {
          Console.WriteLine( "R$:{0}", String.Format("{0:0.00}", c) );
        }*/
        double total = fatura.CarregarFatura(mes, true);
        Console.WriteLine( "Valor total a pagar R$:{0}", String.Format("{0:0.00}", total) );
      } else { Console.WriteLine("Fatura não encontrada"); }
    }

    public void Pagar(int mes, double valor){
      if(fatura.getMes() == mes){
        if(fatura.Pagar(valor, mes)){
          Console.WriteLine("Fatura paga");
          if(mes == 11){ mes = 0; }
          else{ mes += 1; }
        } else { Console.WriteLine("Pagamento não aceito"); }
      } else { Console.WriteLine("Pagamento não aceito"); }
    }  

  }
}