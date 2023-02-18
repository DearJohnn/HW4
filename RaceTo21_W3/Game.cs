using System;
using System.Collections.Generic;

namespace RaceTo21
{
    public class Game
    {
        int numberOfPlayers;
        List<Player> players = new List<Player>();
        CardTable cardTable;
        Deck deck = new Deck();
        int currentPlayer = 0;
        public Task nextTask;
        private bool cheating = false;
        int pot = 0;


        public Game(CardTable c)
        {
            cardTable = c;
            deck.Shuffle();
            deck.ShowAllCards();
            nextTask = Task.GetNumberOfPlayers;
        }

        /* Adds a player to the current game
         * Called by DoNextTask() method
         */
        public void AddPlayer(string n)
        {
            players.Add(new Player(n));
        }

        /* Figures out what task to do next in game
         * as represented by field nextTask
         * Calls methods required to complete task
         * then sets nextTask.
         */
        public void DoNextTask()
        {
            Console.WriteLine("================================"); // this line should be elsewhere right?
            if (nextTask == Task.GetNumberOfPlayers)
            {
                numberOfPlayers = cardTable.GetNumberOfPlayers();
                nextTask = Task.GetNames;
            }
            else if (nextTask == Task.GetNames)
            {
                for (var count = 1; count <= numberOfPlayers; count++)
                {
                    var name = cardTable.GetPlayerName(count);
                    AddPlayer(name); // NOTE: player list will start from 0 index even though we use 1 for our count here to make the player numbering more human-friendly
                }
                nextTask = Task.IntroducePlayers;
            }
            else if (nextTask == Task.IntroducePlayers)
            {
                cardTable.ShowPlayers(players);
                nextTask = Task.Bet;
            }
            else if(nextTask == Task.Bet)
            { 
                // Players bet in this task one by one
                for (var count = 1; count <= numberOfPlayers; count++)
                {
                    Player player = players[currentPlayer];
                    int bet = cardTable.BetChips(player);
                    player.setChip(player.chip - bet);
                    cardTable.ShowChips(player);
                    currentPlayer++;
                    pot += bet;// Calculate pot

                }
                Console.WriteLine("There are " + pot + " bets in pot this round.");
                currentPlayer = 0;
                nextTask = Task.PlayerTurn;
            }
            else if (nextTask == Task.PlayerTurn)
            {
                cardTable.ShowHands(players);
                Player player = players[currentPlayer];
                if (player.status == PlayerStatus.active)
                {
                    if (cardTable.OfferACard(player))
                    {
                        Card card = deck.DealTopCard();
                        player.cards.Add(card);
                        player.setScore(ScoreHand(player));
                        if (player.score > 21)
                        {
                            player.setStatus(PlayerStatus.bust);
                        }
                        else if (player.score == 21)
                        {
                            player.setStatus(PlayerStatus.win);

                        }
                    }
                    else
                    {
                        player.setStatus(PlayerStatus.stay);
                    }
                }
                cardTable.ShowHand(player);
                nextTask = Task.CheckForEnd;
            }
            else if (nextTask == Task.CheckForEnd)
            {
                if (CheckRoundWinner() || !CheckActivePlayers())
                {
                    currentPlayer = 0;
                    Player roundWinner = DoFinalScoring();
                    cardTable.AnnounceRoundWinner(roundWinner, pot);//Show the round winner
                    pot = 0;//reset pot
                    //Check if any player has won 80% of the total chips or if any player has no chips left(This is the new game end condition)
                    if (!CheckGameOver())
                    {
                        //If the game is not over, the remaining chips of all players will be displayed and a new round will be started
                        for (var count = 1; count <= numberOfPlayers; count++)
                        {
                            Player player = players[currentPlayer];
                            cardTable.ShowChips(player);
                            currentPlayer++;
                        }
                        currentPlayer = 0;
                        Console.WriteLine("New Round");
                        nextTask = Task.Bet;
                    }
                    else
                    {
                        //If the game is over, find out who is the winner
                        Player winner = FindFinalWinnner();
                        cardTable.AnnounceWinner(winner);
                        nextTask = Task.GameOver;
                    }
                }
                else
                {
                    currentPlayer++;
                    if (currentPlayer > players.Count - 1)
                    {
                        currentPlayer = 0; // back to the first player...
                    }
                    nextTask = Task.PlayerTurn;
                }
            }
            else // we shouldn't get here...
            {
                Console.WriteLine("I'm sorry, I don't know what to do now!");
                nextTask = Task.GameOver;
            }
        }
        /*Calculate the total score of the player's hand
         * Is called by DoNextTask function playerTurn task
         * DoNextTask function provides the current player object
         * Return the number of score 
         */
        public int ScoreHand(Player player)
        {
            int score = 0;
            if (cheating == true && player.status == PlayerStatus.active)
            {
                string response = null;
                while (int.TryParse(response, out score) == false)
                {
                    Console.Write("OK, what should player " + player.name + "'s score be?");
                    response = Console.ReadLine();
                }
                return score;
            }
            else
            {
                foreach (Card card in player.cards)
                {
                    string faceValue = card.id.Remove(card.id.Length - 1);
                    switch (faceValue)
                    {
                        case "K":
                        case "Q":
                        case "J":
                            score = score + 10;
                            break;
                        case "A":
                            score = score + 1;
                            break;
                        default:
                            score = score + int.Parse(faceValue);
                            break;
                    }
                }
            }
            return score;
        }
        /*Check if there is a player with the active status in all players
         * Is called by DoNextTask function CheckForEnd task
         * No parameter passed
         * Retuen a bool to let DoNextTask function know if have a player is active
         */
        public bool CheckActivePlayers()
        {
            foreach (var player in players)
            {
                if (player.status == PlayerStatus.active)
                {
                    return true; // at least one player is still going!
                }
            }
            return false; // everyone has stayed or busted, or someone won!
        }


