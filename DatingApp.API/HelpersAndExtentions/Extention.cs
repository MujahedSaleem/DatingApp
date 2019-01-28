using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DatingApp.API.HelpersAndExtentions
{
    public static class Extention
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {

            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application Error");
            response.Headers.Add("Access-Control-Allow-Origion", "*");

        }
        public static void AddPagination(this HttpResponse response, int currentPage, int itemPerpage, int totalItem, int totalPages)
        {
            var PaginationHeader = new PaginationHeader(currentPage, totalPages, itemPerpage, totalItem);
            var camelCase = new JsonSerializerSettings();
            camelCase.ContractResolver = new CamelCasePropertyNamesContractResolver();


            response.Headers.Add("Pagination",
             JsonConvert.SerializeObject(PaginationHeader, camelCase));
             response.Headers.Add("Access-Control-Expose-Headers",
             "Pagination");
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