using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Context.DemoApi;
using DemoApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Controllers
{
    [ApiController]
    [Route("/api/activities")]
    public class ActivityController : ControllerBase
    {
        private readonly DemoContext _db;
        public ActivityController(DemoContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpGet]
        public async Task<ActionResult<List<Activity>>> GetAll()
        {
            var activities = await _db.Activities.ToListAsync();

            return activities;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> Get(long id)
        {
            var activity = await _db.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            return activity;
        }

        [HttpPost]
        public async Task Post(Activity activity)
        {
            _db.Activities.Add(activity);
            await _db.SaveChangesAsync();
        }

        [HttpPost("{id}")]
        public async Task Post(Activity activity, long id)
        {
            var activities = await _db.Activities.FindAsync(id);

            activities.Title = activity.Title;
            activities.Location = activity.Location;
            activities.StartTime = activity.StartTime;
            activities.EndTime = activity.EndTime;
            activities.Description = activity.Description;

            await _db.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var activity = await _db.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            _db.Activities.Remove(activity);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}

