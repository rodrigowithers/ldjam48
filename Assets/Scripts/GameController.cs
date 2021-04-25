using UI;
using Tile;
using System;
using Camera;
using Player;
using UnityEngine;

[DefaultExecutionOrder(Int32.MaxValue)]
public class GameController : MonoBehaviour
{
    public enum GameState
    {
        Shop,
        Game,
        Menu
    }
    
    public static GameController Instance { get; set; }
    public event Action OnShopState;

    public PlayerController Player;
    public CameraController CameraController;
    public CanvasController CanvasController;

    public void SetGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Shop:
                OnShopState?.Invoke();
                
                ResourceStorage.Save();
                ResourceStorage.Load();
                
                Player.transform.position = new Vector3(1.94f, 7, 0);
                Player.ResetPlayer();
                TilemapCreator.Instance.Create();
                
                Player.gameObject.SetActive(false);
                CameraController.Target = null;
                
                CanvasController.ShopPanel.SetActive(true);
                
                CanvasController.HUDPanel.SetActive(false);
                CanvasController.MenuPanel.SetActive(false);
                break;
            case GameState.Game:
                
                ResourceStorage.Load();

                Player.gameObject.SetActive(true);
                CameraController.Target = Player.transform;
                
                CanvasController.HUDPanel.SetActive(true);
                
                CanvasController.ShopPanel.SetActive(false);
                CanvasController.MenuPanel.SetActive(false);
                break;
            case GameState.Menu:
                Player.gameObject.SetActive(false);
                CameraController.Target = null;
                
                CanvasController.MenuPanel.SetActive(true);
                CanvasController.ShopPanel.SetActive(false);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }    
    }
    
    private void Awake()
    {
        Instance = this;

        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        ResourceStorage.Load();
        SetGameState(GameState.Shop);
    }
}
