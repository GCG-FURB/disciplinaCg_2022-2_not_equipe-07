
#define CG_Debug
#define CG_OpenGL
// #define CG_DirectX

using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{
    internal class Circulo : ObjetoGeometria
    {
        public Circulo(
            char rotulo, 
            Objeto paiRef, 
            Ponto4D ptoCentral,
            double raio,
            int qtdPontos
             ) : base(rotulo, paiRef)
        {
            // Distancia entre os pontos
            double distanciaPontos = 360 / qtdPontos;
            double anguloAnterior = 0; 
            // calcular pontos
            for (int i = 0; i < qtdPontos; i++)
            {   
                Ponto4D ponto = Matematica.GerarPtosCirculo(anguloAnterior, raio);
                base.PontosAdicionar(this.DeslocarPonto(ponto, ptoCentral));
                anguloAnterior += distanciaPontos;
            };
        }

        private Ponto4D DeslocarPonto(Ponto4D ponto, Ponto4D ptoCentral){
            ponto.X += ptoCentral.X;
            ponto.Y += ptoCentral.Y;
            return ponto;
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

        //TODO: melhorar para exibir não só a lista de pontos (geometria), mas também a topologia ... poderia ser listado estilo OBJ da Wavefrom
#if CG_Debug
        public override string ToString()
        {   
            string retorno;
            retorno = "__ Objeto Circulo: " + base.rotulo + "\n";
            for (var i = 0; i < pontosLista.Count; i++)
            {
                retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
            }
            return (retorno);
        }
#endif

    }
}