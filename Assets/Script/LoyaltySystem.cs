using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoyaltySystem : MonoBehaviour
{
    //Acesso global para o sistema de lealdade.
    public static LoyaltySystem instance;

    [Header("Loyalty Score")]
    //Pontos de lealdade por parte da cidade.
    [SerializeField]float pointsInfluenceCity;
    //Pontos de lealdade por parte da natureza.
    [SerializeField]float pointsInfluenceNature;

    //Total de pontos de lealdade.
    [SerializeField]float totalInfluencePoints;

    [Header("Team Loyalty")]
    //Medidor de influência para multiplicador de recompensas e chefe final. Cálculo --> (Maior - Menor) / Maior. Redução percentual.
    public float influenceFactor = 1f;
    //Lista com as "equipes" para definição do chefe final.
    public enum InfluentialSide{None, Nature, City}
    //Identificação da equipe mais influente.
    public InfluentialSide greatestInfluence;
    
    [Header("Loyalty Visualization")]
    //Valor do slider para demonstrar a lealdade em 100%, natureza a esquerda do marcador e cidade a direita. Natureza -- Cidade.
    [SerializeField]float influenceCounter;
    //Referência para o slider na interface.
    public Slider influenceBar;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        //Define valor mínimo e máximo para o slider.
        influenceBar.minValue = 0;
        influenceBar.maxValue = 1f;
        //Inicia com o marcador ao meio do slider.
        influenceBar.value = 0.5f;
    }
    
    void FixedUpdate()
    {
        //Controla a pontuação e atualiza a lealdade na interface.
        if(totalInfluencePoints != 0)
        {
            //Cálculo para saber a % da influência de cada equipe, uso da natureza somente para o cálculo.
            influenceCounter = pointsInfluenceNature / totalInfluencePoints;
            //Atualiza o marcador do slider.
            influenceBar.value = influenceCounter;
        }

        //Faz os cálculos para definição do chefe final e para possível multiplicador de recompensas.
        if(pointsInfluenceNature > pointsInfluenceCity)
        {
            //Específica a eqipe com maior leadade.
            greatestInfluence = InfluentialSide.Nature;
            //Define o fator do multiplicador e define o quão o jogador é mais leal a esta equipe.
            influenceFactor = (1 + ((pointsInfluenceNature - pointsInfluenceCity) / pointsInfluenceNature));
        }
        else if(pointsInfluenceCity > pointsInfluenceNature)
        {
            //Específica a eqipe com maior leadade.
            greatestInfluence = InfluentialSide.City;
            //Define o fator do multiplicador e define o quão o jogador é mais leal a esta equipe.
            influenceFactor = (1 + ((pointsInfluenceCity - pointsInfluenceNature) / pointsInfluenceCity));
        }
        else
        {
            //Específica a eqipe com maior leadade.
            greatestInfluence = InfluentialSide.None;
            //Define o fator do multiplicador.
            influenceFactor = 1f;
        }
    }
    
    //Método para adição dos pontos de lealdade da equipe da cidade.
    public void AddPointsInfluenceCity(float points)
    {
        pointsInfluenceCity += points;
        totalInfluencePoints += points;
    }
    
    //Método para adição dos pontos de lealdade da equipe da natureza.
    public void AddPointsInfluenceNature(float points)
    {
        pointsInfluenceNature += points;
        totalInfluencePoints += points;
    }
}