namespace UserConnectionService.Infrastructure.Core.Interfaces;

public interface IBaseValidator<T>
{
    bool Validate(T? value);
}
