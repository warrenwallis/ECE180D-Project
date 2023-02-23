import cv2
import mediapipe as mp
import time
import random
mp_drawing = mp.solutions.drawing_utils
mp_drawing_styles = mp.solutions.drawing_styles
mp_holistic = mp.solutions.holistic

current_time = int(time.time())
random_number1 = random.randint(0, 1)
random_number2 = random.randint(0, 1)


font                   = cv2.FONT_HERSHEY_SIMPLEX
bottomLeftCornerOfText = (30,450)
fontScale              = 1
fontColor              = (0,0,255)
thickness              = 3
lineType               = 2


# For webcam input:
cap = cv2.VideoCapture(0)
cap.set(cv2.CAP_PROP_FRAME_WIDTH, 640)
cap.set(cv2.CAP_PROP_FRAME_HEIGHT, 480)
with mp_holistic.Holistic(
    min_detection_confidence=0.5,
    min_tracking_confidence=0.5) as holistic:
  while cap.isOpened():
    success, image = cap.read()
    if not success:
      print("Ignoring empty camera frame.")
      # If loading a video, use 'break' instead of 'continue'.
      continue

    # To improve performance, optionally mark the image as not writeable to
    # pass by reference.
    image.flags.writeable = False
    image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
    image_height, image_width, _ = image.shape
    results = holistic.process(image)

    # Draw landmark annotation on the image.
    image.flags.writeable = True
    image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)
    mp_drawing.draw_landmarks(
        image,
        results.face_landmarks,
        mp_holistic.FACEMESH_CONTOURS,
        landmark_drawing_spec=None,
        connection_drawing_spec=mp_drawing_styles
        .get_default_face_mesh_contours_style())
    mp_drawing.draw_landmarks(
        image,
        results.pose_landmarks,
        mp_holistic.POSE_CONNECTIONS,
        landmark_drawing_spec=mp_drawing_styles
        .get_default_pose_landmarks_style())

    if(int(time.time()) -  current_time > 1):
        current_time = int(time.time())
        random_number1 = random.randint(0, 1)
        random_number2 = random.randint(0, 1)
    cv2.rectangle(image, (0+320*random_number1, 0+240*random_number2), (320+320*random_number1, 240+420*random_number2), (255, 0, 0), 2)
    image = cv2.flip(image, 1)
    if(results.pose_landmarks):
        noseX = results.pose_landmarks.landmark[mp_holistic.PoseLandmark.NOSE].x * image_width
        noseY = results.pose_landmarks.landmark[mp_holistic.PoseLandmark.NOSE].y * image_height
        if(0+320*random_number1 <= noseX <= 320+320*random_number1
        and 0+240*random_number2 <= noseY <= 240+420*random_number2):
            cv2.putText(image,'Inside Quadrant', 
                        bottomLeftCornerOfText, 
                        font, 
                        fontScale,
                        fontColor,
                        thickness,
                        lineType)


    # Flip the image horizontally for a selfie-view display.
    cv2.imshow('MediaPipe Holistic', image)
    if cv2.waitKey(5) & 0xFF == 27:
      break
cap.release()