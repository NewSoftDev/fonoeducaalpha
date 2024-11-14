using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Paciente
{
    public string Nome { get; private set; }
    public string Documento { get; private set; }
    public int Idade { get; private set; }
    public string DisturbioEscolhido { get; private set; }

    public Paciente(string nome, string documento, int idade, string disturbio)
    {
        if (string.IsNullOrEmpty(nome)) throw new ArgumentException("Nome não pode ser vazio.");
        if (string.IsNullOrEmpty(documento)) throw new ArgumentException("Documento não pode ser vazio.");
        if (idade <= 0) throw new ArgumentException("Idade deve ser maior que 0.");
        if (string.IsNullOrEmpty(disturbio)) throw new ArgumentException("Dificuldade não pode ser vazia.");

        Nome = nome;
        Documento = documento;
        Idade = idade;
        DisturbioEscolhido = disturbio;
    }

    public override string ToString()
    {
        return $"{Nome} - {DisturbioEscolhido}";
    }
}

