using Casino.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.Models
{
    public class BetResult
    {

        public BetResult(Bet bet, int winnerNumber)
        {
            this.Color = bet.Color;
            this.Number = bet.Number;
            this.IdBet = bet.Id;
            this.IdUser = bet.IdUser;
            this.WinnerNumber = winnerNumber;
            CalculateAmount(bet);
        }

        public decimal Amount { get; set; }
        public int? Number { get; set; }
        public Color? Color { get; set; }
        public int IdBet { get; set; }
        public int WinnerNumber { get; set; }
        public string IdUser { get; set; }

        private void CalculateAmount(Bet bet)
        {
            this.Amount = 0;
            switch (bet.Type)
            {
                case BetType.Color:
                    Color winnerColor = (this.WinnerNumber %2 ==0) ? Enums.Color.Red : Enums.Color.Black;
                    if (winnerColor == this.Color)
                    {
                        this.Amount = bet.Amount * 1.8M;
                    }
                    break;
                case BetType.Number:
                    if (this.WinnerNumber == this.Number)
                    {
                        this.Amount = bet.Amount * 5;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
