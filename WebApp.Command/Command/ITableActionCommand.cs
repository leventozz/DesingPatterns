using Microsoft.AspNetCore.Mvc;

namespace WebApp.Command.Command
{
    public interface ITableActionCommand
    {
        IActionResult Execute();
    }
}
