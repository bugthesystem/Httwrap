# Httwrap
General purpose HttpClient wrapper

[![Build status](https://ci.appveyor.com/api/projects/status/vyg8a2lsw1jf9nki?svg=true)](https://ci.appveyor.com/project/ziyasal/httwrap)

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

##Bugs
If you encounter a bug, performance issue, or malfunction, please add an [Issue](https://github.com/ziyasal/Httwrap/issues) with steps on how to reproduce the problem.

##TODO
- Add more tests
- Add more documentation

##License

Code and documentation are available according to the *MIT* License (see [LICENSE](https://github.com/ziyasal/Httwrap/blob/master/LICENSE)).
