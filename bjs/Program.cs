using System;
using static bjs.GameHelper;



namespace bjs;
internal class Program : Card
{

    

    static string? playerName;
    static void Main() {

        
        GameHelper gameManager = new GameHelper();
        
        

        // gameManager.RegisterWithGameEventHandler(new GameHelper.GameEventHandler(GameHelper.OnNat21));


        Console.WriteLine("Game initialization");
        Card CardTest = new Card();
    
        CardTest.BuildDeck();
        Console.WriteLine("Enter your name:");

        
        bool valid = new();
        while (valid != true)
        {
            CardTest.InputHandler(Card.InputContext.Naming,out string name,out valid);
            if (valid == true)
            {
                playerName = name;
                Console.WriteLine("Welcome. Remember to type help if you wish to see a list of options.");
                Console.WriteLine("Lady luck be on your side.");
            }
        }
        CardTest.GameState(6,playerName); // int players is how many NPC players in the game. MAX 15
        Card.deckSize = 1;
        //CardTest.GeneratePlayer();

        CardTest.BuildMegaDeck(Card.deckSize);
        CardTest.ShuffleDeck();
        CardTest.CreateTable();
        
        
        
        while (true)
        {
            gameManager.Registration();
            CardTest.Betting();
            CardTest.StartRoundDealCards();
            gameManager.GameStateCheck();
            CardTest.PlayerTurns();
            Console.WriteLine("Dealer turn.");
            CardTest.DealerTurn();
            Console.WriteLine("Checking winners.");
            CardTest.CheckWinners();
            Console.WriteLine("Cleanup.");
            CardTest.Cleanup();
            Console.WriteLine("New round.");

            

        }
    }
}
