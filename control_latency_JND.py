# Study 1 JND: varying the amplitude of the pulse and asking if the participant can feel it

import serial # for communication to arduino
import time # timing of delays
import struct # for converting integers to bytes
import random # for picking random variables
import csv # save our results
import sys

# set up the serial line
ser = serial.Serial('COM7', 115200)
# print(ser) # show serial info
time.sleep(5) # let arduino reset

# auxiliary functions
def check_response(response):
    if response == "Y" or  response == "y":
        return (True, "Y")
    elif response == "N" or  response == "n":
        return (True, "N")
    else:
        print("Sorry but did not understand your response. You said " + response + " . Please use either \"y\" (for yes) or \"n\" (for now)")
        return (False, response)

global_start_amp = 40 # this can easily be felt
trials = 60
amp_tracker = [] # keeps track of the amplitude progressions
trial_tracker = [] # counts valid trials
counter = 0

filename = "2020_03_04_JND-amp_50ms_alex.csv"

# starts program and displays main instruction
response = input("q[enter] to quit program, anything-else[enter] to start")
if response == "q":
    sys.exit(0)

print("study begin...")
time.sleep(1)
amp = global_start_amp
for i in range(1, trials+1):
    print("Prepare to feel:")
    time.sleep(random.randint(1,2)) # random, comfortable delay
    # play the signal
    ser.write(struct.pack('>h', amp)) # must convert to send over serial
    time.sleep(1)
    response_valid = False
    counter += 1
    while not response_valid:
            response = input("Did you feel? Y/N[enter]")
            response_valid, response = check_response(response)
            if response == "Y":
                amp-= 3
            else: 
                amp+= 2
            if response_valid:
                #responses_for_this_frequency.append((response,amp))
                amp_tracker.append(amp)
                trial_tracker.append(counter)
    
ser.close() # close connection to arduino

print(trial_tracker)
print(amp_tracker)

# store the data
rows = zip(trial_tracker, amp_tracker)
with open(filename, "w", newline='') as f:
    writer = csv.writer(f)
    for row in rows:
        writer.writerow(row)
print("Data has been written to " + filename)