using System;
using System.Collections.Generic;
using System.IO;

namespace logistica {
  public class Calculos {
    private List<Posicao> clientes = new List<Posicao>();
    private List<Posicao> cliCheck = new List<Posicao>();
    private List<Ponto> pontos = new List<Ponto>();
    private List<string> memoria = new List<string>();
    private List<int> cliId = new List<int>();   
    private List<Posicao> cliPen = new List<Posicao>();
    private List<double> dist = new List<double>();
    private int[,] mapa = new int[80,160];
    private int[,] mapa2 = new int[80,160];
    private int[] coordIn = new int[2]{-20,-40};
    private string fileMapa = "mapa.txt";
    private int range = 50;
    private int contUni = 0;

    public void Iniciar() {
      Random rand = new Random();
      int[] coord = new int[2]{0,0};
      int[] ii = new int[2];

      clientes.Add( new Posicao(100, coordIn[0], coordIn[1]) );
        mapa[(int)coordIn[0] + 40, (int)coordIn[1] + 80] = 100;
      for(int i = 1; i <= range; i++) {
        coord[0] = rand.Next(0,80) - 40;
        coord[1] = rand.Next(0,160) - 80;
        clientes.Add( new Posicao(i, coord[0], coord[1]) );
          mapa[(int)coord[0] + 40, (int)coord[1] + 80] = i;
      }

      AtualizarMapa(true);
      Combinar(coordIn);
      contUni = 0;
      
      do { ii = Mapa();
        if((ii[0] != -1) && (ii[1] != -1)) { 
          if( !CheckPontos(ii[0], ii[1]) )  { break; } 
        }
      } while((ii[0] != -1) && (ii[1] != -1));
      GerarMapa(ii);
    }


    private void AtualizarMapa(bool sentido = false) {
      if(sentido)
        for(int i = 0; i < 80; i++) {
          for(int j = 0; j < 160; j++) {
            mapa2[i, j] = mapa[i, j];
          }
        }
      else 
        for(int i = 0; i < 80; i++) {
          for(int j = 0; j < 160; j++) {
            mapa[i, j] = mapa2[i, j];
          }
        }
    }


    public void Combinar(int[] coordIn) {    
      Posicao alvo = new Posicao(0,coordIn[0], coordIn[1]);
      double d;
      int id;

      Console.WriteLine ("Combinando");

      cliPen = clientes;
      foreach(Posicao c in cliPen) {
        dist.Add( c.getDist(0, 0) );
      }
      
      d = ListMin(dist);
      id = dist.IndexOf(d);
      alvo = cliPen[id];
      dist.Remove(d);
      cliPen.RemoveAt(id);

      while(cliPen.Count > 0) {
        dist.Clear();
        foreach(Posicao c in cliPen) {
          dist.Add( c.getDist(alvo.getLat(), alvo.getLon()) );
        }
        d = ListMin(dist);
        id = dist.IndexOf(d);

        alvo.setIdDe(cliPen[id].getId());
        cliPen[id].setIdOr(alvo.getId());
        cliCheck.Add(alvo);
        alvo = cliPen[id];
        cliPen.RemoveAt(id);
        dist.Remove(d);
        
      }
      alvo.setIdDe(cliCheck[0].getId());
      cliCheck[0].setIdOr(alvo.getId());
      cliCheck.Add(alvo);

      cliId.Clear();
      foreach(Posicao c in cliCheck){
        cliId.Add(c.getId());
      }

      cliId.Clear();
      foreach(Posicao c in cliCheck){          
        cliId.Add(c.getId());
      }
      
      //Ajustar();

      Relacionar();
    }

