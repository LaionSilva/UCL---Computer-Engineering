// Comunicação TCP --> ESP32

#include <stdio.h>
#include <stdlib.h>
#include "tcpip.h"
#include "listadinamica.h"

int  key=0;

void infMenu (int tipo, int menu) {
	if ((tipo==1)&&(menu)) { printf("\n\n==========SENSOR==========\n\nl = ler dados\nr = ler sequencia de dados\ns = sair \n"); }
	if ((tipo==2)&&(menu)) { printf("\n\n==========ATUADOR=========\n\nl = ler dados\ne = enviar dados\ns = sair \n"); }
}

int main (int argc , char *argv[])
{
	setvbuf(stdout, NULL, _IONBF, 0);
	setvbuf(stderr, NULL, _IONBF, 0);
	setLocaleTextPTBR(); //Configurando acentuação e caracteres especiais do português PTBR.

	int run=1, tipo, returnKey = 0;
	char cont, IP[20], inf[4];

	while (run) { //Loop da interface
		printf("\n==========MENU INICIAL==========\n\nc = conectar - s = sair \n\n");
		scanf("%c", &cont), getchar(); //Tomada de decisão do usuário

		if ((cont=='c')||(cont=='C')) {

			printf("\nInforme o IP: "); //Coletando IP de acesso
			fgets(IP, 20, stdin);
			//scanf("%s", &IP);

			key=iniciarServe(IP); //Conectando e validando acesso pela chave Key

			if (key) {
				//Procedimento de identificação do dispositivo
				tipo = type();
				if 		((tipo>=10)&&(tipo<20)) { tipo=1; }
				else if ((tipo>=20)&&(tipo<30)) { tipo=2; }
				else { tipo=0; }

				if (tipo==0) { printf("Conectando...\n"), key = iniciarServe(IP); }
				else {
					int menu = 1;
					if (key) { infMenu (tipo, menu); } //Mostrar opções de menu
					while (key) { //Interface principal - Menu de opções
						char mop=0;
						scanf("%c", &mop); //Pausar

						//Enviar dados - Atuador
						if (((mop=='e')||(mop=='E'))&&(tipo==2)) { printf("\nInforme o dado: "), scanf("%s",inf), empacotadorA(inf), infMenu (tipo, menu);; }
						//Enviar varios dados
						//if (((cont=='w')||(cont=='W'))&&(tipo==2)) { serverOutTime ("w10#",5,10000); }
						//Ler dados - Sensor
						if ((mop=='l')||(mop=='L')) { empacotadorS(1, 0), infMenu (tipo, menu);; }
						//Ler varias vezes - sensor
						if (((mop=='r')||(mop=='R'))&&(tipo==1)) { printf("\nInforme o numero de leituras (max 4): "), scanf("%s",inf), empacotadorS(2, inf), infMenu (tipo, menu);; }
						//Desconectar
						if ((mop=='s')||(mop=='S')) { printf("\nPrograma finalizado");return 0; }

					}
				}
			}
		}
		if ((cont=='i')||(cont=='I')) { /* Configurações de armazenamento de IPs */ }
		if ((cont=='s')||(cont=='S')) { printf("\nPrograma finalizado"); return 0; }

		cont=0;
	} return 0;
}
