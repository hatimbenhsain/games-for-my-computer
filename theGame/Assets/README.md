Practices:
1. Generally use Pascal Cases in filenames (LikeThis)
2. Use underscore sparingly (Only for different components of the same thing)
    For example: Water_Albedo, Water_Normal
3. Use numbers spairingly (Only for different instances for the same thing)
    For example: RippleMat1, RippleMat2
4. Don't make GameManager object in any scene!
5. Don't switch file names around for 2 different files
6. Always properly name files and game objects

-------------------------------------------------------------

File Structure:
Assets
+ --- Characters (character assets)
| + --- Main Character
| + --- Materials
| + --- NPCs
| + --- Sprites
+ --- DancingGame (the dancing game)
| + --- Game sub folders
+ --- Environment (environment assets)
| + --- Materials
| + --- Models
| + --- Overworld (The TestStreet scene assets)
| + --- Textures
+ --- Level (level related)
| + --- Pregfabs
| + --- Renderer
| + --- Scenes
| + --- TransitionAnimations 
+ --- Packages (external asset packs)
| + --- packages
+ --- Scripts (scripts and shaders)
| + --- Character Controller
| + --- Dialogue
| + --- Level
| + --- Shaders
| + --- UI
+ --- Sound (sounds)
| + --- Music
| + --- SFX
+ --- Surgery Game (the surgery game)
| + --- Game sub folders
+ --- UI (UI assets)
| + --- Dialogue Sprites
| + --- Fonts
| + --- Levels
| + --- MainMenu