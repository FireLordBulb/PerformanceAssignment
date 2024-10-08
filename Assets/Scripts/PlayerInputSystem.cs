using Unity.Entities;
using UnityEngine.InputSystem;

public partial class PlayerInputSystem : SystemBase {
	private Input.PlayerActions playerActions;
	protected override void OnCreate(){
		RequireForUpdate<PlayerInput>();
		RequireForUpdate<PlayerMovement>();
		playerActions = new Input().Player;
	}
	protected override void OnStartRunning(){
		playerActions.Move.started += OnMoveStarted;
		playerActions.Move.canceled += OnMoveCanceled;
		playerActions.Turn.started += OnTurnStarted;
		playerActions.Turn.canceled += OnTurnCanceled;
		playerActions.Shoot.performed += OnShootPerformed;
		playerActions.Enable();
	}
	private void OnMoveStarted(InputAction.CallbackContext context){
		SystemAPI.SetSingleton(new PlayerMoveInput{Value = context.ReadValue<float>()});
	}
	private void OnMoveCanceled(InputAction.CallbackContext context){
		SystemAPI.SetSingleton(new PlayerMoveInput{Value = 0});
	}
	private void OnTurnStarted(InputAction.CallbackContext context){
		SystemAPI.SetSingleton(new PlayerTurnInput{Value = context.ReadValue<float>()});
	}
	private void OnTurnCanceled(InputAction.CallbackContext context){
		SystemAPI.SetSingleton(new PlayerTurnInput{Value = 0});
	}
	private void OnShootPerformed(InputAction.CallbackContext _){
		SystemAPI.SetSingleton(new PlayerShootInput{Value = true});
	}
	protected override void OnUpdate(){}
	
	protected override void OnStopRunning(){
		playerActions.Move.started -= OnMoveStarted;
		playerActions.Move.canceled -= OnMoveCanceled;
		playerActions.Turn.started -= OnTurnStarted;
		playerActions.Turn.canceled -= OnTurnCanceled;
		playerActions.Shoot.performed -= OnShootPerformed;
		playerActions.Disable();
	}
}
