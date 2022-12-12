
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace gcgcg
{
    internal class Tabuleiro : ObjetoGeometria
    {

        private CasaTabuleiro[,] _casas = new CasaTabuleiro[8, 8];
        private List<Peca> _pecas = new List<Peca>();

        public CasaTabuleiro[,] Casas { get { return _casas; } }
        public List<Peca> Pecas { get { return _pecas; } }
        public double TamanhoCasa { get; private set; }
        public double Altura { get; private set; }
        public Ponto4D CentroTabuleiro { get; private set; }

        public Tabuleiro(Ponto4D pontoCentro, double tamanho, double altura, char rotulo, Objeto paiRef) : base(rotulo, paiRef)
        {
            CentroTabuleiro = pontoCentro;
            Altura = altura;

            var minX = pontoCentro.X - tamanho;
            var maxX = pontoCentro.X + tamanho;
            var minY = pontoCentro.Y - altura;
            var maxY = pontoCentro.Y + altura;
            var minZ = pontoCentro.Z - tamanho;
            var maxZ = pontoCentro.Z + tamanho;

            PontosAdicionar(new Ponto4D(minX, minY, maxZ)); // [0] 
            PontosAdicionar(new Ponto4D(maxX, minY, maxZ)); // [1] 
            PontosAdicionar(new Ponto4D(maxX, maxY, maxZ)); // [2] 
            PontosAdicionar(new Ponto4D(minX, maxY, maxZ)); // [3] 
            PontosAdicionar(new Ponto4D(minX, minY, minZ)); // [4] 
            PontosAdicionar(new Ponto4D(maxX, minY, minZ)); // [5] 
            PontosAdicionar(new Ponto4D(maxX, maxY, minZ)); // [6] 
            PontosAdicionar(new Ponto4D(minX, maxY, minZ)); // [7] 

            GerarCasas(pontoCentro, tamanho, altura);

            ObjetoCor = new Cor(0, 0, 0);
        }

        private void GerarCasas(Ponto4D pontoCentro, double tamanho, double altura)
        {
            TamanhoCasa = tamanho / 8;

            var xBase = pontoCentro.X - (7 * TamanhoCasa);
            var yBase = pontoCentro.Y;
            var zBase = pontoCentro.Z - (7 * TamanhoCasa);

            for (var x = 0; x < 8; x++)
            {
                var zAtual = zBase + (2 * TamanhoCasa * x);
                var xAtual = xBase;

                for (var y = 0; y < 8; y++)
                {
                    var isEspacoPreto = (y % 2 == 0 && x % 2 == 0) || (y % 2 == 1 && x % 2 == 1);
                    var corCasa = isEspacoPreto ? new Cor(0, 0, 0) : new Cor(255, 255, 255);

                    var pontoCentroCasa = new Ponto4D(xAtual, yBase, zAtual);

                    var casaNova = new CasaTabuleiro(pontoCentroCasa, TamanhoCasa, altura, x, y, Utilitario.charProximo('@'), this)
                    {
                        ObjetoCor = corCasa
                    };

                    if (isEspacoPreto && (y <= 2 || y >= 5))
                    {
                        var peca = CriarPecaTabuleiro(pontoCentroCasa, TamanhoCasa, altura, x, y);
                        _pecas.Add(peca);
                        FilhoAdicionar(peca);
                    }

                    _casas[x, y] = casaNova;
                    FilhoAdicionar(casaNova);

                    xAtual += TamanhoCasa * 2;
                }
            }
        }

        private Peca CriarPecaTabuleiro(Ponto4D centroCasa, double tamanhoCasa, double alturaCasa, int posX, int posY)
        {
            var centroPeca = new Ponto4D(centroCasa.X, centroCasa.Y + alturaCasa, centroCasa.Z);

            var pecaJogadorUm = posY <= 2;
            var cor = pecaJogadorUm ? new Cor(0, 0, 255) : new Cor(255, 0, 0);
            var pecaNova = new Peca(posX, posY, tamanhoCasa * 0.7, alturaCasa / 5, centroPeca, Utilitario.charProximo('@'), this)
            {
                ObjetoCor = cor,
                IsPecaJogadorUm = pecaJogadorUm
            };

            return pecaNova;
        }

        protected override void DesenharObjeto()
        {
            GL.Begin(PrimitiveType.Quads);

            // Face da frente
            GL.Normal3(0, 0, 1);
            GL.Vertex3(pontosLista[0].X, pontosLista[0].Y, pontosLista[0].Z);    // PtoA
            GL.Vertex3(pontosLista[1].X, pontosLista[1].Y, pontosLista[1].Z);    // PtoB
            GL.Vertex3(pontosLista[2].X, pontosLista[2].Y, pontosLista[2].Z);    // PtoC
            GL.Vertex3(pontosLista[3].X, pontosLista[3].Y, pontosLista[3].Z);    // PtoD
            // Face do fundo
            GL.Normal3(0, 0, -1);
            GL.Vertex3(pontosLista[4].X, pontosLista[4].Y, pontosLista[4].Z);    // PtoE
            GL.Vertex3(pontosLista[7].X, pontosLista[7].Y, pontosLista[7].Z);    // PtoH
            GL.Vertex3(pontosLista[6].X, pontosLista[6].Y, pontosLista[6].Z);    // PtoG
            GL.Vertex3(pontosLista[5].X, pontosLista[5].Y, pontosLista[5].Z);    // PtoF
            // Face da direita
            GL.Normal3(1, 0, 0);
            GL.Vertex3(pontosLista[1].X, pontosLista[1].Y, pontosLista[1].Z);    // PtoB
            GL.Vertex3(pontosLista[5].X, pontosLista[5].Y, pontosLista[5].Z);    // PtoF
            GL.Vertex3(pontosLista[6].X, pontosLista[6].Y, pontosLista[6].Z);    // PtoG
            GL.Vertex3(pontosLista[2].X, pontosLista[2].Y, pontosLista[2].Z);    // PtoC
            // Face da esquerda
            GL.Normal3(-1, 0, 0);
            GL.Vertex3(pontosLista[0].X, pontosLista[0].Y, pontosLista[0].Z);    // PtoA
            GL.Vertex3(pontosLista[3].X, pontosLista[3].Y, pontosLista[3].Z);    // PtoD
            GL.Vertex3(pontosLista[7].X, pontosLista[7].Y, pontosLista[7].Z);    // PtoH
            GL.Vertex3(pontosLista[4].X, pontosLista[4].Y, pontosLista[4].Z);    // PtoE

            GL.End();

        }

        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto Cubo: " + rotulo + "\n";
            for (var i = 0; i < pontosLista.Count; i++)
            {
                retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
            }
            return (retorno);
        }

    }
}
