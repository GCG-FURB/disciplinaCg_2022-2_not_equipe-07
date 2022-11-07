/**
  Autor: Dalton Solano dos Reis
**/

//#define CG_Privado // código do professor.
#define CG_Gizmo  // debugar gráfico.
#define CG_Debug // debugar texto.
#define CG_OpenGL // render OpenGL.
//#define CG_DirectX // render DirectX.

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
        private bool mouseMoverPto = false;
        private SegReta obj_SegReta1;
        private SegReta obj_SegReta2;
        private SegReta obj_SegReta3;
        private Spline obj_Spline;
        private Ponto4D pto1;
        private Ponto4D pto2;
        private Ponto4D pto3;
        private Ponto4D pto4;
        private Ponto4D ptoSelecionado;
        private int qtdPontos = 72;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            camera.xmin = -400; camera.xmax = 400; camera.ymin = -400; camera.ymax = 400;

            Console.WriteLine(" --- Ajuda / Teclas: ");
            Console.WriteLine(" [  H     ] mostra teclas usadas. ");

            pto1 = new Ponto4D(100, 100, 0);
            pto2 = new Ponto4D(100, -100, 0);
            pto3 = new Ponto4D(-100, -100, 0);
            pto4 = new Ponto4D(-100, 100, 0);

            objetoId = Utilitario.charProximo(objetoId);
            obj_SegReta1 = new SegReta(objetoId, null, pto1, pto2);
            obj_SegReta1.ObjetoCor.CorR = 0; obj_SegReta1.ObjetoCor.CorG = 255; obj_SegReta1.ObjetoCor.CorB = 255;
            obj_SegReta1.PrimitivaTipo = PrimitiveType.Lines;
            obj_SegReta1.PrimitivaTamanho = 2;
            objetosLista.Add(obj_SegReta1);

            objetoId = Utilitario.charProximo(objetoId);
            obj_SegReta2 = new SegReta(objetoId, null, pto1, pto4);
            obj_SegReta2.ObjetoCor.CorR = 0; obj_SegReta2.ObjetoCor.CorG = 255; obj_SegReta2.ObjetoCor.CorB = 255;
            obj_SegReta2.PrimitivaTipo = PrimitiveType.Lines;
            obj_SegReta2.PrimitivaTamanho = 2;
            objetosLista.Add(obj_SegReta2);

            objetoId = Utilitario.charProximo(objetoId);
            obj_SegReta3 = new SegReta(objetoId, null, pto3, pto4);
            obj_SegReta3.ObjetoCor.CorR = 0; obj_SegReta3.ObjetoCor.CorG = 255; obj_SegReta3.ObjetoCor.CorB = 255;
            obj_SegReta3.PrimitivaTipo = PrimitiveType.Lines;
            obj_SegReta3.PrimitivaTamanho = 2;
            objetosLista.Add(obj_SegReta3);

            objetoId = Utilitario.charProximo(objetoId);
            obj_Spline = new Spline(objetoId, null, pto2, pto1, pto4, pto3, qtdPontos);
            obj_Spline.ObjetoCor.CorR = 255; obj_Spline.ObjetoCor.CorG = 255; obj_Spline.ObjetoCor.CorB = 0;
            obj_Spline.PrimitivaTipo = PrimitiveType.Points;
            obj_Spline.PrimitivaTamanho = 2;
            objetosLista.Add(obj_Spline);

            ptoSelecionado = pto1;

#if CG_OpenGL
            GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
#endif
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
#if CG_OpenGL
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(camera.xmin, camera.xmax, camera.ymin, camera.ymax, camera.zmin, camera.zmax);
#endif
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
#if CG_OpenGL
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
#endif
#if CG_Gizmo
            Sru3D();
#endif
            for (var i = 0; i < objetosLista.Count; i++)
                objetosLista[i].Desenhar();
#if CG_Gizmo
            if (bBoxDesenhar && (objetoSelecionado != null))
                objetoSelecionado.BBox.Desenhar();
#endif
            this.SwapBuffers();
        }

        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.H)
                Utilitario.AjudaTeclado();
            else if (e.Key == Key.Escape)
                Exit();
            else if (e.Key == Key.V)
                mouseMoverPto = !mouseMoverPto;   //TODO: falta atualizar a BBox do objeto

            else if (e.Key == Key.Number1)
                ptoSelecionado = pto1;

            else if (e.Key == Key.Number2)
                ptoSelecionado = pto2;

            else if (e.Key == Key.Number3)
                ptoSelecionado = pto3;

            else if (e.Key == Key.Number4)
                ptoSelecionado = pto4;

            else if (e.Key == Key.C)
                ptoSelecionado.Y += 1;

            else if (e.Key == Key.B)
                ptoSelecionado.Y -= 1;

            else if (e.Key == Key.E)
                ptoSelecionado.X -= 1;

            else if (e.Key == Key.D)
                ptoSelecionado.X += 1;

            else if (e.Key == Key.Plus)
            {
                obj_Spline.qtdPontos += 1;
            }

            else if (e.Key == Key.Minus)
            {
                if ((obj_Spline.qtdPontos - 1) > 1)
                {
                    obj_Spline.qtdPontos -= 1;
                }
            }

            else if (e.Key == Key.R)
            {
                // resetar valores
                pto1 = new Ponto4D(100, 100, 0);
                pto2 = new Ponto4D(100, -100, 0);
                pto3 = new Ponto4D(-100, -100, 0);
                pto4 = new Ponto4D(-100, 100, 0);

                // Spline
                obj_Spline.PontosAlterar(pto1, 1);
                obj_Spline.PontosAlterar(pto2, 0);
                obj_Spline.PontosAlterar(pto3, 3);
                obj_Spline.PontosAlterar(pto4, 2);

                // Seg Reta
                obj_SegReta1.PontosAlterar(pto1, 0);
                obj_SegReta1.PontosAlterar(pto2, 1);

                obj_SegReta2.PontosAlterar(pto1, 0);
                obj_SegReta2.PontosAlterar(pto4, 1);

                obj_SegReta3.PontosAlterar(pto3, 0);
                obj_SegReta3.PontosAlterar(pto4, 1);

                obj_Spline.qtdPontos = 72;
            }

        }

        //TODO: não está considerando o NDC
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            mouseX = e.Position.X; mouseY = 600 - e.Position.Y; // Inverti eixo Y
            if (mouseMoverPto && (objetoSelecionado != null))
            {
                objetoSelecionado.PontosUltimo().X = mouseX;
                objetoSelecionado.PontosUltimo().Y = mouseY;
            }
        }

#if CG_Gizmo
        private void Sru3D()
        {
#if CG_OpenGL
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
#endif
        }
#endif
    }
    class Program
    {
        static void Main(string[] args)
        {
            ToolkitOptions.Default.EnableHighResolution = false;
            Mundo window = Mundo.GetInstance(600, 600);
            window.Title = "CG_N2_6";
            window.Run(1.0 / 60.0);
        }
    }
}
