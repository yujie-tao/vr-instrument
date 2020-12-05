# Study 1 JND: varying the delaylitude of the pulse and asking if the participant can feel it

from pythonosc import udp_client
import serial # for communication to arduino
import time # timing of delays
import struct # for converting integers to bytes
import random # for picking random variables
import csv # save our results
import sys

print("connecting to arduino ...")

# set up the serial line
#ser_wired = serial.Serial('COM10', 115200)
ser_bluetooth = serial.Serial('COM6', 115200)
ser_bluetooth.flushInput()
# print(ser) # show serial info
time.sleep(5) # let arduino reset

print("connected to arduino!")

# OSC server address
# ================== #
# ip = "127.0.0.1"  # use "127.0.0.1" when testing on unity
ip = "192.168.1.9"  # or local Oculus IP when testing with Oculus, changes with networks,
# download sidequest https://sidequestvr.com/
# connect w/ cable, seem the top bar of sidequest to get the IP
port = 5006
# ================== #

client = udp_client.SimpleUDPClient(ip, port)
print("connected to OSC server at " + ip + ":" + str(port))


# auxiliary functions
def check_response(response):
    if response == "Y" or  response == "y":
        return (True, "Y")
    elif response == "N" or  response == "n":
        return (True, "N")
    else:
        print("Sorry but did not understand your response. You said " + response + " . Please use either \"y\" (for yes) or \"n\" (for now)")
        return (False, response)

global_start_delay = 1.25 # start with delay that can easily be felt
trials = 25
delay_tracker = [] # keeps track of the delaylitude progressions
trial_tracker = [] # counts valid trials
counter = 0

filename = "JND-delay_testing.csv"

# starts program and displays main instruction
response = input("q[enter] to quit program, anything-else[enter] to start")
if response == "q":
    sys.exit(0)

print("study begin...")
time.sleep(1)
delay = global_start_delay
for i in range(1, trials+1):
    # Let the current trial know how much delay to give
    #ser_wired.write(struct.pack('>h', delay)) # must convert to send over serial
    
    
    # flush out leftover 1's
    ser_bluetooth.flushInput()
    # smooth out the study flow
    time.sleep(1)
    
    # prompt user to blow
    print("Please blow:")
    
    while True: # wait forever until we get our blow
        bluetooth_signal = ser_bluetooth.readline()  # read a byte string
        if bluetooth_signal == b"1\r\n":
            ####### send OSC sound here ########
            msg = 1
            # inject our delay here
            time.sleep(delay)
            client.send_message("/sound", msg)
            # I print here to the terminal for debugging
            print("Sent: " + str(msg))
            break
    
    response_valid = False
    counter += 1
    while not response_valid:
            response = input("Did you feel latency? Y/N[enter]")
            response_valid, response = check_response(response)
            if response == "Y":
                delay-= 0.075
            else: 
                delay+= 0.025
            if response_valid:
                #responses_for_this_frequency.append((response,delay))
                delay_tracker.append(delay)
                trial_tracker.append(counter)
    
#ser_wired.close() # close connection to arduino
ser_bluetooth.close()

print(trial_tracker)
print(delay_tracker)

# store the data
rows = zip(trial_tracker, delay_tracker)
with open(filename, "w", newline='') as f:
    writer = csv.writer(f)
    for row in rows:
        writer.writerow(row)
print("Data has been written to " + filename)