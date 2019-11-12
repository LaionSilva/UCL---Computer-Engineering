using System;
using System.Collections.Generic;

namespace tamagushi {
  class MainClass {
    public static void Main (string[] args) { 
      GameTamagushi gt = new GameTamagushi();  
      bool boot = true; 

      while(boot) {
        gt.ActivateClock(0);
        gt.Header();

        try{
          gt.action = int.Parse(gt.inSuport); 
          switch (gt.action) {
            case 0: boot = false; break;
            case 1: gt.TakeCare(); break;
            case 2: gt.ViewAllTamagushi(); break;
            case 3: gt.CreatNewTamagushi(); break;
            case 4: gt.FeedAllTamagushi(); break;                 
          }

          while((gt.seePet) && (gt.nPet >= 0) && (gt.nPet < gt.game.farm.Count)) {
            do { 
              gt.ActivateClock(2);
                gt.seePet = gt.ManageStocks();
              gt.ActivateClock(1);
            } while((!gt.game.farm[gt.nPet].getDied()) && (gt.seePet)); 

            if(gt.game.farm[gt.nPet].getDied()) 
              { Console.WriteLine("Game Over! Your tamagushi is died."); }
          }
        } catch { Console.WriteLine("Invalid action."); }

        gt.ActivateClock(1);

      } Console.WriteLine("\nFinished.");
    }
  }
}