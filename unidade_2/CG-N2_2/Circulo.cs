/**
  Autor: Vinicius Pereira Forte
**/

#define CG_Debug
#define CG_OpenGL
// #define CG_DirectX

using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{
  internal class Circulo : ObjetoGeometria
  {
    //Ajustar a classe de Referência
    public Circulo(char rotulo, Objeto paiRef, Ponto4D ptoCentro, double raio) : base(rotulo, paiRef)
    {
      for (int i = 0; i <= 360; i=i+5)
      {
        base.PontosAdicionar(Matematica.GerarPtosCirculo(i,raio));
      }
      
    }

    protected override void DesenharObjeto()
    {
#if CG_OpenGL && !CG_DirectX
      GL.Begin(base.PrimitivaTipo);
      foreach (Ponto4D pto in pontosLista)
      {
        GL.Vertex2(pto.X, pto.Y);
      }
      GL.End();
#elif CG_DirectX && !CG_OpenGL
    Console.WriteLine(" .. Coloque aqui o seu código em DirectX");
#elif (CG_DirectX && CG_OpenGL) || (!CG_DirectX && !CG_OpenGL)
    Console.WriteLine(" .. ERRO de Render - escolha OpenGL ou DirectX !!");
#endif
    }
  }
}