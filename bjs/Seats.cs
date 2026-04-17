using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static bjs.GameHelper;


namespace bjs
{
    
    public class Seats : Card,IEnumerable
    {
        public int SeatCount { get; set; }
        private string seatedPlayerFN;
        private string seatedPlayerLN;
        public int SkipIndex { get; set; }
        private decimal cash { get; set; }
        private bool isDealer;
        public (int AiStrat, int AiBet) AIStyle { get; set; }

        public int AiStrat { get; set; }

        public int AiBet { get; set; }
        private bool isNPC;
        private int handValueInt { get; set; }
        private int handSoftValue { get; set; }
        private int handHardValue { get; set; }
        private string handValue { get; set; }
        public (int,int) handValueIntTuple { get; set; }

        public List<Cards> ThisHand = new List<Cards>();
        


        private decimal PotAmount
        {
            get; set;
        }
        private object handInfo { get; set; }
        public int hIndex { get; set; }
        private record struct HandValueReport
        {
            public string softValue;
            public string hardValue;
            public HandValueReport(string s, string h)
            {
                softValue = s;
                hardValue = h;
            }
        }
        record struct CardsInHand
        {
            public string card1 { get; set; }
            public string card2 { get; set; }
            public string card3 { get; set; }
            public string card4 { get; set; }
            public string card5 { get; set; }
            public string card6 { get; set; }
            public string card7 { get; set; }
            public string card8 { get; set; }
            public CardsInHand(string c1, string c2="", string c3="", string c4="",string c5="",string c6="", string c7="",string c8="")
            {
                card1 = c1;
                card2 = c2;
                card3 = c3;
                card4 = c4;
                card5 = c5;
                card6 = c6;
                card7 = c7;
                card8 = c8;
            }
            public override string ToString() => $"{card1} {card2} {card3} {card4} {card5} {card6} {card7} {card8}";
        }
        
        public Seats(int seats, string seatedplayerfn, string seatedplayerln, int potn, decimal pcash, (int, int) ai, bool dealerflag, bool npcflag, int handindex, object hand)
        {
            SeatCount = seats;
            seatedPlayerFN = seatedplayerfn;
            seatedPlayerLN = seatedplayerln;
            cash = pcash;
            (AiStrat, AiBet) = ai;
            isDealer = dealerflag;
            isNPC = npcflag;
            PotAmount = potn;
            ThisHand = (List<Cards>)hand;
            hIndex = handindex;
        }
        public Seats(int skidx)
        {
            SkipIndex = skidx;
        }
        public Seats()
        {

        }
        public object DisplayStats(int targetindex)
        {
            Console.WriteLine("STATS");
            Console.WriteLine($"Player: {seatedPlayerFN} {seatedPlayerLN}");
            Console.WriteLine($"Cash: {FinalTable[targetindex].cash}");
            Console.WriteLine($"Cards: {handValue}");
            return 0;
        }
        public override string ToString() => $"{SeatCount},{seatedPlayerFN} {seatedPlayerLN}. Funds {cash}$.";
        public int GetSeat() => SeatCount;
        public bool IsNPC() => isNPC;
        public bool IsDealer() => isDealer;
        public decimal GetPotAmount()
        {
            Console.WriteLine($"Reached GetPotAmount() {PotAmount}");
         return   PotAmount;
        }
        public decimal SetPotAmount(decimal value) => PotAmount = value + PotAmount;
        public string GetFirstName() => seatedPlayerFN;
        public string GetLastName() => seatedPlayerLN;

        public (int, int) SetStrats(int strat, int bet) => AIStyle = (strat, bet);

        public int GetSkipIndex() => SkipIndex;
        public int GetStrat() 
        {
            return AIStyle.AiStrat; 
        }
        public int GetBet() => AIStyle.AiBet;

        public decimal GetCash() => cash;

        public void PlaceBet(decimal value)
        {
            cash = cash - value;
        }

