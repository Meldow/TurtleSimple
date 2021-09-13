namespace Turtle.InputDTO
{
    using System.Collections.Generic;
    using Turtle.Actions;

    public class ActionsDto
    {
        public IEnumerable<IAction> Actions { get; set; }

        public ActionsDto(IEnumerable<IAction> actions)
        {
            this.Actions = actions;
        }
    }
}