# Art Animation

[한국어](README.ko.md)

An Android AR app for tracing a picture on paper. It overlays step-by-step guide drawings on the paper in the camera view, and once the drawing is finished it captures it and turns it into an animation that sways in the wind. Vuforia recognizes an image target printed on the paper, and the captured drawing is applied to sprite meshes rigged with Anima2D bones and animated. It was made as a school team assignment.

<img src="docs/screenshots/screenshot-1.jpg" width="480" alt="Guide drawing overlaid on the paper">


## How to use

Open `source/` in Unity 2018.1.9f2 and build `Assets/01. Scenes/SampleScene.unity` for Android. Get the following ready before you open it.

1. When you install Unity, select the Android Build Support and Vuforia Augmented Reality Support components as well. After opening the project, add an AR Camera once from the menu GameObject > Vuforia > AR Camera. Unity then imports the Vuforia assets into `Assets/Vuforia/`. Delete the new AR Camera at that point. The original project used Vuforia 7.2.24.
2. Enter your own key from developer.vuforia.com in the App License Key field of `Assets/Resources/VuforiaConfiguration.asset`.
3. Download the assets below and put them at the paths in the table. The sprite meshes, bones, and IK of the pictures are all Anima2D components, so the animation in the scene only works once Anima2D is imported. It was a free package on the Unity Asset Store and is no longer sold, so you can download it from My Assets on an account that got it earlier. The scripts only compile with the recording plugin in place, because `MenuCtrl` and `UIController` call the `AndroidUtils` class. Assets made by other people are left out when it is not settled whether they can be redistributed in a public repo.

| Asset | Publisher | Path | Used in the original |
|---|---|---|---|
| Anima2D (original v1.1.5) | Mandarina Games, distributed by Unity Technologies | `Assets/Anima2D/`, `Assets/Editor Default Resources/Anima2D/`, `Assets/Gizmos/` | Sprite meshes, bones, and IK of `ArtBone` and `ArtBone2` |
| Android screen recording plugin `unityrecorder-release.aar`, `AndroidUtils.cs`, `AndroidManifest.xml` | To be confirmed (package `com.setik.kampertee`) | `Assets/Plugins/Android/`, `Assets/02. Scripts/AndroidUtils.cs` | Screen recording, opening the gallery, permission requests |
| Camera Filter Pack (original v4.0.0) | VETASOFT | `Assets/Camera Filter Pack/` | Camera filter scripts that came with the project |
| Low Poly UI Kit (original v1.1c) | To be confirmed | `Assets/04. Images/Low Poly UI Kit - v.1.1c/` | Menu button and panel graphics in the `SampleScene` and `CreateLight` scenes |
| Butterfly Eastern Tailed Blue | Unluck Software | `Assets/07. Models/` | Butterfly model |
| Vuforia emulator target and printable target PDF | PTC (Vuforia) | `Assets/Editor/Vuforia/Emulator/`, `Assets/Editor/Vuforia/ForPrint/` | Emulator target `VuforiaEmulator` for play mode in the editor and the printable `Emulator Ground Plane.pdf` |

In the app, point the camera at paper with the image target (`Assets/Editor/Vuforia/ImageTargetTextures/ARdraw/MJC_scaled.jpg`) printed on it. A guide drawing appears to the right of the target, and you trace over it there.

| Control | Action |
|---|---|
| Dropdown at the top right | Choose the picture to trace. `Picture0` is a sunset with a palm tree (7 steps) and `Picture1` is The Starry Night (13 steps) |
| Previous and next in the Information panel | Step through the guide drawings |
| Slider | Guide drawing opacity |
| MENU > Light | Press Create Light, then touch the picture to place a point light. Pressing it again returns to selection mode, where you drag a point light to move it and remove it with Delete Light |
| MENU > Capture | A 567x627 rectangle appears in the middle of the screen. Capture takes a picture of what is inside it, and Animation makes the captured drawing move |
| MENU > Record | Records the screen for up to 10 seconds and saves it to `DCIM/ARSketch` |
| MENU > Share | Opens the gallery |
| MENU > Quit | Quit |


## How it works

**Only the captured drawing's colors are copied onto premade rigged meshes.** Each picture has `ArtBone`, a 567x627 texture cut to the shape of the part that moves (the palm tree or the cypress), and `ArtTerrain` for the background. `ArtBone` is a sprite mesh rigged with bones and IK in Anima2D. When you press the Animation button, `TextureLoader.Copy` writes only the RGB values of the captured drawing over both textures. The alpha is left as it was, so the moving part keeps its shape and takes on the colors the user painted.

```csharp
destColors[destIndex].r = sourceColors[sourceIndex].r;
destColors[destIndex].g = sourceColors[sourceIndex].g;
destColors[destIndex].b = sourceColors[sourceIndex].b;
```

**The moving part is erased from the background.** If it stayed, the same image would remain in the background and look doubled while the tree sways. `TextureLoader.ClearOverlap` paints every pixel where the alpha of `ArtBone` is not 0 with the color of the rightmost pixel in that row, so the gap is filled with the surrounding color.

```csharp
Color32 ClearColor = targetColors[GetIndex(target, width - 1, j)];
...
if (maskColors[maskIndex].a != 0)
    targetColors[targetIndex] = ClearColor;
```

**The sway comes from changes in a sine curve.** `WindCtrl` rotates the 8 leaf bones of the palm tree, and `WindCtrl2` moves and rotates the 5 IK targets of the cypress. Each frame adds `Mathf.Sin(time + Time.deltaTime) - Mathf.Sin(time)` multiplied by a speed. Since the change is added instead of the sine value itself, the parts sway around the pose they started in.

`MenuCtrl` handles recording and opening the gallery by calling the Android screen recording plugin from the table above.


## Files

| Path | Content |
|---|---|
| `source/Assets/01. Scenes/SampleScene.unity` | App scene |
| `source/Assets/02. Scripts/` | Menu, dropdown, point light, and capture scripts |
| `source/Assets/ArtAnimation/`, `source/Assets/ArtAnimation2/` | Sprite mesh data, textures, and sway scripts for each picture |
| `source/Assets/UnityUIExtensions/` | Unity UI Extensions and its license `LICENSE.md` |
| `source/Assets/04. Images/` | Step-by-step guide drawings |
| `source/Assets/StreamingAssets/Vuforia/` | Image target database `ARdraw` |
| `source/Assets/05. Assets/JW/`, `source/Assets/99. Test/` | Point light experiment scene and test scene |
| `source/ProjectSettings/` | Unity 2018.1.9f2 project settings |
| `docs/screenshots/` | Screenshot |


## Credits

- Unity UI Extensions: BSD-3-Clause licensed UI extensions by the Unity UI Extensions community. The project uses its `UILineRenderer` to draw the capture rectangle. `Assets/UnityUIExtensions/`


## License

zlib. See [LICENSE](LICENSE). Bundled libraries made by other people keep their own licenses.
