namespace CleanMinimalApi.Presentation.Filters;

using System.Reflection;
using FluentValidation;

public static class ValidationFilter
{
    public static EndpointFilterDelegate ValidationFilterFactory(EndpointFilterFactoryContext context, EndpointFilterDelegate next)
    {
        var validationDescriptors = GetValidators(context.MethodInfo, context.ApplicationServices);

        if (validationDescriptors.Any())
        {
            return invocationContext => Validate(validationDescriptors, invocationContext, next);
        }

        // pass-thru
        return invocationContext => next(invocationContext);
    }

    private static async ValueTask<object> Validate(IEnumerable<ValidationDescriptor> validationDescriptors, EndpointFilterInvocationContext invocationContext, EndpointFilterDelegate next)
    {
        foreach (var descriptor in validationDescriptors)
        {
            var argument = invocationContext.Arguments[descriptor.ArgumentIndex];

            if (argument is not null)
            {
                var validationResult = await descriptor.Validator.ValidateAsync(
                    new ValidationContext<object>(argument)
                );

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }
            }
        }

        return await next.Invoke(invocationContext);
    }

    private static IEnumerable<ValidationDescriptor> GetValidators(MethodInfo methodInfo, IServiceProvider serviceProvider)
    {
        var parameters = methodInfo.GetParameters();

        for (var i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];

            if (parameter.GetCustomAttribute<ValidateAttribute>() is not null)
            {
                var validatorType = typeof(IValidator<>).MakeGenericType(parameter.ParameterType);

                // Note that FluentValidation validators needs to be registered as singleton
                var validator = serviceProvider.GetService(validatorType) as IValidator;

                if (validator is not null)
                {
                    yield return new ValidationDescriptor { ArgumentIndex = i, ArgumentType = parameter.ParameterType, Validator = validator };
                }
            }
        }
    }
}
