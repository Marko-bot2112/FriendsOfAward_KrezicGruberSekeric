using Blazored.Toast;
using FriendsOfAward_KrezicGruberSekeric;
using FriendsOfAward_KrezicGruberSekeric.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<AuthenticationStateProvider, MyCustomAuthStateProvider>();
builder.Services.AddScoped<MyCustomAuthStateProvider>();

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

// Register a distributed cache (in-memory for single-server dev)
builder.Services.AddDistributedMemoryCache();

// Blazored Toast
builder.Services.AddBlazoredToast();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
