using System.Collections;

public interface ISpawnEvent
{
    string EventName { get; }
    float Duration { get; }
    IEnumerator Execute(WaveManager manager);
}
