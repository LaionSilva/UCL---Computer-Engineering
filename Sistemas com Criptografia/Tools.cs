using System;
using System.Collections.Generic;

namespace logistica {
  public static class Tools{
    public static void titulo(){
      Clear();
      Console.WriteLine(
        "\n=== 0 == 1 == 0 ====  A N Ô N I M O U S   L . F  ==== 1 == 0 == 1 ===" +
        "\n=== 1 == 0 == 1 ===   D I S T R I B U I D O R A   === 0 == 1 == 0 ===" +
        "\nCode by: Laion Fernandes - Engenharia de Computação"
      );
    }

    
    public static void fimProcesso(){
      Clear();
      Console.WriteLine(
        "\n=== 0 == 1 == 0 ====  A N Ô N I M O U S   H . L  ==== 1 == 0 == 1 ===" +
        "\n=== 1 == 0 == 1 ====   D E S C O N E C T A D O   ==== 0 == 1 == 0 ==="
      );
    }

    
    public static void fachada(){
      Clear();
      Console.WriteLine(
        "                    #                                                        \n" +
        "                  ## ##                                                      \n" +
        "  #####  ##   ##  #####  ##   ## ##    ## ###     ###  #####  ##   ## ###### \n" + 
        " ##   ## ###  ## ##   ## ###  ##  ##  ##  ## ## ## ## ##   ## ##   ## ##     \n" + 
        " ####### ## # ## ##   ## ## # ##   ####   ##  ###  ## ##   ## ##   ## ###### \n" + 
        " ##   ## ##  ### ##   ## ##  ###    ##    ##   #   ## ##   ## ##   ##     ## \n" + 
        " ##   ## ##   ##  #####  ##   ##    ##    ##       ##  #####   #####  ###### \n"
      );
      Pause(3000);
      Tools.Clear();
    }


    public static void Clear(){
      Console.Clear();
    }
    

    public static void Pause(int time = -1){
      if(time >= 0)
        System.Threading.Thread.Sleep(time);
      else
        Console.ReadKey(); 
    }


  }
  

  public class Users{
    public string codMatriz = "";
    public List<string> usuarios = new  List<string>();
    public List<string> senhas = new  List<string>();

    public void Reset(){
      usuarios.Clear();
      senhas.Clear();
      codMatriz = "";
    }
  }
}