"""OSC client
"""
from pythonosc import udp_client

# OSC server address
# ================== #
ip = "127.0.0.1"
port = 5006
# ================== #

client = udp_client.SimpleUDPClient(ip, port)
print("connected to OSC server at " + ip + ":" + str(port))

while True:
    msg = int(input("> send OSC: "))
    if msg == 1:
        client.send_message("/sound", msg)
