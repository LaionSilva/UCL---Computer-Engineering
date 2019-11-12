using System;

namespace cartao {
  class MainClass {
    public static void Main (string[] args) {
      Cartao meuCard = new Cartao("Laion", 0, 1000.00);
      bool run = true;

      while(run){
        Console.WriteLine("1 - Comprar | 2 - Ver fatura | 3 - Pagar | 4 - Sair");
        try{
          int acao = int.Parse(Console.ReadLine());
          switch(acao){
            case 1: Comprar(meuCard); break;
            case 2: VerFatura(meuCard); break;
            case 3: Pagar(meuCard); break;
            case 4: run = false; break;
          }
        } catch { Console.WriteLine("\nAcao invalida"); }
      }

    }

    public static void Comprar(Cartao cartao){
      try{
        Console.Write("\nValor: ");
        cartao.Comprar(double.Parse(Console.ReadLine()));
      } catch { Console.WriteLine("\nFalha ao comprar"); }
    }

    public static void VerFatura(Cartao cartao){
      try{
        Console.Write("\nMes: ");
          int mes = (int.Parse(Console.ReadLine()));
        if((mes > 0)  && (mes <= 12)){
          cartao.verFatura(mes - 1); 
        } else { Console.WriteLine("\nMes invalido"); }
      } catch { Console.WriteLine("\nFatura não encontrada"); }
    }

    public static void Pagar(Cartao cartao){
      try{
        Console.Write("\nMes: ");
          int mes = (int.Parse(Console.ReadLine()));
        Console.Write("Valor: ");
          int valor = (int.Parse(Console.ReadLine()));
        if((mes > 0)  && (mes <= 12) && (valor >= 0)){
          cartao.Pagar(mes - 1, valor);
        } else { Console.WriteLine("\nDados invalidos"); }
      } catch { Console.WriteLine("\nFalha ao pagar"); }
    }

  }

}
