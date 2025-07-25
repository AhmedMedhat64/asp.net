﻿using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MainProject.Middlewares;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;

    private static int _counter = 0;
    private static DateTime _lastRequestDate = DateTime.Now;

    public RateLimitingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        _counter++;
        if (DateTime.Now.Subtract(_lastRequestDate).Seconds > 10)
        {
            _counter = 1;
            _lastRequestDate = DateTime.Now;
            await _next(context);
        }
        else
        {
            if (_counter > 5)
            {
                _lastRequestDate = DateTime.Now;
                await context.Response.WriteAsync("Rate limit exceeded");
            }
            else
            {
                // _lastRequestDate = DateTime.Now; no need for this 
                await _next(context); // No need to update _lastRequestDate here
            }
        }

    }
}
