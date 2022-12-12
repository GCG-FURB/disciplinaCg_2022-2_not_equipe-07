using CG_Biblioteca;
using OpenTK.Input;
using System.Collections.Generic;
using System.Linq;

namespace gcgcg
{
    internal class DamaController
    {

        private readonly Tabuleiro _tabuleiro;
        private readonly List<Peca> _pecas;
        public Cubo Seletor { get; private set; }
        private int SeletorX = 0;
        private int SeletorY = 0;

        private const int Limite = 7;

        private Peca pecaSelecionada = null;
        private Dictionary<CasaTabuleiro, Peca> MovimentosPossiveis = new Dictionary<CasaTabuleiro, Peca>();

        public DamaController(Tabuleiro tabuleiro)
        {
            _tabuleiro = tabuleiro;
            _pecas = tabuleiro.Pecas;

            GerarSeletor();
        }

        public bool HandleEntrada(Key tecla)
        {
            Ponto4D pontoMover = null;

            switch (tecla)
            {
                case Key.D:
                    if (SeletorX < Limite)
                        SeletorX++;
                    pontoMover = _tabuleiro.Casas[SeletorX, SeletorY].PontoCentro;
                    break;
                case Key.A:
                    if (SeletorX > 0)
                        SeletorX--;
                    pontoMover = _tabuleiro.Casas[SeletorX, SeletorY].PontoCentro;
                    break;
                case Key.W:
                    if (SeletorY < Limite)
                        SeletorY++;
                    pontoMover = _tabuleiro.Casas[SeletorX, SeletorY].PontoCentro;
                    break;
                case Key.S:
                    if (SeletorY > 0)
                        SeletorY--;
                    pontoMover = _tabuleiro.Casas[SeletorX, SeletorY].PontoCentro;
                    break;
                case Key.Enter:
                    if (pecaSelecionada == null)
                        SelecionarPeca();
                    else
                        DecelecionarPeca();
                    return true;
            }

            if (pontoMover != null)
            {
                pontoMover = _tabuleiro.Casas[SeletorX, SeletorY].PontoCentro;
                Seletor.MoverPara(pontoMover);

                return true;
            }

            return false;
        }

        private void SelecionarPeca()
        {
            pecaSelecionada = _tabuleiro.Pecas.Where(p => p.PosX == SeletorX && p.PosY == SeletorY).FirstOrDefault();
            if (pecaSelecionada != null)
            {
                pecaSelecionada.Translacao(pecaSelecionada.Altura * 3, 'y');
                GerarMovimentosPossiveis();
            }

        }

        private void GerarMovimentosPossiveis()
        {
            VerificarDiagonalFrenteEsquerda(pecaSelecionada.PosX, pecaSelecionada.PosY);
            VerificarDiagonalFrenteDireita(pecaSelecionada.PosX, pecaSelecionada.PosY);
            VerificarDiagonalTrasEsquerda(pecaSelecionada.PosX, pecaSelecionada.PosY);
            VerificarDiagonaTrasDireita(pecaSelecionada.PosX, pecaSelecionada.PosY);

            MostrarMovimentosDisponiveis();
        }

        private void VerificarDiagonalFrenteDireita(int posX, int posY, Peca pulado = null)
        {
            var diagX = posX + 1;
            var diagY = posY + 1;

            if (diagX > Limite ||
                diagY > Limite ||
                !(pecaSelecionada.IsPecaJogadorUm || pecaSelecionada.IsRainha))
            {
                return;
            }

            var peca = _pecas.FirstOrDefault(p => p.PosX == diagX && p.PosY == diagY);
            if (peca == null)
            {
                MovimentosPossiveis.Add(_tabuleiro.Casas[diagX, diagY], pulado);
                return;
            }
            if (peca.IsPecaJogadorUm != pecaSelecionada.IsPecaJogadorUm && pulado == null)
            {
                VerificarDiagonalFrenteDireita(diagX, diagY, peca);
            }
        }

        private void VerificarDiagonalFrenteEsquerda(int posX, int posY, Peca pulado = null)
        {
            var diagX = posX - 1;
            var diagY = posY + 1;

            if (diagX < 0 ||
                diagY > Limite ||
                !(pecaSelecionada.IsPecaJogadorUm || pecaSelecionada.IsRainha))
            {
                return;
            }

            var peca = _pecas.FirstOrDefault(p => p.PosX == diagX && p.PosY == diagY);
            if (peca == null)
            {
                MovimentosPossiveis.Add(_tabuleiro.Casas[diagX, diagY], pulado);
                return;
            }
            if (peca.IsPecaJogadorUm != pecaSelecionada.IsPecaJogadorUm && pulado == null)
            {
                VerificarDiagonalFrenteEsquerda(diagX, diagY, peca);
            }
        }

