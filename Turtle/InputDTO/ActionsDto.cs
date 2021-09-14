namespace Turtle.InputDTO
{
    using System.Collections.Generic;
    using Turtle.Actions;

    public class ActionsDto
    {
        public ActionsDto(IEnumerable<IAction> actions)
        {
            this.Actions = actions;
        }

        public IEnumerable<IAction> Actions { get; set; }
    }
}