import cv2
import mediapipe as mp
import numpy as np
import csv

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
        
        #get shoulder
        shoulder = results.pose_landmarks.landmark[12]
        shoulderx=int(shoulder.x*width)
        shouldery=int(shoulder.y*height)
        cv2.putText(image,str(shoulderx),(shoulderx,shouldery),cv2.FONT_HERSHEY_PLAIN,2,(255,0,0),2)
        
    except:
        print("Landmark not found")
        pass

    #box for to check if wrist is in
    boxcenter=(shoulderx, shouldery+100)
    boxleft=(boxcenter[0]-75,boxcenter[1]-75)
    boxright=(boxcenter[0]+75,boxcenter[1]+75)
    try:
        cv2.rectangle(image,boxleft,boxright,(0,255,0),2)
    except:
        print("Box Dimensions out of Range")
        pass

    # #box for other movement(ei moving slightly up or down during a swipe right/left)
    # boxcenter1=(shoulderx, shouldery+100)
    # boxleft1=(boxcenter1[0]-125,boxcenter1[1]-125)
    # boxright1=(boxcenter1[0]+125,boxcenter1[1]+125)
    # try:
    #     #cv2.rectangle(image,boxleft1,boxright1,(0,255,0),2)
    #     pass
    # except:
    #     print("Box Dimensions out of Range")
    #     pass
    
    
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
        else:
            wasInBox = True      
    else:
        if inBox:
            wasInBox=True
        else:
            wasInBox=False

    #what side of shoulder box the wrist is on
    # wtosdistancex=shoulderx-wristx
    # wtosdistancey=shouldery+100-wristy

    # #code for tracking where relative to the shoulder the wrist is   
    # if wtosdistancex <=0:
    #     cv2.putText(image,'Left of Box Center',(10,100),cv2.FONT_HERSHEY_PLAIN,1,(255,0,0),1)
    #     if not onleftShoulder:
    #        # previousGesture=2
    #         onleftShoulder=True
    #         wasOnLeftShoulder=False
    #     else:
    #         wasOnLeftShoulder=True
    # else:
    #     cv2.putText(image,'Right of Box Center',(10,100),cv2.FONT_HERSHEY_PLAIN,1,(255,0,0),1)
    #     if onleftShoulder:
    #         #previousGesture=1
    #         onleftShoulder=False
    #         wasOnLeftShoulder=True
    #     else:
    #         wasOnLeftShoulder=False
    # #this maynot be useful anymore
    # if wtosdistancey <=0:
    #     cv2.putText(image,'Below Box Center',(10,200),cv2.FONT_HERSHEY_PLAIN,1,(255,0,0),1)
    #     if not belowShoulder:
    #         #previousGesture=4
    #         belowShoulder=True
    #         wasBelowShoulder=False
    #     else:
    #         wasBelowShoulder=True
    # else:
    #     cv2.putText(image,'Above Box Center',(10,200),cv2.FONT_HERSHEY_PLAIN,1,(255,0,0),1)
    #     if belowShoulder:
    #         #previousGesture=3
    #         belowShoulder=False
    #         wasBelowShoulder=True
    #     else:
    #         wasBelowShoulder=False

    # #box check
    # if not (abs(wtosdistancex)<=75 and abs(wtosdistancey+100)<=75):
    #     if wasInBox:
    #         wasInBox=False
            
    #         #check where they moved too
    #         if onleftShoulder and (wasBelowShoulder==belowShoulder):
    #             previousGesture=1
    #         elif (not onleftShoulder) and (wasBelowShoulder==belowShoulder):
    #             previousGesture=2
    #         elif belowShoulder and (wasOnLeftShoulder==onleftShoulder):
    #             previousGesture=4
    #         elif (not belowShoulder) and (wasOnLeftShoulder==onleftShoulder):
    #             previousGesture=3
    #         else:
    #             previousGesture=0
    # else:
    #     wasInBox=True
    
    # cv2.putText(image,"Gesture: Swipe " + potentialGuess[previousGesture],(300,100),cv2.FONT_HERSHEY_PLAIN,1,(255,0,0),1)



    # #velocity in pixels per frame
    # velox=wristx-prevwristx
    # veloy=wristy-prevwristy
    # velomag=np.sqrt(velox*velox+veloy*veloy)

    # #plot velo arrow
    # start_point=(wristx,wristy)
    # end_point=(int(wristx+(velox)),int(wristy+(veloy)))
    # if end_point[0]<width and end_point[1]<height:
    #     image = cv2.arrowedLine(image, start_point, end_point,(0, 255, 0), 9) 


    # prevwristx=wristx
    # prevwristy=wristy



    # if velomag <10:
    #     cv2.putText(image,"Idle",(100,100),cv2.FONT_HERSHEY_PLAIN,2,(255,0,0),2)
    #     print("idle")
    # elif velox < -20 and abs(veloy) < 20:
    #     cv2.putText(image,"Right",(100,100),cv2.FONT_HERSHEY_PLAIN,2,(255,0,0),2)
    #     print("right")
    # elif velox > 20 and abs(veloy) < 20:
    #     cv2.putText(image,"Left",(100,100),cv2.FONT_HERSHEY_PLAIN,2,(255,0,0),2)
    #     print("left")
    # elif veloy < -20 and abs(velox) < 20:
    #     cv2.putText(image,"up",(100,100),cv2.FONT_HERSHEY_PLAIN,2,(255,0,0),2)
    #     print("up")
    # elif veloy > 20 and abs(velox) < 20:
    #     cv2.putText(image,"down",(100,100),cv2.FONT_HERSHEY_PLAIN,2,(255,0,0),2)
    #     print("down")
    # else:
    #     cv2.putText(image,"Other",(100,100),cv2.FONT_HERSHEY_PLAIN,2,(255,0,0),2)
    #     print("other")
    # flippedimage =cv2.flip(image, 1)
    #cv2.putText(image,str(velox),(wristx,wristy -1),cv2.FONT_HERSHEY_PLAIN,2,(255,0,0),2)
    # Flip the image horizontally for a selfie-view display.
  
    cv2.imshow('MediaPipe Holistic', image)

    if cv2.waitKey(5) & 0xFF == 27:
      break
cap.release()