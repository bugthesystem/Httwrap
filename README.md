# Httwrap
General purpose, simple but useful HttpClient wrapper for .NET & Xamarin/Mono

[![Build status](https://ci.appveyor.com/api/projects/status/vyg8a2lsw1jf9nki?svg=true)](https://ci.appveyor.com/project/ziyasal/httwrap)[![Coverage Status](https://coveralls.io/repos/ziyasal/Httwrap/badge.svg)](https://coveralls.io/r/ziyasal/Httwrap)

## How to use  

**Install**  
```cs
PM> Install-Package Httwrap
```
**Init**  
```csharp
  IHttwrapConfiguration configuration = new HttwrapConfiguration("http://localhost:9000/");
  IHttwrapClient _httwrap = new HttwrapClient(configuration);
```

**GET**  
```csharp
IHttwrapResponse<Product> response = await _httwrap.GetAsync<Product>("api/products/1");
Dump(response.Data);
Dump(response.StatusCode);
```

**GET with QueryString**  
*_It supports `DataMember` and `IgnoreDataMember` attributes._*  
```csharp
/*
public class FilterRequest
{
  [DataMember(Name = "cat")]
  public string Category { get; set; }
  
  public int NumberOfItems { get; set; }
}
*/
var payload = new FilterRequest
{
  Category = "Shoes",
  NumberOfItems = 10
};

//Url: api/test?cat=Shoes&NumberOfItems=10
IHttwrapResponse<List<Product>> response =
                            await _client.GetAsync<List<Product>>("api/test", payload);

Dump(response.Data);
Dump(response.StatusCode);
```

**GET**  
```csharp
IHttwrapResponse response = await _httwrap.GetAsync("api/products");
List<Product> values = response.ResultAs<List<Product>>();
Dump(response.StatusCode);

/* ResultAs<T>() extension method uses Newtonsoft.Json serializer by default.  
To use your own serializer set JExtensions.Serializer = new YourCustomSerializerImpl();*/
```

**POST**  
```csharp
Product product = new Product{ Name= "Product A", Quantity = 3 };
IHttwrapResponse response = await _httwrap.PostAsync<Product>("api/products",product);
Dump(response.StatusCode);
```

**PUT**  
```csharp
Product product = new Product{ Name= "Product A", Quantity = 3 };
IHttwrapResponse response = await _httwrap.PutAsync<Product>("api/products/1",product);
Dump(response.StatusCode);
```

**PATCH**  
```csharp
Product product = new Product{ Name= "Product A", Quantity = 3};
IHttwrapResponse response = await _httwrap.PatchAsync<Product>("api/products/1",product);
Dump(response.StatusCode);
```

**DELETE**  
```csharp
IHttwrapResponse response = await _httwrap.DeleteAsync("api/products/1");
Dump(response.StatusCode);
```


**Error Handler**  
```csharp
IHttwrapResponse<List<Product>> response =
      await _httwrap.GetAsync<List<Product>>("api/products", (statusCode, body) =>
      {
        _logger.Error("Body :{0}, StatusCode :{1}", body, statusCode);
      });
```

**Basic Credentials**
```csharp
IHttwrapConfiguration configuration = new HttwrapConfiguration("http://localhost:9000/")
{
  Credentials = new BasicAuthCredentials("user", "s3cr3t")
};
IHttwrapClient _httwrap = new HttwrapClient(configuration);
```

**OAuth Credentials**  
_**Use existing ```token```**_
```csharp
IHttwrapConfiguration configuration = new HttwrapConfiguration("http://localhost:9000/")
{
  Credentials = new OAuthCredentials("token")
};
IHttwrapClient _httwrap = new HttwrapClient(configuration);
```
_**Use Username / password to get token from ```edpoint```**_

```csharp
IHttwrapConfiguration configuration = new HttwrapConfiguration("http://localhost:9000/")
{
  Credentials = new OAuthCredentials("us3r", "p4ssw0rd", BaseAddress + "/token")
};
IHttwrapClient _httwrap = new HttwrapClient(configuration);
```

**Interceptor**
```csharp
public class DummyInterceptor : IHttpInterceptor
    {
        private readonly IHttwrapClient _client;

        public void OnRequest(HttpRequestMessage request)
        {
            
        }

        public void OnResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            response.StatusCode = HttpStatusCode.Accepted;
        }
    }
    
client.AddInterceptor(new DummyInterceptor());
    
```

## Bugs
If you encounter a bug, performance issue, or malfunction, please add an [Issue](https://github.com/ziyasal/Httwrap/issues) with steps on how to reproduce the problem.

## TODO
- Add more tests
- Add more documentation

## License

Code and documentation are available according to the *MIT* License (see [LICENSE](https://github.com/ziyasal/Httwrap/blob/master/LICENSE)).

@ziλasal
