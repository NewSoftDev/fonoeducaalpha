using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class RelacioneGame : MonoBehaviour
{
    public Transform painelJogo;
    public Transform painelInformacoes;
    public GameObject cartaPrefab;
    public GameObject botaoPrefab;

    public GridLayoutGroup gridLayoutGroup;
    public GameObject textoDicaPrefab;

    public AudioClip somCorreto;
    public AudioClip somIncorreto;
    private AudioSource audioSource;

    private GameObject cartaAtual;
    private GameObject textoDicaObj;
    private string respostaCorreta;
    private string nomeObjetoDica;
    private string letrasSelecionadas;

    [Range(40, 50)] public float tamanhoFonte = 45f;

    private Button botaoLetra1;
    private Button botaoLetra2;

    private Sprite[] imagensCartas;

    // Definir as letras selecionadas para escolher a carta mostrada
    public void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void DefinirLetrasSelecionadas(string letras)
    {
        letrasSelecionadas = letras;
        imagensCartas = ImagemManager.Instance.ObterImagensPorLetras(letrasSelecionadas); // Atualiza as imagens utilizadas
    }

    public void ComecaJogoRelacione()
    {
        if (gridLayoutGroup != null)
        {
            gridLayoutGroup.enabled = false;
        }

        if (cartaAtual != null)
        {
            Destroy(cartaAtual);
        }

        if (textoDicaObj != null)
        {
            Destroy(textoDicaObj);
            textoDicaObj = null;
        }

        // Verifica se o banco de imagens não está vazio
        if (imagensCartas.Length == 0)
        {
            Debug.LogError("Não há imagens disponíveis para a seleção de letras.");
            return;
        }

        int imagemIndex = Random.Range(0, imagensCartas.Length);
        // Modifica a lógica da resposta correta com base nas letras selecionadas
        respostaCorreta = imagensCartas[imagemIndex].name.StartsWith(letrasSelecionadas.Substring(0, 1)) ? letrasSelecionadas.Substring(0, 1) : letrasSelecionadas.Substring(1, 1);

        cartaAtual = Instantiate(cartaPrefab, painelJogo);
        Image cartaImagem = cartaAtual.GetComponent<Image>();
        cartaImagem.sprite = imagensCartas[imagemIndex];
        cartaImagem.raycastTarget = false;

        CentralizarCarta();
        CriarBotoesResposta();
        CriarTextoDica(imagensCartas[imagemIndex].name);
    }
    // Cria o texto com o recorte da letra para servir como dica no jogo
    private void CriarTextoDica(string nomeCompleto)
    {
        nomeObjetoDica = nomeCompleto.Replace(letrasSelecionadas.Substring(0, 1), "").Replace(letrasSelecionadas.Substring(1, 1), "");

        textoDicaObj = Instantiate(textoDicaPrefab, painelInformacoes);
        TextMeshProUGUI textoDica = textoDicaObj.GetComponent<TextMeshProUGUI>();

        textoDica.text = '_' + nomeObjetoDica;
        textoDica.alignment = TextAlignmentOptions.Center;

        RectTransform textoRect = textoDica.GetComponent<RectTransform>();
        textoRect.sizeDelta = new Vector2(650f, textoRect.sizeDelta.y);
        textoRect.anchoredPosition = Vector2.zero;
    }

    private void CentralizarCarta()
    {
        RectTransform cartaRect = cartaAtual.GetComponent<RectTransform>();
        cartaRect.localPosition = Vector3.zero;
        cartaRect.sizeDelta = new Vector2(400, 400);
    }

    private void CriarBotoesResposta()
    {
        botaoLetra1 = Instantiate(botaoPrefab, painelJogo).GetComponent<Button>();
        botaoLetra2 = Instantiate(botaoPrefab, painelJogo).GetComponent<Button>();

        TextMeshProUGUI textoBotaoF = botaoLetra1.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI textoBotaoV = botaoLetra2.GetComponentInChildren<TextMeshProUGUI>();

        textoBotaoF.text = letrasSelecionadas.Substring(0, 1); // Exibe a letra selecionada
        textoBotaoV.text = letrasSelecionadas.Substring(1, 1); // Exibe a outra letra selecionada
        textoBotaoF.fontSize = tamanhoFonte;
        textoBotaoV.fontSize = tamanhoFonte;

        botaoLetra1.onClick.AddListener(() => VerificaResposta(letrasSelecionadas.Substring(0, 1), botaoLetra1));
        botaoLetra2.onClick.AddListener(() => VerificaResposta(letrasSelecionadas.Substring(1, 1), botaoLetra2));

        OrganizarBotoes(botaoLetra1.gameObject, botaoLetra2.gameObject);
    }

    private void OrganizarBotoes(GameObject botaoF, GameObject botaoV)
    {
        RectTransform rectF = botaoF.GetComponent<RectTransform>();
        RectTransform rectV = botaoV.GetComponent<RectTransform>();

        rectF.localScale = Vector3.one;
        rectV.localScale = Vector3.one;

        rectF.sizeDelta = new Vector2(200, 60);
        rectV.sizeDelta = new Vector2(200, 60);

        // Abaixando os botões em 30 pixels em relação a carta principal
        rectF.localPosition = new Vector3(-100f, -230f, 0);
        rectV.localPosition = new Vector3(100f, -230f, 0);
    }

    private void VerificaResposta(string resposta, Button botao)
    {
        botaoLetra1.interactable = false;
        botaoLetra2.interactable = false;

        if (resposta == respostaCorreta)
        {
            audioSource.PlayOneShot(somCorreto);
            botao.image.color = Color.green;
            Debug.Log("Resposta correta!");
        }
        else
        {
            audioSource.PlayOneShot(somIncorreto);
            botao.image.color = Color.red;
            Debug.Log("Resposta incorreta!");
        }

        StartCoroutine(NovaCarta());
    }

    private IEnumerator NovaCarta()
    {
        yield return new WaitForSeconds(1f);

        FimDeJogo();
        ComecaJogoRelacione();
    }

    public void FimDeJogo()
    {
        if (gridLayoutGroup != null)
        {
            gridLayoutGroup.enabled = true;
        }

        if (cartaAtual != null)
        {
            Destroy(cartaAtual);
            cartaAtual = null;
        }

        if (textoDicaObj != null)
        {
            Destroy(textoDicaObj);
            textoDicaObj = null;
        }

        if (botaoLetra1 != null)
        {
            Destroy(botaoLetra1.gameObject);
            botaoLetra1 = null;
        }

        if (botaoLetra2 != null)
        {
            Destroy(botaoLetra2.gameObject);
            botaoLetra2 = null;
        }
    }
}
