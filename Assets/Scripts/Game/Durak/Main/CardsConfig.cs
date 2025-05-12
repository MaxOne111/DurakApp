using System.Collections.Generic;
using System.Linq;
using Game.Durak;
using Game.Durak.Enums;
using Game.Durak.Network.Responses;
using UnityEngine;

[CreateAssetMenu(menuName = "CardsConfig", fileName = "CardsConfig", order = 0)]
public class CardsConfig : ScriptableObject
{
    [SerializeField] private List<TestCard> cardPrefabs;
    
    [SerializeField] private Sprite[] trumpSprites;

    public TestCard GetCard(CardInfo cardInfo)
    {
        var result = cardPrefabs.First(value => value.CardInfo.rank == cardInfo.rank &&
                                                value.CardInfo.suit == cardInfo.suit);

        return result;
    }

    public Sprite GetTrump(ECardSuit suit) //Test
    {
        switch (suit)
        {
            case ECardSuit.Spades:
                return trumpSprites[0];
         
            case ECardSuit.Clubs:
                return trumpSprites[1];
              
            case ECardSuit.Diamonds:
                return trumpSprites[2];
              
            case ECardSuit.Hearts:
                return trumpSprites[3];
              
        }

        return null;
    }

}