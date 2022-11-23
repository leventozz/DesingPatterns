using Microsoft.AspNetCore.Mvc;

namespace WebApp.Command.Command
{
    public class FileCreateInvoker
    {
        private ITableActionCommand _command;
        private List<ITableActionCommand> _commandList = new List<ITableActionCommand>();
        public void SetCommand(ITableActionCommand tableActionCommand)
        {
            _command = tableActionCommand;
        }
        public void AddCommand(ITableActionCommand tableActionCommand)
        {
            _commandList.Add(tableActionCommand);
        }
        public IActionResult CreateFile()
        {
            return _command.Execute();
        }

        public List<IActionResult> CreateFiles()
        {
            return _commandList.Select(x => x.Execute()).ToList();
        }
    }
}
