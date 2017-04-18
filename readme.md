webhose.io client for .NET
============================

[![webhoseio MyGet Build Status](https://www.myget.org/BuildSource/Badge/webhoseio?identifier=183db351-e812-4cf6-9be6-b5c3977977b3)](https://www.myget.org/)

A simple way to access the [Webhose.io](https://webhose.io) API from your .NET code.
Supported .NET frameworks are .NET 3.5 and higher.


```csharp
using webhoseio;

var client = new WebhoseClient(token: YOUR_API_KEY);
var output = await client.QueryAsync("filterWebContent", new Dictionary<string, string> { { "q", "github" } });

Console.WriteLine(output["posts"][0]["text"]); // Print the text of the first post
Console.WriteLine(output["posts"][0]["published"]); // Print the text of the first post publication date

// Get the next batch of posts
output = await output.GetNextAsync();
Console.WriteLine(output["posts"][0]["thread"]["site"]); // Print the site of the first post
```

API Key
-------

To make use of the webhose.io API, you need to obtain a token that would be
used on every request. To obtain an API key, create an account at
https://webhose.io/auth/signup, and then go into
https://webhose.io/dashboard to see your token.

Installing
----------
You can install using NuGet:

```powershell
Install-Package webhoseio
```
 
 Use the API
-----------

To get started, you need to import the library, and set your access token.
(Replace `YOUR_API_KEY` with your actual API key).

```csharp
using webhoseio;

var client = new WebhoseClient(token: YOUR_API_KEY);
```

**API Endpoints**

The first parameter the `Query` function accepts is the API endpoint string. Available endpoints:
* `filterWebContent` - access to the news/blogs/forums/reviews API
* `productFilter` - access to data about eCommerce products/services
* `darkFilter` - access to the dark web (coming soon)

Now you can make a request and inspect the results:

```csharp
var output = await client.QueryAsync("filterWebContent", new Dictionary<string, string> { { "q", "github" } });

Console.WriteLine(output["totalResults"]); 
// 15565094

Console.WriteLine(output["posts"].Count());
// 100

Console.WriteLine(output["posts"][0]["language"]);
// english

Console.WriteLine(output["posts"][0]["title"]);
// Putting quotes around dictionary keys in JS
```

For your convenience, the ouput object is iterable, so you can loop over it
and get all the results of this batch (up to 100). 

```csharp
var totalWords = 0;
foreach(var post in output["posts"])
{
    totalWords += post["text"].ToString().Split(' ').Length;
}
Console.WriteLine(totalWords);
// 8822
```

Full documentation
------------------

### Configuration

#### `new WebhoseClient(string token)`

Creates new client with the provided API key.

Arguments:

  * `token` - your API key

#### `new WebhoseClient()`

Creates new client with the API key read from `appSettings` with key `webhoseio:token`.

Example `App.config` or `Web.config`:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <appSettings>
    <add key="webhoseio:token" value="YOUR_API_KEY" />
  </appSettings>
</configuration>
```

### Query

#### `QueryAsync(endpoint, parameters)`

Construct query and fetch first page of results.

Signature: `Task<WebhoseJsonResponseMessage> WebhoseClient.QueryAsync(string endpoint, IDictionary<string, string> parameters)`

Arguments:

  * `endpoint`: 
    * `filterWebContent` - access to the news/blogs/forums/reviews API
    * `productFilter` - access to data about eCommerce products/services
    * `darkFilter` - access to the dark web (coming soon)
  * `parameters`: A key value dictionary. The most common key is the "q" parameter that hold the filters Boolean query. [Read about the available filters](https://webhose.io/documentation).

##### Example

```csharp
WebhoseClient client = new WebhoseClient();
WebhoseJsonResponseMessage response = await client.QueryAsync("filterWebContent", new Dictionary<string, string> { { "q", "github" } }));
```

#### `Query(endpoint, parameters)`

Synchronous variant of `Json(endpoint, parameters)`.

Signature: `WebhoseJsonResponseMessage WebhoseClient.Query(string endpoint, IDictionary<string, string> parameters)`

##### Example

```csharp
WebhoseClient client = new WebhoseClient();
WebhoseJsonResponseMessage response = client.Query("filterWebContent", new Dictionary<string, string> { { "q", "github" } }));
```

### Response

##### `Json`

Response object as [`Newtonsoft.Json`](https://github.com/JamesNK/Newtonsoft.Json) object.

Signature: `JObject Json { get; }`

##### `GetNextAsync()`

Fetch the next page of results.

Signature: `Task<WebhoseJsonResponseMessage> WebhoseJsonResponseMessage.GetNextAsync()`

##### Example

```csharp
WebhoseJsonResponseMessage output = await response.GetNextAsync();
```

##### `GetNext()`

Synchronous variant of `GetNextAsync()`.

Signature: `WebhoseJsonResponseMessage WebhoseJsonResponseMessage.GetNext()`

##### Example

```csharp
WebhoseJsonResponseMessage output = response.GetNext();
```