        /*This fuction will help the task of CheckForEnd to check corner case . When anyone get 21, win immediately. When all player bust but one , win immediately.
         * Is called by DoNextTask function CheckForEnd task
         * No parameter passed
         * Retuen a bool to let DoNextTask function know if have a player is active
        */
        public bool CheckRoundWinner()
        {
            int bustcounter = 0;
            foreach (var player in players)
            {
                if (player.status == PlayerStatus.win)
                {
                    return true;
                }
            }

            foreach(var player in players)
            {
                if (player.status == PlayerStatus.bust)
                {
                    bustcounter++;
                    if(bustcounter == players.Count - 1)//If the bust player count has reached one less player total,the last one win immediately
                    {
                        return true;
                    }
                }
            }
            return false; 
        }
        /*Check if the game is over
        * Is called by DoNextTask function CheckForEnd task
        * No parameter passed
        * Retuen a bool to let DoNextTask function know whether the conditions for the end of the game are met
        */
        public bool CheckGameOver()
        {
            foreach(var player in players)
            {
                if(player.chip >= ((100 * numberOfPlayers) * 0.8) || player.chip == 0)
                {
                    player.setStatus(PlayerStatus.winner);
                    return true;
                }
                else
                {
                    player.setStatus(PlayerStatus.active);
                    player.cards = new List<Card>();
                }
            }
            return false;
        }

        /*Find out who is the final winner
        * Is called by DoNextTask function CheckForEnd task
        * No parameter passed
        * Retuen Player object who is the final winner
        */
        public Player FindFinalWinnner()
        {
            foreach (var player in players)
            {
                if(player.status == PlayerStatus.winner)
                {
                    return player;
                }
            }
            return null;
        }

        /*Find out who is the winner of the current round based on the player state or score
         *Is called by DoNextTask function CheckForEnd task
         * No parameter passed
         * Retuen Player object who is the round winner
         */
        public Player DoFinalScoring()
        {
            int highScore = 0;
            foreach (var player in players)
            {
                cardTable.ShowHand(player);
                if (player.status == PlayerStatus.win) // someone hit 21
                {
                    return player;
                }
                if (player.status == PlayerStatus.stay || player.status == PlayerStatus.active) // if all bust but one, the remaining player also can win
                {
                    if (player.score > highScore)
                    {
                        highScore = player.score;
                    }
                }
                // if busted don't bother checking!
            }
            if (highScore > 0) // someone scored, anyway!
            {
                // find the FIRST player in list who meets win condition
                return players.Find(player => player.score == highScore);
            }
            return null; // everyone must have busted because nobody won!
        }
    }
}
