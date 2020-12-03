import argparse
import math

from pythonosc import dispatcher
from pythonosc import osc_server

IP = "0.0.0.0"  # local device's IP on LAN
PORT = 5006


def msg_handler(address, *args):
    # print(f"{address}: {args}")
    print("OSC receives: " + str(args[0]))


if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--ip", default=IP, help="The ip to listen on")
    parser.add_argument(
        "--port", type=int, default=PORT, help="The port to listen on"
    )
    args = parser.parse_args()

    dispatcher = dispatcher.Dispatcher()
    dispatcher.map("/sound", msg_handler)

    server = osc_server.ThreadingOSCUDPServer((args.ip, args.port), dispatcher)
    print("Serving on {}".format(server.server_address))
    server.serve_forever()
