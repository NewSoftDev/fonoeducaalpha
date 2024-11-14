using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    // Canvas e Botões
    public Canvas menuPrincipal, menuEscolhaJogo, menuTipoJogo, JogoRodando;
    public Button botaoNovoJogo, botaofimdejogo, botaoJogoMemoria, botaoJogoRelacione;
    public Button botaoCadastro, botaoNovoPaciente, botaoEscolherPaciente, botaoCadastroProfissional, botaoCadastroPaciente;
    public Button botaoJogoFeV, botaoJogoDeT;

    // Referência aos outros modulos
    public Cadastro cadastro;
    public MemoryGame memoryGame;
    public RelacioneGame relacioneGame;

    void Start()
    {
        // Inicializa os botões
        botaoNovoJogo.onClick.AddListener(() => ShowMenu(menuEscolhaJogo, false, 0));
        botaoJogoMemoria.onClick.AddListener(() => ShowMenu(JogoRodando, true, 1));
        botaoJogoRelacione.onClick.AddListener(() => ShowMenu(JogoRodando, true, 2));
        botaoJogoFeV.onClick.AddListener(() => IniciarJogo("FV"));
        botaoJogoDeT.onClick.AddListener(() => IniciarJogo("DT"));
        botaofimdejogo.onClick.AddListener(() => FimDeJogo());
        botaoCadastro.onClick.AddListener(() => cadastro.AbrirMenuCadastro());
        botaoCadastroProfissional.onClick.AddListener(() => cadastro.CadastroProfissional());
        botaoCadastroPaciente.onClick.AddListener(() => cadastro.CadastroPaciente());
        botaoNovoPaciente.onClick.AddListener(() => cadastro.NovoPaciente());
        botaoEscolherPaciente.onClick.AddListener(() => cadastro.EscolherPaciente());

        // Oculta os botões de paciente no início
        AtualizarVisibilidadeBotoesPaciente(false);

        // Exibe o menu principal
        ShowMenu(menuPrincipal, false, 0);
    }

    // Função para exibir os menus
    public void ShowMenu(Canvas menu, bool JogaInicio, int TipoJogo)
    {
        menuPrincipal.sortingOrder = 0;
        menuEscolhaJogo.sortingOrder = 0;
        menuTipoJogo.sortingOrder = 0;
        JogoRodando.sortingOrder = 0;

        if (JogaInicio)
        {
            JogoRodando.sortingOrder = 1;
            if (TipoJogo == 1)
            {

                memoryGame.ComecaJogoMemoria();  // Começa o jogo de memória }
            }
            else
            {
                relacioneGame.ComecaJogoRelacione();  // Começa o jogo de memória }
            }
        }
        else
        {
            menu.sortingOrder = 1;
        }
    }
    private void IniciarJogo(string letras)
    {
        relacioneGame.DefinirLetrasSelecionadas(letras);
        memoryGame.DefinirLetrasSelecionadas(letras);  // Definindo as letras
        ShowMenu(menuTipoJogo, false, 0);
    }

    // Função de fim de jogo
    public void FimDeJogo()
    {
        ShowMenu(menuPrincipal, false, 0);
    }

    // Função para atualizar a visibilidade dos botões de paciente
    public void AtualizarVisibilidadeBotoesPaciente(bool visivel)
    {
        botaoNovoPaciente.gameObject.SetActive(visivel);
    }
}