    private void Ajustar() {
      Console.WriteLine ("Realizando ajustes");
      bool recal = false;
      int contRecal = 0;
      do {
        recal = false;
        contRecal++;

        foreach(Posicao c in cliCheck) {
          dist.Clear();
          foreach(Posicao c2 in cliCheck) {
            if((c.getLat() == c2.getLat()) && (c.getLon() == c2.getLon())) {
              dist.Add(10000);
            } else { dist.Add(c.getDist(c2.getLat(), c2.getLon())); }
          } System.Threading.Thread.Sleep(1);

          double d1 = 0, d2 = 0;
          int id1 = 0, id2 = 0;
          d1 = ListMin(dist);
          id1 = dist.IndexOf(d1);
          dist[ dist.IndexOf(d1) ] = 10000;
          d2 = ListMin(dist);
          id2 = dist.IndexOf(d2);
          
          if(cliCheck[id1].getIdOr() == cliCheck[id2].getId()) {
            //Console.WriteLine ("{0} {1}", cliId.IndexOf(c.getIdDe()), cliId.IndexOf(c.getIdOr()));
            cliCheck[id1].setIdOr(c.getId());
            cliCheck[id2].setIdDe(c.getId());

            cliCheck[ cliId.IndexOf(c.getIdOr()) ].setIdDe(c.getIdDe());
            cliCheck[ cliId.IndexOf(c.getIdDe()) ].setIdOr(c.getIdOr());  

            c.setIdOr(cliCheck[id2].getId());
            c.setIdDe(cliCheck[id1].getId());   
            recal = true;       
          }
          else if(cliCheck[id1].getIdDe() == cliCheck[id2].getId()) {
            //Console.WriteLine ("{0} {1}", cliId.IndexOf(c.getIdDe()), cliId.IndexOf(c.getIdOr()));
            cliCheck[id1].setIdDe(c.getId());
            cliCheck[id2].setIdOr(c.getId());

            cliCheck[ cliId.IndexOf(c.getIdDe()) ].setIdOr(c.getIdOr());
            cliCheck[ cliId.IndexOf(c.getIdOr()) ].setIdDe(c.getIdDe());  

            c.setIdDe(cliCheck[id2].getId());
            c.setIdOr(cliCheck[id1].getId());
            recal = true;
          }
        }
      } while(recal && contRecal < range);
    }


    private void Relacionar(){
      double[] coord = new double[2]{0,0};
      double[] coordOr = new double[2]{0,0};
      AtualizarMapa();
      Console.WriteLine ("Conectando");

      foreach(Posicao c in cliCheck) {
        coord[0] = c.getLat(); 
        coord[1] = c.getLon();
        int auxIdOr = c.getIdOr(); 
        
        coordOr[0] = cliCheck[ cliId.IndexOf(auxIdOr) ].getLat(); 
        coordOr[1] = cliCheck[ cliId.IndexOf(auxIdOr) ].getLon(); 
        
        Conectar(coordOr[0], coordOr[1], coord[0], coord[1], cliCheck[ cliId.IndexOf(auxIdOr) ].getId(), c.getId());
      }
    }


    private double ListMin(List<double> list){
      double min = 100000;
      foreach(double l in list){
        if(l < min) { min = l; }
      }
      return min;
    }

    
    private int[] Mapa() {
      int[] ii = new int[2]; ii[0] = -1; ii[1] = -1;

      for(int i = 0; i < 80; i++) {
        for(int j = 0; j < 160; j++) {
          if(mapa[i,j] == -2) { ii[0] = i; ii[1] = j; break; }
        }
      }
      GerarMapa(ii);   
      return ii;
    }


    private void GerarMapa(int[] ii) {
      if((ii[0] == -1) && (ii[1] == -1)) { Console.WriteLine ("Gerando mapa");
        if (!System.IO.File.Exists(fileMapa))
          { using (StreamWriter Salvar = File.AppendText(fileMapa)) {} }

        System.Threading.Thread.Sleep(50);
        System.IO.File.Delete(fileMapa);
        using (StreamWriter Salvar = File.AppendText(fileMapa)) { 
          for(int i = 0; i < 80; i++) {
            for(int j = 0; j < 160; j++) {
              if(mapa[i,j] == 0) { Salvar.Write (" "); }
              else if(mapa[i,j] == -1) { Salvar.Write ("."); }
              else { Salvar.Write ("{0}", mapa[i,j]); }
            }
            Salvar.WriteLine();
            System.Threading.Thread.Sleep(5);
          }        
          System.Threading.Thread.Sleep(50);
        }
      }     
    }


