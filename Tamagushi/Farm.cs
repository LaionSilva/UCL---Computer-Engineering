using System;
using System.Collections.Generic;

namespace tamagushi {
  public class Farm {
    public List<Tamagushi> farm = new List<Tamagushi>();
    private int[] barn = new int[6] {0,0,0,0,0,0};
    protected int money = 10;
    protected int moneyLife = 0;
    protected int allTimeFarm = 0;
    protected float coefMoney = 50.00f;

    public Farm (){
      Random rand = new Random();
      for(int i = 0; i < 6; i++)
        { barn[i] = rand.Next(0, 20 - (3 * i)); }
    }

    public void Money(int m, bool addMoney = false){
      if(addMoney) { money += m; }
      else { money -= m; }
    }

    public void FarmPlay(int game, int nPet, int time){
      farm[nPet].Play(game, time);
      Money((int)((farm[nPet].PointGame(game) * 10) - ((farm[nPet].PointGame(game) * 10) % 1)) * time * farm[nPet].getCoefPlay() / 60000, true);
      Console.WriteLine("Play money: {0}", (int)Math.Ceiling(farm[nPet].PointGame(game)) * time * farm[nPet].getCoefPlay() / 6000);
    }

    public void calcMoneyLife() { moneyLife = (int)(allTimeFarm / 360); }
    public void setAllTimeFarm(int t) { allTimeFarm += t; }

    public int getMoney() { 
      return money + moneyLife;
    }

    public void PrintBarn(){
      string[,] data = new string[2,6];
        data = getAllBarn(); 
      for(int i = 0; i < barn.Length; i++)
        { Console.Write ("{1}({0}) | ", data[0, i], data[1, i]); }
    }

    public void UserBarn (int idFood, int nFood){
      barn[idFood] -= nFood;
      for(int i = 0; i < barn.Length; i++){
        Console.WriteLine(" aqui2: {0} {1}", i, barn[i]); 
      }      
    }  

    public int getBarn(int id) { return barn[id]; }
    
    public string[,] getAllBarn() { 
      string[,] b = new string[barn.Length, barn.Length];
      string[] f = new string[barn.Length];
        f = getFood();
      for(int i = 0; i < barn.Length; i++) {
        b[0, i] = String.Format("{0}", barn[i]); 
        b[1, i] = String.Format("{0}", f[i]); 
      }
      return b; 
    }

    public void BuyFood(int money, int quantity) {
      string inSuport;
      int nFood, cont = 0;
      int[] costs = new int[8];

      Console.Write("\n===Shopping Food===\n\n");
      try{
        Console.Write ("Foods: ");
        foreach(string f in getFood()) 
          { Console.Write ("{0} | ", f); }
        Console.Write("\n\nFood: ");
          inSuport = Console.ReadLine();
        Console.Write("Quantity: ");
          nFood = int.Parse(Console.ReadLine());
        costs = costFood();
        foreach(string f in getFood()) {
          if((f == inSuport) && (nFood * costs[cont] <= getMoney())) {
            barn[cont] = nFood;
            Money(nFood * costs[cont]);
          }
          cont++;
        }
      } catch { Console.Write (" Error when buying "); }
    }

    private int[] costFood(){ 
      int[] cost = new int[6];
      cost[0] = 1; 
      cost[1] = 2; 
      cost[2] = 3; 
      cost[3] = 5; 
      cost[4] = 7; 
      cost[5] = 10; 
      return cost;
    } 

    public string[] getFood(){ 
      string[] foods = new string[6];
      foods[0] = "cherry"; 
      foods[1] = "grape"; 
      foods[2] = "strawberry"; 
      foods[3] = "banana"; 
      foods[4] = "apple"; 
      foods[5] = "watermelon"; 
      return foods;
    } 

  }
}