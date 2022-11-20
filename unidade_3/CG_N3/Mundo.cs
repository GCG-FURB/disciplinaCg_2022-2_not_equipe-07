#define CG_Gizmo
// #define CG_Privado

using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenTK.Input;
using CG_Biblioteca;

namespace gcgcg
{
  class Mundo : GameWindow
  {
    private static Mundo instanciaMundo = null;

    private Mundo(int width, int height) : base(width, height) { }

    public static Mundo GetInstance(int width, int height)
    {
      if (instanciaMundo == null)
        instanciaMundo = new Mundo(width, height);
      return instanciaMundo;
    }

    private CameraOrtho camera = new CameraOrtho();
    protected List<Objeto> objetosLista = new List<Objeto>();
    private ObjetoGeometria objetoSelecionado = null;
    private char objetoId = '@';
    private bool bBoxDesenhar = false;
    int mouseX, mouseY;   //TODO: achar método MouseDown para não ter variável Global
    private Poligono obj_Poligno = null;
    private bool estaSelecionado = true;
#if CG_Privado
    private Retangulo obj_Retangulo;
    private Privado_SegReta obj_SegReta;
    private Privado_Circulo obj_Circulo;
#endif

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      camera.xmin = 0; camera.xmax = 600; camera.ymin = 0; camera.ymax = 600;

      Console.WriteLine(" --- Ajuda / Teclas: ");
      Console.WriteLine(" [  H     ] mostra teclas usadas. ");

      /*objetoId = Utilitario.charProximo(objetoId);
      obj_Poligno = new Poligono(objetoId, null);
      objetosLista.Add(obj_Poligno);
      obj_Poligno.PontosAdicionar(new Ponto4D( 50,  50));
      obj_Poligno.PontosAdicionar(new Ponto4D(350,  50));
      obj_Poligno.PontosAdicionar(new Ponto4D(350, 350));
      obj_Poligno.PontosAdicionar(new Ponto4D( 50, 350));
      objetoSelecionado = obj_Poligno;
      obj_Poligno = null;*/

#if CG_Privado
      objetoId = Utilitario.charProximo(objetoId);
      obj_Retangulo = new Retangulo(objetoId, null, new Ponto4D(50, 50, 0), new Ponto4D(150, 150, 0));
      obj_Retangulo.ObjetoCor.CorR = 255; obj_Retangulo.ObjetoCor.CorG = 0; obj_Retangulo.ObjetoCor.CorB = 255;
      objetosLista.Add(obj_Retangulo);
      objetoSelecionado = obj_Retangulo;

      objetoId = Utilitario.charProximo(objetoId);
      obj_SegReta = new Privado_SegReta(objetoId, null, new Ponto4D(50, 150), new Ponto4D(150, 250));
      obj_SegReta.ObjetoCor.CorR = 255; obj_SegReta.ObjetoCor.CorG = 99; obj_SegReta.ObjetoCor.CorB = 71;
      objetosLista.Add(obj_SegReta);
      objetoSelecionado = obj_SegReta;

      objetoId = Utilitario.charProximo(objetoId);
      obj_Circulo = new Privado_Circulo(objetoId, null, new Ponto4D(100, 300), 50);
      obj_Circulo.ObjetoCor.CorR = 177; obj_Circulo.ObjetoCor.CorG = 166; obj_Circulo.ObjetoCor.CorB = 136;
      objetosLista.Add(obj_Circulo);
      objetoSelecionado = obj_Circulo;
#endif
      GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);
      GL.MatrixMode(MatrixMode.Projection);
      GL.LoadIdentity();
      GL.Ortho(camera.xmin, camera.xmax, camera.ymin, camera.ymax, camera.zmin, camera.zmax);
    }
    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);
      GL.Clear(ClearBufferMask.ColorBufferBit);
      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadIdentity();
#if CG_Gizmo      
      Sru3D();
