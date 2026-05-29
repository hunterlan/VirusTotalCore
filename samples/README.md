# VirusTotalCore Samples

A runnable console application demonstrating all endpoint groups provided by the VirusTotalCore library suite.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- A valid [VirusTotal API key](https://www.virustotal.com/gui/user/apikey)

## Setup

Set your API key as an environment variable before running:

```bash
# Linux / macOS
export VIRUSTOTAL_API_KEY=<your-api-key>

# Windows (Command Prompt)
set VIRUSTOTAL_API_KEY=<your-api-key>

# Windows (PowerShell)
$env:VIRUSTOTAL_API_KEY="<your-api-key>"
```

## Running

From the repository root:

```bash
dotnet run --project samples/VirusTotalCore.Samples
```

Or from inside the project folder:

```bash
cd samples/VirusTotalCore.Samples
dotnet run
```

## What the sample demonstrates

| Section       | Endpoints used                                              |
|---------------|-------------------------------------------------------------|
| Files         | `FilesEndpoint.PostFile`, `FilesEndpoint.GetReport`         |
| URLs          | `UrlEndpoint.Scan`, `UrlEndpoint.GetReport`                 |
| IP Addresses  | `AddressIpEndpoint.GetReport`                               |
| Domains       | `DomainsEndpoint.GetReport`                                 |
| Comments      | `CommentEndpoint.GetLatestComments`, `CommentEndpoint.AddVote`, `AddressIpEndpoint.AddComment` |

All sections catch `VirusTotalException` and print a user-friendly message so errors from one section do not abort the rest of the demo.
