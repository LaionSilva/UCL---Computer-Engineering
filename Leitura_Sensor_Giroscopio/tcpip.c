#include <stdio.h>
#include "tcpip.h"
#include <winsock2.h>
#include <locale.h>
#include <time.h>
#include <windows.h>
#include <string.h>

WSADATA wsa;
SOCKET s;
char server_reply[20];
int recv_size;
struct sockaddr_in server;
char d[2];

void delay (int time, int carr) {
    int time_ms = time;

    if (carr) { printf("Carregando...\n "); }
    if (time>0) {
		clock_t start_time = clock(); // Iniciando contagem
		while (clock() < start_time + time_ms); // Loop até que o tempo solicitado seja alcançado
    }
}

void setLocaleTextPTBR () { setlocale(LC_ALL, "Portuguese"); } //Habilita a acentuação para o português

int iniciarServe (char *IP) {
    printf("\nInicializando o Winsock...");

    if (WSAStartup(MAKEWORD(2,2),&wsa) != 0) { printf("Falha. Erro no código : %d", WSAGetLastError()); }
	else {
		printf("Inicializado.\n");

	    //Criar um soquete
	    if((s = socket(AF_INET , SOCK_STREAM , 0)) == INVALID_SOCKET) { printf("Não foi possível criar o soquete : %d" , WSAGetLastError()); }
		else {
			printf("Soquete criado.\n");

		    //Conectando ao IP
			printf("Conectando.\n");
		    server.sin_addr.s_addr = inet_addr(IP);
		    server.sin_family = AF_INET;
		    server.sin_port = htons( 80 );

		    //Conecte-se ao servidor remoto
		    if (connect(s , (struct sockaddr *)&server , sizeof(server)) < 0) { puts("Erro na conexão"); return 0; }
		    else { puts("Conectado"); return 1; }
		}
	} return 0;
}

int serverOut (char *m, int relatorio) { //Envie alguns dados
	if( send(s , m , strlen(m) , 0) < 0) {
		if (relatorio) { puts("\nO envio falhou\n"); }
		return 0; }
	else {
		if (relatorio) { printf("\nDados enviados com sucesso\n "); }
		return 1; }
}

void empacotadorA (char inf[4]) { //Gerando comando antes de enviar ao atuador
	char dado[10];
	strcpy(dado,"A");

	if (inf[0]=='-'){ strcat(dado,"1"), inf[0]=inf[1],inf[1]=inf[2],inf[2]=inf[3],inf[3]=0; }
	else { strcat(dado,"0"); }

	int c = strlen(inf);
	if 		(c==1) 	{ strcat(dado, "00"); strcat(dado, inf); }
	else if (c==2) 	{ strcat(dado, "0" ); strcat(dado, inf); }
	else if (c>=3) 	{ strcat(dado, "100"); }

	strcat(dado,"#");// printf("Comando: "), printf("%s",dado);
	serverOut(dado,1), delay(0,1000), serverIn(1,1); //-------------------------------Enviar
}

void empacotadorS (int acao, char inf[10]) { //---------------------------------------Gerar comando antes de enviar ao sensor
	char dado[20];
	if (acao==1) { strcpy(dado,"L#"); } //printf("Comando: %s",dado); } //----------------Solicitar leitura unica
	else if	(acao==2) {	//------------------------------------------------------------Solicitar leitura em cadeia
		if ((decodCharInt(inf[0])>0) && (decodCharInt(inf[0])<=4)) { strcpy(dado,"L"), strcat(dado,inf), strcat(dado,"#"); }
		else { printf("\n\nErro: Empacotamento de dados - estouro de cadeia\n\n"); }
		//printf("Comando: %s",dado);
	}
	else { printf("\n\nErro: Empacotamento de dados\n\n"); }
	serverOut(dado,1), delay(0,1000), serverIn(1,1); //-------------------------------Enviar
}

