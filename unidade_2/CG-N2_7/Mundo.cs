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
        private Circulo obj_CirculoPequeno;
        private Circulo obj_CirculoGrande;
        private Retangulo obj_Retangulo;
        private Ponto4D ptoCentral;
        private Ponto obj_Pto;
        private Ponto4D ptoInf;
        private Ponto4D ptoSup;
        private Ponto4D ptoSelecionado;

        public bool dentro_quadrado(Retangulo obj_Retangulo, Ponto4D ponto)
        {
            return (ponto.X <= obj_Retangulo.BBox.obterMenorX ||
                    ponto.X >= obj_Retangulo.BBox.obterMaiorX ||
                    ponto.Y <= obj_Retangulo.BBox.obterMenorY ||
                    ponto.Y >= obj_Retangulo.BBox.obterMaiorY);
        }
        public bool dentro_circulo(Circulo obj_Circulo, Ponto4D ponto, double raio)
        {
            double dist = Math.Sqrt(Math.Pow(obj_Circulo.BBox.obterCentro.X - ponto.X, 2) +
                                    Math.Pow(obj_Circulo.BBox.obterCentro.Y - ponto.Y, 2));
            return dist <= raio;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            camera.xmin = -400; camera.xmax = 400; camera.ymin = -400; camera.ymax = 400;

            Console.WriteLine(" --- Ajuda / Teclas: ");
            Console.WriteLine(" [  H     ] mostra teclas usadas. ");

            ptoCentral = new Ponto4D(100, 100);

            objetoId = Utilitario.charProximo(objetoId);
            obj_Pto = new Ponto(objetoId, null, ptoCentral);
            obj_Pto.ObjetoCor.CorR = 0; obj_Pto.ObjetoCor.CorG = 0; obj_Pto.ObjetoCor.CorB = 0;
            obj_Pto.PrimitivaTamanho = 5;
            obj_Pto.PrimitivaTipo = PrimitiveType.Points;
            objetosLista.Add(obj_Pto);

            objetoId = Utilitario.charProximo(objetoId);
            obj_CirculoPequeno = new Circulo(objetoId, null, ptoCentral, 100, 72);
            obj_CirculoPequeno.ObjetoCor.CorR = 0; obj_CirculoPequeno.ObjetoCor.CorG = 0; obj_CirculoPequeno.ObjetoCor.CorB = 0;
            obj_CirculoPequeno.PrimitivaTamanho = 2;
            obj_CirculoPequeno.PrimitivaTipo = PrimitiveType.LineLoop;
            objetosLista.Add(obj_CirculoPequeno);

            ptoSelecionado = ptoCentral;

            objetoId = Utilitario.charProximo(objetoId);
            obj_CirculoGrande = new Circulo(objetoId, null, ptoCentral, 200, 72);
            obj_CirculoGrande.ObjetoCor.CorR = 0; obj_CirculoGrande.ObjetoCor.CorG = 0; obj_CirculoGrande.ObjetoCor.CorB = 0;
            obj_CirculoGrande.PrimitivaTamanho = 2;
            obj_CirculoGrande.PrimitivaTipo = PrimitiveType.LineLoop;
            objetosLista.Add(obj_CirculoGrande);

            ptoSup = Matematica.GerarPtosCirculo(45, 200);
            ptoSup.X += ptoCentral.X;
            ptoSup.Y += ptoCentral.Y;

            ptoInf = Matematica.GerarPtosCirculo(225, 200);
            ptoInf.X += ptoCentral.X;
            ptoInf.Y += ptoCentral.Y;

            objetoId = Utilitario.charProximo(objetoId);
            obj_Retangulo = new Retangulo(objetoId, null, ptoInf, ptoSup);
            obj_Retangulo.ObjetoCor.CorR = 255; obj_Retangulo.ObjetoCor.CorG = 0; obj_Retangulo.ObjetoCor.CorB = 255;
            obj_Retangulo.PrimitivaTamanho = 2;
            objetosLista.Add(obj_Retangulo);


#if CG_OpenGL
            GL.ClearColor(1f, 1f, 1f, 1.0f);
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

            obj_Retangulo.BBox.Desenhar();

            if (dentro_quadrado(obj_Retangulo, obj_CirculoPequeno.BBox.obterCentro))
            {
                obj_Retangulo.ObjetoCor.CorR = 255; obj_Retangulo.ObjetoCor.CorG = 255; obj_Retangulo.ObjetoCor.CorB = 0;

                if (!dentro_circulo(obj_CirculoGrande, obj_CirculoPequeno.BBox.obterCentro, 200))
                {
                    obj_Retangulo.ObjetoCor.CorR = 0; obj_Retangulo.ObjetoCor.CorG = 255; obj_Retangulo.ObjetoCor.CorB = 255;
                }
            }
            else
            {
                obj_Retangulo.ObjetoCor.CorR = 255; obj_Retangulo.ObjetoCor.CorG = 0; obj_Retangulo.ObjetoCor.CorB = 255;
            }

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
            else
                Console.WriteLine(" __ Tecla não implementada.");
        }

        //TODO: não está considerando o NDC
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            mouseX = e.Position.X; mouseY = (e.Position.Y * -1);
            if (mouseMoverPto && (ptoCentral != null))
            {
                ptoCentral.X = mouseX - 300;
                ptoCentral.Y = mouseY + 300;
                obj_CirculoPequeno.AtualizarPtoCentral(ptoCentral);
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
            Mundo window = Mundo.GetInstance(600, 600);
            window.Title = "CG_N2";
            window.Run(1.0 / 60.0);
        }
    }
}
