// Import packages
using DotNetEnv;
using RestSharp;
using System.Text.Json;


// Load .env file
DotNetEnv.Env.Load();
DotNetEnv.Env.TraversePath().Load();

// Create a new Rest Client and call the Nylas API endpoint
var client = new RestSharp.RestClient("https://api.us.nylas.com/v3/grants/" +
             Environment.GetEnvironmentVariable("GRANT_ID") + "/events?calendar_id=" +
             Environment.GetEnvironmentVariable("CALENDAR_ID"));

var request = new RestRequest();
// Adding header and authorization
request.AddHeader("Content-Type", "application/json");
request.AddHeader("Authorization", "Bearer " +
        Environment.GetEnvironmentVariable("API_KEY_V3"));

DateTime now = DateTime.Now;
DateTimeOffset after = new DateTimeOffset(now.Year, now.Month, now.Day, 0, 0, 0, TimeSpan.Zero);
DateTimeOffset before = new DateTimeOffset(now.Year, now.Month, now.Day, 23, 59, 59, TimeSpan.Zero);

request.AddParameter("start", after.ToUnixTimeSeconds());
request.AddParameter("end", before.ToUnixTimeSeconds());

// We send the request
RestResponse response = (RestResponse)client.Execute(request);

// Parse the content as JSON
var content = JsonDocument.Parse(response.Content);
var data = content.RootElement.GetProperty("data");

// Loop the events
foreach (var calendar in data.EnumerateArray())
{
    DateTime dt_start = new DateTime(1970, 1, 1, 0, 0, 0, 0);
    var when = calendar.GetProperty("when");
    var participants = calendar.GetProperty("participants");
    string start_date = "";
    string end_date = "";
    string event_date = "";
    String participants_list = "";
    // Manage the different kind of date/time
    switch (when.GetProperty("object").ToString())
    {
        case "timespan":
            var start = when.GetProperty("start_time");
            var end = when.GetProperty("end_time");
            start_date = dt_start.AddSeconds(start.GetDouble()).ToLocalTime().ToString();
            end_date = dt_start.AddSeconds(end.GetDouble()).ToLocalTime().ToString();
            event_date = "From: " + start_date + " To: " + end_date;
            break;
        case "datespan":
            var _start = when.GetProperty("start_date");
            var _end = when.GetProperty("end_date");
            start_date = _start.ToString();
            end_date = _end.ToString();
            event_date = "From: " + start_date + " To: " + end_date;
            break;
        case "date":
            var date = when.GetProperty("date");
            start_date = date.ToString();
            event_date = "On: " + start_date;
            break;
    }
    // Get a list of participants
    foreach(var email in participants.EnumerateArray())
    {
        participants_list = participants_list + email.GetProperty("email") + ",";
    }

    // Print the details
    Console.WriteLine("Title : {0}, {1}, Participants: {2} \n",
    calendar.GetProperty("title"), event_date, participants_list.TrimEnd(','));
}

// Wait for user input before closing the terminal
Console.Read();