void serverIn (int relatorio, int tipol) { //Ler resposta do servidor
	char tinf[20]={'0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0'};
	int valor[4]={0,0,0,0}, i=0, c=0;

	if (((recv_size = recv(s , server_reply , 2000 , 0)) == SOCKET_ERROR) && (relatorio)) { puts("Falha: nenhuma resposta"); }
	//Adicione um caracter NULL para torná-lo uma sequência adequada antes de imprimir
	else if (tipol==0) { printf("\nDados recebidos: "), printf(server_reply), server_reply[recv_size] = '\0'; }
	else {
		strcpy(tinf,server_reply);

		if ((tinf[0]==d[0]) && (tinf[1]==d[1])) {
			for(i=0;i<4;i++) {
				if (!(tinf[2+(i*4)]==35)) {
					valor[i] = (decodCharInt(tinf[2+(i*4)])*(-1)) * (decodCharInt(tinf[3+(i*4)])*100) + (decodCharInt(tinf[4+(i*4)])*10) + decodCharInt(tinf[5+(i*4)]);
					c++;
				} else { i=4; }
			}
			printf("\n__________________\nResultado:");
			if (c>4) { c=4; }
			for(i=0;i<c;i++) {
				int esc=decodEsc( decodCharInt( d[1] ) );
				if (esc==1) { printf("\n%c%c %i%c%c", d[0], d[1], valor[i], 67, 77); }
				else 		{ printf("\n%c%c %i%c",   d[0], d[1], valor[i], esc); }
			}
			printf("\n__________________");
		} else { printf("\n\nErro na leitura. Tipo incoerente."); }
	}
}

//Limpar lixo presete na linha de comunicação
void limparLinha () { if ((recv_size = recv(s , server_reply , 2000 , 0)) == SOCKET_ERROR) {} }

int decodCharInt (char s) { //Converter números de Char para Int
	//Números
	if 		(s=='0') { return 0; }	else if (s=='1') { return 1; }	else if (s=='2') { return 2; }
	else if (s=='3') { return 3; }	else if (s=='4') { return 4; }	else if (s=='5') { return 5; }
	else if (s=='6') { return 6; }	else if (s=='7') { return 7; }	else if (s=='8') { return 8; }
	else if (s=='9') { return 9; }

	//Sendor
	else if	(s=='T') { return 11; } else if (s=='U') { return 12; }	else if (s=='D') { return 13; }
	else if (s=='L') { return 14; }	else if (s=='G') { return 15; }

	//Atuador
	else if	(s=='S') { return 21; }	else if (s=='M') { return 22; }

	else { return 100; }
}

void decodIntStr (int s) { //Traduzir informação numerica - Informações gerais do dispositivo. Int --> String
	if		(s==11) { printf("Sensor de Temperatura - Range: -20°C a 100°C"); }
	else if	(s==12) { printf("Sensor de Umidade -  Range: 0%c a 100%c",37,37); }
	else if	(s==13) { printf("Sensor de Distância - Range: 2cm a 30cm"); }
	else if	(s==14) { printf("Sensor de Luminosidade - Range: 0%c a 100%c",37,37); }
	else if	(s==15) { printf("Sensor Giroscópio - Range: 0%c a 100%c (0°~180°)",37,37); }

	else if	(s==21) { printf("Atuador Servo - Range: 0%c a 100%c (0°~180°)",37,37); }
	else if	(s==22) { printf("Atuador Motor (ESC) - Range: 0%c a 100%c em RPM",37,37); }
}

int decodEsc (int s) { //Traduzir informação numerica - Escala dos valores lidos. Int --> Char(Tb. ASCI)
	int r;
	if		(s==11) { r=67; }
	else if	((s==12)||(s==14)||(s==15)||(s==21)||(s==22)) { r=37; }
	else if (s==13) { r=1; }

	return r;
}

int type () { //Solicitar validar e informar tipo de dispositivo ao usuário e às demais funções
	int result, n;
	serverOut("TP#",0), delay(0,1000); //--------------------------------------------------------------Solicitando tipo
	memset(&server_reply, 0, 20);
	if ((recv_size = recv(s , server_reply , 20 , 0)) == SOCKET_ERROR) { result = 100; } //------------Lendo tipo e interpretando dados
	{
		char tinf[20];
		strcpy(tinf,server_reply);
		if ((tinf[0]=='G') && (tinf[1]=='R') && (tinf[2]=='U') && (tinf[3]=='P') && (tinf[4]=='O') && (tinf[8]=='#')) {
			n = decodCharInt(tinf[5]);
			if ((n>0) && (n<=20)) {
				if 	 ((tinf[6]=='S') || (tinf[6]=='A')) { d[0] = tinf[6], d[1] = tinf[7], result = decodCharInt(tinf[7]); }
				else { result = 100; }
			}
		}
		else { result = 100; }
		server_reply[recv_size] = '\0';
	}
	if (result != 100) 	{ printf("\nDispositivo identificado:\nGRUPO %i\n", n), decodIntStr(result); } //Imprimindo informações
	else 				{ printf("\nDispositivo não identificado\n"); }

	return result;
}


