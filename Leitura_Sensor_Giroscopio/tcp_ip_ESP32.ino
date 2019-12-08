#include <Wire.h>
#include <WiFi.h>
#include <Kalman.h> 
#define RESTRICT_PITCH 

Kalman kalmanX; // Create the Kalman instances
Kalman kalmanY;

////////WEB SERVER///////////////////////////////////////////////////////////////////////////////////////////////

//CREDENCIAIS PARA CONEXÃO WIFI
const char* ssid     = "VIVO-F3A1";
const char* password = "2F8A2AF3A1";
char a1=' ', a2=' ', a3=' ';
int erro=0;

WiFiServer server(80); //Porta do servidor tcp

//CONFIGURAÇÃO DE IP FIXO
IPAddress local_IP(192, 168, 15, 101); //DEFINIÇÃO DE IP FIXO PARA O NODEMCU
IPAddress gateway(192, 168, 0, 1);     //GATEWAY DE CONEXÃO (ALTERE PARA O GATEWAY DO SEU ROTEADOR)
IPAddress subnet(255, 255, 0, 0);
IPAddress primaryDNS(8, 8, 8, 8);      //optional
IPAddress secondaryDNS(8, 8, 4, 4);    //optional

//////////GIROSCÓPIO//////////
/* IMU Data */
double accX, accY, accZ;
double gyroX, gyroY, gyroZ;
int16_t tempRaw;

double gyroXangle, gyroYangle; // Angle calculate using the gyro only
double compAngleX, compAngleY; // Calculated angle using a complementary filter
double kalAngleX, kalAngleY; // Calculated angle using a Kalman filter

uint32_t timer;
uint8_t i2cData[14]; // Buffer for I2C data

void setup() {
  Serial.begin(115200);
  Wire.begin();
  
  #if ARDUINO >= 157
    Wire.setClock(400000UL); // Set I2C frequency to 400kHz
  #else
    TWBR = ((F_CPU / 400000UL) - 16) / 2; // Set I2C frequency to 400kHz
  #endif

  i2cData[0] = 7; // Set the sample rate to 1000Hz - 8kHz/(7+1) = 1000Hz
  i2cData[1] = 0x00; // Disable FSYNC and set 260 Hz Acc filtering, 256 Hz Gyro filtering, 8 KHz sampling
  i2cData[2] = 0x00; // Set Gyro Full Scale Range to ±250deg/s
  i2cData[3] = 0x00; // Set Accelerometer Full Scale Range to ±2g
  while (i2cWrite(0x19, i2cData, 4, false)); // Write to all four registers at once
  while (i2cWrite(0x6B, 0x01, true)); // PLL with X axis gyroscope reference and disable sleep mode

  while (i2cRead(0x75, i2cData, 1));
  if (i2cData[0] != 0x68) { // Read "WHO_AM_I" register
    Serial.print(F("Error reading sensor"));
    while (1);
  }
  delay(100); // Wait for sensor to stabilize

  /* Set kalman and gyro starting angle */
  while (i2cRead(0x3B, i2cData, 6));
  accX = (int16_t)((i2cData[0] << 8) | i2cData[1]);
  accY = (int16_t)((i2cData[2] << 8) | i2cData[3]);
  accZ = (int16_t)((i2cData[4] << 8) | i2cData[5]);

  // Source: http://www.freescale.com/files/sensors/doc/app_note/AN3461.pdf eq. 25 and eq. 26
  // atan2 outputs the value of -p to p (radians) - see http://en.wikipedia.org/wiki/Atan2
  // It is then converted from radians to degrees
  #ifdef RESTRICT_PITCH // Eq. 25 and 26
    double roll  = atan2(accY, accZ) * RAD_TO_DEG;
    double pitch = atan(-accX / sqrt(accY * accY + accZ * accZ)) * RAD_TO_DEG;
  #else // Eq. 28 and 29
    double roll  = atan(accY / sqrt(accX * accX + accZ * accZ)) * RAD_TO_DEG;
    double pitch = atan2(-accX, accZ) * RAD_TO_DEG;
  #endif

  kalmanX.setAngle(roll); // Set starting angle
  kalmanY.setAngle(pitch);
  gyroXangle = roll;
  gyroYangle = pitch;
  compAngleX = roll;
  compAngleY = pitch;

  timer = micros();

  //////////WEB SERVER//////////
  if (!WiFi.config(local_IP, gateway, subnet,  primaryDNS, secondaryDNS)) //configurando ip fixo definido
    { Serial.println("STA Failed to configure"); }
  
  Serial.print("Conectando: "), Serial.println(ssid);
  
  WiFi.begin(ssid, password); //Conectando wifi
  while (WiFi.status() != WL_CONNECTED) { delay(500), Serial.print("."); }

  Serial.println("\nWiFi conectado.\nEndereço de IP : "), Serial.println(WiFi.localIP()); //Imprimindo relatório de conexão
  server.begin();
}

