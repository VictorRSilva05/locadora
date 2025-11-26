namespace Locadora.Dominio.Autenticacao;
public interface ITokenProvider
{
    Task<IAccessToken> GenerateAccessToken(User user);
}