using System;
using System.Collections.Generic;

namespace RaceTo21
{
    public class CardTable
    {
        public CardTable()
        {
            Console.WriteLine("Setting Up Table...");
        }

        /* Shows the name of each player and introduces them by table position.
         * Is called by Game object.
         * Game object provides list of players.
         * Calls Introduce method on each player object.
         */
        public void ShowPlayers(List<Player> players)
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].Introduce(i+1); // List is 0-indexed but user-friendly player positions would start with 1...
            }
        }

        /* Gets the user input for number of players.
         * Is called by Game object.
         * Returns number of players to Game object.
         */
        public int GetNumberOfPlayers()
        {
            Console.Write("How many players? ");
            string response = Console.ReadLine();
            int numberOfPlayers;
            while (int.TryParse(response, out numberOfPlayers) == false
                || numberOfPlayers < 2 || numberOfPlayers > 8)
            {
                Console.WriteLine("Invalid number of players.");
                Console.Write("How many players?");
                response = Console.ReadLine();
            }
            return numberOfPlayers;
        }

        /* Gets the name of a player
         * Is called by Game object
         * Game object provides player number
         * Returns name of a player to Game object
         */
        public string GetPlayerName(int playerNum)
        {
            Console.Write("What is the name of player# " + playerNum + "? ");
            string response = Console.ReadLine();
            while (response.Length < 1)
            {
                Console.WriteLine("Invalid name.");
                Console.Write("What is the name of player# " + playerNum + "? ");
                response = Console.ReadLine();
            }
            return response;
        }

        /*
         *Ask if the player wants a card
         * Is called by Game object
         * Game object provides the current player object
         * Returns a bool to Game object to let it know player wants a card or not
         */
        public bool OfferACard(Player player)
        {
            while (true)
            {
                Console.Write(player.name + ", do you want a card? (Y/N)");
                string response = Console.ReadLine();
                if (response.ToUpper().StartsWith("Y"))
                {
                    return true;
                }
                else if (response.ToUpper().StartsWith("N"))
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Please answer Y(es) or N(o)!");
                }
            }
        }
        /* Get the number of the player how much to bet in this round
         * Is called by Game object
         * Game object provides the current player object
         * Return the number of chips that player wants to bet
         */
        public int BetChips(Player player)
        {
            Console.Write("How many chips does " + player.name + " want to bet this round? (1 - " + player.chip + ")");
            int bet;
            string response = null;
            response = Console.ReadLine();
            while (int.TryParse(response, out bet) == false || bet < 1 || bet > player.chip)
            {
                Console.WriteLine("Invalid number of chips.");
                Console.Write("How many chips does " + player.name + " want to bet this round? (1 - " + player.chip + ")");
                response = Console.ReadLine();
            }
            return bet;
        }

        /*Show all of the card in player's hand one by one and show player's total score
         * Is called by Showhands function
         * Showhands function provides the current player object
         * No return type
         */
        public void ShowHand(Player player)
        {
            if (player.cards.Count > 0)
            {
                Console.Write(player.name + " has: ");
                foreach (Card card in player.cards)
                {
                    //If it is the last card in the hand, do not put a comma after the card name
                    if (player.cards.IndexOf(card) == player.cards.Count - 1 )
                    {
                        Console.Write(card.displayName + " ");
                    }
                    else{
                        Console.Write(card.displayName + ", ");
                    }
                    
                }
                Console.Write("=" + player.score + "/21 ");
                if (player.status != PlayerStatus.active)
                {
                    Console.Write("(" + player.status.ToString().ToUpper() + ")");
                }
                Console.WriteLine();
            }
        }

        /*Show how many chips in player's account
         * Is called by Game object
         * Game object provides the current player object
         * No return type
         */
        public void ShowChips(Player player)
        {
            Console.WriteLine(player.name + " has " + player.chip + " left to bet.");
        }

        /*Traverses all players and shows their hands
         * Is called by Game object
         * Game object provides the player list
         * No return type
         */
        public void ShowHands(List<Player> players)
        {
            foreach (Player player in players)
            {
                ShowHand(player);
            }
        }

        /*Print out the round result
         * Is called by Game object
         * Game object provides the player object and round pot
         * No return type
         */
        public void AnnounceRoundWinner(Player player, int pot)
        {
            if (player != null)
            {
                player.setChip(player.chip + pot);
                Console.WriteLine(player.name + " wins " + pot + " chips!");
            }
            else
            {
                Console.WriteLine("Everyone busted!");
            }
        }
        /*Print out the final winner
         * Is called by Game object
         * Game object provides the player object
         * No return type
         */
        public void AnnounceWinner(Player player)
        {
            if (player != null)
            {
                Console.WriteLine(player.name + " is the final winner!");
            }
            Console.Write("Press <Enter> to exit... ");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
        }

    }
}