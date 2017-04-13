using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Backend.DataObjects;
using Backend.Models;
using Microsoft.Azure.Mobile.Server;

namespace Backend.Controllers
{
    public class HabitController : TableController<Habit>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Habit>(context, Request, enableSoftDelete: true);
        }


        public string UserId => ((ClaimsPrincipal)this.User)
                                .FindFirst(ClaimTypes.NameIdentifier)
                                .Value;

        public void ValidateOwner(string id)
        {
            var result = Lookup(id).Queryable.FirstOrDefault(habit => habit.UserId.Equals(UserId));
            if (result == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
        // GET tables/Habit
        public IQueryable<Habit> GetAllHabits()
        {
            return Query().Where(habit=>habit.UserId.Equals(UserId));
        }

        // GET tables/Habit/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Habit> GetHabit(string id)
        {
            return new SingleResult<Habit>(Lookup(id).Queryable.Where(habit=>habit.UserId.Equals(UserId)));
        }

        // PATCH tables/Habit/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Habit> PatchHabit(string id, Delta<Habit> patch)
        {
            ValidateOwner(id);
            return UpdateAsync(id, patch);
        }

        // POST tables/Habit
        public async Task<IHttpActionResult> PostHabit(Habit habit)
        {
            habit.UserId = UserId;
            Habit current = await InsertAsync(habit);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Habit/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteHabit(string id)
        {
            ValidateOwner(id);
            return DeleteAsync(id);
        }
    }
}