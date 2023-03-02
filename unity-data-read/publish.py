import datetime
import time
import random

def fileToArray(f):
    content=[]
    for line in f:
        content.append(line)
    print(content)
    return content

def arrayshift(a,newline):
    length=len(a)
    for i in range(0,length-1):
        a[i]=a[i+1]
    a[length-1]=newline
    return a

def arraytostring(a):
    string=''
    for line in a:
        string = string + line
  
    return string

temp= ''
retryflag=False


open("demofile3.txt", "w").close()


while True:
    #add some type of exception handling when permission is denied, it has to remember previous data
    randomint=random.randint(1,10)
    now = '[' + str(datetime.datetime.now())+']'+str(randomint)+'\n'
    #print(now)

    while retryflag:
        try:
            f = open("demofile3.txt", "r+")
        except:
            print("Couldn't Access File")
            continue
        lines = fileToArray(f)
        if len(lines)<10:
            newtext=arraytostring(lines)+temp
            f.seek(0)
            f.write(newtext)
        else:
            newlines=arrayshift(lines,temp)
            newtext=arraytostring(newlines)
            f.seek(0)
            f.truncate(0)
            f.write(newtext)
        retryflag=False
        temp=''
        
    try:
        f = open("demofile3.txt", "r+")
    except:
        print("Couldn't Access File")
        temp=now
        retryflag=True
        continue
    lines = fileToArray(f)
    if len(lines)<10:
        newtext=arraytostring(lines)+now
        f.seek(0)
        f.write(newtext)
    else:
        newlines=arrayshift(lines,now)
        newtext=arraytostring(newlines)
        f.seek(0)
        f.truncate(0)
        f.write(newtext)
    f.close()
    randomtime=random.randint(1,4)
    time.sleep(randomtime)
    


