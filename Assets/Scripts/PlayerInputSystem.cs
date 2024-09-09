using Unity.Entities;
using UnityEngine.InputSystem;

public partial class PlayerInputSystem : SystemBase {
	private Input.PlayerActions playerActions;
	private Entity player;
	protected override void OnCreate(){
		RequireForUpdate<PlayerInput>();
		RequireForUpdate<Player>();
		playerActions = new Input().Player;
	}
	protected override void OnStartRunning(){
		playerActions.Move.performed += OnMovePerformed;
		playerActions.Turn.performed += OnTurnPerformed;
		playerActions.Shoot.performed += OnShootPerformed;
		playerActions.Enable();
		player = SystemAPI.GetSingletonEntity<Player>();
	}
	private void OnMovePerformed(InputAction.CallbackContext context){
		if (context.started){
			SystemAPI.SetSingleton(new PlayerMoveInput{Value = context.ReadValue<float>()});
		} else if (context.canceled){
			SystemAPI.SetSingleton(new PlayerMoveInput{Value = 0});
		}
	}
	private void OnTurnPerformed(InputAction.CallbackContext context){
		if (context.started){
			SystemAPI.SetSingleton(new PlayerTurnInput{Value = context.ReadValue<float>()});
		} else if (context.canceled){
			SystemAPI.SetSingleton(new PlayerTurnInput{Value = 0});
		}
	}
	private void OnShootPerformed(InputAction.CallbackContext _){
		SystemAPI.SetComponentEnabled<PlayerShootInput>(player, true);
	}
	protected override void OnUpdate(){}
	
	protected override void OnStopRunning(){
		playerActions.Move.performed -= OnMovePerformed;
		playerActions.Turn.performed -= OnTurnPerformed;
		playerActions.Shoot.performed -= OnShootPerformed;
		playerActions.Disable();
		player = Entity.Null;
	}
}
