using System;
using System.Collections.Generic;

namespace tamagushi {
  public class GameTamagushi {
    public Farm game = new Farm();
    public int nPet = 0;
    public int intValue = 0;
    public string inSuport = " ";   
    public int action = 0;  
    public bool seePet = false;
    public int[] dtAc = new int[3];
    public bool programmerKey = false;

    public void Header(){
      Console.WriteLine("\n===== Tamagushi tamagushi.farm =====\nMoney: {0}\n", game.getMoney());
      Console.Write("We tamagushi farm have {0} tamagushi at this moment", game.farm.Count);
      if(game.farm.Count > 0) {
        int i = 0;  
        Console.Write(":\n");    
        foreach(Tamagushi t in game.farm) { 
          Console.Write(i + " - " + t.getName()); 
          Console.Write((i + 1) % 5 != 0? " | ": "");
          if((i + 1) % 5 == 0) { Break(); }
          i++;
        }
      } else { Console.WriteLine("."); }

      Console.WriteLine("\n1 - Take care of a tamagushi | 2 - View all tamagushis | 3 - Creat new tamagushis | 4 - Feed all the tamagushis | 0 - Desligar");
      Console.Write("What are we do now?\naction: ");
        inSuport = Console.ReadLine();
    }

    //  Action Management
    public bool ManageStocks(bool menu = true){
      if(menu) { ManageStockMenu(); }
      try {
        action = int.Parse(inSuport);
        switch (action) {
          case 0: return false;
          case 1: int[] n = new int[1];
                  n[0] = nPet;
                  FeedTamagushi(n); 
                  break;
          case 2: PlayGame(); break;
          case 1001:  ActivateClock(0);
                      game.farm[nPet].PrintReport(); 
                      programmerKey = true;
                      break;
          default: Console.WriteLine("Invalid action."); break;
        }
      } catch { Console.WriteLine("Invalid action."); }   
      return true;
    }

     //  Action menu options
    public void ManageStockMenu(){
      object[,] report = new object[2,9]; 
        report = game.farm[nPet].Report();
      Break();
      if(!programmerKey){        
        Console.WriteLine("Data of {0}:", report[1,0]);
        game.farm[nPet].PrintDataTamagushi();

        Break();
        Console.WriteLine("1 - To feed | 2 - Play a game | 0 - Back to the tamagushi.farm");
      } else { programmerKey = false; }
      Console.Write("What are we do now?\naction: ");
        inSuport = Console.ReadLine();
    }

    //  Actions: 0 - Food | 1 - Play games
    public int[] Action(int idAction){
      string inFirst = " ", inSecond = " ";
      int[] results = new int[3] {0, 0, 0};
      string[] data = new string[6];
        if(idAction == 0) { data = game.getFood(); }
        else { data = game.farm[nPet].getGame(); }

      Console.Write(idAction == 0? "\nFood options: ": "\nGame options: ");     
      if(idAction == 0) { game.PrintBarn(); }
      else { 
        for(int i = 0; i < data.Length; i++)
          { Console.Write ("{0} | ", data[i]); }
      }
      
      Break();
      Console.Write(idAction == 0? "\nFood: ": "\nGame: ");
        inFirst = Console.ReadLine();
      Console.Write(idAction == 0? "Quantity: ": "Time(s): ");
        inSecond = Console.ReadLine();

      for(int i = 0; i < data.Length; i++) {
        if(inFirst == data[i]){
          try {
            int q = int.Parse(inSecond);           
            results[0] = 1; //  signal for possible action
            results[1] = i; //  type food
            results[2] = q; //  quantily of food
          } catch {}
          i = data.Length;
        }
      }
      return results;
    }

    public void TakeCare(){
      Console.Write("Enter id of tamagushi: ");
      nPet = int.Parse(Console.ReadLine());
      ActivateClock(0);
      if((nPet >= 0) && (nPet < game.farm.Count)) { 
        seePet = true;
      } else { Console.WriteLine("Invalid id."); }
    }

    public void ViewAllTamagushi(){
      for(int i = 0; i < game.farm.Count; i++) { 
        ActivateClock(0);
        Break();
        Console.WriteLine("{0}º tamagushi:", i + 1);
        game.farm[i].PrintDataTamagushi(); 
        Break();
      }
    }

    public void CreatNewTamagushi(){
      Console.Write("Enter name of new tamagushi: ");
        inSuport = Console.ReadLine();
      game.farm.Add(new Tamagushi(inSuport, 0, 0));
      Console.WriteLine("Tamagushi created.");
    }

    public void FeedTamagushi(int[] ids){
      if(game.farm.Count > 0) {
        dtAc = Action(0);
        if(dtAc[0] == 1) {
          if(game.getBarn(dtAc[1]) >= dtAc[2]) {
            for(int i = 0; i < ids.Length; i++) {
              game.farm[ids[i]].Feed(dtAc[1], dtAc[2]); 
              game.UserBarn(dtAc[1], dtAc[2]);
              Console.WriteLine("aqui: id {0} | qHave {1} | qDown {2}",dtAc[1], game.getBarn(dtAc[1]), dtAc[2]);//////
            }
            Console.WriteLine("Tamagushis fed.");
          } else { Console.WriteLine("There is not enough food."); }
        } else { Console.WriteLine("Food not performed."); }
      } else { Console.WriteLine("There is none tamagushi."); }
    }

    public void FeedAllTamagushi(){
      int[] ids = new int [game.farm.Count];
      for(int i = 0; i < game.farm.Count; i++) 
        { ids[i] = i; }
      FeedTamagushi(ids);
    }

    public void PlayGame(){
      dtAc = Action(1);
      if(dtAc[0] == 1) 
        { game.FarmPlay(dtAc[1], nPet, dtAc[2]); }
      else 
        { Console.WriteLine("Error! Game not played."); }
    }

    public void ActivateClock(int sense = 0) {
      for(int i = 0; i < game.farm.Count; i++) {
        if(sense == 0) { game.farm[i].Clock(2); game.farm[i].Clock(1); }
        else if(sense == 1) { game.farm[i].Clock(0); }
        else if(sense == 2) { game.farm[i].Clock(1); }
      }
    }

    public void Break() { Console.WriteLine(); }
  }
}