namespace Beergam.Services.Auth;

public enum RevocationReason
{
    Login,
    Logout
}

public record RevocationInfo(
    string RevokedToken,
    RevocationReason reason,
    DateTime RevokedAt,
    string Ip);