import cv2
import mediapipe as mp
import numpy as np
mp_drawing = mp.solutions.drawing_utils
mp_drawing_styles = mp.solutions.drawing_styles
mp_holistic = mp.solutions.holistic
mp_pose = mp.solutions.pose

prevwristx=0
prevwristy=0

#previousGuesss=np.zeros(7)
potentialGuess=['other','right','left','up','down']

onleftShoulder=False
belowShoulder=False
previousGesture=0


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
        
        #what side of shoulder
        wtosdistancex=shoulderx-wristx
        wtosdistancey=shouldery-wristy
        if wtosdistancex <=0:
            cv2.putText(image,'Left of Shoulder',(10,100),cv2.FONT_HERSHEY_PLAIN,1,(255,0,0),1)
            if not onleftShoulder:
                previousGesture=2
                onLeftShoulder=True
        else:
            cv2.putText(image,'Right of Shoulder',(10,100),cv2.FONT_HERSHEY_PLAIN,1,(255,0,0),1)
            if onLeftShoulder:
                previousGesture=1
                onLeftShoulder=False
        if wtosdistancey <=0:
            cv2.putText(image,'Below Shoulder',(10,200),cv2.FONT_HERSHEY_PLAIN,1,(255,0,0),1)
            if not belowShoulder:
                previousGesture=4
                belowShoulder=True
        else:
            cv2.putText(image,'Above Shoulder',(10,200),cv2.FONT_HERSHEY_PLAIN,1,(255,0,0),1)
            if belowShoulder:
                previousGesture=3
                belowShoulder=False


        cv2.putText(image,"Gesture: Swipe " + potentialGuess[previousGesture],(300,100),cv2.FONT_HERSHEY_PLAIN,1,(255,0,0),1)



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
        cv2.putText(image,str(velox),(wristx,wristy -1),cv2.FONT_HERSHEY_PLAIN,2,(255,0,0),2)
        # Flip the image horizontally for a selfie-view display.
    except:
        pass

    cv2.imshow('MediaPipe Holistic', image)
    if cv2.waitKey(5) & 0xFF == 27:
      break
cap.release()