This is a space shooter game. You play as the turquoise spaceship (just a triangle), and your enemies are the red spaceships (also just triangles) that pursue you.

A wave of 6 enemies spawn every 7 seconds. The player can shoot projectiles that kill a single enemy instantly on hit. The player can't be damaged or killed by the enemies in any way, so you're free to let more and more enemies spawn to see what happens. 

If a spaceship reaches the edge of the screen its position will loop around the screen to the other side, like in Asteroids. If a projectile reaches the edge of the screen it is destroyed. 


Move forward with W, turn with AD, reverse slowly with S, Shoot with Spacebar or Left-Click.


This game runs at around 200 fps with 1000 enemies in the Unity editor on my home computer. My home computer has a six-year old Intel Core i5-8400 and DDR4 RAM. 1000 is the cap I set on how many enemies are allowed to exist, since 1000 is ridiculously big and still runs without any frame drops. I also tested the same setup with 10000 enemies in the editor, I got around 45-65 fps.


I used Unity's DOTS to program this game in the Data Oriented Programming paradigm for efficient memory usage and minimal heap allocations.

Almost all systems are Burst-compiled ISystem structs since that's more performant. Only PlayerInputSystem and ScreenBoundsSystem are not. Both of them have an empty OnUpdate() method, so they don't impact performance. ScreenBoundsSystem has to be a SystemBase class because it's purpose is to get the main camera and put its bounds in an IComponentData struct, so the bounds can be accessed in the unmanaged world, which needs the bounds for the looping around the screen and projectiles being destroyed when off screen.

The most performance intensive systems are collision and the ones that update a transform every frame, for the projectile, player, and enemies. The projectile moves at a constant velocity, while the player and enemies have a physics velocity which changes depending on player input/enemy trying to reach the player's position/linear drag. I did not use Unity's existing Physics System package for this, what I wanted to achieve was so simple I coded it myself. Collision between the enemies and projectiles is the single most performance intensive system. I implemented circular hitboxes (hitcircles) by comparing the square distance between the positions with the square of the sum of hitcircle radiuses. I compare the squares to avoid an expensive square root calculation, letting me use only simple arithmetic for my collision calculations.