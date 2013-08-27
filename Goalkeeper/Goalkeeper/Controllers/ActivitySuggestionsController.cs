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
    public class ActivitySuggestionsController : RavenDbController
    {
        [HttpGet("api/goals/{goalId}/open-suggestions")]
        public async Task<object> GetByGoalId(string goalId)
        {
            goalId = goalId.Replace('-', '/');
            var goal = await Session.LoadAsync<Goal>(goalId);

            var activitySuggestions = await Session.Query<ActivitySuggestion>()
                                                   .Where(x => x.GoalId == goalId && x.SuggestionState == ActivitySuggestionState.Open)
                                                   .ToListAsync();

            return new { goal, activitySuggestions };
        }

        public async Task<IEnumerable<ActivitySuggestion>> Get()
        {
            return await Session.Query<ActivitySuggestion>()
                                .ToListAsync();
        }

        public async Task<ActivitySuggestion> Get(string id)
        {
            return await Session.LoadAsync<ActivitySuggestion>(id.Replace('-', '/'));
        }

        public async Task<HttpResponseMessage> Post([FromBody]ActivitySuggestion value)
        {
            await Session.StoreAsync(value);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        public async Task<HttpResponseMessage> Put(string id, [FromBody]ActivitySuggestion value)
        {
            await Session.StoreAsync(value, id.Replace('-', '/'));

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        public void Delete(string id)
        {
            Session.Advanced.Defer(new DeleteCommandData { Key = id.Replace('-', '/') });
        }
    }
}