    private bool CheckPontos(int la, int lo){
      int or1 = 0, or2 = 0, de1 = 0, de2 = 0, lat1 = 0, lon1 = 0, cont = 0;
      cliId.Clear(); 
      contUni++;
      foreach(Posicao c in cliCheck){
        cliId.Add(c.getId());
      } System.Threading.Thread.Sleep(10);
      foreach(Ponto p in pontos){
        if((p.lat == la) && (p.lon == lo) && (contUni < 50) && (memoria.IndexOf(String.Format("{0:0}", p.lat) + String.Format("{0:0}", p.lon)) == -1)) {
          System.Threading.Thread.Sleep(1);
          if(cont == 0) { 
            or1 = p.ido;
            de1 = p.idd; 
            lat1 = (int)p.lat;
            lon1 = (int)p.lon;
            Console.WriteLine("ponto cruzado: {0} {1} {2} {3} 1", p.lat, p.lon, p.ido, p.idd); 
            cont++; 
          }
          else if(cont == 1) { 
            or2 = p.ido; 
            de2 = p.idd; 
            Console.WriteLine("ponto cruzado: {0} {1} {2} {3} 2", p.lat, p.lon, p.ido, p.idd); 
            memoria.Add(String.Format("{0:0}", p.lat) + String.Format("{0:0}", p.lon));
            cont++; 
            break;
          }
        } System.Threading.Thread.Sleep(1);
      }
      if(cont == 2) {
        int or = 0, de = 0;

        de = cliCheck[ cliId.IndexOf( de1 ) ].getIdDe();
        or = cliCheck[ cliId.IndexOf( or2 ) ].getIdOr();

        cliCheck[ cliId.IndexOf(or1) ].setIdDe( cliCheck[ cliId.IndexOf(or2) ].getId() );
        cliCheck[ cliId.IndexOf(de2) ].setIdOr( cliCheck[ cliId.IndexOf(de1) ].getId() );

        cliCheck[ cliId.IndexOf(  cliCheck[ cliId.IndexOf( or2 ) ].getIdOr() ) ].setIdDe( cliCheck[ cliId.IndexOf(de1) ].getId() );
        cliCheck[ cliId.IndexOf(  cliCheck[ cliId.IndexOf( de1 ) ].getIdDe() ) ].setIdOr( cliCheck[ cliId.IndexOf(or2) ].getId() );

        cliCheck[ cliId.IndexOf(or2) ].setIdDe( de );
        cliCheck[ cliId.IndexOf(or2) ].setIdOr( or1 );
        cliCheck[ cliId.IndexOf(de1) ].setIdOr( or );
        cliCheck[ cliId.IndexOf(de1) ].setIdDe( de2 );
        
        pontos.Clear();
        Relacionar();
        return true;
      }
      else if (cont == 1) { 
        mapa[lat1, lon1] = -1; 
        Relacionar();
        return true;
      }
      Console.WriteLine (0);
      return false;
    }


