﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Goalkeeper.Models;
using Raven.Abstractions.Commands;
using Raven.Client;

namespace Goalkeeper.Controllers
{
    public class AreasController : RavenDbController
    {
        public async Task<IEnumerable<Area>> Get()
        {
            return await Session.Query<Area>()
                .ToListAsync();
        }

        public async Task<Area> Get(string id)
        {
            return await Session.LoadAsync<Area>(id.Replace('-', '/'));
        }

        public async Task<HttpResponseMessage> Post([FromBody]Area value)
        {
            await Session.StoreAsync(value);

            var response = Request.CreateResponse(HttpStatusCode.Created, value);

            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = value.Id }));
            return response;
        }

        public async Task<HttpResponseMessage> Put(string id, [FromBody]Area value)
        {
            await Session.StoreAsync(value, id.Replace('-', '/'));

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public void Delete(string id)
        {
            Session.Advanced.Defer(new DeleteCommandData { Key = id.Replace('-', '/') });
        }
    }
}