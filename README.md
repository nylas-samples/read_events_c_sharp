# read_events_c_sharp

This sample will show you to easily read events for the current day using the Events Endpoint API.

## Setup

```bash
$ mkdir read_events && cd read_events

$ dotnet new console
```

### System dependencies

- RestSharp
- DotNetEnv
- Newtonsoft.Json

### Gather environment variables

You'll need the following values:

```text
ACCESS_TOKEN = ""
CALENDAR_ID = ""
```

Add the above values to a new `.env` file:

```bash
$ touch .env # Then add your env variables
```

# Compilation

To compile the comment we need to use this `dotnet` command:

```bash
$ dotnet run --project read_events.csproj
```

## Usage

Run the app:

```bash
$ ./bin/Debug/net6.0/read_events
```

When you run it, it will display all the events for the current date and wait for a keystoke to end


## Learn more

Visit our [Nylas Calendar API documentation](https://developer.nylas.com/docs/connectivity/calendar/) to learn more.
