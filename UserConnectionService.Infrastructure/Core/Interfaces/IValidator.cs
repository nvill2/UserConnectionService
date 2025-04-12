namespace UserConnectionService.Infrastructure.Core.Interfaces;

public interface IValidator<T>
{
    bool Validate(T value);

}
