using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using CG_Biblioteca;

namespace gcgcg
{

    internal class CasaTabuleiro : ObjetoGeometria
    {
        public Ponto4D PontoCentro { get; private set; }

        public int PosX { get; private set; }
        public int PosY { get; private set; }

        public CasaTabuleiro(Ponto4D pontoCentro, double tamanho, double altura, int posX, int posY, char rotulo, Objeto paiRef) : base(rotulo, paiRef)
        {
            PontoCentro = pontoCentro;

            PosX = posX;
            PosY = posY;

            var minX = pontoCentro.X - tamanho;
            var maxX = pontoCentro.X + tamanho;
            var minY = pontoCentro.Y - altura;
            var maxY = pontoCentro.Y + altura;
            var minZ = pontoCentro.Z - tamanho;
            var maxZ = pontoCentro.Z + tamanho;

            base.PontosAdicionar(new Ponto4D(minX, minY, minZ)); // [0]
            base.PontosAdicionar(new Ponto4D(maxX, minY, minZ)); // [1]
            base.PontosAdicionar(new Ponto4D(maxX, minY, maxZ)); // [2]
            base.PontosAdicionar(new Ponto4D(minX, minY, maxZ)); // [3]
            base.PontosAdicionar(new Ponto4D(minX, maxY, maxZ)); // [4]
            base.PontosAdicionar(new Ponto4D(maxX, maxY, maxZ)); // [5]
            base.PontosAdicionar(new Ponto4D(maxX, maxY, minZ)); // [6]
            base.PontosAdicionar(new Ponto4D(minX, maxY, minZ)); // [7]
        }

        protected override void DesenharObjeto()
        {
            GL.Begin(PrimitiveType.Quads);

            GL.Normal3(0, -1, 0); // Face de baixo
            GL.Vertex3(base.pontosLista[0].X, base.pontosLista[0].Y, base.pontosLista[0].Z);
            GL.Vertex3(base.pontosLista[1].X, base.pontosLista[1].Y, base.pontosLista[1].Z);
            GL.Vertex3(base.pontosLista[2].X, base.pontosLista[2].Y, base.pontosLista[2].Z);
            GL.Vertex3(base.pontosLista[3].X, base.pontosLista[3].Y, base.pontosLista[3].Z);

            GL.Normal3(0, 1, 0); // Face de cima 
            GL.Vertex3(base.pontosLista[4].X, base.pontosLista[4].Y, base.pontosLista[4].Z);
            GL.Vertex3(base.pontosLista[5].X, base.pontosLista[5].Y, base.pontosLista[5].Z);
            GL.Vertex3(base.pontosLista[6].X, base.pontosLista[6].Y, base.pontosLista[6].Z);
            GL.Vertex3(base.pontosLista[7].X, base.pontosLista[7].Y, base.pontosLista[7].Z);

            GL.End();

        }

        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto Cubo: " + base.rotulo + "\n";
            for (var i = 0; i < pontosLista.Count; i++)
            {
                retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
            }
            return (retorno);
        }

    }
}
