using System;
using System.Collections.Generic;
using System.Linq;

namespace RaceTo21
{
    public class Deck
    {
        List<Card> cards = new List<Card>();
        // Create a Dictionary to store two string type date, one is card image file name another is card id 
        Dictionary<string, string> imagesDictionary = new Dictionary<string, string>();
        public Deck()
        {
            Console.WriteLine("*********** Building deck...");
            string[] suits = { "Spades", "Hearts", "Clubs", "Diamonds" };

            for (int cardVal = 1; cardVal <= 13; cardVal++)
            {
                foreach (string cardSuit in suits)
                {
                    string cardName;
                    string cardLongName;
                    string cardID;
                    string imageID;


                    switch (cardVal)
                    {
                        case 1:
                            cardName = "A";
                            cardLongName = "Ace";
                            break;
                        case 11:
                            cardName = "J";
                            cardLongName = "Jack";
                            break;
                        case 12:
                            cardName = "Q";
                            cardLongName = "Queen";
                            break;
                        case 13:
                            cardName = "K";
                            cardLongName = "King";
                            break;
                        default:
                            cardName = cardVal.ToString();
                            cardLongName = cardName;
                            break;
                    }
                    // Generate card image file name
                    imageID = "card_" + cardSuit + "_" + cardName + ".png";
                    cardID = cardName + cardSuit.First<char>();
                    cards.Add(new Card(cardID, cardLongName + " of " + cardSuit));
                    imagesDictionary.Add(cardID, imageID);
                    Console.WriteLine( $"{imagesDictionary[cardID]}");
                }
            }
        }

        public void Shuffle()
        {
            Console.WriteLine("Shuffling Cards...");

            Random rng = new Random();
            for (int i=0; i<cards.Count; i++)
            {
                Card tmp = cards[i];
                int swapindex = rng.Next(cards.Count);
                cards[i] = cards[swapindex];
                cards[swapindex] = tmp;

            }
        }

        public void ShowAllCards()
        {
            for (int i=0; i<cards.Count; i++)
            {
                Console.Write(i+":"+cards[i].displayName);
                if (i < cards.Count -1)
                {
                    Console.Write(" ");
                } else
                {
                    Console.WriteLine("");
                }
            }
        }

        public Card DealTopCard()
        {
            Card card = cards[cards.Count - 1];
            cards.RemoveAt(cards.Count - 1);
            return card;
        }
    }
}

