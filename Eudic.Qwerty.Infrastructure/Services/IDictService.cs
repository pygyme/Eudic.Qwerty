namespace Eudic.Qwerty.Infrastructure.Services;

using Eudic.Qwerty.Core.Entities;

public interface IDictService
{
    Dict? GetDict(string word);
}