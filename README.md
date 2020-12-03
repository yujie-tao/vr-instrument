# vr-instrument

## Initialization
### Unity
* Unity version: 2019.4.1f1, installed with Android Build Support 
* The scene is located in `VR` folder

### Test OSC

### Build for Oculus
* Go to `File --> Build Setting --> Andrioid --> Switch Platform`. 
* In Build Setting window, click `Player Setting --> XR Settings`, make sure `Virtual Reality Supported` is checked. And then add `Oculus` as a Virtual Reality SDKs. 
* Go to `Other Settings` and remove Vulkan froom the graphic API list.
* Now you should be ready to build for the Oculus. Refer to [this tutorial](https://www.youtube.com/watch?v=eySe4Wj6xbk&t=135s) for further instructions. 