        public void WinBet(int targetindex)
        {
            Console.WriteLine($"{FinalTable[targetindex].GetFirstName()} wins vs the House.");
            decimal pot = FinalTable[targetindex].GetPotAmount();
            decimal winnings = (pot * Card.PayOutMultiplier);
            decimal gains = cash + winnings;
            Console.WriteLine($"Won: {winnings}$ from the original bet of {pot}");
            FinalTable[targetindex].SetCash(gains);
            PotAmount = 0;
        }
        
        public void LoseBet(int targetindex)
        {
            Console.WriteLine($"Bad luck {FinalTable[targetindex].GetFirstName()} loses their bet.");
            PotAmount = 0;
        }
        public decimal SetCash(decimal value) => cash = value;
        public string GetHandValue(int targetindex, out string handreport)
        {
            var handvaluereport = new HandValueReport();
            int i = 0;
            int tempvalueleft = new();
            int tempvalueright = new();
            int temphardvalue = new();

            foreach (Cards c in FinalTable[targetindex].ThisHand)
            {

                var cvalue = FinalTable[targetindex].ThisHand.ElementAt(i).GetCardValue();

                (int cval1, int cval2) = cvalue;
                //tempcardarray1[i] = cval1;
                //tempcardarray2[i] = cval2;
                tempvalueleft = tempvalueleft + cval1;
                tempvalueright = tempvalueright + cval2;
                i++;
            }
            if (FinalTable[targetindex].ThisHand.Count == 2)
            {
                if (tempvalueright == 11)
                {
                    temphardvalue = tempvalueleft + tempvalueright - 1;
                }
                else if (tempvalueright == 22 && tempvalueleft == 2)
                {
                    temphardvalue = Math.Max(tempvalueleft, 0) + tempvalueright - 12;

                }
            }
            else
            {
                temphardvalue = Math.Max(tempvalueleft, 0) + tempvalueright;
            }
            
        
            
            handValueIntTuple = (tempvalueleft, temphardvalue);
            handvaluereport.softValue = tempvalueleft.ToString();
            handvaluereport.hardValue = temphardvalue.ToString();
                
            handreport = handvaluereport.ToString();
         //   Console.WriteLine($"GetHandValue(): returning ");
            return handreport;
        }
        
        public (int,int) GetHandValueAsInt(int targetindex)
        {
           
            if (targetindex == 0)
            {
                (FinalTable[0].ThisHand).ElementAt(0).SetParity(true);
                (FinalTable[0].ThisHand).ElementAt(1).SetParity(true);
            }
            
            FinalTable[targetindex].GetHandValue(targetindex, out _);
            //Console.WriteLine($"{handValueIntTuple}");
            return handValueIntTuple;
        }
        public object GetHandInfo(int targetindex)
        {
            string tempcard ="";
            int i = 0;
            var h1 = new CardsInHand(tempcard);

            Console.WriteLine($"GetHandInfo(): Cards in hand: {FinalTable[targetindex].ThisHand.Count}");
            
            foreach(Cards c in FinalTable[targetindex].ThisHand)
            {
                
                tempcard = FinalTable[targetindex].ThisHand.ElementAt(i).ToString();
                
                switch (i)
                {
                    case 0:
                        {
                            h1.card1 = tempcard;
                            break;
                        }
                    case 1:
                        {
                            h1.card2 = tempcard;
                            break;
                        }
                    case 2:
                        {
                            h1.card3 = tempcard; 
                            break;
                        }
                    case 3:
                        {
                            h1.card4 = tempcard;
                            break;
                        }
                    case 4:
                        {
                            h1.card5 = tempcard;
                            break;
                        }
                    case 5:
                        {
                            h1.card6 = tempcard;
                            break;
                        }
                    case 6:
                        {
                            h1.card7 = tempcard;
                            break;
                        }
                    case 7:
                        {
                            h1.card8 = tempcard;
                            break;
                        }
                        
                }
                i++;
            }
            handInfo = h1;
            return handInfo;
            
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