#endif
      for (var i = 0; i < objetosLista.Count; i++)
        objetosLista[i].Desenhar();
      if (bBoxDesenhar && (objetoSelecionado != null))
        objetoSelecionado.BBox.Desenhar();
      this.SwapBuffers();
    }

    protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
    {
      //Lista ajuda no console
      if (e.Key == Key.H)
        Utilitario.AjudaTeclado();
      else if (e.Key == Key.Escape)
        Exit();
      
      //Lista Polígonos e Vértices
      else if (e.Key == Key.E)
      {
        Console.WriteLine("--- Objetos / Pontos: ");
        for (var i = 0; i < objetosLista.Count; i++)
        {
          Console.WriteLine(objetosLista[i]);
          List<Objeto> filhos = objetosLista[i].Filhos();
          if(filhos != null){
            foreach (ObjetoGeometria objetoFilho in filhos){
              if(objetoFilho == objetoSelecionado){
                Console.WriteLine(objetoFilho);
              }
            }
          }
        }
      }

      //Exibe BBox do Objeto Selecionado
      else if (e.Key == Key.O)
        bBoxDesenhar = !bBoxDesenhar;

      //Alteração de Cores formato RGB
       else if (e.Key == Key.R && objetoSelecionado != null) {
        objetoSelecionado.ObjetoCor.CorR = 255;
        objetoSelecionado.ObjetoCor.CorG = 0;
        objetoSelecionado.ObjetoCor.CorB = 0;
      }
      else if (e.Key == Key.G && objetoSelecionado != null) {
        objetoSelecionado.ObjetoCor.CorR = 0;
        objetoSelecionado.ObjetoCor.CorG = 255;
        objetoSelecionado.ObjetoCor.CorB = 0;
      }
      else if (e.Key == Key.B && objetoSelecionado != null) {
        objetoSelecionado.ObjetoCor.CorR = 0;
        objetoSelecionado.ObjetoCor.CorG = 0;
        objetoSelecionado.ObjetoCor.CorB = 255;
      }

      //Remover Polígono
      else if(e.Key == Key.C && objetoSelecionado != null){
        for (var i = 0; i < objetosLista.Count; i++){
          if(objetosLista[i].Equals(objetoSelecionado)){
            objetosLista.Remove(objetoSelecionado);
            Console.WriteLine("Objeto Apagado /n" + objetoSelecionado.ToString());
            objetoSelecionado = null;
            estaSelecionado = true;
          }
          else {
            List<Objeto> filhos = objetosLista[i].Filhos();
            if(filhos != null){
              for (int j=0; j < filhos.Count; j++){
                if(filhos[j] == objetoSelecionado){
                  objetosLista[j].FilhoRemover(filhos[j]);
                  Console.WriteLine("Objeto Filho Apagado /n" + objetoSelecionado.ToString());
                  objetoSelecionado = null;
                }
              }
            }
          }
        }
      }

      //Termina adição e mover pontos, desseleciona polígono
      else if (e.Key == Key.Enter)
      {
        if (obj_Poligno != null)
        {
          obj_Poligno.PontosRemoverUltimo();   // N3-Exe6: "truque" para deixar o rastro
          objetoSelecionado = obj_Poligno;
          obj_Poligno = null;
          
        }
        estaSelecionado = true;
        Console.WriteLine("Terminado objeto: " + objetoSelecionado.ToString());
      }

      //Move o vértice do polígono selecionado que estiver mais perto do mouse.
      else if (e.Key == Key.V && objetoSelecionado != null){
          objetoSelecionado.moverComandoV(new Ponto4D(mouseX, mouseY));
      }

      //Remove o vértice do polígono selecionado que estiver mais perto do mouse.
      else if(e.Key == Key.D && objetoSelecionado != null){
        objetoSelecionado.removerComandoD(new Ponto4D(mouseX,mouseY));
      }

      //Alterna entre aberto e fechado o polígono selecionado.
      else if (e.Key == Key.S && objetoSelecionado != null){
        if(objetoSelecionado.PrimitivaTipo == PrimitiveType.LineLoop){
          objetoSelecionado.PrimitivaTipo = PrimitiveType.LineStrip;
        } else {
          objetoSelecionado.PrimitivaTipo = PrimitiveType.LineLoop;
        }
      }

      //Adicionar Vértices ao Polígono
      else if (e.Key == Key.Space){
        if(estaSelecionado == true){
          objetoId = Utilitario.charProximo(objetoId);
          obj_Poligno =  new Poligono(objetoId, null);
          obj_Poligno.PrimitivaTipo = PrimitiveType.LineLoop;
          obj_Poligno.PontosAdicionar(new Ponto4D(mouseX, mouseY));
          obj_Poligno.PontosAdicionar(new Ponto4D(mouseX, mouseY));
          obj_Poligno.ObjetoCor.CorR = 255; obj_Poligno.ObjetoCor.CorG = 255; obj_Poligno.ObjetoCor.CorB = 255;
          if(objetoSelecionado != null){
            objetoSelecionado.FilhoAdicionar(obj_Poligno);
          }else{
            objetosLista.Add(obj_Poligno);
          }
          objetoSelecionado = obj_Poligno;
          estaSelecionado = false;
        } else {
          objetoSelecionado.PontosRemoverUltimo();
          objetoSelecionado.PontosAdicionar(new Ponto4D(mouseX, mouseY));
          objetoSelecionado.PontosAdicionar(new Ponto4D(mouseX, mouseY));
        }
      }

      else if (objetoSelecionado != null)
      {
        if (e.Key == Key.M)
          Console.WriteLine(objetoSelecionado.Matriz);
        else if (e.Key == Key.P)
          Console.WriteLine(objetoSelecionado);
        else if (e.Key == Key.I)
          objetoSelecionado.AtribuirIdentidade();
        //TODO: não está atualizando a BBox com as transformações geométricas
        else if (e.Key == Key.Left)
          objetoSelecionado.TranslacaoXYZ(-10, 0, 0);
        else if (e.Key == Key.Right)
          objetoSelecionado.TranslacaoXYZ(10, 0, 0);
        else if (e.Key == Key.Up)
          objetoSelecionado.TranslacaoXYZ(0, 10, 0);
        else if (e.Key == Key.Down)
          objetoSelecionado.TranslacaoXYZ(0, -10, 0);
        else if (e.Key == Key.PageUp)
          objetoSelecionado.EscalaXYZ(2, 2, 2);
        else if (e.Key == Key.PageDown)
          objetoSelecionado.EscalaXYZ(0.5, 0.5, 0.5);
        else if (e.Key == Key.Home)
          objetoSelecionado.EscalaXYZBBox(0.5, 0.5, 0.5);
        else if (e.Key == Key.End)
          objetoSelecionado.EscalaXYZBBox(2, 2, 2);
        else if (e.Key == Key.Number1)
          objetoSelecionado.Rotacao(10);
        else if (e.Key == Key.Number2)
          objetoSelecionado.Rotacao(-10);
        else if (e.Key == Key.Number3)
          objetoSelecionado.RotacaoZBBox(10);
        else if (e.Key == Key.Number4)
          objetoSelecionado.RotacaoZBBox(-10);
        else if (e.Key == Key.Number9)
          objetoSelecionado = null;                     // desmarcar objeto selecionado
        else
          Console.WriteLine(" __ Tecla não implementada.");
      }
      else
        Console.WriteLine(" __ Tecla não implementada.");
    }

    //TODO: não está considerando o NDC
    protected override void OnMouseMove(MouseMoveEventArgs e)
    {
      mouseX = e.Position.X; mouseY = 600 - e.Position.Y; // Inverti eixo Y
      if (obj_Poligno != null)
      {
        obj_Poligno.PontosUltimo().X = mouseX;
        obj_Poligno.PontosUltimo().Y = mouseY;
      }
    }

#if CG_Gizmo
    private void Sru3D()
    {
      GL.LineWidth(1);
      GL.Begin(PrimitiveType.Lines);
      // GL.Color3(1.0f,0.0f,0.0f);
      GL.Color3(Convert.ToByte(255), Convert.ToByte(0), Convert.ToByte(0));
      GL.Vertex3(0, 0, 0); GL.Vertex3(200, 0, 0);
      // GL.Color3(0.0f,1.0f,0.0f);
      GL.Color3(Convert.ToByte(0), Convert.ToByte(255), Convert.ToByte(0));
      GL.Vertex3(0, 0, 0); GL.Vertex3(0, 200, 0);
      // GL.Color3(0.0f,0.0f,1.0f);
      GL.Color3(Convert.ToByte(0), Convert.ToByte(0), Convert.ToByte(255));
      GL.Vertex3(0, 0, 0); GL.Vertex3(0, 0, 200);
      GL.End();
    }
#endif
  }
  class Program
  {
    static void Main(string[] args)
    {
      ToolkitOptions.Default.EnableHighResolution = false;
      Mundo window = Mundo.GetInstance(600, 600);
      window.Title = "CG_N3";
      window.Run(1.0 / 60.0);
    }
  }
}
