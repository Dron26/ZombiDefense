using System.Collections.Generic;
using Infrastructure.Location;

public class LocationData
{
    private readonly List<int> _completedLocations = new();
    private int _selectedLocationId;
    private WorkPoint _selectedPoint;
    public int TimeTimeBeforeNextWave=> _timeBeforeNextWave;
    private int _timeBeforeNextWave=5;
    public int MaxEnemiesOnScene => _maxEnemiesOnScene;

    public int SelectedLocationId => _selectedLocationId;
    private int _maxEnemiesOnScene;

    public IReadOnlyList<int> CompletedLocations => _completedLocations.AsReadOnly();

    public void SetSelectedLocationId(int id) => _selectedLocationId = id;
    public void CompleteCurrentLocation() => _completedLocations.Add(_selectedLocationId);
    public void ClearCompletedLocations() => _completedLocations.Clear();
    public void ChangeSelectedPoint(WorkPoint point) => _selectedPoint = point;
    public void SetMaxEnemyOnScene(int count) => _maxEnemiesOnScene = count;
}