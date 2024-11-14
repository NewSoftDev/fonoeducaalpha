using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Cadastro : MonoBehaviour
{
    public Canvas menuCadastroProfissional, menuCadastroPaciente, menuEscolhaCadastro;

    // Inputs de cadastro do profissional
    public TMP_InputField inputNomeProfissional, inputEmailProfissional, inputCodigoProfissional;

    // Inputs de cadastro do paciente
    public TMP_InputField inputNomePaciente, inputDocumentoPaciente, inputIdadePaciente;

    // Textos de informações do menu inicial
    public Text textoNomeProfissional, textoNomePaciente;

    // Dropdown de distúrbios e de escolha de paciente
    public TMP_Dropdown dropdownDisturbio, dropdownEscolherPaciente;

    // Variáveis de controle
    private bool isProfissionalCadastrado = false;

    // Lista de pacientes cadastrados
    private List<Paciente> listaPacientes = new List<Paciente>();

    // Variável para o paciente selecionado
    private Paciente pacienteSelecionado;

    public MenuController menuController;

    public Button botaoConfirmarCadProfissional, botaoConfirmarCadPaciente, botaoEscolherPaciente;

    private void Start()
    {
        // Inicializa o dropdown, os textos e o botão "Escolher Paciente" como ocultos
        dropdownEscolherPaciente.gameObject.SetActive(false);
        textoNomeProfissional.gameObject.SetActive(false);
        textoNomePaciente.gameObject.SetActive(false);
        botaoEscolherPaciente.gameObject.SetActive(false);

        botaoConfirmarCadProfissional.onClick.AddListener(() => ConfirmarCadastro(true));
        botaoConfirmarCadPaciente.onClick.AddListener(() => ConfirmarCadastro(false));

        // Inicializa a lista de disturbios para escolha
        InicializarDropdownDisturbios();

        // Atualiza a visibilidade do botão "Escolher Paciente" baseado no número inicial de pacientes cadastrados
        AtualizarVisibilidadeBotaoEscolherPaciente();
    }

    public void AbrirMenuCadastro()
    {
        menuController.menuPrincipal.sortingOrder = 0;
        menuEscolhaCadastro.sortingOrder = 1;
    }

    public void CadastroProfissional()
    {
        menuController.menuPrincipal.sortingOrder = 0;
        menuEscolhaCadastro.sortingOrder = 0;
        menuCadastroProfissional.sortingOrder = 1;
    }

    public void CadastroPaciente()
    {
        menuController.menuPrincipal.sortingOrder = 0;
        menuEscolhaCadastro.sortingOrder = 0;
        menuCadastroPaciente.sortingOrder = 1;
    }

    public void ConfirmarCadastro(bool profissional)
    {
        if (profissional && (inputCodigoProfissional.text == "1234" || inputCodigoProfissional.text == "4321"))
        {
            isProfissionalCadastrado = true;
            AtualizarNomeProfissional();
            menuController.AtualizarVisibilidadeBotoesPaciente(true);
            FecharMenusCadastro();
        }
        else if (!profissional)
        {
            RegistrarPaciente();
            InicializarDropdownPacientes();
            AtualizarVisibilidadeBotaoEscolherPaciente();
            FecharMenusCadastro();
        }
    }

    private void AtualizarNomeProfissional()
    {
        textoNomeProfissional.gameObject.SetActive(true);
        textoNomeProfissional.text = "Profissional: " + inputNomeProfissional.text;
        menuController.botaoCadastro.gameObject.SetActive(false);
    }

    private void RegistrarPaciente()
    {
        string nomePaciente = inputNomePaciente.text;
        string documentoPaciente = inputDocumentoPaciente.text;
        int idadePaciente = int.Parse(inputIdadePaciente.text);
        string disturbioPaciente = dropdownDisturbio.options[dropdownDisturbio.value].text;

        Paciente novoPaciente = new Paciente(nomePaciente, documentoPaciente, idadePaciente, disturbioPaciente);
        listaPacientes.Add(novoPaciente);

        // Formatação do texto para mostrar primeiro nome, idade e distúrbio
        string nomePacienteFormatado = novoPaciente.Nome.Split(' ')[0];  // Pega o primeiro nome caso tenha sobrenome cadastrado
        textoNomePaciente.gameObject.SetActive(true);
        textoNomePaciente.text = "Paciente: " + $"{nomePacienteFormatado} - {novoPaciente.Idade} anos - {novoPaciente.DisturbioEscolhido}";
        pacienteSelecionado = novoPaciente;
    }

    private void FecharMenusCadastro()
    {
        menuCadastroProfissional.sortingOrder = 0;
        menuCadastroPaciente.sortingOrder = 0;
        menuEscolhaCadastro.sortingOrder = 0;
        menuController.menuPrincipal.sortingOrder = 1;
    }

    public void NovoPaciente()
    {
        if (isProfissionalCadastrado)
        {
            CadastroPaciente();
        }
        else
        {
            Debug.LogError("Profissional não cadastrado! Cadastre-se primeiro.");
        }
    }

    public void EscolherPaciente()
    {
        if (dropdownEscolherPaciente.value >= 0)
        {
            pacienteSelecionado = listaPacientes[dropdownEscolherPaciente.value];
            // Exibe as mesmas informações formatadas
            string nomePacienteFormatado = pacienteSelecionado.Nome.Split(' ')[0];  // Pega o primeiro nome caso tenha sobrenome cadastrado
            textoNomePaciente.text = "Paciente: " + $"{nomePacienteFormatado} - {pacienteSelecionado.Idade} anos - {pacienteSelecionado.DisturbioEscolhido}";
            textoNomePaciente.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Nenhum paciente selecionado.");
        }

        dropdownEscolherPaciente.gameObject.SetActive(true);
    }

    public void InicializarDropdownDisturbios()
    {
        // A lista de distúrbios
        List<string> disturbios = new List<string> { "F e V", "D e T" };
        dropdownDisturbio.ClearOptions();
        dropdownDisturbio.AddOptions(disturbios);
    }

    public void InicializarDropdownPacientes()
    {
        List<string> nomesPacientes = new List<string>();

        foreach (Paciente p in listaPacientes)
        {
            string nomePacienteFormatado = p.Nome.Split(' ')[0];  // Pega o primeiro nome caso tenha sobrenome cadastrado
            string textoDropdown = nomePacienteFormatado + " - " + p.Idade + " anos - " + p.DisturbioEscolhido;
            nomesPacientes.Add(textoDropdown);
        }

        dropdownEscolherPaciente.ClearOptions();
        dropdownEscolherPaciente.AddOptions(nomesPacientes);
    }

    private void AtualizarVisibilidadeBotaoEscolherPaciente()
    {
        // Mostra o botão de "Escolher Paciente" apenas se houver mais de um paciente cadastrado
        botaoEscolherPaciente.gameObject.SetActive(listaPacientes.Count > 1);
    }

    private void LimparCamposCadastroProfissional()
    {
        inputNomeProfissional.text = "";
        inputEmailProfissional.text = "";
        inputCodigoProfissional.text = "";
    }

    private void LimparCamposCadastroPaciente()
    {
        inputNomePaciente.text = "";
        inputDocumentoPaciente.text = "";
        inputIdadePaciente.text = "";
        dropdownDisturbio.value = 0;  // Reseta o valor do dropdown de distúrbios para a primeira opção
    }
}