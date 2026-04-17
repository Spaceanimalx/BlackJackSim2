using System;
using System.Collections.Generic;
using System.Text;

namespace bjs
{

    public class GameHelper : Card
    {
        private static int skipIteratingIndex=0;
        public void CheckHandValues(bool dealerindex = false)
        {
            if (dealerindex == true)
            {
                FinalTable[0].ThisHand.ElementAt(0).SetParity(true);
                FinalTable[0].GetHandValue(0, out string handvaluereport);
                FinalTable[0].GetHandInfo(0);
                CheckHandNaturals(0,handvaluereport);
            }
            else
            {
                for(int i = 0;i<totalPlayers;i++)
                {
                    FinalTable[i].GetHandValue(i, out string handvaluereport);
                  //  Console.WriteLine($"CheckHandValues(): Checking hand of {FinalTable[i].GetFirstName()} {FinalTable[i].GetLastName()}");
                    CheckHandNaturals(i,handvaluereport);
                    currentIndex++;
                }
            }
        }
        public static void OnNat21(int index)
        {
            Console.WriteLine($"OnNat21(): {FinalTable[index].GetFirstName()} has a natural 21.");
        }
        public void EndOfRound()
        {
            Console.WriteLine($"EndOfRound(): Dealer reveal.");
            CheckHandValues(true);
        }
        public void GameStateCheck()
        {
            

            Console.WriteLine($"GameStateCheck(): {FinalTable[UserIndex].GetHandInfo(UserIndex)}");
            Console.WriteLine($"GameStateCheck(): {FinalTable[UserIndex].GetHandValue(UserIndex, out _)}");
            Console.WriteLine($"GameStateCheck(): Checking for naturals..");
            CheckHandValues(false);
            Console.WriteLine($"GameStateCheck(): All hands dealt. Type help for options, or press return to continue.");
            InputHandler(InputContext.Return, out string input, out bool valid);


        }
        public void CheckHandNaturals(int index, string handvaluereport)
        {

            char[] delimiters = [','];
            String pattern = @"(\d+)";
            string[] splitstr = handvaluereport.Split(delimiters);
            int value1;
            int value2;
            int hardcheck = 0;
            int softcheck = new();
            string handvaluereportsoft = splitstr[0];
            string handvaluereporthard = splitstr[1];

            foreach (System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(handvaluereportsoft, pattern))
            {
                value1 = Int32.Parse(m.Groups[1].Value);
                softcheck = value1;
            }
            foreach (System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(handvaluereporthard, pattern))
            {
                value2 = Int32.Parse(m.Groups[1].Value);
                hardcheck = value2;
                Console.WriteLine($"{softcheck} {hardcheck}");
            }
            if (hardcheck == 21)
            {
                Console.WriteLine($"{index} Nat21");
               // Nat21?.Invoke(index);
            }
        }
    }
}