void loop() {
  //////////WEB SERVER//////////
  WiFiClient client = server.available();   

  if (client) { Serial.println("\nNova conexão"); }
  while (client) {                                
    String  dado = "";   
    bool    id = true, d = false, endd = false, in = false, endfalha = true;      
    while (client.connected() && id) {  
      char c;      
      if (client.available()) {   
        in = true;          
        c = client.read();
        if (c == 35) { id = false, a3 = c, endd = true, endfalha = false; }
        else {
          d = true;
          dado = dado + c; 
          a1 = a2, a2 = c;
        }
      } 
      else if (in) { 
        id=false;
        in = false; 
      }
    }  
        
      //if (data == "S") { Serial.print("\nFim da conexão"), client.print("desconectado"), client.stop(); }
      int v=0;
      if (!(endfalha)) {
        if ((a1=='L') && (a3=='#')) { v = convertCharInt(a2),client.print(empacotadorS(v)), Serial.println(empacotadorS(v)); }
        else if (dado == "TP")      { client.print("GRUPO9SG#"), Serial.println("GRUPO9SG#"); }
        else if (dado == "L")       { client.print(empacotadorS(1)),  Serial.println(empacotadorS(1));}
        else if (dado == "EO") { 
          if (erro==0) { client.print("SG0000#"), Serial.println("SG0000#"); }
          else if (erro==1) { client.print("SG0025#"), Serial.println("SG0025#"); }
          else if (erro==2) { client.print("SG0020#"), Serial.println("SG0020#"); }
          erro = 0;
        }    
        else { Serial.println("EO#"), client.print("EO#"); }  
      }
      else if ( d && !endd) { erro = 1, Serial.println("EO#"), client.print("EO#"); }
      else if (!d &&  endd) { erro = 2, Serial.println("EO#"), client.print("EO#"); }
      else                  { Serial.println("EO#"), client.print("EO#"); }
      //else { Serial.print("Dados recebidos: "), Serial.println(data), client.print(data); }
    
    a1=' ', a2=' ', a3=' ';
  }
  delay(90); //delay do sensor = 10ms
}

//Criar resposta para cliente no formato definido pelo protocolo 
String empacotadorS( int n ){
  String dado, id="SG", fim="#";
  int c=0;
  
  dado = id;
  for (c=0;c<n;c++) {
    int valorS = round((sensorGiroscopio())*100/180);
    String resposta, i="0";
    if (valorS < 0) { i = "1", valorS = valorS * (-1); }
    if      (valorS >= 100) { resposta = convertIntStr((valorS-(valorS%100))/100) + convertIntStr(((valorS-(valorS -(valorS%10)))%100)/10) + convertIntStr(valorS-(valorS%10)); }
    else if (valorS >= 10 ) { resposta = "0"  + convertIntStr((valorS-(valorS%10))/10) + convertIntStr(valorS%10); }
    else if (valorS >  0  ) { resposta = "00" + convertIntStr(valorS); } 
    else if (valorS == 0  ) { resposta = "000"; }
    dado = dado + i + resposta;
    if (n>1) { delay(1000); }
  }
  dado = dado + fim;
 
  return dado;
}

// Conversor numerico - string --> int
int convertCharInt (char s) {
  int ret=0;
  if      (s=='0') { ret = 0; } else if (s=='1') { ret = 1; }
  else if (s=='2') { ret = 2; } else if (s=='3') { ret = 3; }
  else if (s=='4') { ret = 4; } else if (s=='5') { ret = 5; }
  else if (s=='6') { ret = 6; } else if (s=='7') { ret = 7; }
  else if (s=='8') { ret = 8; } else if (s=='9') { ret = 9; }
  else { ret = 0; }
  return ret;
}

