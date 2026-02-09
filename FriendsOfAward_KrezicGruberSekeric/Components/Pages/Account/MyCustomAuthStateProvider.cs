using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace FriendsOfAward_KrezicGruberSekeric
{

	public class MyCustomAuthStateProvider : AuthenticationStateProvider
	{
        private readonly ProtectedSessionStorage _sessionStorage;
        private ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
        private ClaimsPrincipal? _currentUser = null;

        public MyCustomAuthStateProvider(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Try to load from session storage if not already loaded
            if (_currentUser == null)
            {
                try
                {
                    var storedUsername = await _sessionStorage.GetAsync<string>("username");
                    if (storedUsername.Success && !string.IsNullOrEmpty(storedUsername.Value))
                    {
                        ClaimsIdentity identity = new([new Claim(ClaimTypes.Name, storedUsername.Value)],
                            "MyCustomAuthType");
                        _currentUser = new ClaimsPrincipal(identity);
                    }
                }
                catch
                {
                    // Session storage not available yet (first render)
                }
            }

            return new AuthenticationState(_currentUser ?? _anonymous);
        }

        public async Task Login(string username)
        {
            // Store in session
            await _sessionStorage.SetAsync("username", username);

            ClaimsIdentity identity = new([new Claim(ClaimTypes.Name, username)],
                "MyCustomAuthType");

            _currentUser = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Logout()
        {
            // Clear session
            await _sessionStorage.DeleteAsync("username");

            _currentUser = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
