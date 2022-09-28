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
  internal class SegReta : ObjetoGeometria
  {
    //Ajustar a classe de Referência
     public SegReta(char rotulo, Objeto pai, Ponto4D ptoIni, Ponto4D ptoFim) : base(rotulo, pai)
        {
            base.PontosAdicionar(ptoIni);
            base.PontosAdicionar(ptoFim);
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