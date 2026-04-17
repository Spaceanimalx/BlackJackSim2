using System;
using System.Collections;
using System.Text;


namespace bjs;



public class Card
{
    public static event GameEventHandler Bust;
    public delegate void GameEventHandler(object sender, GameStateEventArgs e);
    GameEventHandler bustHandler = new GameEventHandler(PlayerBustHandler);
    GameEventHandler endTurnHandler = new GameEventHandler(EndTurnHandler);
    public void Registration()
    {
        Bust += bustHandler;
        Bust += endTurnHandler;
    }

    public static void PlayerBustHandler(object sender, GameStateEventArgs e)
    {
        Console.WriteLine($"Bust. {FinalTable[e.idx].GetFirstName()} loses {FinalTable[e.idx].GetPotAmount()}");
        FinalTable[e.idx].LoseBet(e.idx);
    }

    
    public static int deckSize = new();
    private static int finalIndex = 52;
    public static List<Seats> FinalTable = new List<Seats>() { };
    
    public static List<Seats> RoundLosers = new List<Seats>() { };
    public static List<Seats> RoundWinners = new List<Seats>() { };
    private List<Cards> deckTest = new List<Cards>() { };
    private List<Cards> CloneDeck = new List<Cards>() { };
    private static List<Cards> FinalDeck = new List<Cards>() { };
    private List<Cards> DiscardDeck = new List<Cards>() { };
    //public List<Cards> thisHand = new();
    //private Queue<Hand> PlayerHand = new Queue<Hand>() { };

