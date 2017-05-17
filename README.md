# GoogleMeasurement
Object oriented portable client library for Google Measurement Protocol. Allows you to engage Google Analytics in your .NET application.

Usage example
```c#
var measurementClient = new MeasurementClient("<Your GA Property ID>", Guid.NewGuid())
{
    // If needed
    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36",
    ThrowExceptions = false
};

measurementClient.ExceptionAsync("Exception message", "Application name", "1.0.0.0", true);

measurementClient.Event("app", "launch", Application.ProductName, "Main", "1.0.0.0");

measurementClient.EventAsync(new AppEventData
{
    ApplicationName = Application.ProductName,
    ApplicationVersion = Application.ProductVersion,
    EventAction = "action",
    EventCategory = "category",
    ScreenName = "Main"
});
```
