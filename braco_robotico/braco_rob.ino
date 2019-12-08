#include <Servo.h> 

Servo servo[5];

int m1 = 3;
int m2 = 2;
int m3 = 5;
int m4 = 4;
int m5 = 6;
int vm[5] = {90,90,90,90,10};
int lendo, id, acao = 0;
bool teste = false, ler = false, garra = false;

void setup() {
  servo[0].attach(m1); //  Base
  servo[1].attach(m2); //  Eixo central inferior
  servo[2].attach(m3); //  Eixo central superior
  servo[3].attach(m4); //  Eixo de rotação garra
  servo[4].attach(m5); //  Garra
  Serial.begin(9600); //  Bluetooth
}

void loop() {
  lendo = 0;
  delay(20);
  if(Serial.available() > 0){ lendo = Serial.read();
    if(lendo == 64){ 
      ler = true;       
    } 
    if(ler){ delay(20), lendo = Serial.read();
      if(lendo == 42){ acao = 0; }
      if(lendo == 109){ delay(20), lendo = Serial.read();
        if      (lendo == 49){ delay(20), Mover(0, Serial.read()); }
        else if (lendo == 50){ delay(20), Mover(1, Serial.read()); }
        else if (lendo == 51){ delay(20), Mover(2, Serial.read()); }
        else if (lendo == 52){ delay(20), Mover(3, Serial.read()); }
        if(teste){ Serial.print("2: "), Serial.println(lendo); } 
      }      
      else if(lendo == 103){ delay(20), lendo = Serial.read();
        if      (lendo == 48){ garra = false; }
        else if (lendo == 49){ garra = true; }
        else { ler = false; }
      } 
    }    
  }

  if((acao == 1) && (vm[id] < 180)){
    vm[id]++;
    servo[id].write(vm[id]);
    delay[100];
  } 
  else if ((acao == 2) && (vm[id] > 0)){
    vm[id]--;
    servo[id].write(vm[id]);
    delay[100];
  }
    
  if(garra) { servo[4].write(80); }
  else      { servo[4].write(10); }
}

void Mover(int i, int a){
  id = i;
  if(a == 49){ acao = 1; }
  else if(a == 48){ acao = 2; }
}

int LerAngulo(){
  int angulo[3]{0,0,0}, tAngulo;
  for(int i = 0; i < 3; i++){ delay(15);
    if(Serial.available() > 0){ lendo = Serial.read();
      if((lendo >= 48) && (lendo <= 57) && (lendo != 35)) { angulo[i] = lendo - 48; }
      if(lendo == 35) { ler = false; break; }
    } else { break; }
  }
  tAngulo = (angulo[0] - 1) * 100 + angulo[1] * 10 + angulo[2];
  if(teste){ Serial.print("1: "), Serial.println(tAngulo); }
  return tAngulo;
}
