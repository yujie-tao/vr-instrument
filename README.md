# vr-instrument
## Test OSC without Unity
### Setup OSC server
```
cd OSC
python osc_server_test.py
```

### Test OSC client
In `osc_client_test.py`, update the IP as `127.0.0.1`

```
python osc_client_test.py
```
The server window should receive any message sent from this test client side. 

If it work, goes back to root folder, and update the IP in `main.py`
```
python main.py
```
This allows you to test with OSC and BLE, with the end device as your laptop. 

## Test OSC with Unity

### Unity Setup
* Unity version: 2019.4.1f1, installed with Android Build Support 
* The scene is located in `VR` folder


## Test OSC with Oculus
### Oculus Setup
* Go to `File --> Build Setting --> Andrioid --> Switch Platform`. 
* In Build Setting window, click `Player Setting --> XR Settings`, make sure `Virtual Reality Supported` is checked. And then add `Oculus` as a Virtual Reality SDKs. 
* Go to `Other Settings` and remove Vulkan froom the graphic API list.
* Now you should be ready to build for the Oculus. Refer to [this tutorial](https://www.youtube.com/watch?v=eySe4Wj6xbk&t=135s) for further instructions. 

### Update OSC server IP
