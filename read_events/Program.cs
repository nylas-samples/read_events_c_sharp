// Import packages
using RestSharp;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// Load .env file
DotNetEnv.Env.Load();
DotNetEnv.Env.TraversePath().Load();

// Create a new Rest Client and call the Nylas API endpoint
var client = new RestSharp.RestClient("https://api.nylas.com/events");
// Wait until the connection is done
client.Timeout = -1;
// We want a GET method
var request = new RestSharp.RestRequest(Method.GET);
// Adding header and authorization
request.AddHeader("Content-Type", "application/json");
request.AddHeader("Authorization", "Bearer " + System.Environment.GetEnvironmentVariable("ACCESS_TOKEN"));

// Get current date and time
DateTime now = DateTime.Now;
DateTimeOffset after = new DateTimeOffset(now.Year, now.Month, now.Day, 0, 0, 0, TimeSpan.Zero);
DateTimeOffset before = new DateTimeOffset(now.Year, now.Month, now.Day, 23, 59, 59, TimeSpan.Zero);

//Add parameters
request.AddParameter("calendar_id", System.Environment.GetEnvironmentVariable("CALENDAR_ID")!);
request.AddParameter("starts_after", after.ToUnixTimeSeconds());
request.AddParameter("ends_before", before.ToUnixTimeSeconds());

// Call the API
IRestResponse response = client.Execute(request);
// Convert the response from JSON
dynamic? responseContent = JsonConvert.DeserializeObject( response.Content );
// Loop through each item
foreach( dynamic? i in responseContent! )
{
    string title = i.title;
    double start = i.when["start_time"];
    double end = i.when["end_time"];
    DateTime dt_start = new DateTime(1970, 1, 1, 0, 0, 0, 0);
    JArray participants = i.participants;
    String participants_list = "";
    foreach (JObject item in participants){
        string? email = item.GetValue("email")!.ToString();
        participants_list = participants_list + email + ",";
    }
    // Print results
    Console.WriteLine("Title : {0}, Start: {1}, End: {2}, Participants: {3} \n", 
    title, dt_start.AddSeconds(start).ToLocalTime(),dt_start.AddSeconds(end).ToLocalTime(),participants_list.TrimEnd(','));
}
// Wait for user input before closing the terminal
Console.Read();
