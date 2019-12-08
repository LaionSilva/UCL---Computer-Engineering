#ifndef TCPIP_H
#define TCPIP_H

void delay (int time, int carr);

void setLocaleTextPTBR ();

int iniciarServe (char *IP);

int serverOut (char *m, int relatorio);

void empacotadorA (char inf[10]);

void empacotadorS (int acao, char inf[10]);

void serverIn (int relatorio, int tipol);

void serverInTime (unsigned int repeat, int time);

void serverOutTime (char *message, unsigned int repeat, int time);

void limparLinha ();

int decodCharInt (char s);

int type ();

#endif
