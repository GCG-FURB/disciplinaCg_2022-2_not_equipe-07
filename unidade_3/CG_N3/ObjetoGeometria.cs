/**
  Autor: Dalton Solano dos Reis
**/

using System.Collections.Generic;
using CG_Biblioteca;
using System;//Para usar Math

namespace gcgcg
{
  internal abstract class ObjetoGeometria : Objeto
  {
    protected List<Ponto4D> pontosLista = new List<Ponto4D>();

    public ObjetoGeometria(char rotulo, Objeto paiRef) : base(rotulo, paiRef) { }
    
    protected override void DesenharGeometria()
    {
      DesenharObjeto();
    }
    protected abstract void DesenharObjeto();
    public void PontosAdicionar(Ponto4D pto)
    {
      pontosLista.Add(pto);
      if (pontosLista.Count.Equals(1))
        base.BBox.Atribuir(pto);
      else
        base.BBox.Atualizar(pto);
      base.BBox.ProcessarCentro();
    }

    public void PontosRemoverUltimo()
    {
      pontosLista.RemoveAt(pontosLista.Count - 1);
    }

    protected void PontosRemoverTodos()
    {
      pontosLista.Clear();
    }

    public Ponto4D PontosUltimo()
    {
      return pontosLista[pontosLista.Count - 1];
    }

    public void PontosAlterar(Ponto4D pto, int posicao)
    {
      pontosLista[posicao] = pto;
    }

    public int encontrarVerticeProximo(Ponto4D mouse){
      double menorDistancia = 0;
      double distanciaCompara = 0;
      int posicaoMenorNaLista = 0;
      for (int i = 0; i < pontosLista.Count; i++){
        distanciaCompara = distanciaEuclidiana(pontosLista[i],mouse);
        if(i.Equals(0) || distanciaCompara < menorDistancia){
          menorDistancia = distanciaCompara;
          posicaoMenorNaLista = i;
        }
      }
      return posicaoMenorNaLista;
    }

    public void moverComandoV(Ponto4D mouse){
      int pontoobje = encontrarVerticeProximo(mouse);
      PontosAlterar(mouse, pontoobje);
    }
    public void removerComandoD(Ponto4D mouse){
      int pontoobje = encontrarVerticeProximo(mouse);
      pontosLista.RemoveAt(pontoobje);
    }

    public double distanciaEuclidiana(Ponto4D distancia1,Ponto4D distancia2){
      double distance = Math.Sqrt((Math.Pow(distancia1.X - distancia2.X, 2) + Math.Pow(distancia1.Y - distancia2.Y, 2)));
      Console.WriteLine(distance);    
      return distance;  
    }

    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto: " + base.rotulo + "\n";
      for (var i = 0; i < pontosLista.Count; i++)
      {
        retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
      }
      return (retorno);
    }
  }
}