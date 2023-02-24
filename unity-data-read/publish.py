import datetime
import time

while True:
    f = open("demofile3.txt", "a")
    now = str(datetime.datetime.now()) + '\n'
    print(now)
    f.write(now)
    f.close()
    time.sleep(5)

