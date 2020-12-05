"""OSC client
"""
from pythonosc import udp_client

# OSC server address
# ================== #
#ip = "127.0.0.1"  # use "127.0.0.1" when testing on unity
ip = "192.168.1.9"  # or local Oculus IP when testing with Oculus
port = 5006
# ================== #

client = udp_client.SimpleUDPClient(ip, port)
print("connected to OSC server at " + ip + ":" + str(port))

while True:
    msg = int(input("> send OSC: "))
    if msg == 1:
        client.send_message("/sound", msg)
