import cv2
import mediapipe as mp
import numpy as np
import csv
import datetime

file = open('output_results.txt','w')

mp_drawing = mp.solutions.drawing_utils
mp_drawing_styles = mp.solutions.drawing_styles
mp_holistic = mp.solutions.holistic
mp_pose = mp.solutions.pose

prevwristx=0
prevwristy=0

#previousGuesss=np.zeros(7)
potentialGuess=['other','right','left','up','down']
directions=['right','left','up','down']
previousGesture=0

onleftShoulder=False
wasOnLeftShoulder=False
belowShoulder=False
wasBelowShoulder=False
wasInBox=False
firstRun=True
protentialGesture=0

def inBoxCheck(x,y,boxlocation,boxdimension):
    diffx=boxcenter[0]-x
    diffy=boxcenter[1]-y
    if abs(diffx)<=boxdimension and abs(diffy)<=boxdimension:
        return True
    else:
        return False
def directionLeftBoxIn(x,y,boxlocation,boxdimension):
    diffx=boxcenter[0]-x
    diffy=boxcenter[1]-y
    print("y: " +str(diffy))
    print("x: " +str(diffx))
    if abs(diffx)>boxdimension and abs(diffy)>boxdimension:
        return -1
    if diffx<=0:
        #left
        if abs(diffx)>boxdimension and abs(diffy)<=boxdimension:
            return 1
    if diffx>=0:
        #right
        if abs(diffx)>boxdimension and abs(diffy)<=boxdimension:
            return 0
    if diffy<=0:
        #below
        if abs(diffy)>boxdimension and abs(diffx)<=boxdimension:
            return 3
    if diffy>=0:
        #up
        if abs(diffy)>boxdimension and abs(diffx)<=boxdimension:
            return 2

    return -1  
    


# For webcam input:
cap = cv2.VideoCapture(0)
cap.set(3, 640)
cap.set(4, 420)

with mp_holistic.Holistic(
    min_detection_confidence=0.5,
    min_tracking_confidence=0.5) as holistic:
  while cap.isOpened():
    success, image = cap.read()
    if not success:
      print("Ignoring empty camera frame.")
      # If loading a video, use 'break' instead of 'continue'.
      continue


    width  = cap.get(3)  # float `width`
    height = cap.get(4) 
    # To improve performance, optionally mark the image as not writeable to
    # pass by reference.
    image.flags.writeable = False
    image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
    results = holistic.process(image)

    # Draw landmark annotation on the image.
    image.flags.writeable = True
    image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)
    mp_drawing.draw_landmarks(
        image,
        results.pose_landmarks,
        mp_holistic.POSE_CONNECTIONS,
        landmark_drawing_spec=mp_drawing_styles
        .get_default_pose_landmarks_style())
    

    try:

        #get wrist
        wrist = results.pose_landmarks.landmark[16]
        wristx=int(wrist.x*width)
        wristy=int(wrist.y*height)

        #get elbow
        elbow = results.pose_landmarks.landmark[14]
        elbowx=int(elbow.x*width)
        elbowy=int(elbow.y*height)
        
        #get shoulder
        shoulder = results.pose_landmarks.landmark[12]
        shoulderx=int(shoulder.x*width)
        shouldery=int(shoulder.y*height)
        cv2.putText(image,str(shoulderx),(shoulderx,shouldery),cv2.FONT_HERSHEY_PLAIN,2,(255,0,0),2)

        #box for to check if wrist is in
        boxcenter=(elbowx, elbowy)
        boxleft=(boxcenter[0]-50,boxcenter[1]-50)
        boxright=(boxcenter[0]+50,boxcenter[1]+50)

        #get nose
        nose = results.pose_landmarks.landmark[0]
        nosex=int(nose.x*width)
        nosey=int(nose.y*height)

        inBox=inBoxCheck(wristx,wristy,boxcenter,75)
        cv2.putText(image,"In Box: " + str(inBox),(10,100),cv2.FONT_HERSHEY_PLAIN,1,(255,0,0),1)
        cv2.putText(image,"Most Recent Gesture: " + directions[previousGesture],(300,100),cv2.FONT_HERSHEY_PLAIN,1,(255,0,0),1)

        if wasInBox or firstRun:  
            if firstRun:
                firstRun=False
            if not inBox:
                direction = directionLeftBoxIn(wristx,wristy,boxcenter,75)
                print(direction)
                wasInBox=False
                if direction ==-1:
                    cv2.putText(image,"Gesture: Other ",(300,100),cv2.FONT_HERSHEY_PLAIN,1,(255,0,0),1)
                else:
                    previousGesture=direction
                    cv2.putText(image,"Gesture: " + directions[direction],(300,100),cv2.FONT_HERSHEY_PLAIN,1,(255,0,0),1)
                    file.writelines(str(datetime.datetime.now()))
                    file.writelines(',')
                    file.writelines(directions[direction])
                    file.writelines(',')
                    file.writelines(str(nosex))
                    file.writelines(',')
                    file.writelines(str(nosey))
                    file.write('\n')
                    file.flush()

            else:
                wasInBox = True      
        else:
            if inBox:
                wasInBox=True
            else:
                wasInBox=False
        
    except:
        print("Landmark not found")
        pass


    
    try:
        cv2.rectangle(image,boxleft,boxright,(0,255,0),2)
    except:
        print("Box Dimensions out of Range")
        pass
  
    cv2.imshow('MediaPipe Holistic', image)

    if cv2.waitKey(5) & 0xFF == 27:
      break
cap.release()