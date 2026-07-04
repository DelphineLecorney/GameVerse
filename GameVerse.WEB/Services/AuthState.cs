namespace GameVerse.WEB.Services
{
    public class AuthState
    {
        public string? Token { get; private set; }
        public string? Username { get; private set; }
        public bool IsAuthenticated => !string.IsNullOrEmpty(Token);

        public event Action? OnChange;

        public void SetAuth(string token, string username)
        {
            Token = token;
            Username = username;
            NotifyStateChanged();
        }

        public void Logout()
        {
            Token = null;
            Username = null;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }

}
