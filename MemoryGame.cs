using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryGame : MonoBehaviour
{
    private Sprite[] imagensCartas;  // Cartas que vão ser usadas no jogo (dependendo da letra selecionada)

    public GameObject cartaPrefab;  // Prefab da carta (deve estar configurado no Unity)
    public Transform painelJogo;  // Onde as cartas serão geradas

    private List<GameObject> cartasAtivas = new List<GameObject>();  // Lista de cartas ativas no jogo
    private Dictionary<GameObject, Sprite> imagemCartaMap = new Dictionary<GameObject, Sprite>();  // Mapeia cartas para imagens

    private GameObject cartaSelecionada1 = null;
    private GameObject cartaSelecionada2 = null;

    private bool jogoFinalizado = false;

    private string letrasSelecionadas = "FV";  // Letras selecionadas, padrão é "FV"

    private bool verificandoPar = false;

    void Start()
    {

    }

    // Definir as letras selecionadas para escolher o conjunto de cartas correto
    public void DefinirLetrasSelecionadas(string letras)
    {
        letrasSelecionadas = letras;

        // Obter as imagens de acordo com as letras selecionadas a partir do ImagemManager
        imagensCartas = ImagemManager.Instance.ObterImagensPorLetras(letrasSelecionadas);
    }

    public void ComecaJogoMemoria()
    {
        Debug.Log("Começa Jogo Memória com letras: " + letrasSelecionadas);
        jogoFinalizado = false;
        LimparCartas();

        int totalCartas = 16;  // Sempre 16 cartas
        int quantidadePares = totalCartas / 2;  // Número de pares de cartas

        // Garantir que o número de imagens seja suficiente para 16 cartas
        int totalImagens = imagensCartas.Length;
        if (totalImagens < quantidadePares)
        {
            // Se o número de imagens for menor que a quantidade de pares, são duplicadas
            List<Sprite> imagensCopia = new List<Sprite>(imagensCartas);
            while (imagensCopia.Count < quantidadePares)
            {
                imagensCopia.AddRange(imagensCartas);  // Duplicar imagens até pelo menos o número de pares
            }
            imagensCartas = imagensCopia.ToArray();  // Atualiza o array com as imagens duplicadas
        }

        List<Sprite> cartasEmbaralhadas = new List<Sprite>();
        if (imagensCartas.Length == quantidadePares)
        {
            // Se o número de imagens for igual ao número de pares, é associado uma imagem para cada
            for (int i = 0; i < quantidadePares; i++)
            {
                cartasEmbaralhadas.Add(imagensCartas[i]);
                cartasEmbaralhadas.Add(imagensCartas[i]);
            }
        }
        else if (imagensCartas.Length > quantidadePares)
        {
            // Se o número de imagens for maior que o número de pares, são escolhidas aleatoriamente
            List<Sprite> imagensDisponiveis = new List<Sprite>(imagensCartas);
            for (int i = 0; i < quantidadePares; i++)
            {
                int randomIndex = Random.Range(0, imagensDisponiveis.Count);
                cartasEmbaralhadas.Add(imagensDisponiveis[randomIndex]);
                cartasEmbaralhadas.Add(imagensDisponiveis[randomIndex]);
                imagensDisponiveis.RemoveAt(randomIndex); // Remove a imagem para não repetir
            }
        }
        else
        {
            // Encontrar motivo do erro sem essa parte ***redundante
            List<Sprite> imagensCopia = new List<Sprite>(imagensCartas);
            while (imagensCopia.Count < quantidadePares)
            {
                imagensCopia.AddRange(imagensCartas); 
            }

            for (int i = 0; i < quantidadePares; i++)
            {
                cartasEmbaralhadas.Add(imagensCopia[i]);
                cartasEmbaralhadas.Add(imagensCopia[i]);
            }
        }

        // Embaralha as imagens
        Shuffle(cartasEmbaralhadas);

        // Verifica se o tamanho da lista é igual ao número total de cartas (16)
        if (cartasEmbaralhadas.Count != totalCartas)
        {
            Debug.LogError("Erro: o número de cartas embaralhadas não é o esperado.");
            return;
        }

        // Instancia as cartas e distribui as imagens embaralhadas
        for (int i = 0; i < totalCartas; i++)
        {
            GameObject novaCarta = Instantiate(cartaPrefab, painelJogo);
            cartasAtivas.Add(novaCarta);

            // Usando o índice correto para acessar as imagens embaralhadas
            imagemCartaMap[novaCarta] = cartasEmbaralhadas[i];
            novaCarta.name = "carta" + i;

            var button = novaCarta.GetComponent<Button>();
            button.onClick.AddListener(() => RevelaCarta(novaCarta));
        }
    }

    // Método para embaralhar a lista de imagens
    private void Shuffle(List<Sprite> lista)
    {
        for (int i = 0; i < lista.Count; i++)
        {
            // Gera um índice aleatório
            int j = Random.Range(i, lista.Count);

            // Troca os elementos de lugar
            Sprite temp = lista[i];
            lista[i] = lista[j];
            lista[j] = temp;
        }
    }

    private void RevelaCarta(GameObject carta)
    {
        if (verificandoPar || carta == cartaSelecionada1)
            return;

        if (cartaSelecionada1 == null)
        {
            cartaSelecionada1 = carta;
            carta.GetComponent<Image>().sprite = imagemCartaMap[carta];
        }
        else if (cartaSelecionada2 == null)
        {
            cartaSelecionada2 = carta;
            carta.GetComponent<Image>().sprite = imagemCartaMap[carta];
            StartCoroutine(VerificaPar());
        }
    }

    private IEnumerator VerificaPar()
    {
        verificandoPar = true;
        yield return new WaitForSeconds(0.5f);

        if (imagemCartaMap[cartaSelecionada1] == imagemCartaMap[cartaSelecionada2])
        {
            Destroy(cartaSelecionada1);
            Destroy(cartaSelecionada2);
        }
        else
        {
            cartaSelecionada1.GetComponent<Image>().sprite = null;
            cartaSelecionada2.GetComponent<Image>().sprite = null;
        }

        cartaSelecionada1 = null;
        cartaSelecionada2 = null;
        verificandoPar = false;
    }

    private void LimparCartas()
    {
        foreach (GameObject carta in cartasAtivas)
        {
            Destroy(carta);
        }
        cartasAtivas.Clear();
        imagemCartaMap.Clear();
    }

    public void FimDeJogo()
    {
        LimparCartas();
        jogoFinalizado = true;
    }
}