using System.Text;
using ApiWeb.Context;
using ApiWeb.Endpoints;
using ApiWeb.Models;
using ApiWeb.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using  ApiWeb.AppServiceExtensions;

var builder = WebApplication.CreateBuilder(args);
//Add services to the container.

builder.AddConnection();
builder.AddAutorizationEx();

var app = builder.Build();
//Configure The HTTP request pipeline.

app.MapAuthenticationEndpoints();
app.MapClassificationEndpoints();
app.MapGameEndpoints();

app.UseAuthentication();
app.UseAuthorization();

app.Run();


