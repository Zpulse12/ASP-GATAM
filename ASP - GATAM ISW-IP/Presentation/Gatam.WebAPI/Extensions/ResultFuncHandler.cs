using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Gatam.WebAPI.Extensions
{

    public delegate IActionResult onSuccesDelegate<T>(T result);
    public delegate IActionResult onFailureDelegate<T>(T result);
    public static class ResultFuncHandler
    {
        public static IActionResult Handle<T>(T result, onSuccesDelegate<T> onSucces, onFailureDelegate<T> onFailure) {
            Debug.WriteLine(result);
            return result == null ? onSucces(result) : onFailure(result);
        }
    }
}
