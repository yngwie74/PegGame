namespace Game
{
    using System.Collections.Generic;

    public interface IRoutable
    {
        IEnumerable<IRoute> Routes { get; }
    }

    public interface ICell
    {
        bool IsPin { get; }
        float Probability { get; }

        IRoutable NextStepFrom(Coord p);
    }

    public interface IRoute
    {
        Coord NextStep { get; }
        float Probability { get; }
    }
}