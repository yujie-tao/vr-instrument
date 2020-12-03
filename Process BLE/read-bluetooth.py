import numpy as np
import serial  # for communication to arduino
import time
import csv

print("connecting to Arduino...")

# set up the serial line
ser = serial.Serial('COM6', 115200)
time.sleep(5) # there needs to be this delay here to let Arduino boot
ser.flushInput()

print("connected!")

# length of time recording
seconds = 10

# Read and record the data
data = []  # empty list to store the data
timer = []

start_time = time.time()
print("start data flow")

while True:
    current_time = time.time()
    elapsed_time = current_time - start_time

    if elapsed_time > seconds:
        break

    timer.append(elapsed_time)

    bluetooth_signal = ser.readline()  # read a byte string
    #print(bluetooth_signal)
    
    if bluetooth_signal == b'1\r\n':
        ####### send OSC sound here ########
        print("sound") # I print here to the terminal for debugging
    elif bluetooth_signal == b'0\r\n':
        ####### end OSC sound here #########
        print("end sound") # I print here to the terminal for debugging
    
    # record the signal, decode outside the loop to save time
    data.append(bluetooth_signal)

print("end data flow")

# Make sure to close the serial port
ser.close()

# convert the format of the data to strings instead of bytes to make it more readable
processed_data = []
for i in range(len(data)):
    b = data[i]
    string_n = b.decode()  # decode byte string into Unicode
    string = string_n.rstrip()  # remove \n and \r
    processed_data.append(string)

# save to CSV if doing a full-test
with open('exhale-1.csv', 'w', newline='') as f:
    writer = csv.writer(f)
    writer.writerows(zip(timer, processed_data))

