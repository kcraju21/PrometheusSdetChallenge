using System;
using System.Collections.Generic;

namespace ApiTests.Config;

public class ApiConfig
{
    public string BaseUrl { get; set; } = string.Empty;
    public Dictionary<string, string> DefaultHeaders { get; set; } = new();
    public int TimeoutSeconds { get; set; } = 30;
}
