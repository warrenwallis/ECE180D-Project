import datetime
import time
import random

while True:
    f = open("demofile3.txt", "a")
    now = str(datetime.datetime.now())
    print(now)
    f.write(now+"\n")
    f.close()
    randomtime=random.randint(5,20)
    time.sleep(5)

