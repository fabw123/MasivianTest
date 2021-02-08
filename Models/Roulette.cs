using Casino.DataAccess;
using Casino.Models.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.Models
{
    public class Roulette : IDomain
    {
        private readonly RedisDataAccessModel dataModel;
        private readonly ILogger<Roulette> logger;

        public Roulette()
        {
            this.dataModel = new RedisDataAccessModel();
        }
        public int Id { get; set; }

        public Status Status { get; set; }


        public bool Create()
        {
            try
            {
                var list = this.FindAll();
                var lastId = (list?.Any() ?? false) ? list.Max(x => x.Id) : 0;
                this.Id = lastId + 1;
                this.Status = Status.Close;
                this.dataModel.Connect();
                return this.dataModel.Save(this);
            }
            finally
            {
                this.dataModel.Disconnect();
            }
        }

        public bool Find(int id)
        {
            try
            {
                this.dataModel.Connect();
                var roulette = this.dataModel.Find<Roulette>(id);
                if (roulette != null)
                {
                    this.Id = roulette.Id;
                    this.Status = roulette.Status;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                this.dataModel.Disconnect();
            }
        }

        public List<Roulette> FindAll()
        {
            try
            {
                this.dataModel.Connect();
                var list = this.dataModel.FindAll<Roulette>().ToList();
                return list;
            }
            finally
            {
                this.dataModel.Disconnect();
            }
        }

        public bool Edit()
        {
            this.dataModel.Connect();
            return this.dataModel.Save(this);
        }

        public string CreateKey()
        {
            string key = $"{typeof(Roulette)}";
            return key;
        }


    }
}
