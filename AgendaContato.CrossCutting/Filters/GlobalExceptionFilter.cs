using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AgendaContato.CrossCutting.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Erro não tratado");

        context.Result = new ViewResult
        {
            ViewName = "Error", // View personalizada
            ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
            {
                { "Mensagem", "Erro interno no sistema. Por favor, tente novamente mais tarde." }
            }
        };

        context.ExceptionHandled = true;
    }
}
