using System;

namespace FlightBooking.Gateway.Auth;

public class JwtConfiguration
{
    private readonly string _issuer = null!;
    public string Issuer
    {
        get => _issuer;
        init
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(Issuer));
            _issuer = value;
        }
    }
}