using System;

namespace HackedDesign
{
    public interface ISensor
    {
        event Action OnTargetChanged;
    }
}