using System;
using System.Collections.Generic;

namespace RaceTo21
{
	public class Player
	{
		
		public string name { get; set; }
		public List<Card> cards { get; set; }
		public PlayerStatus status { get; private set; }
		public int score { get; private set; }
		public int chip { get; private set; }//Create a int field called chip to store chips for each player, player can get this but can not set this by themselves
		public Player(string n)
		{
			name = n;
			cards = new List<Card>();
			status = PlayerStatus.active;
			chip = 100;
        }
		/* Set player's score privately
		 * Called by Game class
		 */
		public void setScore(int score)
		{
			this.score = score;
		}
		/* Set player's status privately
		 * Called by Game class
		 */
		public void setStatus(PlayerStatus status)
		{
			this.status = status;
		}
		/* Set player's chips privately
		* Called by Game class
		*/
		public void setChip(int chip)
		{
			this.chip = chip;
		}

		/* Introduces player by name
		 * Called by CardTable object
		 */
		public void Introduce(int playerNum)
		{
			Console.WriteLine("Hello, my name is " + name + " and I am player #" + playerNum +" and have "+ chip + " chips");
		}
	}
}

