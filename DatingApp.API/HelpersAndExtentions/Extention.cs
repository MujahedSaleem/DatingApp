using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.ExtentionsMethods
{
    public static class Extention
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {

            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Header", "Application Error");
            response.Headers.Add("Access-Control-Allow-Origion", "*");

        }
        public static int Age(this DateTime DateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - DateOfBirth.Year;
            if (DateTime.Now.DayOfYear < DateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }
    }
}