namespace Turtle.GameObjects.InputDTO
{
    using System.Collections.Generic;

    public class ActionsDTO
    {
        public IEnumerable<IAction> Actions { get; set; }

        public ActionsDTO(IEnumerable<IAction> actions)
        {
            this.Actions = actions;
        }
    }
}