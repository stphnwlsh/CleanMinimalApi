namespace CleanMinimalApi.Presentation.Filters;

using FluentValidation;

public class ValidationFilter<T> : IEndpointFilter
{
    private readonly IValidator<T> validator;

    public ValidationFilter(IValidator<T> validator)
    {
        this.validator = validator;
    }

    public async ValueTask<object> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        if (context.Arguments.FirstOrDefault(x => x?.GetType() == typeof(T)) is not T argument)
        {
            return Results.BadRequest("Unable to find parameters or body for validation");
        }

        var validationResult = await this.validator.ValidateAsync(argument!);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        return await next(context);
    }
}
