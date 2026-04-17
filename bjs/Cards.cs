using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static bjs.GameHelper;

namespace bjs
{
    public class Cards : Seats, IEnumerable
    {
        
        public string CardName { get; set; }
        public (int, int) CardValues { get; set; }
        public bool FaceParity { get; set; }
        private (int, int) UnknownValues;
        




        public Cards(string cardn, (int, int) value, bool fp)
        {
            CardName = cardn;
            CardValues = value;
            FaceParity = fp;
        }
        public Cards() { }


        public override string ToString() => $"{CardName},{CardValues}";
        public string GetCardInfo()
        {
            return CardName;
        }
        public (int, int) GetCardValue()
        {
            //Console.WriteLine($"Cards.GetCardValue reached.");
            //Console.WriteLine($"Faceparity = {FaceParity}");
            if (FaceParity == true)
            {
                //Console.WriteLine($"Card is faceup");
                return CardValues;
            }
            else
            {
                //Console.WriteLine($"Card is facedown");
                UnknownValues = (0, 0);
                Console.WriteLine($"Facedown Card.");
                return UnknownValues;
            }
        }
        public bool IsFaceDown(bool parity)
        {
            switch (parity)
            {
                case true:
                    {
                        return true;
                        break;
                    }
                case false:
                    {
                        return false;
                        break;
                    }
            }
        }
        
        public bool SetParity(bool parity)
        {
            switch (parity)
            {
                case true:
                    {
                        //Console.WriteLine($"Parity swapped. Cards are faceup.");
                        FaceParity = true;
                        break;
                    }
                case false:
                    {
                        //Console.WriteLine($"Parity swapped. Cards are facedown.");
                        FaceParity = false;
                        break;
                    }
            }

            
            return FaceParity;
        }
        public bool SwapParity()
        {
            
            if (FaceParity == true)
            {
                //Console.WriteLine($"Parity swapped. Cards are facedown.");
                FaceParity = false;
            }
            else
            {
                //Console.WriteLine($"Parity swapped. Cards are faceup.");
                FaceParity = true;
            }
            return FaceParity;
        }

        
    }
}
