using FluentValidation;
using FullStack.Produtos.Domain;

namespace FullStack.Produtos.Application;

public static class ValidatorExtensions
{
    public static async Task<IEnumerable<ErrorResponse>> ObterErrosValidacoesAsync<T>(this IValidator<T> validator,
        T instance, CancellationToken cancellationToken)
    {
        var errors = new List<ErrorResponse>();
        
        var validationResult = await validator.ValidateAsync(instance, cancellationToken);
        if (validationResult.IsValid)
            return errors;
        
        var validationFailures = validationResult.Errors.Where(c => c != null).ToList();
        if (validationFailures.Count == 0)
            return errors;

        var validationErrors = validationFailures
            .Select(c => new { c.PropertyName, c.ErrorMessage })
            .Distinct()
            .ToList();
        
        errors.AddRange(validationErrors.Select(validationError =>
            new ErrorResponse($"Validacao.{validationError.PropertyName}", validationError.ErrorMessage)));

        return errors;
    }    
}