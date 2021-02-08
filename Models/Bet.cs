using Casino.DataAccess;
using Casino.Models.Enums;
using Casino.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.Models
{
    public class Bet : IDomain
    {
        private RedisDataAccessModel dataModel;
        private Roulette roulette;

        public Bet()
        {
            this.dataModel = new RedisDataAccessModel();
            this.roulette = new Roulette();
        }

        public int Id { get; set; }
        public BetType Type { get; set; }
        public int? Number { get; set; }
        public Color? Color { get; set; }
        public decimal Amount { get; set; }
        public string IdUser { get; set; }
        public int IdRoulette { get; set; }

        public List<Bet> FindAll()
        {
            try
            {
                this.dataModel.Connect();
                var list = this.dataModel.FindAll<Bet>().ToList();
                return list;
            }
            finally
            {
                this.dataModel.Disconnect();
            }
        }

        public List<Bet> FindAllByRoulette()
        {
            try
            {
                this.dataModel.Connect();
                var list = this.dataModel.FindAll<Bet>().Where(x => x.IdRoulette == this.IdRoulette).ToList();
                return list;
            }
            finally
            {
                this.dataModel.Disconnect();
            }
        }

        public List<BetResult> FindResults(List<Bet> bets)
        {
            var numberResult = RandomHelper.GetRouletteResult();
            var results = new List<BetResult>();
            if (bets?.Any() ?? false)
            {
                foreach (var bet in bets)
                {
                    BetResult result = new BetResult(bet, numberResult);
                    results.Add(result);
                }
            }
            return results;
        }

        public bool Create()
        {
            try
            {
                var list = this.FindAll();
                var lastId = (list?.Any() ?? false) ? list.Max(x => x.Id) : 0;
                this.Id = lastId + 1;
                this.dataModel.Connect();
                return this.dataModel.Save(this);
            }
            finally
            {
                this.dataModel.Disconnect();
            }
        }

        public bool DeleteBetsByRoulette()
        {
            try
            {
                var bets = FindAllByRoulette();
                this.dataModel.Connect();
                foreach (var bet in bets)
                {
                    if (!this.dataModel.Delete(bet))
                    {
                        return false;
                    }
                }
                return true;
            }
            finally
            {
                this.dataModel.Disconnect();
            }
        }

        public bool GetValidationErrors()
        {
            bool valid = true;
            var roulettes = this.roulette.FindAll();
            if (this.Amount > 10000)
            {
                valid = false;
            }
            if (!roulettes.Exists(x => x.Id == this.IdRoulette && x.Status == Status.Open))
            {
                valid = false;
            }
            return valid;
        }

        public string CreateKey()
        {
            string key = $"{typeof(Bet)}";
            return key;
        }
    }
}
