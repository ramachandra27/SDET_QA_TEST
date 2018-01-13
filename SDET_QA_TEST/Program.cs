using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace SDET_QA_TEST
{
    public static class StateDetailService
    {
        static void Main(string[] args)
        {

            HttpClient cons = new HttpClient
            {
                BaseAddress = new Uri("http://services.groupkt.com/state/")
            };
            cons.DefaultRequestHeaders.Accept.Clear();
            cons.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));           
            Console.WriteLine("Press q to quit");
            string stateName = string.Empty; string stateAbbrivation = string.Empty;
            bool quitFlag = false;
            while (!quitFlag)
            {
                string statePattern = @"^([\sA-Za-z]+)$";
                Console.WriteLine("\n Please enter the state name ");
                stateName = Console.ReadLine();
                bool stateStatus = Regex.IsMatch(stateName, statePattern);
                if (!stateStatus)
                {
                    Console.WriteLine("\n Please enter the correct format of state");
                    Console.WriteLine("\n Press q to quit");
                    Console.ReadLine();
                    break;
                }

                Console.WriteLine("\n Please enter the state abbrivation");
                string abbrPattern = @"^([\sA-Za-z]+)$";
               
                stateAbbrivation = Console.ReadLine();
                bool abbStatus = Regex.IsMatch(stateAbbrivation, abbrPattern);
                if(!abbStatus)
                {
                    Console.WriteLine("\n Please enter the correct format of abbrivation");
                    Console.WriteLine("\n Press q to quit");
                    Console.ReadLine();
                    break;
                }
              

                if (string.IsNullOrWhiteSpace(stateAbbrivation) && string.IsNullOrWhiteSpace(stateName))
                {
                    Console.WriteLine("\n Please enter the correct format of state or abbrivation");
                    Console.WriteLine("\n Press q to quit");
                    Console.ReadLine();
                    break;
                }
                else
                {
                    GetStateDetails(cons, stateName, stateAbbrivation.ToUpper()).Wait();
                }
            } 
        }

        public static async Task GetStateDetails(HttpClient cons, string stateName, string stateAbbrivation)
        {
            try
            {
                HttpResponseMessage res = await cons.GetAsync("get/USA/all");
                res.EnsureSuccessStatusCode();
                if (res.IsSuccessStatusCode)
                {
                    var responseAsString = await res.Content.ReadAsStringAsync();
                    var responseAsConcreteType = JsonConvert.DeserializeObject<RootObject>(responseAsString);
                    IEnumerable<Result> state = new List<Result>();
                    Console.WriteLine("\n");
                    if (!string.IsNullOrWhiteSpace(stateName) && !string.IsNullOrWhiteSpace(stateAbbrivation))
                    {
                        state = from p in responseAsConcreteType.RestResponse.result
                                where p.name == stateName && p.abbr == stateAbbrivation
                                select p;
                    }
                    else if (!string.IsNullOrWhiteSpace(stateName) && string.IsNullOrWhiteSpace(stateAbbrivation))
                    {
                        state = from p in responseAsConcreteType.RestResponse.result
                                where p.name == stateName
                                select p;
                    }
                    else if (string.IsNullOrWhiteSpace(stateName) && string.IsNullOrWhiteSpace(stateAbbrivation))
                    {
                        state = from p in responseAsConcreteType.RestResponse.result
                                where p.abbr == stateAbbrivation
                                select p;
                    }

                    var result = state.FirstOrDefault(); 
                    if(result != null)
                    {
                        Console.WriteLine("\n");
                        Console.WriteLine("LargestCity---------------------capital");
                        Console.WriteLine("{0}\t\t{1}", result.largest_city, result.capital);
                    }
                    else
                    {
                        Console.WriteLine("no records found for the input");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n opps some error occurred please check entered correct state name and abbrivation", ex.Message);
            } 
        }
    }
}
