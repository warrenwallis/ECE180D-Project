import datetime
import time

while True:
    f = open("demofile3.txt", "w")
    now = str(datetime.datetime.now())
    print(now)
    f.write(now)
    f.close()
    time.sleep(5)

