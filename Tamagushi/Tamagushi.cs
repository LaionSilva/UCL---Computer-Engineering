using System;
using System.Collections.Generic;

namespace tamagushi {  // Bichinho eletrônico
  public class Tamagushi {
    protected string name;
    protected float hunger = .00f;
    protected float health = .00f;
    protected int age = 0;
    protected int allTime = 0;
    protected int generalCoef;
    protected float coefHealth = .00f;
    protected float coefHunger = .00f;
    protected int coefAge;
    protected float coefPlay = .00f;
    protected bool died = false;
    protected const float timeToAgeUp = 604800;
    protected float joy = 0.00f;
    protected bool rest = false;
    private int[,] time = new int[2,3];

    public Tamagushi(string na = "bite",  float hu = 0.40f, float ha = 0.60f, int ag = 0, int gc = 50) 
    {
      Random rand = new Random();
      if(hu == 0){ hu = (float)(rand.Next(30, 100) / 100.00f); }
      if(ha == 0){ ha = (float)(rand.Next(30, 100) / 100.00f); }
      name = na;
      hunger = hu;
      health = ha;
      age = ag;
      allTime = age * 3600;
      generalCoef = gc;
      coefHealth = (float)gc;
      coefHunger = (float)gc;
      coefPlay = (float)Math.Ceiling(gc * 100 / Math.Sqrt(gc));
      coefAge = (int)Math.Ceiling(gc + Math.Pow(gc, 2) / 20);
    }
  
    public void AlterName(string n) { setName(n); }

    public object[,] Report(){
      object[,] report = new object[2,9] 
      {{"name", "hunger", "health", "age", "humor", "allTime", "age up", "alive", "rest"}, 
      {name, Math.Round(hunger, 2), Math.Round(health, 2), age, Math.Round(Humor(), 2), allTime, PercentAge(), !died, rest}};
      return report;
    }
    
    public void PrintReport(){ 
      object[,] report = new object[2,9];
        report = Report();
        Console.Write(""); 
      for(int i = 0; i < 9; i++){
        Console.Write("{0}: ", report[0, i]); 
        Console.WriteLine("{0}", report[1, i]); 
      }
    }

    public void PrintDataTamagushi(){
      Console.WriteLine("name: {0}", name); 
      Console.WriteLine("hunger: {0}", Math.Round(hunger, 2) >= 0.7? "full": (Math.Round(hunger, 2) > 0.4? "normal": "hungry")); 
      Console.WriteLine("health: {0}", Math.Round(health, 2) >= 0.7? "healthy": (Math.Round(health, 2) > 0.4? "normal": "bad")); 
      Console.WriteLine("age: {0} years old", age); 
      Console.WriteLine("humor: {0}", Math.Round(Humor(), 2) >= 0.7? "happy": (Math.Round(Humor(), 2) > 0.4? "impartial": "bored"));
      Console.WriteLine("alive: {0}", died? "died": "alive"); 
      Console.WriteLine("rest: {0}", rest? "sleeping": "awake"); 
    }

    public void Feed(int typeFood, int quantity){
      if ((!died) && (!rest)) { 
        hunger += PointFood(typeFood) * quantity * (float)Math.Round( Math.Pow(0.90, age), 2 ); 
      }
    }

    private void Hunger(float value, bool activity = false) {
      float coefDown = 0.20f;
      if (!died) {
        if(!activity) 
          { hunger -= (float)(0.01f * (value / 60) * (coefDown + hunger) ); }
        else 
          { hunger -= (float)(value / 6000 * (coefDown + hunger)); }
        if((health <= 0.00) || (hunger <= 0.00))
          { hunger = 0; died = true; }
        if(hunger > 1.00)
          { hunger = 1.00f; }
      }
    }

    private float PointFood(int idFood){ 
      switch (idFood) {
        case 1: return 0.03f; 
        case 2: return 0.07f;
        case 3: return 0.15f;
        case 4: return 0.30f;
        case 5: return 0.50f; 
        case 6: return 0.70f; 
        default: return 0.00f; 
      }
    }

    public void Health(float value, bool activity = false) {
      float coefDown = 0.10f;
      if (!died) {
        if(!activity) 
          { health += (float)(0.03f * (value / 60) * (-0.4 + hunger) * (coefDown + hunger)); } 
        else 
          { health -= (float)(value / 6000 * (coefDown + health)); }
        if((health <= 0.00) || (hunger <= 0.00))
          { health = 0; died = true; }
        if(health > 1.00)
          { health = 1.00f; }
      }
    }

    public void Play(int game, int time){  
      if ((!died) && (!rest)) {
        double tiredness = Math.Ceiling(PointGame(game) * time / 60 * 100) / 100 * coefPlay;
        Health(-(float)(tiredness * (2 - hunger / 2.00f)));      
        Hunger(-(float)(tiredness * 1.40f));
        Age(time);
      }
      joy += PointGame(game);
      if(joy > 1.00)
          { joy = 1.00f; }
    }

    public float PointGame(int idgame){ 
      switch (idgame) {
        case 1: return 0.15f; 
        case 2: return 0.25f; 
        case 3: return 0.30f; 
        default: return 0.00f; 
      }
    }

    public void Timer(int time){
      if(!rest) {
        Hunger(time * coefHunger);
        Health(time * coefHealth);
        Age(time * coefAge);
        joy -= (float)Math.Round(time / 100.00f * joy, 2); 
      }
    }

    public void Age(int time){
      if (!died) {
        allTime += time;
        if(allTime >= (age + 1) * timeToAgeUp){ age++; }
      }
    }

    private float PercentAge(){
      return (float)Math.Round((allTime / timeToAgeUp) % 1, 2);
    }

    public float Humor() {
      float nHumor = (health + hunger) / 2 + joy; 
      if(nHumor > 1) { nHumor = 1;}
      return nHumor;
    }  

    public string[] getGame(){ 
      string[] games = new string[3];
      games[0] = "bool"; 
      games[1] = "bike"; 
      games[2] = "soccer"; 
      return games;
    } 

    //Método de Temporização
    public void Clock (int id) {
      if((id == 0) || (id == 1)){
        time[id,0] = System.DateTime.Now.Second;
        time[id,1] = System.DateTime.Now.Minute;
        time[id,2] = System.DateTime.Now.Hour;

        if(id == 1){
          int firstTime = time[0,0] + time[0,1] * 60 + time[0,2] * 3600;
          int secondTime = time[1,0] + time[1,1] * 60 + time[1,2] * 3600;
          int totalTime = secondTime - firstTime;
          if(totalTime < 0) { totalTime *= -1; }
          Timer(totalTime);
        }
      }       
    } 

    public int getCoefPlay() { return (int)coefPlay; }
    public string getName() { return name; }
    public bool getDied() { return died; }
    public void setName(string n) { name = n;}
  }
}