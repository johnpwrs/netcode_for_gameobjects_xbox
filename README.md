# Netcode for GameObjects Xbox Crash
Minimal project using Netcode for GameObjects that crashes as a UWP app on the Xbox One and Xbox One X. 

The Xbox will completely shut down.

This project just attempts to move a GameObject around on a Grid using ClientNetworkTransform.

## Steps to reproduce
- Drag Sample Scene into your Hierarchy and remove default scene.
- Update the NetworkManager prefab's Network Home component with the IP address of the host you want to connect to.  This must be a private IP on a device in your network.
- Build the project and deploy it to Xbox as UWP Game
- Start the host.
- Start the game on the xbox.  Press B on the controller to join as a client
- Watch as it crashes.  If no crash, move the joystick to move your player around on the Xbox and watch as it crashes.
