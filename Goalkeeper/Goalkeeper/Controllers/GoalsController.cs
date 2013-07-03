﻿using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Goalkeeper.Models;
using Raven.Abstractions.Commands;
using Raven.Client;
using Raven.Client.Linq;

namespace Goalkeeper.Controllers
{
    public class GoalsController : RavenDbController
    {
        private const string IdFormat = "Goals/{0}";

        [HttpGet("areas/{areaId}/goals")]
        public async Task<IEnumerable<Goal>> GetGoalsByArea(string areaId)
        {
            return await Session.Query<Goal>()
                .Where(x => x.AreaId == areaId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Goal>> Get()
        {
            return await Session.Query<Goal>().ToListAsync();
        }

        public async Task<Goal> Get(string id)
        {
            return await Session.LoadAsync<Goal>(string.Format(IdFormat, id));
        }

        public async Task<HttpResponseMessage> Post([FromBody]Goal value)
        {
            await Session.StoreAsync(value);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        public async Task<HttpResponseMessage> Put(string id, [FromBody]Goal value)
        {
            await Session.StoreAsync(value, id);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        public void Delete(string id)
        {
            Session.Advanced.Defer(new DeleteCommandData { Key = string.Format(IdFormat, id) });
        }
    }
}