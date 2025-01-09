using UnityEngine.Events;

public static class GameEvents
{
    public static UnityEvent<EntityUpdateType, bool> EntityUpdated = new();
    public static UnityEvent<GameSaveData> GameSavesLoaded = new();
    public static UnityEvent GameReady = new();
    public static UnityEvent GameUserConfirmPlay = new();
    public static UnityEvent GameExit = new();
}

public enum EntityUpdateType
{
    Mines = 1
}
