using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Context.DemoApi;
using DemoApi.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Controllers
{
    [Route("api/Autho")]
    [ApiController]
    public class AuthoController : ControllerBase
    {
        private readonly TimeSpan _expireTime = new TimeSpan(0, 10, 0);
        private string TokenKeyName = "token";
        private DemoContext _db;
        public AuthoController(DemoContext db)
        {
            _db = db ?? throw new NullReferenceException(nameof(db));
        }
        [HttpGet]
        public ActionResult<bool> Authorize()
        {
            if (!Request.Cookies.ContainsKey(TokenKeyName)) { return false; }
            var token = Request.Cookies[TokenKeyName];
            if (string.Compare(token, GetRouteHsah()) != 0) { return false; }
            UpdateCookieTokenExpireTime(token);
            KeepCurrentCookies();
            return true;
        }

        // POST: api/Autho
        [HttpPost]
        public async Task<ICollection<Permission>> Post(User user)
        {
            user.Autho = Hash(user.Autho);
            var tmpUser = await _db.Users
                .Where(u => u.Account == user.Account && u.Autho == user.Autho)
                .FirstAsync();

            if (tmpUser == null) { throw new AuthenticationException($"{user.Account}"); }
            UpdateCookieTokenExpireTime(GetRouteHsah());
            KeepCurrentCookies();
            return tmpUser.Permissions;
        }

        [HttpGet("logout")]
        public void Logout()
        {
            KeepCurrentCookies();
        }

        // PUT: api/Autho
        [HttpPut]
        public async Task Put(User user)
        {
            _db.Add(user);
            await _db.SaveChangesAsync();
        }

        #region Private method
        private void RemoveCookieToken()
        {
            KeepCurrentCookies();
        }

        private void UpdateCookieTokenExpireTime(string token)
        {
            Response.Cookies.Append(
                TokenKeyName,
                token,
                new CookieOptions { HttpOnly = true, Expires = new DateTimeOffset(DateTime.Now, _expireTime) });
        }

        //Todo: 這邊可能會傳遞不應傳遞的資訊
        private void KeepCurrentCookies()
        {
            var cookies = Request.Cookies.Where(c => c.Key != TokenKeyName);

            foreach (var cookie in cookies)
            {
                Response.Cookies.Append(cookie.Key, cookie.Value);
            }
        }
        private string GetRouteHsah()
        {
            var route = Request.RouteValues
                    .Select(x => $"{x.Key}{x.Value}")
                    .Aggregate((x, y) => x + y);
            return Hash(route);
        }

        private string Hash(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return Convert.ToBase64String(KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA1, 10000, 256 / 8));
        }
        #endregion
    }
}
