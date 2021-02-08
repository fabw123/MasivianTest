using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Casino.Models;
using Casino.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Casino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        private readonly ILogger<RouletteController> logger;

        public RouletteController(ILogger<RouletteController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public IEnumerable<Roulette> Get()
        {
            this.logger.LogInformation("List of roulettes called");
            var roulette = new Roulette();
            return roulette.FindAll();
        }

        [HttpPost]
        public int Post()
        {
            var roulette = new Roulette();
            roulette.Create();
            return roulette.Id;
        }

        [HttpPut("{id}")]
        [Route("open/{id}")]
        public bool Put(int id)
        {
            var roulette = new Roulette();
            var bet = new Bet();
            bet.IdRoulette = id;
            if (roulette.Find(id))
            {
                if (roulette.Status == Status.Open)
                {
                    this.logger.LogWarning($"Roulette {id} is already open");
                    return false;
                }
                bet.DeleteBetsByRoulette();
                roulette.Status = Status.Open;
                roulette.Edit();
                return true;
            }
            else
            {
                this.logger.LogWarning($"Does not exist roulette with the id {id}");
                return false;
            }
        }

        [HttpPost]
        [Route("bet")]
        public bool Bet([FromBody] Bet bet)
        {
            if (Request.Headers.TryGetValue("idUser", out var headerValue))
            {
                bet.IdUser = headerValue.ToString();
                if (!bet.GetValidationErrors())
                {
                    return false;
                }
                return bet.Create();
            }
            else
            {
                throw new Exception("No id User");
            }
        }

        [HttpPut("{id}")]
        [Route("close/{id}")]
        public IEnumerable<BetResult> Close(int id)
        {
            var roulette = new Roulette();
            if (roulette.Find(id))
            {
                if (roulette.Status == Status.Close)
                {
                    throw new Exception("Roulette already close");
                }
                roulette.Status = Status.Close;
                roulette.Edit();
            }
            else
            {
                return null;
            }
            var bet = new Bet() { IdRoulette = id };
            var bets = bet.FindAllByRoulette();
            var results = bet.FindResults(bets);
            return results;
        }
    }
}
