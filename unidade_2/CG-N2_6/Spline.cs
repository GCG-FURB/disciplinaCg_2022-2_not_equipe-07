using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{
    internal class Spline : ObjetoGeometria
    {

        public double qtdPontos { get; set; }
        
        public Spline(char rotulo, Objeto paiRef, Ponto4D ponto1, Ponto4D ponto2, Ponto4D ponto3, Ponto4D ponto4, int qtdPontos) : base(rotulo, paiRef)
        {
            PrimitivaTipo = PrimitiveType.LineStrip;
            
            base.PontosAdicionar(ponto1);
            base.PontosAdicionar(ponto2);
            base.PontosAdicionar(ponto3);
            base.PontosAdicionar(ponto4);

            this.qtdPontos = qtdPontos;
        }

        protected override void DesenharObjeto()
        {
            Ponto4D ponto1 = pontosLista[0];
            Ponto4D ponto2 = pontosLista[1];
            Ponto4D ponto3 = pontosLista[2];
            Ponto4D ponto4 = pontosLista[3];

            GL.Begin(PrimitivaTipo);
            
            GL.Vertex2(ponto1.X, ponto1.Y);
            for (double i = 0; i < qtdPontos; i++)
            {
                double t = i/qtdPontos;
                Ponto4D p0 = FuncaoSpline(ponto1, ponto2, t);
                Ponto4D p1 = FuncaoSpline(ponto2, ponto3, t);
                Ponto4D p2 = FuncaoSpline(ponto3, ponto4, t);
                Ponto4D p01 = FuncaoSpline(p0, p1, t);
                Ponto4D p12 = FuncaoSpline(p1, p2, t);
                Ponto4D resultado = FuncaoSpline(p01, p12, t);

                GL.Vertex2(resultado.X, resultado.Y);
            }
            GL.Vertex2(ponto4.X, ponto4.Y);
            
            GL.End();
        }

        private Ponto4D FuncaoSpline(Ponto4D ponto1, Ponto4D ponto2, double t)
        {
            double pontoX = ponto1.X + (ponto2.X - ponto1.X) * t;
            double pontoY = ponto1.Y + (ponto2.Y - ponto1.Y) * t;

            return new Ponto4D(pontoX, pontoY);
        }

        
    }
}