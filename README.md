# Art Animation

An Android AR app that overlays step by step guide drawings onto the paper seen through the camera so you can trace a picture, then captures the finished drawing and turns it into an animation swaying in the wind. Vuforia recognizes an image target printed on the paper, and the captured drawing is applied to sprite meshes rigged with bones in Anima2D. It was made as a school team assignment.

<img src="docs/screenshots/screenshot-1.jpg" width="480" alt="Guide drawing overlaid on the paper">


## How to use

Open `source/` in Unity 2018.1.9f2 and build `Assets/01. Scenes/SampleScene.unity` for Android. Prepare the following before opening it.

1. When installing Unity, select the Android Build Support and Vuforia Augmented Reality Support components. After opening the project, add GameObject > Vuforia > AR Camera once from the menu and Unity imports the Vuforia assets into `Assets/Vuforia/`. Delete the AR Camera that was added. The original used Vuforia 7.2.24.
2. Put your own key issued at developer.vuforia.com into App License Key in `Assets/Resources/VuforiaConfiguration.asset`.
3. Get the assets below and place them at the listed paths. The sprite meshes, bones, and IK of the pictures are all Anima2D components, so Anima2D must be imported for the animation in the scenes to work. It was distributed for free on the Unity Asset Store and is no longer sold, so it can be downloaded from My Assets on an account that got it before. `MenuCtrl` and `UIController` call the `AndroidUtils` class, so the recording plugin is needed for the scripts to compile. Assets made by other people whose redistribution in a public repo is not clearly allowed are not included.

| Asset | Publisher | Path | Used in the original |
|---|---|---|---|
| Anima2D (original v1.1.5) | Mandarina Games, distributed by Unity Technologies | `Assets/Anima2D/`, `Assets/Editor Default Resources/Anima2D/`, `Assets/Gizmos/` | Sprite meshes, bones, and IK of `ArtBone` and `ArtBone2` |
| Android screen recording plugin `unityrecorder-release.aar`, `AndroidUtils.cs`, `AndroidManifest.xml` | To be confirmed (package `com.setik.kampertee`) | `Assets/Plugins/Android/`, `Assets/02. Scripts/AndroidUtils.cs` | Screen recording, opening the gallery, permission requests |
| Camera Filter Pack (original v4.0.0) | VETASOFT | `Assets/Camera Filter Pack/` | Camera filter scripts bundled with the project |
| Low Poly UI Kit (original v1.1c) | To be confirmed | `Assets/04. Images/Low Poly UI Kit - v.1.1c/` | Menu button and panel graphics in the `SampleScene` and `CreateLight` scenes |
| Butterfly Eastern Tailed Blue | Unluck Software | `Assets/07. Models/` | Butterfly model |
| Vuforia emulator target and printable target PDF | PTC (Vuforia) | `Assets/Editor/Vuforia/Emulator/`, `Assets/Editor/Vuforia/ForPrint/` | Emulator target `VuforiaEmulator` for play mode in the editor and the printable `Emulator Ground Plane.pdf` |

In the app, point the camera at paper with the image target (`Assets/Editor/Vuforia/ImageTargetTextures/ARdraw/MJC_scaled.jpg`) printed on it. A guide drawing appears to the right of the target, and you draw over it in that spot.

| Control | Action |
|---|---|
| Dropdown at the top right | Choose the picture to trace. `Picture0` is a sunset with a palm tree (7 steps) and `Picture1` is The Starry Night (13 steps) |
| Previous and next in the Information panel | Step through the guide drawings |
| Slider | Guide drawing opacity |
| MENU > Light | Press Create Light and touch the picture to place a point light. Press it again to return to selection mode, where you drag a point light to move it and remove it with Delete Light |
| MENU > Capture | A 567x627 rectangle appears in the middle of the screen. Capture takes the area inside it and Animation sets the captured drawing in motion |
| MENU > Record | Records the screen for up to 10 seconds and saves it to `DCIM/ARSketch` |
| MENU > Share | Opens the gallery |
| MENU > Quit | Quit |


## How it works

**Only the colors of the captured drawing are moved onto premade rigged meshes.** Each picture has a 567x627 texture `ArtBone` cut to the shape of the part that moves (the palm tree, the cypress) and a background texture `ArtTerrain`, and `ArtBone` is a sprite mesh rigged with bones and IK in Anima2D. When the Animation button is pressed, `TextureLoader.Copy` overwrites only the RGB of the captured drawing into both textures and leaves the alpha untouched. So the shape of the moving part stays the same while it takes on the colors the user painted.

```csharp
destColors[destIndex].r = sourceColors[sourceIndex].r;
destColors[destIndex].g = sourceColors[sourceIndex].g;
destColors[destIndex].b = sourceColors[sourceIndex].b;
```

**The moving part is erased from the background.** Otherwise the same picture would stay on the background and show doubled while the tree sways. `TextureLoader.ClearOverlap` paints every pixel where the alpha of `ArtBone` is not 0 with the color of the rightmost pixel in that row, filling the gap with the surrounding color.

```csharp
Color32 ClearColor = targetColors[GetIndex(target, width - 1, j)];
...
if (maskColors[maskIndex].a != 0)
    targetColors[targetIndex] = ClearColor;
```

**The sway is made from the change in a sine curve.** `WindCtrl` rotates the 8 leaf bones of the palm tree, and `WindCtrl2` moves and rotates the 5 IK targets of the cypress. The amount added each frame is `Mathf.Sin(time + Time.deltaTime) - Mathf.Sin(time)` multiplied by a speed. Because the change is added rather than the sine value itself, the parts sway around the pose they were placed in.

Recording and opening the gallery are handled by `MenuCtrl`, which calls the Android screen recording plugin listed in the table above.


## Files

| Path | Content |
|---|---|
| `source/Assets/01. Scenes/SampleScene.unity` | App scene |
| `source/Assets/02. Scripts/` | Menu, dropdown, point light, and capture scripts |
| `source/Assets/ArtAnimation/`, `source/Assets/ArtAnimation2/` | Sprite mesh data, textures, and sway scripts for each picture |
| `source/Assets/UnityUIExtensions/` | Unity UI Extensions and its license `LICENSE.md` |
| `source/Assets/04. Images/` | Step by step guide drawings |
| `source/Assets/StreamingAssets/Vuforia/` | Image target database `ARdraw` |
| `source/Assets/05. Assets/JW/`, `source/Assets/99. Test/` | Point light experiment scene and test scene |
| `source/ProjectSettings/` | Unity 2018.1.9f2 project settings |
| `docs/screenshots/` | Screenshot |


## Credits

- Unity UI Extensions: BSD-3-Clause licensed UI extensions by the Unity UI Extensions community. `UILineRenderer` draws the capture rectangle. `Assets/UnityUIExtensions/`


## License

zlib. See [LICENSE](LICENSE). Bundled libraries made by other people keep their own licenses.
