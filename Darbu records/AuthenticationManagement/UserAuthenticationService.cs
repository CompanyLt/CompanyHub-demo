
using Darbu_records.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System;
using Darbu_records.OldItems;
using Darbu_records.Query;

namespace Darbu_records.AuthenticationManagement
{
    public class UserAuthenticationService : IUserAuthenticationService
    {

        IHttpContextAccessor _httpContextAccessor;

        public UserAuthenticationService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> SetAuthentication(Login_validation validationForm)
        {
            SqlQuery query = new SqlQuery();

            //operacijos marsrutizavimas
            Query_Action action = new Query_Action(query.dbConn, query.Login_Check);     
            //patikriname per sql uzklausa ar yra toks vartotojas jei taip tai sukuriam authentication
            if (validationForm.Name != null && validationForm.Password != null && action.Client_check(validationForm.Name, validationForm.Password))
            {
                
                List<Claim> claims = new List<Claim>();

             
                claims.Add(new Claim(ClaimTypes.NameIdentifier, action.Client_get().ToString()));            
                claims.Add(new Claim("DepartmentId", action.Department_get().ToString()));
                claims.Add(new Claim(ClaimTypes.Role, action.role));
                claims.Add(new Claim(ClaimTypes.Name, validationForm.Name));
              

                //perduodame i konstrktoriu claims konteineri ir objekta cookie
                ClaimsIdentity identifikacija = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


                //i objekta principal perduodame objekta identity
                ClaimsPrincipal pagrindinis = new ClaimsPrincipal(identifikacija);

                AuthenticationProperties properties = new AuthenticationProperties();
                //cia kodas jei true tai neistrina slapuko po uzdarymo
                if (validationForm.rememberMe)
                {
                    properties.IsPersistent = true;
                }

                properties.ExpiresUtc = DateTime.UtcNow.AddDays(10);

                //cia ikisam i sesija
              
                await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, pagrindinis, properties);
                return true;





            }
            else
            {
                return false;
            }
        }
    }
}