    private void Conectar(double c, double cc, double b, double bb, int ido, int idd) {
        int or = 0, de = 0, auxOr = 0, auxDe = 0, salto = 0;
        double espaco = 0;
        bool sentido;

        int[] c1 = new int[2];
        int[] c2 = new int[2];
        c1[0] = (int)c + 40;
        c2[0] = (int)b + 40;
        c1[1] = (int)cc + 80;
        c2[1] = (int)bb + 80;

        double distanciaLat = c1[0] - c2[0];
        double distanciaLon = c1[1] - c2[1];
        if(distanciaLat < 0) { distanciaLat *= -1; }
        if(distanciaLon < 0) { distanciaLon *= -1; }
        
        if(distanciaLat > distanciaLon){
          espaco = distanciaLat / distanciaLon;
          or = c1[0]; 
          auxOr = c1[1];
          de = c2[0];
          auxDe = c2[1];
          sentido = true;
        } else {
          espaco = distanciaLon / distanciaLat;
          or = c1[1];
          auxOr = c1[0]; 
          de = c2[1];
          auxDe = c2[0];
          sentido = false;
        }

        if(or < de) {
          for(int i = or; i < de; i++) {
            if(((i - or) % espaco < 1) && (i > 0)) { 
              if(auxOr < auxDe)  { salto++; }
              else { salto--; }
            }
            if(sentido) { 
              if(mapa[i, c1[1] + salto] <= 0) { 
                if((i - or > 2) && (i < de - 2)) { 
                  mapa[i, c1[1] + salto] -= 1; 
                  pontos.Add( new Ponto(i, c1[1] + salto, ido, idd) ); 
                }
                else { mapa[i, c1[1] + salto] = -1; }
              } 
            } else {
              if(mapa[c1[0] + salto, i] <= 0) { 
                if((i - or > 2) && (i < de - 2)) {
                  mapa[c1[0] + salto, i] -= 1; 
                  pontos.Add( new Ponto(c1[0] + salto, i, ido, idd) ); 
                }
                else { mapa[c1[0] + salto, i] = -1; }
              }
            }
          } 
        } else {
          for(int i = or; i > de; i--) { 
            if(((i - de) % espaco < 1) && (i > 0)) { 
              if(auxOr < auxDe)  { salto++; }
              else { salto--; }
            }
            if(sentido) { 
              if(mapa[i, c1[1] + salto] <= 0) { 
                if((i < or - 2) && (i > de + 2)) { 
                  mapa[i, c1[1] + salto] -= 1; 
                  pontos.Add( new Ponto(i, c1[1] + salto, ido, idd) ); 
                }
                else { mapa[i, c1[1] + salto] = -1; }
              } 
            } else {
              if(mapa[c1[0] + salto, i] <= 0) { 
                if((i < or - 2) && (i > de + 2)) { 
                  mapa[c1[0] + salto, i] -= 1; 
                  pontos.Add( new Ponto(c1[0] + salto, i, ido, idd) ); 
                }
                else { mapa[c1[0] + salto, i] = -1; }
              }
            }
          } 
        }
    }

  }


  public class Ponto{
    public int ido;
    public int idd;
    public double lat;
    public double lon;
    public Ponto(double la, double lo, int o, int d) { 
      lat = la;
      lon = lo;
      ido = o;
      idd = d;
    }
  }
  

  public class Posicao {
    private int id;
    private double lat;
    private double lon;
    private int idOr;
    private int idDe;
    private bool config;

    public Posicao(int i, double c1, double c2, bool c = false) {
      id = i;
      lat = c1;
      lon = c2;
      config = c;
    }

    private double calcularDistancia(double lat2,double lon2) {
      try { return Math.Sqrt( Math.Pow(lat - lat2, 2) + Math.Pow(lon - lon2, 2)); } 
      catch { Console.WriteLine ("Erro: Calcular Distancia - log:CDi"); return 100; }
    }

    public int getId() { return id; }
    public double getLat() { return lat; }
    public double getLon() { return lon; }
    public int getIdOr() { return idOr; }
    public int getIdDe() { return idDe; }
    public double getDist(double la,double lo) { return calcularDistancia(la, lo); }
    public bool getConfig() { return config; }

    public void setId(int i) { id = i; }
    public void setConfig(bool c) { config = c; }
    public void setLat(double l) { lat = l; }
    public void setLon(double l) { lon = l; }
    public void setIdOr(int i) { idOr = i; }
    public void setIdDe(int i) { idDe = i; }
  }
}