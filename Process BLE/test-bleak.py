import asyncio
from bleak import discover
from bleak import BleakClient

'''
async def run():
    devices = await discover()
    for d in devices:
        print(d)

loop = asyncio.get_event_loop()
loop.run_until_complete(run())
'''
address = "DA:0B:A9:E1:D2:41"
MODEL_NBR_UUID = "Adafruit Bluefruit LE"

async def run(address):
    async with BleakClient(address) as client:
        model_number = await client.read_gatt_char(MODEL_NBR_UUID)
        print("Model Number: {0}".format("".join(map(chr, model_number))))

loop = asyncio.get_event_loop()
loop.run_until_complete(run(address))