        private void VerificarDiagonaTrasDireita(int posX, int posY, Peca pulado = null)
        {
            var diagX = posX + 1;
            var diagY = posY - 1;

            if (diagX > Limite ||
                diagY < 0 ||
                !(!pecaSelecionada.IsPecaJogadorUm || pecaSelecionada.IsRainha))
            {
                return;
            }

            var peca = _pecas.FirstOrDefault(p => p.PosX == diagX && p.PosY == diagY);
            if (peca == null)
            {
                MovimentosPossiveis.Add(_tabuleiro.Casas[diagX, diagY], pulado);
                return;
            }
            if (peca.IsPecaJogadorUm != pecaSelecionada.IsPecaJogadorUm && pulado == null)
            {
                VerificarDiagonaTrasDireita(diagX, diagY, peca);
            }
        }

        private void VerificarDiagonalTrasEsquerda(int posX, int posY, Peca pulado = null)
        {
            var diagX = posX - 1;
            var diagY = posY - 1;

            if (diagX < 0 ||
                diagY < 0 ||
                !(!pecaSelecionada.IsPecaJogadorUm || pecaSelecionada.IsRainha))
            {
                return;
            }

            var peca = _pecas.FirstOrDefault(p => p.PosX == diagX && p.PosY == diagY);
            if (peca == null)
            {
                MovimentosPossiveis.Add(_tabuleiro.Casas[diagX, diagY], pulado);
                return;
            }
            if (peca.IsPecaJogadorUm != pecaSelecionada.IsPecaJogadorUm && pulado == null)
            {
                VerificarDiagonalTrasEsquerda(diagX, diagY, peca);
            }
        }

        private void MostrarMovimentosDisponiveis()
        {
            foreach (var casa in MovimentosPossiveis)
            {
                casa.Key.ObjetoCor = new Cor(200, 0, 125);
            }
        }

        private void DecelecionarPeca()
        {
            if (SeletorX != pecaSelecionada.PosX || SeletorY != pecaSelecionada.PosY)
            {
                MoverPeca();
            }

            pecaSelecionada.Translacao(-pecaSelecionada.Altura * 3, 'y');
            pecaSelecionada = null;

            RemoverMovimentosDisponiveis();
        }

        private void RemoverMovimentosDisponiveis()
        {
            foreach (var casa in MovimentosPossiveis)
            {
                casa.Key.ObjetoCor = new Cor(0, 0, 0);
            }

            MovimentosPossiveis.Clear();
        }

        private void MoverPeca()
        {
            var casaMover = MovimentosPossiveis.Keys.FirstOrDefault(m => m.PosX == SeletorX && m.PosY == SeletorY);
            if (casaMover == null)
            {
                return;
            }

            var difX = casaMover.PontoCentro.X - pecaSelecionada.PosicaoAtual.X;
            var difZ = casaMover.PontoCentro.Z - pecaSelecionada.PosicaoAtual.Z;

            pecaSelecionada.Translacao(difX, 'x');
            pecaSelecionada.Translacao(difZ, 'z');

            pecaSelecionada.PosicaoAtual = casaMover.PontoCentro;
            pecaSelecionada.PosX = SeletorX;
            pecaSelecionada.PosY = SeletorY;

            if (MovimentosPossiveis[casaMover] != null)
            {
                var pecaMorta = MovimentosPossiveis[casaMover];

                _pecas.Remove(pecaMorta);
                _tabuleiro.FilhoRemover(pecaMorta);
            }

        }

        private void GerarSeletor()
        {
            var tamanho = _tabuleiro.TamanhoCasa * 1.2;
            var altura = _tabuleiro.Altura * 1.2;

            var pontoCentro = _tabuleiro.Casas[0, 0].PontoCentro;

            Seletor = new Cubo(pontoCentro, tamanho, altura, Utilitario.charProximo('@'), null)
            {
                ObjetoCor = new Cor(200, 200, 0, 100)
            };

            _tabuleiro.FilhoAdicionar(Seletor);
        }

    }
}