// Conversor numerico - int --> string
String convertIntStr (int s) {
  String ret;
  if      (s==0) { ret = "0"; } else if (s==1) { ret = "1"; }
  else if (s==2) { ret = "2"; } else if (s==3) { ret = "3"; }
  else if (s==4) { ret = "4"; } else if (s==5) { ret = "5"; }
  else if (s==6) { ret = "6"; } else if (s==7) { ret = "7"; }
  else if (s==8) { ret = "8"; } else if (s==9) { ret = "9"; }
  else { ret = "0"; }
  return ret;
}

//Código do giroscópio
int sensorGiroscopio() {
  /* Update all the values */
  while (i2cRead(0x3B, i2cData, 14));
  accX =    (int16_t)((i2cData[0] << 8) | i2cData[1]);
  accY =    (int16_t)((i2cData[2] << 8) | i2cData[3]);
  accZ =    (int16_t)((i2cData[4] << 8) | i2cData[5]);
  tempRaw = (int16_t)((i2cData[6] << 8) | i2cData[7]);
  gyroX =   (int16_t)((i2cData[8] << 8) | i2cData[9]);
  gyroY =   (int16_t)((i2cData[10]<< 8) | i2cData[11]);
  gyroZ =   (int16_t)((i2cData[12]<< 8) | i2cData[13]);;

  double dt = (double)(micros() - timer) / 1000000; // Calculate delta time
  timer = micros();

  // It is then converted from radians to degrees
  #ifdef RESTRICT_PITCH // Eq. 25 and 26
    double roll  = atan2(accY, accZ) * RAD_TO_DEG;
    double pitch = atan(-accX / sqrt(accY * accY + accZ * accZ)) * RAD_TO_DEG;
  #else // Eq. 28 and 29
    double roll  = atan(accY / sqrt(accX * accX + accZ * accZ)) * RAD_TO_DEG;
    double pitch = atan2(-accX, accZ) * RAD_TO_DEG;
  #endif

  double gyroXrate = gyroX / 131.0; // Convert to deg/s
  double gyroYrate = gyroY / 131.0; // Convert to deg/s

  #ifdef RESTRICT_PITCH
    // This fixes the transition problem when the accelerometer angle jumps between -180 and 180 degrees
    if ((roll < -90 && kalAngleX > 90) || (roll > 90 && kalAngleX < -90)) {
      kalmanX.setAngle(roll);
      compAngleX = roll;
      kalAngleX = roll;
      gyroXangle = roll;
    } else
      kalAngleX = kalmanX.getAngle(roll, gyroXrate, dt); // Calculate the angle using a Kalman filter
  
    if (abs(kalAngleX) > 90)
      gyroYrate = -gyroYrate; // Invert rate, so it fits the restriced accelerometer reading
      kalAngleY = kalmanY.getAngle(pitch, gyroYrate, dt);
    #else
  // This fixes the transition problem when the accelerometer angle jumps between -180 and 180 degrees
  if ((pitch < -90 && kalAngleY > 90) || (pitch > 90 && kalAngleY < -90)) {
    kalmanY.setAngle(pitch);
    compAngleY = pitch;
    kalAngleY = pitch;
    gyroYangle = pitch;
  } else
    kalAngleY = kalmanY.getAngle(pitch, gyroYrate, dt); // Calculate the angle using a Kalman filter

  if (abs(kalAngleY) > 90)
    gyroXrate = -gyroXrate; // Invert rate, so it fits the restriced accelerometer reading
    kalAngleX = kalmanX.getAngle(roll, gyroXrate, dt); // Calculate the angle using a Kalman filter
  #endif

  gyroXangle += kalmanX.getRate() * dt; // Calculate gyro angle using the unbiased rate
  gyroYangle += kalmanY.getRate() * dt;

  compAngleX = 0.93 * (compAngleX + gyroXrate * dt) + 0.07 * roll; // Calculate the angle using a Complimentary filter
  compAngleY = 0.93 * (compAngleY + gyroYrate * dt) + 0.07 * pitch;

  // Reset the gyro angle when it has drifted too much
  if (gyroXangle < -180 || gyroXangle > 180) { gyroXangle = kalAngleX; }
  if (gyroYangle < -180 || gyroYangle > 180) { gyroYangle = kalAngleY; }

  /* Print Data */
  int valor = round(gyroXangle);
  if      (valor >  180) { valor =  180;}
  else if (valor < -180) { valor = -180;}
  delay(10);

  return valor;
}