    public static string[] cards = new string[13] { "Ace", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King" };
    public static string[] suits = new string[4] { " of Hearts", " of Spades", " of Diamonds", " of Clubs" };
    static List<(int, int)> cardValues = new List<(int, int)>() { (1, 11), (2, 0), (3, 0), (4, 0), (5, 0), (6, 0), (7, 0), (8, 0), (9, 0), (10, 0), (10, 0), (10, 0), (10, 0) };
    private bool cardParity;
    private string playerName;
    static int preDiscardTop;
    static int preDiscardFinalIndex;
    private string result = "";
    public static decimal PayOutMultiplier = 1.5M;
    private int cloneAmt = new();
    private int targetIndex = new();
    private string? chosenCard;
    private int topCard = 52;
    static int discardIndex = 0;
    
    private bool cardRevealed;
    private bool playerDealerFlag;
    private bool validInput;
    private static int DealerIndex = 0;
    
    public static int UserIndex { get; set; } //the player's index in FinalTable list
    public enum InvalidInputContext
    {
        NameCharacterLimit = 1,
        InsufficientFunds = 2,
        BettingLimit = 3,
        InvalidOption = 4,
        Other = 5,
        Return = 6
    }
    public InvalidInputContext invalidInput { get; set; }
    /// <summary>
    /// These variables are used in creating the NPC players, and table.
    /// </summary>
    private int playerCount;
    protected static int totalPlayers; // totalplayers in the game including the user & dealer.
    private int cutCardsAmt; // how many cards are cut from the deck after each shuffle.
    private decimal playerCash;
    private string[] FNameArray = new string[15] { "Magnus", "Adam", "Carl", "Susan", "Jason", "Mike", "Cedric", "Ronald", "Blaise", "Seamus", "Bill", "Patrick", "Ginger", "Melanie", "Emily" };
    private string[] LNameArray = new string[15] { "Kowalski", "Salamanca", "von Strucker", "Ward", "Stockton", "LaRiviere", "Giroux", "Schmidt", "Eckart", "Amega", "Singh", "Nguyen", "Power", "Bolton", "Smasher" };
    private string[,] playerAISetting = new string[3, 3];
    static int iIndex;
    static int jIndex;
    static int aiStylePlay;
    static int aiStyleBet;
    protected decimal pCash;
    private string npcFName;
    private string npcLName;
    private string dealerFName = "Dealer";
    private string dealerLName = "Casino";
    private bool NPCFlag;
    public static bool PlayerBust { get; set; }
    
    
    /// <summary>
    /// Utility stuff
    /// </summary>
    
    public int currentIndex { get; set; } // currentIndex is who's turn it is
    

    private int roundCount = 1;
    private int loopCount = 1; // For help options, the 'help' wall of text doesn't appear until loopcount is reset to 1
    /// <summary>
    /// Game Sim Logic
    /// </summary>
    private decimal maxBet = 500;
    private decimal minBet = 10;
    private decimal avgBet = 250;
    private int handValue = 0;
    
    public int handIndex { get; set; }
    public int currSoftValue;
    public int currSoftValueDealer;
    public int currHardValue;
    public int currHardValueDealer;
    public int highestValue;
    public int highestValueDealer;
    public int lowestValuePlayer;
    public int lowestValueDealer;

    private enum GameCardChoice : int
    {
        Hit = 0,
        Stand = 1,
            Bust=2
    }
    GameCardChoice gameCardChoice;
    



    /// <summary>
    /// Input strings idk
    /// </summary>
    /*private string helpInput;
    private string input;
    private int x;*/

   

    private struct Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PersonCash { get; set; }
        public (int, int) npcPlayStyle { get; set; }



        public override string ToString() => $"Npc {FirstName} {LastName} playStyle {npcPlayStyle}";

        public string GetFirstName() => FirstName;
        public string GetLastName() => LastName;
    }
    public void BuildDeck()
    {
        for (int j = 0; j < suits.Length; j++)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                result = cards[i] + suits[j];
                deckTest.Insert(i, new Cards(result, cardValues[i],cardParity));
                //Console.WriteLine($"{deckTest[i].ToString()}");
            }
        }
    }
    public void BuildMegaDeck(int deckCount)
    {

        cloneAmt = deckCount;
        finalIndex = 52;

        targetIndex = 0;
        preDiscardFinalIndex = finalIndex;

        if (cloneAmt == 2 || cloneAmt == 4 || cloneAmt == 6 || cloneAmt == 8)
        {


            topCard = cloneAmt * 52;
            preDiscardTop = topCard;
            finalIndex = deckCount * 52;

            preDiscardFinalIndex = finalIndex;
            CloneDeck.InsertRange(targetIndex, deckTest);
            /*        Console.WriteLine("Successfully cloned");
                    Console.WriteLine($"Final Deck.Length before cloning: {CloneDeck.Count}");

                    Console.WriteLine($"Resizing Final Deck");*/


            switch (cloneAmt)
            {

                case 2:

                    Console.WriteLine($"Baby Mode: {Card.deckSize} decks selected.");


                    targetIndex = targetIndex + 52;
                    CloneDeck.InsertRange(targetIndex, deckTest);

                    break;

                case 4:

                    Console.WriteLine($"Basic Mode: {Card.deckSize} decks selected.");
                    for (int i = 0; i < cloneAmt - 1; i++)
                    {

                        targetIndex = targetIndex + 52;
                        CloneDeck.InsertRange(targetIndex, deckTest);


                    }
                    break;
                case 6:

                    Console.WriteLine($"CASINO MODE: {Card.deckSize} DECKS SELECTED.");
                    for (int i = 0; i < cloneAmt - 1; i++)
                    {

                        targetIndex = targetIndex + 52;
                        CloneDeck.InsertRange(targetIndex, deckTest);


                    }
                    break;
                case 8:
                    Console.WriteLine($"PSYCHO MODE: {Card.deckSize} DECKS SELECTED.");
                    for (int i = 0; i < cloneAmt - 1; i++)
                    {

                        targetIndex = targetIndex + 52;
                        CloneDeck.InsertRange(targetIndex, deckTest);


                    }
                    break;
            }
        }
        else
        {
            CloneDeck.InsertRange(targetIndex, deckTest);
            Console.WriteLine("1 Deck selected.");
            return;

        }
        //   Console.WriteLine($"Final Deck.Length: {FinalDeck.Count}");

    }
    public void ShuffleDeck()
    {
        int x = topCard / 5 + 10;
        int y = topCard / 5 - 10;
        Random cutAmt = new();
        cutCardsAmt = cutAmt.Next(y, x);
        //string topName1 = CloneDeck.ElementAt(finalIndex - 1).ToString(); for testing
        FinalDeck = CloneDeck.OrderBy(i => Random.Shared.Next()).ToList();
        //string topName = FinalDeck.ElementAt(finalIndex - 1).ToString(); for testing 
        int cutIndex = FinalDeck.Count - cutCardsAmt;
        finalIndex = FinalDeck.Count - cutCardsAmt;
        for (int o = 0; o < cutCardsAmt; o++)
        {

            DiscardDeck.Insert(discardIndex, FinalDeck.ElementAt(cutIndex));
            FinalDeck.RemoveAt(cutIndex);
            cutIndex--;
            discardIndex++;
        }
        topCard = topCard - cutCardsAmt;
        finalIndex = FinalDeck.Count - 1;
        Console.WriteLine("Deck shuffled and cut.");
    }
    
    public void DealEveryone()
    {
        currentIndex = 0;
        roundCount = 1;
        while (roundCount <= 2)
        {
            for (int i = 0; i < totalPlayers; i++)
            {
                if (currentIndex == 0 && roundCount == 2)
                {

                    DealCardFacedown();

                }
                else
                {
                    DealCard();

                }
                currentIndex++;
            }
            currentIndex = 0;
            roundCount++;
        }
    }
    public void IsInputValid(string givenvalue, string expectedvalue,out bool valid)
    {
        int givenvalueasint;
        int expectedvalueasint;
        valid = false;
        Int32.TryParse(expectedvalue, out expectedvalueasint);
        Int32.TryParse(givenvalue, out givenvalueasint);
        if (givenvalueasint == expectedvalueasint)
        {

        }
        string trimmedgivenvalue = givenvalue.Trim();
        
        if (trimmedgivenvalue.Equals(expectedvalue))
        {
            valid = true;
        }
        else
        {
            valid = false;
        }
    }
    public void StartRoundDealCards()
    {
        string expectedvalue = "";
        Console.WriteLine("Dealing cards. Press return to proceed, or help to view options");
        InputHandler(InputContext.Return,out string input,out bool valid);
       // IsInputValid(input,expectedvalue,out valid);
        if (valid == true)
        {
            DealEveryone();
            return;
        }
        if (valid!=true)
        {
            StartRoundDealCards();
        }
    }
    public void DealCardFacedown()
    {
        discardIndex = DiscardDeck.Count;
        FinalTable[currentIndex].handIndex = FinalTable[currentIndex].ThisHand.Count;
        finalIndex = FinalTable.Count;
        //Console.WriteLine($"Hand index for {FinalTable[currentIndex].GetFirstName()}: {FinalTable[currentIndex].handIndex}");


        if (finalIndex != -1)
        {
            chosenCard = FinalDeck.ElementAt(finalIndex).ToString();
            FinalDeck.ElementAt(finalIndex).SetParity(false);
            //Console.WriteLine($"Current index {currentIndex}");
            //FinalTable[currentIndex].thisHand.Add(FinalDeck.ElementAt(finalIndex));

            (FinalTable[currentIndex].ThisHand).Insert(handIndex, FinalDeck.ElementAt(finalIndex));
            Console.WriteLine("");
            
            
            Console.WriteLine($"Dealt card facedown to player {FinalTable[currentIndex].GetFirstName()} {FinalTable[currentIndex].GetLastName()}.");
            FinalDeck.RemoveAt(finalIndex);

            topCard--;
            finalIndex--;
            //FinalDeck.ElementAt(finalIndex).SetParity(true);
        }
        else
        {
            Console.WriteLine($"Deck empty. Reshuffling.");
            Reshuffle();
            DealCardFacedown();
        }
        
    }
    public void DealCard()
    {
        discardIndex = DiscardDeck.Count;
        FinalTable[currentIndex].handIndex = FinalTable[currentIndex].ThisHand.Count;
        finalIndex = FinalTable.Count;
        //Console.WriteLine($"Hand index for {FinalTable[currentIndex].GetFirstName()}: {FinalTable[currentIndex].handIndex}");
        
        

        if (finalIndex != -1)
        {
            FinalDeck.ElementAt(finalIndex).SetParity(true);
            chosenCard = FinalDeck.ElementAt(finalIndex).ToString();
            //FinalTable[currentIndex].thisHand.Add(FinalDeck.ElementAt(finalIndex));

            (FinalTable[currentIndex].ThisHand).Insert(handIndex, FinalDeck.ElementAt(finalIndex));
            //Console.WriteLine($"Hand index = {FinalTable[currentIndex].handIndex}");
            Console.WriteLine($"Dealt card {FinalDeck.ElementAt(finalIndex)} to player {FinalTable[currentIndex].GetFirstName()} {FinalTable[currentIndex].GetLastName()}.");
            FinalDeck.RemoveAt(finalIndex);
            //Console.WriteLine($"Proof: {FinalTable[currentIndex].GetHandInfo(currentIndex)}");
            topCard--;
            finalIndex--;
            //Console.WriteLine($"{FinalTable[currentIndex]} {FinalTable[currentIndex].ThisHand} {FinalTable[currentIndex].ThisHand.ElementAt(handIndex)}");
        }
        else
        {
            Console.WriteLine($"Deck empty. Reshuffling.");
            Reshuffle();
            DealCard();
        }
    }
    public void Reshuffle()
    {
        Console.WriteLine($"Discard deck size: {DiscardDeck.Count}");
        int targetindex = FinalDeck.Count;
        FinalDeck.InsertRange(targetindex, DiscardDeck);
        DiscardDeck.Clear();
        Console.WriteLine($"Discard added to deck. Final deck now contains {FinalDeck.Count} cards. Discard Deck contains {DiscardDeck.Count}");
        finalIndex = FinalDeck.Count - 1;
    }
    public void DiscardHand(int targetindex)
    {
        handIndex = FinalTable[targetindex].ThisHand.Count;
        
        int c = FinalTable[targetindex].ThisHand.Count;
        
        DiscardDeck.AddRange(FinalTable[targetindex].ThisHand);
        (FinalTable[targetindex].ThisHand).Clear();
        discardIndex = discardIndex+c;
        handIndex = FinalTable[targetindex].ThisHand.Count;
        Console.WriteLine($"{FinalTable[targetindex].GetFirstName()} has {(FinalTable[targetindex].ThisHand).Count} cards in hand after discarding.");
    }

    public void GameState(int playeramount, string playername)
    {
        playerName = playername;
        playerCount = playeramount;
        totalPlayers = playerCount + 1;
        playerCash = 5000;
        
    }
    
    public void CreateTable()
    {
        ///
        /// Creating rng seeds, and using them to assign random values from a few arrays to the elements of the FinalTable list. ///
        ///
        /// Seats players equal to totalPlayers.
        ///
        ///
        Random rndi = new();
        Random rndj = new();

        Random aip = new();
        Random aib = new();

        Random randomforseat = new();
        
        int randomseat = randomforseat.Next(1, totalPlayers);
        UserIndex = randomseat;
        
        Random rndcash = new();
        for (int i = 0; i < totalPlayers; i++)
        {

            List<Cards> c=new();
            iIndex = rndi.Next(0, 14);
            jIndex = rndj.Next(0, 14);
            pCash = rndcash.Next(100, 2500);
            npcFName = FNameArray[iIndex];
            npcLName = LNameArray[jIndex];

            if (i == randomseat && i != 0)
            {
                int tempindex = i;
                FinalTable.Insert(i, new Seats(i, playerName, "Gambler", 0, playerCash, (0, 0), playerDealerFlag = false, NPCFlag = false, handIndex, c));
            }
            else if (i != 0)
            {
                int tempindex = i;
                FinalTable.Insert(i, new Seats(i, npcFName, npcLName, 0, pCash, (0, 0), playerDealerFlag = false, NPCFlag = true, handIndex, c));
            }
            else
            {
                FinalTable.Insert(i, new Seats(i, dealerFName, dealerLName, 0, 1000000, (0, 0), playerDealerFlag = true, NPCFlag = true, handIndex, c));
            }

        }
        ///
        /// Assigns AI styles to NPC players.
        /// x/y where x is their general strategy, and y is their bet aggression
        /// EXCEPTIONS
        /// 5/5 = Dealer ------ Dealer always follows a strict set of play rules and does not bet.
        /// 4/4 = Player ------ not AI controlled.
        /// 
        ///
        for (int i = 0; i < totalPlayers; i++)
        {
            aiStylePlay = aip.Next(1, 4);
            aiStyleBet = aib.Next(1, 4);
            if (FinalTable[i].IsDealer() == true)
            {
                FinalTable[i].SetStrats(4, 4);
            }
            else if (FinalTable[i].IsDealer() == false && FinalTable[i].IsNPC() == true)
            {
                FinalTable[i].SetStrats(aiStylePlay, aiStyleBet);
              //  Console.WriteLine($"CreateTable(): setting AI style to {aiStylePlay} {aiStyleBet} for player {FinalTable[i].GetFirstName()}");
            }
            else
            {
                FinalTable[i].SetStrats(4, 4);
            }
        }
    }

    /// <summary>
    /// Testing something to handle invalid inputs
    /// InvalidInputContext is a flag of where the user was in the game logic when the invalid input occured
    /// e.g. if they were in the betting section - it will handle invalid bet values or insufficient funds
    /// 
    /// </summary>
    /// <param name="invalidInputContext"></param>
    public void InvalidInputHandler(InvalidInputContext invalidInputContext)
    {
        invalidInput = invalidInputContext;
        switch (invalidInput)
        {
            case InvalidInputContext.NameCharacterLimit:
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    break;
                }
            case InvalidInputContext.InsufficientFunds:
                {
                    Console.WriteLine("Insufficient funds {0}", FinalTable[currentIndex].GetCash());
                    Console.WriteLine("Please enter a valid amount or type help for options.");
                    break;
                }
            case InvalidInputContext.BettingLimit:
                {
                    Console.WriteLine("Invalid bet. Minimum bet is {0}, maximum is {1}", minBet, maxBet);
                    Console.WriteLine("Please enter valid bet amount to continue or type help for options.");
                    break;
                }
            case InvalidInputContext.InvalidOption:
                {
                    Console.WriteLine("Invalid option.");
                    Console.WriteLine("Please enter <return> to continue, or <help> to see the list of options.");
                    break;
                }
            case InvalidInputContext.Other:
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    break;
                }
        }
    }
    
    /// <summary>
    /// 
    /// Testing a Help/Options feature.
    /// I don't know how to pause the method they were currently in other than a validity bool immediately following the users' call to InputHandler.
    /// 
    /// </summary>
    public void HelpHandler()
    {

        if (loopCount == 1)
        {
            Console.WriteLine("---Help----------------------------");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("------Type <help #>---to select----");
            Console.WriteLine("-----------<return>---to return----");
            Console.WriteLine("-----------<exit>-----to quit------");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("--Available settings---------------");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("--<help 1>-Check balance-----------");
            Console.WriteLine("--<help 2>-Check cards-------------");
            Console.WriteLine("--<help 3>-Check dealer------------");
            Console.WriteLine("--<help 4>-Check remaining cards---");
            Console.WriteLine("--<help 5>-Check remaining players-");
            Console.WriteLine("--<help 6>-Check seat.-------------");
            Console.WriteLine("-----------------------------------");
        }
        string input = Console.ReadLine();
        string trimmedInput = input.Trim();
        bool done = false;

        while (done == false)
        {

            if (trimmedInput.Equals("help 1", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"{FinalTable[UserIndex].GetCash()}");
                loopCount++;
                HelpHandler();
            }
            if (trimmedInput.Equals("help 2", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"user index is {UserIndex}");
                Console.WriteLine($"Your cards: {FinalTable[UserIndex].GetHandInfo(UserIndex)}");
                Console.WriteLine($"");
                FinalTable[UserIndex].GetHandValue(UserIndex, out _);
                loopCount++;
                HelpHandler();
            }
            if (trimmedInput.Equals("help 3", StringComparison.OrdinalIgnoreCase))
            {

                Console.WriteLine($"{FinalTable[0].DisplayStats(DealerIndex)}");
                FinalTable[DealerIndex].GetHandInfo(DealerIndex);
                loopCount++;
                HelpHandler();

            }
            if (trimmedInput.Equals("help 4", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Cards remaining in deck: {FinalDeck.Count}");
                loopCount++;
                HelpHandler();
            }
            if (trimmedInput.Equals("help 5", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Remaining players:");
                loopCount++;
                HelpHandler();
            }
            if (trimmedInput.Equals("help 6", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Your seat: {FinalTable[UserIndex].GetSeat()}");
                loopCount++;
                HelpHandler();
            }
            if (trimmedInput.Equals("return", StringComparison.OrdinalIgnoreCase))
            {
                loopCount = 1;
                done = true;

            }
            if (trimmedInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {

                CloseProgram();
            }
            else
            {
                loopCount = 1;
                done = true;
            }
        }
    }
    public enum InputContext : int
    {
        Naming = 0,
        Bet = 1,
        Game = 2,
        Basic = 3,
        Return = 4,
        PlayerHitOrStand =5
    }
    public InputContext inputContext { get; set; }
    

    public void InputHandler(InputContext Context, out string input, out bool valid)
    {
        valid = false;
        inputContext = Context;
        input = Console.ReadLine();
        string trimmedInput = input.Trim();
        

        if ((trimmedInput.Equals("help", StringComparison.OrdinalIgnoreCase) || inputContext != InputContext.Naming) && trimmedInput.Equals("<help>", StringComparison.OrdinalIgnoreCase))
        {
            HelpHandler();
        }
        switch (inputContext)
        {
            case InputContext.Naming:
                {
                    while (trimmedInput.Length > 15)
                    {
                        valid = false;
                        InvalidInputHandler(InvalidInputContext.NameCharacterLimit);
                        InputHandler(InputContext.Naming, out trimmedInput, out valid);
                    }
                    Console.WriteLine("");
                    Console.WriteLine("InputHandler(): Player name: {0}", trimmedInput);
                    valid = true;
                    break;
                }
            case InputContext.Bet:
                {
                    valid = true;
                    break;
                }
            case InputContext.Game:
                {
                    while (trimmedInput == "")
                    {
                        valid = false;
                        InvalidInputHandler(InvalidInputContext.InvalidOption);
                        InputHandler(InputContext.Game, out input, out valid);
                    }
                    if (trimmedInput == "hit"|| trimmedInput == "stand")
                    valid = true;
                    Console.WriteLine("");
                    Console.WriteLine("");
                    break;
                }
            case InputContext.Basic:
                {
                    valid = true;
                    Console.WriteLine("");
                    Console.WriteLine("");
                    break;
                }
                case InputContext.Return:
                {
                    while (trimmedInput != "" && trimmedInput != "return" && trimmedInput != "<return>")
                    {
                        valid = false;
                        InvalidInputHandler(InvalidInputContext.InvalidOption);
                        InputHandler(InputContext.Return, out input, out valid);
                    }
                    valid = true;

                    break;
                }
                case InputContext.PlayerHitOrStand:
                {
                    while (trimmedInput != "hit" && trimmedInput != "stand")
                    {
                        valid = false;
                        InvalidInputHandler(InvalidInputContext.InvalidOption);
                        InputHandler(InputContext.PlayerHitOrStand, out input, out valid);
                    }
                    if (trimmedInput == "hit")
                    {
                        valid = false;
                        Console.WriteLine($"InputHandler(): Player is hitting");
                        DecisionHandler(GameCardChoice.Hit);
                        if (IsBust(currentIndex) == true)
                        {
                            
                            break;
                        }
                        InputHandler(InputContext.PlayerHitOrStand, out input, out valid);
                        
                    }
                    if (trimmedInput == "stand")
                    {
                        valid = true;
                    }
                    break;
                }
        }
    }
    public bool IsBust(int targetindex)
    {
        ReturnHandValues(targetindex);
        ReturnHandValuesDealer();
        if (highestValue > 21 || highestValueDealer > 21)
        {
            Bust?.Invoke(this, new GameStateEventArgs(targetindex));
            return true;
        }
        else
        {
            return false;
        }
    }
    public void CloseProgram()
    {
        System.Environment.Exit(0);
    }

    public void ReturnHandValues(int targetindex)
    {
        var (soft, hard) = FinalTable[targetindex].GetHandValueAsInt(targetindex);
        currSoftValue = soft;
        currHardValue = hard;
        if (currSoftValue < currHardValue && currHardValue <= 21)
        {
            highestValue = currHardValue;
        }
        else if ((currSoftValue> 0 && currHardValue == 0) || (currSoftValue == currHardValue))
        {
            highestValue = currSoftValue;
        }
        else if ((currSoftValue <= currHardValue) && currHardValue > 21)
        {
            highestValue = currSoftValue;
        }
    }
    public void ReturnHandValuesDealer()
    {
        var (soft, hard) = FinalTable[0].GetHandValueAsInt(0);
        currSoftValueDealer = soft;
        currHardValueDealer = hard;
        if (currSoftValueDealer < currHardValueDealer && currHardValueDealer <= 21)
        {
            highestValueDealer = currHardValueDealer;
        }
        else if ((currSoftValueDealer > 0 && currHardValueDealer == 0) || (currSoftValueDealer == currHardValueDealer))
        {
            highestValueDealer = currSoftValueDealer;
        }
        else if(currSoftValueDealer <= 21 && currHardValueDealer > 21)
        {
            highestValueDealer = currSoftValueDealer;
        }
       // highestValueDealer = Math.Max(currSoftValueDealer, currHardValueDealer);
      //  lowestValueDealer = Math.Min(currSoftValueDealer, currHardValueDealer);
        //Console.WriteLine($"ReturnHandValuesDealer(): currsoft {currSoftValueDealer} currHard {currHardValueDealer} highest {highestValueDealer}");
    }
    public async void Betting()
    {
        decimal betAmount;
        decimal potTotal;
        string input;
        Console.WriteLine("-------BETTING-------");
        Console.WriteLine($"Round: {roundCount}");
        Console.WriteLine("Press enter to continue or type help for options.");
        InputHandler(InputContext.Return, out input, out bool valid);

        if (input.Equals("help", StringComparison.OrdinalIgnoreCase))
        {

            Console.WriteLine("-------BETTING-------");
            Console.WriteLine("Press enter to continue or type help for options.");
            InputHandler(InputContext.Return, out input, out valid);
            valid = false;
        }
        if (valid == true)
        { 
            for (int i = 0; i < totalPlayers; i++)
            {

                if (FinalTable[i].IsDealer() == true && roundCount == 1)
                {
                    Console.WriteLine("Betting round begins.");
                    roundCount++;
                    currentIndex = i;
                }
                else if (FinalTable[i].IsDealer() == true && roundCount != 1)
                {
                    roundCount++;
                    currentIndex = i;
                }
                else if (FinalTable[i].IsNPC() == true && FinalTable[i].IsDealer() == false)
                {
                    currentIndex = i;
                    Console.WriteLine($"NPC {FinalTable[i].GetFirstName()} {FinalTable[i].GetLastName()} plays.");
                    betAmount = BetHandler(FinalTable[i].GetBet());
                    potTotal = betAmount;
                    Console.WriteLine($"Bets {betAmount}");

                    FinalTable[i].SetPotAmount(potTotal);

                }
                else if (FinalTable[i].IsNPC() != true && FinalTable[i].IsDealer() != true)
                {
                    currentIndex = i;
                    Console.WriteLine($"{playerName}'s turn.");
                    Console.WriteLine("----Options----");
                    Console.WriteLine("---------------");
                    Console.WriteLine("---Placing Bet-");
                    Console.WriteLine("---------------");
                    Console.WriteLine("---Min=10------");
                    Console.WriteLine("---Max=500-----");
                    if (FinalTable[i].GetCash() < 1000)
                    {
                        Console.WriteLine($"---Chips:{FinalTable[i].GetCash()}---");
                    }
                    else if (FinalTable[i].GetCash() > 9999)
                    {
                        Console.WriteLine($"---Chips:{FinalTable[i].GetCash()}-");
                    }
                    else if (FinalTable[i].GetCash() < 100)
                    {
                        Console.WriteLine($"---Chips:{FinalTable[i].GetCash()}----");
                    }
                    else
                    {
                        Console.WriteLine($"---Chips:{FinalTable[i].GetCash()}--");
                    }
                    Console.WriteLine("---Betting...--");
                    Console.WriteLine("---------------");
                    Console.WriteLine("---Enter Amt---");
                    Console.WriteLine();

                    validInput = false;
                    int x;


                    while (!validInput)
                    {


                        InputHandler(InputContext.Bet, out input, out valid);
                        if (input.Equals("help", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("---Betting...--");
                            Console.WriteLine("---------------");
                            Console.WriteLine("---Enter Amt---");
                            Console.WriteLine();

                            valid = false;

                        }
                        if (valid == true)
                        {


                            Int32.TryParse(input, out x);
                            if (x >= minBet && x <= maxBet)
                            {
                                validInput = true;
                                betAmount = x;
                                Console.WriteLine($"Bet amount = {betAmount}");
                                FinalTable[i].PlaceBet(betAmount);
                                FinalTable[i].SetPotAmount(betAmount);
                                Console.WriteLine($"Total pot: {FinalTable[i].GetPotAmount()}");
                                Console.WriteLine($"Chips: {FinalTable[i].GetCash()}");
                                Console.WriteLine("Press enter to continue or type help for options.");
                                InputHandler(InputContext.Basic, out _, out _);
                            }
                            if (x < minBet || x > maxBet)
                            {
                                InvalidInputHandler(InvalidInputContext.BettingLimit);
                            }
                        }

                    }
                }
            }
        }
    }
    public decimal BetHandler(int bettype)
    {
        // Console.WriteLine("init bethandler");
        //Console.WriteLine($"Bet type is {bettype}");
        decimal betamt = 0;
        Random variance = new();
        Random fifty = new();
        int avgvariance = fifty.Next(1, 3);
        int betvariance = variance.Next(0, 50);

        switch (bettype)
        {
            case 1:
                {
                    betamt = maxBet - betvariance;
                    break;
                }
            case 2:
                {
                    switch (avgvariance)
                    {
                        case 1:
                            {
                                betamt = avgBet - betvariance;
                                break;
                            }
                        case 2:
                            {
                                betamt = avgBet + betvariance;
                                break;
                            }
                    }
                    break;
                }
            case 3:
                {
                    betamt = minBet + betvariance;
                    break;
                }
        }
        return betamt;
    }
    
    
    private void AIStrats(int strattype,out bool done)
    {
        done = false;
        if (currSoftValue == 21||currHardValue == 21 || currSoftValueDealer == 21 || currHardValueDealer==21)
        {
            done = true;
            DecisionHandler(GameCardChoice.Stand);
            
        }
        else
        {
            switch (strattype)
            {
                case 1: //21er
                    {
                        if (currSoftValue < 21|| currHardValue < 21)
                        {
                            DecisionHandler(GameCardChoice.Hit);
                        }
                        else
                        {
                            done = true;
                            DecisionHandler(GameCardChoice.Stand);
                            
                        }
                        break;
                    }
                case 2: // Get 16
                    {
                        if (currSoftValue <= 16)
                        {
                            DecisionHandler(GameCardChoice.Hit);
                        }
                        else
                        {
                            done = true;
                            DecisionHandler(GameCardChoice.Stand);
                            
                        }
                        break;
                    }
                case 3: //Perfecto
                    {
                        if (currSoftValue >= 17 || currHardValue >= 19)
                        {
                            done = true;
                            DecisionHandler(GameCardChoice.Stand);
                        }
                        else
                        {
                            DecisionHandler(GameCardChoice.Hit);
                        }

                        break;
                    }
                case 4: //Dealer
                    {
                        ReturnHandValuesDealer();
                        Console.WriteLine($"AIStrats(): dealer hand value {highestValueDealer}");
                        
                        if (highestValueDealer < 17)
                        {
                            DecisionHandler(GameCardChoice.Hit);

                        }
                        if (highestValueDealer >= 17)
                        {
                            done = true;
                            DecisionHandler(GameCardChoice.Stand); 
                        }
                        break;
                    }
            }
            
        }
        
    }
    private void DecisionHandler(GameCardChoice c)
    {
        gameCardChoice = c;
       // Console.WriteLine($"DecisionHandler(): Decision Handler");
        switch (gameCardChoice)
        {
            case GameCardChoice.Hit:
                {
                    Console.WriteLine($"DecisionHandler(): {FinalTable[currentIndex].GetFirstName()} has chosen to hit.");
                    DealCard();
                    break;
                }
            case GameCardChoice.Stand:
                {
                    Console.WriteLine($"DecisionHandler(): {FinalTable[currentIndex].GetFirstName()} Standing.");
                    break;
                }
            case GameCardChoice.Bust:
            {
                break;
            }
        }
    }
    public void PlayerTurns()
    {
        Console.WriteLine($"PlayerTurns(): PLAYER TURNS REACHED");
        Console.WriteLine($"PlayerTurns(): {FinalTable[currentIndex].GetFirstName()}'s turn.");
        
        for (int i = 0; i < totalPlayers; i++)
        {
            bool turnisdone = false;
            //  Console.WriteLine($"PlayerTurns(): {currentIndex}");
            if (FinalTable[currentIndex].IsDealer() == true)
            {

                //Console.WriteLine($"PlayerTurns(): Dead end. Doing Nothing");


            }
            else if (FinalTable[currentIndex].IsNPC() == true && FinalTable[currentIndex].IsDealer() == false)
            {


                while (turnisdone == false)
                {
                    FinalTable[currentIndex].GetHandInfo(currentIndex);
                    ReturnHandValues(currentIndex);
                    AIStrats(FinalTable[currentIndex].GetStrat(), out bool done);
                    FinalTable[currentIndex].GetHandValue(currentIndex, out _);


                    turnisdone = done;
                }

            }
            else if (FinalTable[currentIndex].IsNPC() != true && FinalTable[currentIndex].IsDealer() != true)
            {

                while (turnisdone == false)
                {
                    if (PlayerBust == true)
                    {
                        Console.WriteLine($"Bad Luck. Ending turn.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"Your cards: {FinalTable[UserIndex].GetHandInfo(UserIndex)}");

                        if (turnisdone==false)
                        {

                            FinalTable[currentIndex].GetHandValue(currentIndex, out string report);
                            Console.WriteLine($"Select an option: Hit, stand");
                            InputHandler(InputContext.PlayerHitOrStand, out string input, out turnisdone);
                        }
                        else
                            Console.WriteLine($"Standing. Press return to continue");
                        InputHandler(InputContext.Return, out _, out _);
                    }
                }
            }
            else
            {
                // Console.WriteLine($"Reached else, no turns made");
            }
            currentIndex++;
        }
        
    }
    public void DealerTurn()
    {
        currentIndex = 0;
        bool turnisdone = false;
        while (turnisdone == false)
        {
            ReturnHandValuesDealer();
            FinalTable[currentIndex].GetHandInfo(currentIndex);
            Console.WriteLine($"Dealer has {FinalTable[currentIndex].GetHandValueAsInt(currentIndex)}");
            AIStrats(FinalTable[currentIndex].GetStrat(), out bool done);
            FinalTable[currentIndex].GetHandValue(currentIndex, out _);


            turnisdone = done;
        }
    }
    public void CheckWinners()
    {
        
        ReturnHandValuesDealer();
        Console.WriteLine($"{FinalTable[0].GetHandValue(0, out _).ToString()}");
        currentIndex = 0;
        foreach (Seats c in FinalTable)
        {
            if (currentIndex == 0) 
            { 
            
            }
            else
            {
                
                ReturnHandValues(currentIndex);
                Console.WriteLine($"{FinalTable[currentIndex].GetFirstName()} had {FinalTable[currentIndex].GetHandValueAsInt(currentIndex)}");
                if ((highestValue > highestValueDealer) && highestValue <= 21)
                {
                    
                    FinalTable[currentIndex].WinBet(currentIndex);
                }
                if (highestValue == highestValueDealer)
                {
                    FinalTable[currentIndex].TieBet(currentIndex);
                }
                if (highestValue <= 21 && highestValueDealer > 21)
                {
                    FinalTable[currentIndex].WinBet(currentIndex);
                }
                if (highestValue > 21 || (highestValue < highestValueDealer) && highestValueDealer <=21)
                {
                    FinalTable[currentIndex].LoseBet(currentIndex);
                }
            }
            currentIndex++;

        }
    }
    public static void EndTurnHandler(object sender, GameStateEventArgs e)
    {
        e.playerIsBust = true;
        PlayerBust = e.playerIsBust;
    }
    public void TieBet(int targetindex)
    {
        Console.WriteLine($"Player tie {FinalTable[currentIndex].GetFirstName()}");
        decimal pot = FinalTable[currentIndex].GetPotAmount();
        decimal cash = FinalTable[currentIndex].GetCash();
        decimal returnamount = cash + pot;
        FinalTable[currentIndex].SetCash(returnamount);
        FinalTable[currentIndex].SetPotAmount(0);
    }
    public void Cleanup()
    {
       
        currentIndex = 0;
        PlayerBust = false;
        foreach (Seats c in FinalTable)
        {
            
            DiscardHand(currentIndex);
            decimal t = FinalTable[currentIndex].GetCash();
             if (t < 50)
            {
                FinalTable.RemoveAt(currentIndex);
            }
            currentIndex++;
        }
        Reshuffle();
        currentIndex = 0;
        
    }
    public void Insurance() { }
}
        
        
        

        
        

        
    
    

    


