using System;

namespace RaceTo21
{
    class Program
    {
        static void Main(string[] args)
        {
            CardTable cardTable = new CardTable();
            Game game = new Game(cardTable);
            while (game.nextTask != Task.GameOver)
            {
                game.DoNextTask();
            }
        }
        /********* Draft *********
         
         *Level 1: 
                    Design a chip account for each player. Players can bet each round, and the winner wins all the chips. 
                    The player's bet is kept for the next turn if no one wins this turn.
         *Level 2：
                   If a player wins more than 80% of the total chips of all players, or if a player loses all 
                   chips, the game ends immediately. 
         
         
         */
    }
}

